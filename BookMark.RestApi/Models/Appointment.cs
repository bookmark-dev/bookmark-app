using System;
using System.Collections.Generic;
using BookMark.RestApi.Abstracts;

namespace BookMark.RestApi.Models {
	public class Appointment : AModel {
		public long AppointmentID { get; set; }
		public DateTime DateTime { get; set; }
		public List<User> Users { get; set; }
		public override long GetID() {
			return AppointmentID;
		}
		public Appointment() {
			AppointmentID = DateTime.Now.Ticks;
		}
	}
}