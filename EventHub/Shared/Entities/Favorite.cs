﻿namespace Shared.Entities
{
	public class Favorite
	{
		public int FavoriteId { get; set; }
		public int EventId { get; set; }
		public Guid UserId { get; set; }
		public User? User { get; set; }
		public Event? Event { get; set; }
	}
}
