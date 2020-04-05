using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using BookMark.RestApi.Databases;
using BookMark.RestApi.Abstracts;
using BookMark.RestApi.Models;

namespace BookMark.RestApi.Repositories {
	public class AppointmentRepository : ARepository<Appointment> {
		public AppointmentRepository(BookMarkDbContext ctx) : base(ctx) {

		}
		public override List<Appointment> All() {
			DbSet<Appointment> table = _ctx.Set<Appointment>();
			return table
				.Include(a => a.User)
				.Include(a => a.AppointmentGroup)
					.ThenInclude(ag => ag.Organization)
				.ToList();
		}
		public override Appointment Get(long ID) {
			DbSet<Appointment> table = _ctx.Set<Appointment>();
			return table.SingleOrDefault(u => u.AppointmentID == ID);
		}
		
		//TODO: Post, etc
	}
}