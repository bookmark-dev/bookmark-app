using System;
using BCrypt.Net;
using System.Collections.Generic;
using BookMark.RestApi.Abstracts;

namespace BookMark.RestApi.Models {
	public class Organization : AModel {
		public long OrganizationID { get; set; }
		public string Name { get; set; }
		public string Password { get; set; }

		#region NAVIGATIONAL PROPERTIES
		public List<Event> Events { get; set; }
        // List of AppointmentGroup
		#endregion // NAVIGATIONAL PROPERTIES

		public Organization() {
			OrganizationID = DateTime.Now.Ticks;
		}
		public override long GetID() {
			return OrganizationID;
		}
		public bool CheckCredentials(string password) {
			return BCrypt.Net.BCrypt.Verify(password, this.Password);
		}
	}
}