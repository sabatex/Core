using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Radzen.Documents.Markdown;
using RadzenBlazorDemo.Data;
using RadzenBlazorDemo.Models;
using RadzenBlazorDemo.Services;
using Sabatex.Core.Identity;
using Sabatex.Core.RadzenBlazor;
using Sabatex.RadzenBlazor;
using Sabatex.RadzenBlazor.Server;
using SabatexBlazorDemo.Components;
using System.Reflection;


var builder = WebApplication.CreateBuilder(args);

            //var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(connection));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddIdentity<ApplicationUser,IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddRoles<IdentityRole>()
                .AddSignInManager()
                .AddDefaultTokenProviders();

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveWebAssemblyComponents()
                .AddAuthenticationStateSerialization();

            builder.Services.AddCascadingAuthenticationState();
            

            var authBuilder = builder.Services.AddAuthentication(options =>
                {
                    options.DefaultScheme = IdentityConstants.ApplicationScheme;
                    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
                });

            authBuilder.AddIsConfiguredMicrosoft(builder.Configuration)
                       .AddIsConfiguredGoogle(builder.Configuration);

            // JWT configuration for API clients (WASM)
            var jwtKey = builder.Configuration["Jwt:Key"] ?? "ChangeThisSecretForProduction_ReplaceIt";
            var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? builder.Configuration["ApplicationUrl"] ?? "https://localhost";
            var signingKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtKey));

            authBuilder.AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = true;
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtIssuer,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = signingKey,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(2)
                };
            });
            builder.Services.AddAuthorization();

                

            builder.Services.AddSingleton<Sabatex.Core.Identity.IEmailSender<ApplicationUser>, IdentityEmailSender>();
            builder.Services.AddLocalization(options => { options.ResourcesPath = "Resources"; });
            builder.Services.AddControllers();
            builder.Services.AddSabatexRadzenBlazor();
            builder.Services.AddSabatexRadzenBlazorServer();
            builder.Services.AddScoped<IIdentityAdapter,IdentityAdapterServer>();
            builder.Services.AddScoped<ISabatexRadzenBlazorDataAdapter, SabatexServerRadzenBlazorDataAdapter>();
            builder.Services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, ApplicationUserClaimsPrincipalFactory>();
            builder.Services.AddScoped<AuthenticationStateProvider, PersistingServerAuthenticationStateProvider>();


            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.Events.OnRedirectToLogin = context =>
                {
                    if (context.Request.Path.StartsWithSegments("/api"))
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return Task.CompletedTask;
                    }
                    context.Response.Redirect(context.RedirectUri);
                    return Task.CompletedTask;
                };
            });

            var app = builder.Build();

            // Enable authentication/authorization middleware
            app.UseAuthentication();
            app.UseAuthorization();

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
            }


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseWebAssemblyDebugging();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseAntiforgery();

            app.MapStaticAssets();
            app.UseRequestLocalization(
                new RequestLocalizationOptions() { ApplyCurrentCultureToResponseHeaders = true }
                .AddSupportedCultures(new[] { "en-US", "uk-UA" })
                .AddSupportedUICultures(new[] { "en-US", "uk-UA" })
                .SetDefaultCulture("uk-UA")
                );

            var additionalAssemblies = new Assembly[] {
                //typeof(SabatexBlazorDemo.WASMClientA._Imports).Assembly,
                // typeof(SabatexBlazorDemo.WASMClientB._Imports).Assembly,
                typeof(Sabatex.RadzenBlazor._Imports).Assembly,
                typeof(Sabatex.RadzenBlazor.Server.ApplicationUser).Assembly
            };

            //app.UseSabatexServerBlazor(additionalAssemblies);

            app.MapRazorComponents<App>()
                //.AddInteractiveWebAssemblyRenderMode()
                .AddAdditionalAssemblies(additionalAssemblies);

            // Add additional endpoints required by the Identity /Account Razor components.
            app.MapAdditionalIdentityEndpoints();

            // Token endpoint for JWT issuance for SPA clients
            app.MapPost("/api/token", async (
                [FromServices] UserManager<ApplicationUser> userManager,
                [FromServices] SignInManager<ApplicationUser> signInManager,
                [FromServices] IConfiguration config,
                [FromBody] Microsoft.AspNetCore.Identity.Data.LoginRequest model) =>
            {
                if (model == null || string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
                    return Results.BadRequest(new { error = "invalid_request" });

                var user = await userManager.FindByEmailAsync(model.Email);
                if (user == null)
                    return Results.Unauthorized();

                var pwValid = await userManager.CheckPasswordAsync(user, model.Password);
                if (!pwValid)
                    return Results.Unauthorized();

                var roles = await userManager.GetRolesAsync(user);
                var claims = new List<System.Security.Claims.Claim>
                {
                    new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.NameIdentifier, user.Id),
                    new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, user.UserName ?? user.Email ?? string.Empty),
                    new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Email, user.Email ?? string.Empty),
                };
                claims.AddRange(roles.Select(r => new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Role, r)));

                var jwtKey = config["Jwt:Key"] ?? "ChangeThisSecretForProduction_ReplaceIt";
                var jwtIssuer = config["Jwt:Issuer"] ?? config["ApplicationUrl"] ?? "https://localhost";
                var signingKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtKey));

                var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(
                    issuer: jwtIssuer,
                    audience: null,
                    claims: claims,
                    notBefore: DateTime.UtcNow,
                    expires: DateTime.UtcNow.AddHours(8),
                    signingCredentials: new Microsoft.IdentityModel.Tokens.SigningCredentials(signingKey, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256)
                );

                var tokenString = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler().WriteToken(token);

                return Results.Ok(new { access_token = tokenString, expires_in = 8 * 3600 });
            });

            app.MapControllers();
            //app.MapFallbackToFile("/wasm-stand-alone-with-identity/{*path:nonfile}", "wasm-stand-alone-with-identity/index.html");
            await app.RunAsync(args,
                async () => 
                {
                    var serviceProvider = app.Services.CreateScope().ServiceProvider;
                    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                    var adminRole = await roleManager.GetOrCreateRoleAsync(Sabatex.Core.ISecurityRoles.Administrator);
                    var userRole =  await roleManager.GetOrCreateRoleAsync("ApplicationUser");

                    var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                    var admin = await userManager.GetOrCreateUserAsync("testAdmin@mail.com", "Test Admin", "Aa1234567890-");
                    var user = await userManager.GetOrCreateUserAsync("testUser@mail.com", "Test User", "Aa1234567890-");

                    var startDate = DateOnly.FromDateTime(DateTime.Now);
                    var summaries = new[] { "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" };
                    
                    for ( var i = 0;i<100;i++)
                    {
                        var weatherForecast = new WeatherForecast
                        {
                            Id = Guid.NewGuid(),
                            Date = startDate.AddDays(i),
                            TemperatureC = Random.Shared.Next(-20, 55),
                            Summary = summaries[Random.Shared.Next(summaries.Length)]
                        };
                        serviceProvider.GetRequiredService<ApplicationDbContext>().Add(weatherForecast);
                    } 
                    await serviceProvider.GetRequiredService<ApplicationDbContext>().SaveChangesAsync();
                },
                async (string userName) => 
                {
                    var serviceProvider = app.Services.CreateScope().ServiceProvider;
                    var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                    await userManager.GrandUserAdminRoleAsync(userName);
                },
                async () => 
                {
                    var serviceProvider = app.Services.CreateScope().ServiceProvider;
                    var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
                    //await dbContext.Database.MigrateAsync();

                });
