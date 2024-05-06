﻿using EngineTool.Entities;

namespace EngineTool.DataAccess.Services
{
    public class EngineService(IRepository<Engine> repository) : IEngineService
    {
        private readonly IRepository<Engine> repository = repository;

        public Engine? GetByIgdbId(int igdbId)
        {
            return this.repository.GetAll().SingleOrDefault(e => e.IgdbId == igdbId);
        }

        public bool GetContainsGame(Guid id, Guid gameId)
        {
            return this.repository.GetAll().Where(e => e.Id == id).Any(e => e.Games.Any(g => g.Id == gameId));
        }

        public void Add(Engine engine)
        {
            this.repository.Add(engine);
        }

        public void Update(Engine engine)
        {
            this.repository.Update(engine);
        }
    }
}
