using System.Security.Cryptography;
using System.Text;

namespace Rest.Helpers
{
	public static class HashHelper
	{
		private static SHA256 hash = SHA256.Create();

		public static string sha256_hash(string value)
		{
			var stringBuilder = new StringBuilder();

			var enc = Encoding.UTF8;
			var result = hash.ComputeHash(enc.GetBytes(value));

			foreach (var b in result)
				stringBuilder.Append(b.ToString("x2"));

			return stringBuilder.ToString();
		}
	}
}