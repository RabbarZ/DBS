using EngineTool.DataAccess.Entities;
using EngineTool.DataAccess.Interfaces;

namespace EngineTool.DataAccess.Repositories;

public class PlayerStatsRepository(IEngineContext context) : Repository<PlayerStats>(context), IPlayerStatsRepository;