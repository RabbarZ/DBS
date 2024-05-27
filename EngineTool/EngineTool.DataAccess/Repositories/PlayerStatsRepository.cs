using EngineTool.DataAccess.Entities;
using EngineTool.DataAccess.Interfaces;

namespace EngineTool.DataAccess.Repositories;

internal sealed class PlayerStatsRepository(IEngineContext context) : Repository<PlayerStats>(context), IPlayerStatsRepository;