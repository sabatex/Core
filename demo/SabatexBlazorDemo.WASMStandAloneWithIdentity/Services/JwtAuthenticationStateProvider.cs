using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Security.Claims;
using System.Text.Json;

namespace SabatexBlazorDemo.WASMStandAloneWithIdentity.Services;

public class JwtAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly IJSRuntime _js;
    private const string TokenKey = "authToken";

    public JwtAuthenticationStateProvider(IJSRuntime js)
    {
        _js = js;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await GetTokenAsync();
        if (string.IsNullOrWhiteSpace(token))
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

        var identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");
        var user = new ClaimsPrincipal(identity);
        return new AuthenticationState(user);
    }

    public async Task<string?> GetTokenAsync() => await _js.InvokeAsync<string>("localStorage.getItem", TokenKey);

    public async Task MarkUserAsAuthenticated(string token)
    {
        await _js.InvokeVoidAsync("localStorage.setItem", TokenKey, token);
        var identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");
        var user = new ClaimsPrincipal(identity);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
    }

    public async Task MarkUserAsLoggedOut()
    {
        await _js.InvokeVoidAsync("localStorage.removeItem", TokenKey);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()))));
    }

    static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var parts = jwt.Split('.');
        if (parts.Length < 2) return Array.Empty<Claim>();
        var payload = parts[1];
        string json = DecodeBase64(payload);
        var doc = JsonDocument.Parse(json);
        var claims = new List<Claim>();
        foreach (var prop in doc.RootElement.EnumerateObject())
        {
            if (prop.Value.ValueKind == JsonValueKind.Array)
            {
                foreach (var item in prop.Value.EnumerateArray())
                {
                    claims.Add(new Claim(prop.Name, item.GetString() ?? string.Empty));
                }
            }
            else
            {
                claims.Add(new Claim(prop.Name, prop.Value.GetString() ?? string.Empty));
            }
        }
        return claims;
    }

    static string DecodeBase64(string str)
    {
        str = str.Replace('-', '+').Replace('_', '/');
        switch (str.Length % 4)
        {
            case 2: str += "=="; break;
            case 3: str += "="; break;
        }
        var bytes = Convert.FromBase64String(str);
        return System.Text.Encoding.UTF8.GetString(bytes);
    }
}
