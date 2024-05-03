using EngineTool.Models;

namespace EngineTool.Interfaces
{
    public interface ISteamService
    {
        Task<SteamPlayerStatsResponse> GetCurrentPlayerCountAsync(int steamAppId);

        Task<SteamQuerySummary> GetRatingAsync(int steamAppId);
    }
}
