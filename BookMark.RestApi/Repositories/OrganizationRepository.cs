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
			return table
					.Include(o => o.Events)
					.Include(o => o.AppointmentGroups)
					.ThenInclude(ag => ag.Appointments)
					.SingleOrDefault(o => o.OrganizationID.Equals(ID));
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
		public Organization FindOrgByEmail(string email) 
    	{
			DbSet<Organization> table = _ctx.Set<Organization>();
			IQueryable<Organization> query = table.Where(o => o.Email.Equals(email));
			if (query.Count() == 0) 
      		{
				return null;
			}
			return query.First();
		}
		public bool CheckCredentials(string email, string password) 
    	{
			Organization org = this.FindOrgByEmail(email);
			if (org != null) 
      		{
				return org.CheckCredentials(password);
			}
			return false;
		}
	}
}