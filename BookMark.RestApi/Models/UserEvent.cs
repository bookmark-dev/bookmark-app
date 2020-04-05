using System;

namespace BookMark.RestApi.Models {
	public class UserEvent {
		public long UserEventID { get; set; }
		public long UserID { get; set; }
 		public long EventID { get; set; }

		#region NAVIGATIONAL PROPERTIES
		public User User { get; set; }
    	public Event Event { get; set; }
		#endregion // NAVIGATIONAL PROPERTIES

		public UserEvent() {
			UserEventID = DateTime.Now.Ticks;
		}
	}
}