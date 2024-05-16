using EngineTool.DataAccess.Entities;
using EngineTool.DataAccess.Interfaces;

namespace EngineTool.DataAccess.Repositories;

public class RatingRepository(IEngineContext context) : Repository<Rating>(context), IRatingRepository;