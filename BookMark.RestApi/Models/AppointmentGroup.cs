using System;
using System.Collections.Generic;
using BookMark.RestApi.Abstracts;

namespace BookMark.RestApi.Models {
	public class AppointmentGroup : AModel {
		public long AppointmentGroupID { get; set; }
		public string Name { get; set; }
		public string Location { get; set; }
		public string Info { get; set; }
		public long OrganizationID { get; set; }
		public bool IsPublic { get; set; }

		#region NAVIGATIONAL PROPERTIES
		public List<Appointment> Appointments { get; set; }
		public Organization Organization{ get; set; }
		#endregion // NAVIGATIONAL PROPERTIES
		public override long GetID() {
			return AppointmentGroupID;
		}
		public AppointmentGroup() {
			AppointmentGroupID = DateTime.Now.Ticks;
		}
	}
}