using EngineTool.Entities;

namespace EngineTool.DataAccess.Repositories
{
    public class RatingRepository : IRatingRepository
    {
        private readonly IRepository<Rating> repository;

        public RatingRepository(IRepository<Rating> repository)
        {
            this.repository = repository;
        }

        public void Add(Rating rating)
        {
            this.repository.Add(rating);
        }
    }
}
