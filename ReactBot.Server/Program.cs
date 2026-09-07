var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Configure Ollama options and client
var ollamaSection = builder.Configuration.GetSection("Ollama");
builder.Services.Configure<ReactBot.Server.Services.OllamaOptions>(ollamaSection);
var ollamaBase = ollamaSection.GetValue<string>("BaseUrl") ?? "http://127.0.0.1:11434";
builder.Services.AddHttpClient<ReactBot.Server.Services.IOllamaService, ReactBot.Server.Services.OllamaService>(c =>
{
    c.BaseAddress = new Uri(ollamaBase);
    // Increase timeout because Ollama generate requests can take longer than the
    // HttpClient default (100s). Set to 5 minutes for local/dev usage.
    c.Timeout = TimeSpan.FromMinutes(5);
});

var app = builder.Build();

app.UseDefaultFiles();
app.MapStaticAssets();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
