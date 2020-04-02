using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BCrypt.Net;
using BookMark.RestApi.Abstracts;

namespace BookMark.RestApi.Models {
	public class User : AModel {
		public long UserID { get; set; }
		[Required]
		public string Name { get; set; }
		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }
		public List<Appointment> Appointments { get; set; }
		public User() {
			UserID = DateTime.Now.Ticks;
		}
		public override long GetID() {
			return UserID;
		}
		public bool CheckCredentials(string password) {
			return BCrypt.Net.BCrypt.Verify(password, this.Password);
		}
	}
}