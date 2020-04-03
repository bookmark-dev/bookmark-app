using System;

namespace BookMark.RestApi.Models {
	public class UserAppointment {
		public long UserAppointmentID { get; set; }
		public long UserID { get; set; }
		public long AppointmentID { get; set; }
		#region NAVIGATIONAL PROPERTIES
		public User User { get; set; }
		public Appointment Appointment { get; set; }
		#endregion // NAVIGATIONAL PROPERTIES
		public UserAppointment() {
			UserAppointmentID = DateTime.Now.Ticks;
		}
	}
}