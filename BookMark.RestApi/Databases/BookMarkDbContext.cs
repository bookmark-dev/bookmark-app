using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using BookMark.RestApi.Models;

namespace BookMark.RestApi.Databases {
	public class BookMarkDbContext : DbContext {
		private DbSet<User> Users { get; set; }
		public DbSet<Organization> Organizations { get; set; }
		public DbSet<Event> Events { get; set; }
		public DbSet<UserEvent> UserEvents { get; set; }
		private DbSet<Appointment> Appointments { get; set; }
		public DbSet<AppointmentGroup> AppointmentGroups { get; set; }
		public BookMarkDbContext(DbContextOptions options) : base(options) {

		}
		protected override void OnModelCreating(ModelBuilder builder) {
			// Setup Primary Keys
			builder.Entity<User>().HasKey(u => u.UserID);
			builder.Entity<User>().Property(u => u.UserID).ValueGeneratedNever();
			builder.Entity<Organization>().HasKey(o => o.OrganizationID);
			builder.Entity<Organization>().Property(o => o.OrganizationID).ValueGeneratedNever();
			builder.Entity<Appointment>().HasKey(a => a.AppointmentID);
			builder.Entity<Appointment>().Property(a => a.AppointmentID).ValueGeneratedNever();
			builder.Entity<AppointmentGroup>().HasKey(ag => ag.AppointmentGroupID);
			builder.Entity<AppointmentGroup>().Property(ag => ag.AppointmentGroupID).ValueGeneratedNever();
			builder.Entity<Event>().HasKey(e => e.EventID);
			builder.Entity<Event>().Property(e => e.EventID).ValueGeneratedNever();
			builder.Entity<UserEvent>().HasKey(ue => new { ue.UserID, ue.EventID });
			// builder.Entity<UserEvent>().Property(ue => ue.UserEventID).ValueGeneratedNever();

			//For User, Appointment, UserAppointment, and AppointmentGroup
			
			builder.Entity<User>().HasMany(u => u.UserEvents).WithOne(ue => ue.User).HasForeignKey(ue => ue.UserID);
			builder.Entity<Event>().HasMany(e => e.UserEvents).WithOne(ue => ue.Event).HasForeignKey(ue => ue.EventID);
			
			builder.Entity<UserEvent>().HasOne(ue => ue.User).WithMany(u => u.UserEvents).HasForeignKey(ue => ue.UserID);
			builder.Entity<UserEvent>().HasOne(ue => ue.Event).WithMany(e => e.UserEvents).HasForeignKey(ue => ue.EventID);

			builder.Entity<User>().HasMany(u => u.Appointments).WithOne(a => a.User).HasForeignKey(a => a.UserID);
			// builder.Entity<Appointment>().HasOne(a => a.User).WithMany(u => u.Appointments).HasForeignKey(a => a.AppointmentID);

			builder.Entity<AppointmentGroup>().HasMany(ag => ag.Appointments).WithOne(a => a.AppointmentGroup).HasForeignKey(a => a.AppointmentGroupID);
			// builder.Entity<Appointment>().HasOne(a => a.AppointmentGroup).WithMany(ag => ag.Appointments).HasForeignKey(a => a.AppointmentID);
			
			builder.Entity<Organization>().HasMany(o => o.AppointmentGroups).WithOne(ag => ag.Organization).HasForeignKey(ag => ag.OrganizationID);
			// builder.Entity<AppointmentGroup>().HasOne(ag => ag.Organization).WithMany(o => o.AppointmentGroups).HasForeignKey(ag => ag.AppointmentGroupID);

			// For Organization, Event, and UserEvent
			
			builder.Entity<Organization>().HasMany(o => o.Events).WithOne(e => e.Organization).HasForeignKey(e => e.OrganizationID);
			// builder.Entity<Event>().HasOne(e => e.Organization).WithMany(o => o.Events).HasForeignKey(e => e.EventID);
			
			/* TODO:	for User:
				Trim whitespaces from name, email, and passwords, when submitted for login and registration
				Also, make string comparisons for name and email non-case sensitive. 
			*/
			
			// Seed Data
			User[] users = new User[] {
				new User() { UserID = 1, Name = "synaodev", Email = "tylercadena@alum.calarts.edu", Password = BCrypt.Net.BCrypt.HashPassword("tylercadena") },
				new User() { UserID = 2, Name = "adrienne", Email = "adrienne-sparkman@gmail.com", Password = BCrypt.Net.BCrypt.HashPassword("Sparkman") }
			};
			builder.Entity<User>().HasData(users);
			
			Organization[] org = new Organization[] {
				new Organization() { OrganizationID = 1, Email = "revature@mail.com", Name = "revature", Password = BCrypt.Net.BCrypt.HashPassword("Revature") }
			};
			builder.Entity<Organization>().HasData(org);
		}
	}
}