﻿using EngineTool.DataAccess.Entities;
using EngineTool.DataAccess.Interfaces;

namespace EngineTool.DataAccess.Repositories;

internal sealed class GameRepository(IEngineContext context) : Repository<Game>(context), IGameRepository
{
    public Game? GetByIgdbId(int igdbId)
    {
        return GetQueryable().FirstOrDefault(g => g.IgdbId == igdbId);
    }
}
