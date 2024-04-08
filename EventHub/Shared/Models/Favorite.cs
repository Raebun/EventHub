namespace Shared.Models
{
	public class Favorite
	{
		public int FavoritesId { get; set; }
		public int EventId { get; set; }
		public int UserId { get; set; }
		public User User { get; set; }
		public Event Event { get; set; }
	}
}
