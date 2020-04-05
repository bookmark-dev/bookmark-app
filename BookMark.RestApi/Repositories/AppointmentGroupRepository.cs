using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using BookMark.RestApi.Databases;
using BookMark.RestApi.Abstracts;
using BookMark.RestApi.Models;


namespace BookMark.RestApi.Repositories {
	public class AppointmentGroupRepository : ARepository<AppointmentGroup> {
		public AppointmentGroupRepository(BookMarkDbContext ctx) : base(ctx) {

		}
        public override List<AppointmentGroup> All() {
			DbSet<AppointmentGroup> table = _ctx.Set<AppointmentGroup>();
			return table
				.Include(a => a.Organization)
				.Include(a => a.Appointments)
					.ThenInclude(a => a.User)
				.ToList();
		}

        public override AppointmentGroup Get(long ID) {
			DbSet<AppointmentGroup> table = _ctx.Set<AppointmentGroup>();
			return table.SingleOrDefault(u => u.AppointmentGroupID == ID);
        }

		//TODO: Post, etc
    }
}