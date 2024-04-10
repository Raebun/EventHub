using System.Security.Cryptography;
using System.Text;

namespace Api.Helpers
{
	/// <summary>
	/// Helper class for hashing passwords using the SHA256 algorithm.
	/// </summary>
	public static class PasswordHasher
	{
		/// <summary>
		/// Hashes the given password using SHA256 algorithm.
		/// </summary>
		/// <param name="password">The password to hash.</param>
		/// <returns>The hashed password as a hexadecimal string.</returns>
		public static string HashPassword(string password)
		{
			using (SHA256 sha256 = SHA256.Create())
			{
				byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

				StringBuilder builder = new StringBuilder();
				for (int i = 0; i < hashedBytes.Length; i++)
				{
					builder.Append(hashedBytes[i].ToString("x2"));
				}
				return builder.ToString();
			}
		}
	}
}
