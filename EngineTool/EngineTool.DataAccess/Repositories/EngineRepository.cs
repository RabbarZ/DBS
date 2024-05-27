using EngineTool.DataAccess.Entities;
using EngineTool.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EngineTool.DataAccess.Repositories;

internal sealed class EngineRepository(IEngineContext context) : Repository<Engine>(context), IEngineRepository
{
    public Engine? GetByIgdbId(int igdbId)
    {
        return GetQueryable().FirstOrDefault(e => e.IgdbId == igdbId);
    }

    public bool? GetContainsGame(Guid id, Guid gameId)
    {
        return GetQueryable().Include(e => e.Games).FirstOrDefault(e => e.Id == id)?.Games.Any(g => g.Id == gameId);
    }
}
