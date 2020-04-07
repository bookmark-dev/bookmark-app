using BCrypt.Net;

namespace BookMark.RestApi.Models {
	public class Compare {
		public string Password { get; set; }
		public string Hashed { get; set; }
		public bool Check() {
			return BCrypt.Net.BCrypt.Verify(Password, Hashed);
		}
	}
}