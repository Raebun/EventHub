﻿namespace Shared.Entities
{
	public class Event
	{
		public int EventId { get; set; }
		public string EventName { get; set; }
		public string EventDescription { get; set; }
		public DateTime EventDate { get; set; }
		public string Location { get; set; }
		public float TicketPrice { get; set; }
		public int UserId { get; set; }
		public User User { get; set; }
	}
}
