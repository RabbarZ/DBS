using EngineTool.DataAccess.Entities;
using EngineTool.DataAccess.Interfaces;

namespace EngineTool.DataAccess.Repositories;

internal sealed class RatingRepository(IEngineContext context) : Repository<Rating>(context), IRatingRepository;