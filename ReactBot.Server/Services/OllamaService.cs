using System.Net.Http.Json;
using Microsoft.Extensions.Options;

namespace ReactBot.Server.Services;

public class OllamaService : IOllamaService
{
    private readonly HttpClient _httpClient;
    private readonly OllamaOptions _options;

    public OllamaService(HttpClient httpClient, IOptions<OllamaOptions> options)
    {
        _httpClient = httpClient;
        _options = options.Value;
    }

    public async Task<string> GenerateAsync(string prompt, CancellationToken cancellationToken = default)
    {
        var body = new { model = _options.Model, prompt = prompt, stream = false };
        using var resp = await _httpClient.PostAsJsonAsync("/api/generate", body, cancellationToken);
        var content = await resp.Content.ReadAsStringAsync(cancellationToken);
        if (!resp.IsSuccessStatusCode)
        {
            // Provide detailed error to aid debugging (status + body)
            throw new HttpRequestException($"Ollama returned {(int)resp.StatusCode} {resp.ReasonPhrase}: {content}");
        }

        // Return raw response (caller can parse JSON)
        return content;
    }
}
