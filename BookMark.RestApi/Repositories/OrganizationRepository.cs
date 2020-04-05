using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using BookMark.RestApi.Databases;
using BookMark.RestApi.Abstracts;
using BookMark.RestApi.Models;

namespace BookMark.RestApi.Repositories {
	public class OrganizationRepository : ARepository<Organization> {
		public OrganizationRepository(BookMarkDbContext ctx) : base(ctx) {

		}
		public override List<Organization> All() 
    {
			DbSet<Organization> table = _ctx.Set<Organization>();
			return 
				table.Include(o => o.Events)
				.Include(o => o.AppointmentGroups)
					.ThenInclude(ag => ag.Appointments)
				.ToList();
		}
		public override Organization Get(long ID) 
    {
			DbSet<Organization> table = _ctx.Set<Organization>();
			return table.SingleOrDefault(o => o.OrganizationID == ID);
		}
		public override bool Post(Organization org) 
    {
			org.Password = BCrypt.Net.BCrypt.HashPassword(org.Password);
			DbSet<Organization> table = _ctx.Set<Organization>();
			table.Add(org);
			return _ctx.SaveChanges() >= 1;
		}
		public override bool Put(Organization org) 
    {
			Organization found = this.Get(org.OrganizationID);
			if (found != null) 
      {
				found = org;
				found.Password = BCrypt.Net.BCrypt.HashPassword(found.Password);
				return _ctx.SaveChanges() >= 1;
			}
			return false;
		}
		public Organization FindByName(string name) 
    {
			DbSet<Organization> table = _ctx.Set<Organization>();
			IQueryable<Organization> query = table.Where(o => o.Name == name);
			if (query.Count() == 0) 
      {
				return null;
			}
			return query.First();
		}
		public bool CheckCredentials(string name, string password) 
    {
			Organization org = this.FindByName(name);
			if (org != null) 
      {
				return org.CheckCredentials(password);
			}
			return false;
		}
	}
}