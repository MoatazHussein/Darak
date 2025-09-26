using System.Net.Http.Headers;
using System.Net.Http.Json;
using Darak.Application.Common.Interfaces.Messaging;
using Microsoft.Extensions.Configuration;

namespace Darak.Infrastructure.Services.Messaging.Infobip;

internal sealed class InfobipSmsSender : ISmsSender
{
    private readonly HttpClient _http;
    private readonly IConfiguration _cfg;

    public InfobipSmsSender(HttpClient http, IConfiguration cfg)
    {
        _http = http;
        _cfg = cfg;

        var baseUrl = _cfg["Infobip:BaseUrl"]!;
        var apiKey = _cfg["Infobip:ApiKey"]!; 
        _http.BaseAddress = new Uri(baseUrl);
        _http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        _http.DefaultRequestHeaders.Add("Authorization", $"App {apiKey}");
    }

    public Task SendAsync(string to, string message, CancellationToken ct = default)
    {
        var from = _cfg["Infobip:From"] ?? "MyApp";
        var payload = new
        {
            messages = new[]
            {
                new {
                    destinations = new[] { new { to } },
                    from,
                    text = message
                }
            }
        };

        return _http.PostAsJsonAsync("/sms/2/text/advanced", payload, ct);
    }
}

