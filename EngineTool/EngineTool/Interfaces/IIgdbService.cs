using EngineTool.Entities;
using EngineTool.Models;

namespace EngineTool.Interfaces
{
    public interface IIgdbService
    {
        IAsyncEnumerable<IgdbGame> GetGamesAsync(int count);

        IAsyncEnumerable<IgdbGame> GetGamesAsync();

        int? GetSteamId(IgdbGame game);
    }
}
