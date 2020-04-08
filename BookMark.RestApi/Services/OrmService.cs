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
		private AppointmentGroupRepository _agr;

		public OrmService(BookMarkDbContext ctx) {
			_ur = new UserRepository(ctx);
			_ar = new AppointmentRepository(ctx);
      _or = new OrganizationRepository(ctx);
      _er = new EventRepository(ctx);
			_agr = new AppointmentGroupRepository(ctx);
		}
		// ALL
		public List<User> AllUsers() {
			return _ur.All();
		}
		public List<Appointment> AllAppointments() {
			return _ar.All();
		}
		public List<AppointmentGroup> AllAppointmentGroups() {
			return _agr.All();
		}
		// GET
		public User GetUser(long ID) {
			return _ur.Get(ID);
		}
		public Appointment GetAppointment(long ID) {
			return _ar.Get(ID);
		}
		public AppointmentGroup GetAppointmentGroup(long ID) {
			return _agr.Get(ID);
		}
		// POST
		public bool PostUser(User user) {
			return _ur.Post(user);
		}
		public bool PostAppointment(Appointment appt) {
			return _ar.Post(appt);
		}
		public bool PostAppointmentGroup(AppointmentGroup apptGroup) {
			return _agr.Post(apptGroup);
		}
		// PUT
		public bool PutUser(User user) {
			return _ur.Put(user);
		}
		public bool PutAppointment(Appointment appt) {
			return _ar.Put(appt);
		}
		public bool PutAppointmentGroup(AppointmentGroup apptGroup) {
			return _agr.Put(apptGroup);
		}
		// DELETE
		public bool DeleteUser(User user) {
			return _ur.Delete(user);
		}
		public bool DeleteAppointment(Appointment appt) {
			return _ar.Delete(appt);
		}
		public bool DeleteAppointmentGroup(AppointmentGroup apptGroup) {
			return _agr.Delete(apptGroup);
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
		public Organization FindOrgByEmail(string email) {
			return _or.FindOrgByEmail(email);
		}
		public bool CheckOrgExists(string email) {
			Organization org = this.FindOrgByEmail(email);
			return org != null;
		}
		public bool CheckOrgCredentials(string email, string password) {
			return _or.CheckCredentials(email, password);
		}

		// EXTRA for EventRepo
		public List<Event> SearchEventByName(string name) 
		{
			return _er.SearchByName(name);
		}
		// TODO: update when update user email
		public List<Event> FindEventsByUser(string name) 
		{
			return _er.FindByUser(name);
		}
		public List<Event> FindEventsByOrg(string email) 
		{
			return _er.FindByOrg(email);
		}

		public bool PostUserEvent(UserEvent userevent)
		{
			return _er.UserEventPost(userevent);
		}

	}
}