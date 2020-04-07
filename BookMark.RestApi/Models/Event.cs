using System;
using System.Collections.Generic;
using BookMark.RestApi.Abstracts;

namespace BookMark.RestApi.Models {
	public class Event : AModel {
		public long EventID { get; set; }
		public string Name { get; set; }
		public DateTime DateTime { get; set; }
		public string Location { get; set; }
		public string Info { get; set; }
		public bool IsPublic { get; set; }
		public long OrganizationID { get; set; }
        
		#region NAVIGATIONAL PROPERTIES
		public Organization Organization { get; set; }
		public List<UserEvent> UserEvents { get; set; }
		#endregion // NAVIGATIONAL PROPERTIES

		public Event() {
			EventID = DateTime.Now.Ticks;
		}

    	public override long GetID() {
			return EventID;
		}
		public override string ToString() {
			return $"Event: {EventID}\n{Name}\n{DateTime}\n{Location}\n{Info}\n{IsPublic}\n{OrganizationID}";
		}

		public Event(long id, string name, string info, DateTime datetime, string location, Organization org)
		{
			EventID = id;
			Name = name;
			DateTime = datetime;
			Info = info;
			Location = location;
			Organization = org;
		}
	}
}