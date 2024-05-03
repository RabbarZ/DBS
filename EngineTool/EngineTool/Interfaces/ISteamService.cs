using EngineTool.Models;

namespace EngineTool.Interfaces
{
    public interface ISteamService
    {
        Task<int?> GetCurrentPlayerCountAsync(int steamAppId);

        Task<SteamRating?> GetRatingAsync(int steamAppId);
    }
}
