
using System.ComponentModel.DataAnnotations;
using BookMark.RestApi.Models;

namespace BookMark.Client.Models {
	public class OrganizationViewModel {
		public string Name { get; set; }
		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }
		[Required]
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }
		
    public OrganizationViewModel(Organization org) {
			Name = org.Name;
			Email = org.Email;
			Password = org.Password;
		}
		public OrganizationViewModel() {
			
		}
	}
}
