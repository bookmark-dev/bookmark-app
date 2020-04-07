
using System.ComponentModel.DataAnnotations;
using BookMark.RestApi.Models;

namespace BookMark.Client.Models {
	public class UserViewModel {
		[Required]
		public string Name { get; set; }

		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }
		public UserViewModel(User user) {
			Name = user.Name;
			Email = user.Email;
			Password = user.Password;
		}
		public UserViewModel() {
			
		}
	}
}