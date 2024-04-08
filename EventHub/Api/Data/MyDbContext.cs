using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace Api.Data
{
	public class MyDbContext : DbContext
	{
		public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
		{
		}

		public DbSet<User> Users { get; set; }
		public DbSet<Event> Events { get; set; }
		public DbSet<Ticket> Tickets { get; set; }
		public DbSet<Review> Reviews { get; set; }
		public DbSet<Favorite> Favorites { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Favorite>()
				.HasKey(f => f.FavoritesId);
		}
	}
}
