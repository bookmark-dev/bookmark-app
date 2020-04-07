using System.ComponentModel.DataAnnotations;
using BookMark.RestApi.Models;
using System;
using System.Collections.Generic;

namespace BookMark.Client.Models 
{
	public class EventViewModel 
  { 
    public long EventID { get; set; }
    [Required]
		public string Name { get; set; }
    [Required]
    [DataType(DataType.DateTime)]
		public DateTime DateTime { get; set; }
		public string Location { get; set; }
		public string Info { get; set; }
		public bool IsPublic { get; set; }
		// public long OrganizationID { get; set; }

		public long OrganizationID { get; set; }
		public List<UserEvent> UserEvents { get; set; }

    // FIXME: maybe 
    //public Organization Organization { get; set; }
    
		public EventViewModel() {}

    public EventViewModel(Event ev)
    {
      EventID = ev.EventID;
      Name = ev.Name;		
      DateTime = ev.DateTime;
      Location = ev.Location;
      Info = ev.Info;
      IsPublic = ev.IsPublic;
      OrganizationID = ev.Organization.OrganizationID;
      UserEvents = ev.UserEvents;
      //Organization = ev.Organization;
    }
	}
}