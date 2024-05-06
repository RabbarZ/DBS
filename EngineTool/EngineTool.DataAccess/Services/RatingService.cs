using EngineTool.Entities;

namespace EngineTool.DataAccess.Services
{
    public class RatingService : IRatingService
    {
        private readonly IRepository<Rating> repository;

        public RatingService(IRepository<Rating> repository)
        {
            this.repository = repository;
        }

        public void Add(Rating rating)
        {
            this.repository.Add(rating);
        }
    }
}
