namespace ReactBot.Server.Models;

public class ChatResponse
{
    // Model identifier returned by Ollama (e.g. "llama3.2:latest")
    public string Model { get; set; } = string.Empty;

    // Timestamp returned by Ollama
    public string CreatedAt { get; set; } = string.Empty;

    // The generated text/response
    public string Response { get; set; } = string.Empty;

    // Fallback raw reply when parsing fails
    public string Raw { get; set; } = string.Empty;
}
