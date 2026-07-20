using DataBases.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;


namespace DataBases.Contexts
{
    public class DbContextGames : DbContext
    {
        public DbContextGames(DbContextOptions<DbContextGames> options) : base(options) { }

        public DbSet<Game> Games { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
