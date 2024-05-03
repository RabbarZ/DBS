using EngineTool.Models;

namespace EngineTool.Interfaces
{
    public interface IIgdbService
    {
        Task<List<IgdbGame>> GetGamesAsync(int count);
    }
}
