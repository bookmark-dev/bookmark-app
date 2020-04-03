using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using BookMark.RestApi.Models;

namespace BookMark.RestApi.Databases {
	public class BookMarkDbContext : DbContext {
		private DbSet<User> Users { get; set; }
		private DbSet<Appointment> Appointments { get; set; }
		public BookMarkDbContext(DbContextOptions options) : base(options) {

		}
		protected override void OnModelCreating(ModelBuilder builder) {
			// Setup Primary Keys
			builder.Entity<User>().HasKey(u => u.UserID);
			builder.Entity<User>().Property(u => u.UserID).ValueGeneratedNever();
			builder.Entity<Appointment>().HasKey(a => a.AppointmentID);
			builder.Entity<Appointment>().Property(a => a.AppointmentID).ValueGeneratedNever();
			builder.Entity<User>().HasMany(u => u.UserAppointments).WithOne(ua => ua.User).HasForeignKey(ua => ua.UserID);
			builder.Entity<Appointment>().HasMany(a => a.UserAppointments).WithOne(ua => ua.Appointment).HasForeignKey(ua => ua.AppointmentID);
			builder.Entity<UserAppointment>().HasKey(ua => ua.UserAppointmentID);
			builder.Entity<UserAppointment>().Property(ua => ua.UserAppointmentID).ValueGeneratedNever();
			builder.Entity<UserAppointment>().HasOne(ua => ua.User).WithMany(u => u.UserAppointments).HasForeignKey(u => u.UserID);
			builder.Entity<UserAppointment>().HasOne(ua => ua.Appointment).WithMany(a => a.UserAppointments).HasForeignKey(a => a.AppointmentID);
			// Seed Data
			User[] users = new User[] {
				new User() { UserID = 1, Name = "synaodev", Password = BCrypt.Net.BCrypt.HashPassword("tylercadena") }
			};
			builder.Entity<User>().HasData(users);
		}
	}
}