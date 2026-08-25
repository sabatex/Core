using Microsoft.JSInterop;
using System.Net.Http.Headers;

namespace SabatexBlazorDemo.WASMStandAloneWithIdentity.Services;

public class JwtAuthorizationMessageHandler : DelegatingHandler
{
    private readonly IJSRuntime _js;
    private const string TokenKey = "authToken";

    public JwtAuthorizationMessageHandler(IJSRuntime js)
    {
        _js = js;
        InnerHandler = new HttpClientHandler();
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        try
        {
            var token = await _js.InvokeAsync<string>("localStorage.getItem", TokenKey);
            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }
        catch
        {
            // ignore errors reading token
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
