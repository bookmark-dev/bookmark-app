using System;
using BCrypt.Net;
using System.Collections.Generic;
using BookMark.RestApi.Abstracts;

namespace BookMark.RestApi.Models {
	public class User : AModel {
		public long UserID { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		
		#region NAVIGATIONAL PROPERTIES
		public List<Appointment> Appointments { get; set; }
		public List<UserEvent> UserEvents { get; set; }
		#endregion // NAVIGATIONAL PROPERTIES
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