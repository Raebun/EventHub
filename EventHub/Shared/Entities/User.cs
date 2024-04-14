using Microsoft.AspNetCore.Identity;

namespace Shared.Entities
{
	public class User : IdentityUser
	{
		public string? FirstName { get; set; }
		public string? LastName { get; set; }
		public string? ProfilePicture { get; set; }
	}
}
