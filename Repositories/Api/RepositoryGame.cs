using DataBases.Contexts;
using DataBases.Models;
using Microsoft.EntityFrameworkCore;

namespace Repositories.Api
{
    public interface IRepositoryGame
    {
        Task<Game> CreateAsync(Game model, CancellationToken token = default);
        Task<Game> UpdateAsync(Game model, CancellationToken token = default);
        Task DeleteAsync(Game model, CancellationToken token = default);

        Game GetNew();
        Task<Game?> GetSignalOrDefaultAsync(int id, CancellationToken token = default);
        Task<IEnumerable<Game>> GetListAsync(CancellationToken token = default);
    }
    public class RepositoryGame : IRepositoryGame
    {
        private readonly DbContextGames context;

        public RepositoryGame(DbContextGames context, CancellationToken token = default)
        {
            this.context = context;
        }

        public async Task<Game> CreateAsync(Game model, CancellationToken token = default)
        {
            await context.AddAsync(model, token);
            return model;
        }

        public async Task<Game> UpdateAsync(Game model, CancellationToken token = default)
        {
            var local = context.Games.Local.FirstOrDefault(p =>  p.Id.Equals(model.Id));
            if (local != null && !object.ReferenceEquals(local, model))
            {
                context.Entry(local).State = EntityState.Detached;
            }
            context.Update(model);
            return model;
        }

        public Task DeleteAsync(Game model, CancellationToken token = default)
        {
            context.Remove(model);
            return Task.CompletedTask;
        }

        public Game GetNew()
        {
            return new Game();
        }

        public async Task<Game?> GetSignalOrDefaultAsync(int id, CancellationToken token = default)
        {
            var query = context.Games.AsNoTracking();
            return await query.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Game>> GetListAsync(CancellationToken token = default)
        {
            var query = context.Games.AsNoTracking();
            return await query.ToListAsync();
        }
    }
}
