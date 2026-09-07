using System.Threading;
using System.Threading.Tasks;

namespace ReactBot.Server.Services;

public interface IOllamaService
{
    Task<string> GenerateAsync(string prompt, CancellationToken cancellationToken = default);
}
