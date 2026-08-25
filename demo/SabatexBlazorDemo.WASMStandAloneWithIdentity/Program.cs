using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SabatexBlazorDemo.WASMStandAloneWithIdentity;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Register JWT based authentication where backend is same domain
builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<Microsoft.AspNetCore.Components.Authorization.AuthenticationStateProvider, SabatexBlazorDemo.WASMStandAloneWithIdentity.Services.JwtAuthenticationStateProvider>();
builder.Services.AddScoped<SabatexBlazorDemo.WASMStandAloneWithIdentity.Services.JwtAuthenticationStateProvider>();
builder.Services.AddScoped<SabatexBlazorDemo.WASMStandAloneWithIdentity.Services.JwtAuthorizationMessageHandler>();

// HttpClient that automatically adds Bearer token from localStorage
builder.Services.AddScoped(sp =>
{
    var js = sp.GetRequiredService<Microsoft.JSInterop.IJSRuntime>();
    var handler = sp.GetRequiredService<SabatexBlazorDemo.WASMStandAloneWithIdentity.Services.JwtAuthorizationMessageHandler>();
    var client = new HttpClient(handler)
    {
        BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
    };
    return client;
});

await builder.Build().RunAsync();
