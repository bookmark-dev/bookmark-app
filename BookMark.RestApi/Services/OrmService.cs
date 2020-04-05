using System.Collections.Generic;
using BookMark.RestApi.Databases;
using BookMark.RestApi.Models;
using BookMark.RestApi.Repositories;

namespace BookMark.RestApi.Services {
	public class OrmService {
		private UserRepository _ur;
		private AppointmentRepository _ar;
    private OrganizationRepository _or;
    private EventRepository _er;
		public OrmService(BookMarkDbContext ctx) {
			_ur = new UserRepository(ctx);
			_ar = new AppointmentRepository(ctx);
      _or = new OrganizationRepository(ctx);
      _er = new EventRepository(ctx);
		}
		// ALL
		public List<User> AllUsers() {
			return _ur.All();
		}
		public List<Appointment> AllAppointments() {
			return _ar.All();
		}
		// GET
		public User GetUser(long ID) {
			return _ur.Get(ID);
		}
		public Appointment GetAppointment(long ID) {
			return _ar.Get(ID);
		}
		// POST
		public bool PostUser(User user) {
			return _ur.Post(user);
		}
		public bool PostAppointment(Appointment appt) {
			return _ar.Post(appt);
		}
		// PUT
		public bool PutUser(User user) {
			return _ur.Put(user);
		}
		public bool PutAppointment(Appointment appt) {
			return _ar.Put(appt);
		}
		// DELETE
		public bool DeleteUser(User user) {
			return _ur.Delete(user);
		}
		public bool DeleteAppointment(Appointment appt) {
			return _ar.Delete(appt);
		}
		// EXTRA
		public User FindUserByName(string name) {
			return _ur.FindByName(name);
		}
		public bool CheckUserExists(string name) {
			User user = this.FindUserByName(name);
			return user != null;
		}
		public bool CheckUserCredentials(string name, string password) {
			return _ur.CheckCredentials(name, password);
		}

    // For Organization and Event Repos
    public List<Organization> AllOrgs() {
			return _or.All();
		}
		public List<Event> AllEvents() {
			return _er.All();
		}
		// GET
		public Organization GetOrg(long ID) {
			return _or.Get(ID);
		}
		public Event GetEvent(long ID) {
			return _er.Get(ID);
		}
		// POST
		public bool PostOrg(Organization org) {
			return _or.Post(org);
		}
		public bool PostEvent(Event ev) {
			return _er.Post(ev);
		}
		// PUT
		public bool PutOrg(Organization org) {
			return _or.Put(org);
		}
		public bool PutEvent(Event ev) {
			return _er.Put(ev);
		}
		// DELETE
		public bool DeleteOrg(Organization org) {
			return _or.Delete(org);
		}
		public bool DeleteEvent(Event ev) {
			return _er.Delete(ev);
		}
		// EXTRA
		public Organization FindOrgByName(string name) {
			return _or.FindByName(name);
		}
		public bool CheckOrgExists(string name) {
			Organization org = this.FindOrgByName(name);
			return org != null;
		}
		public bool CheckOrgCredentials(string name, string password) {
			return _or.CheckCredentials(name, password);
		}
	}
}