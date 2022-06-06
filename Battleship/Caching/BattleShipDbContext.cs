using Microsoft.EntityFrameworkCore;
using Battleship.Models;

namespace Battleship.Caching
{
    public class BattleShipDbContext : DbContext
    {
        public BattleShipDbContext(DbContextOptions<BattleShipDbContext> options) : base(options)
        {
        }

        public DbSet<Game> Games { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Board> Boards { get; set; }
        public DbSet<Ship> Ships { get; set; }
        public DbSet<BoardSquare> BoardSquares { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Game>()
                .HasMany(g => g.Players)
                .WithOne(p=>p.Game)
                .HasForeignKey(p=>p.GameId)
                .HasPrincipalKey(g=>g.Id);

            modelBuilder.Entity<Player>()
                .HasOne(p => p.Board)
                .WithOne(b => b.Player)
                .HasForeignKey<Board>(p=>p.PlayerId);

            modelBuilder.Entity<Board>()
                .HasMany(b => b.BoardPoints)
                .WithOne(bp => bp.Board)
                .HasForeignKey(bp => bp.BoardId)
                .HasPrincipalKey(b => b.Id);
        }
    }
}
