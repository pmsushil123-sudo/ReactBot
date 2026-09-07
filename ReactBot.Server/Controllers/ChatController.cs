using Microsoft.AspNetCore.Mvc;
using ReactBot.Server.Models;
using ReactBot.Server.Services;

namespace ReactBot.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChatController : ControllerBase
{
    private readonly IOllamaService _ollama;

    public ChatController(IOllamaService ollama)
    {
        _ollama = ollama;
    }

    [HttpPost]
    public async Task<ActionResult<ChatResponse>> Post([FromBody] ChatRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request?.Message))
            return BadRequest("Message is required.");

        var reply = await _ollama.GenerateAsync(request.Message, cancellationToken);

        // Try to parse the Ollama JSON response and return a structured object.
        try
        {
            using var doc = System.Text.Json.JsonDocument.Parse(reply);
            var root = doc.RootElement;

            var model = root.TryGetProperty("model", out var m) ? m.GetString() ?? string.Empty : string.Empty;
            var createdAt = root.TryGetProperty("created_at", out var c) ? c.GetString() ?? string.Empty : string.Empty;
            var responseText = root.TryGetProperty("response", out var r) ? r.GetString() ?? string.Empty : string.Empty;

            return Ok(new ChatResponse
            {
                Model = model,
                CreatedAt = createdAt,
                Response = responseText,
                Raw = reply
            });
        }
        catch (System.Text.Json.JsonException)
        {
            // If reply is not valid JSON, return it as Raw and put it in Response for compatibility.
            return Ok(new ChatResponse { Raw = reply, Response = reply });
        }
    }
}
