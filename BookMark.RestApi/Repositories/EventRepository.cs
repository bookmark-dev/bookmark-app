using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using BookMark.RestApi.Databases;
using BookMark.RestApi.Abstracts;
using BookMark.RestApi.Models;

namespace BookMark.RestApi.Repositories 
{
	public class EventRepository : ARepository<Event> 
  {
		public EventRepository(BookMarkDbContext ctx) : base(ctx) {	}
		public override List<Event> All() 
    {
			DbSet<Event> table = _ctx.Set<Event>();
			return table
				// .Include(e => e.Organization)
				// .Include(e => e.UserEvents)
				// .ThenInclude(ue => ue.User)
				.ToList();
		}
		public override Event Get(long ID) 
    {
			DbSet<Event> table = _ctx.Set<Event>();
      
		  return table
        // .Include(e => e.Organization)
        // .Include(e => e.UserEvents)
        // .ThenInclude(ue => ue.User)
        .SingleOrDefault(e => e.EventID == ID);
		}
    public override bool Post(Event ev) 
    {
			DbSet<Event> table = _ctx.Set<Event>();
			table.Add(ev);
			return _ctx.SaveChanges() >= 1;
		}
		public override bool Put(Event ev) 
    {
			Event found = this.Get(ev.EventID);
			if (found != null) {
				found = ev;
				return _ctx.SaveChanges() >= 1;
			}
			return false;
		}

    // to search for event
		public List<Event> SearchByName(string name) 
    {
			DbSet<Event> table = _ctx.Set<Event>();
			IQueryable<Event> query = table.Where(e => e.Name.Contains(name));
				// .Include(e => e.Organization)
				// .Include(e => e.UserEvents)
				// .ThenInclude(ue => ue.User);
			if (query.Count() == 0) {
				return null;
			}
			return query.ToList();
		}

		// TODO: update when update user email
		public List<Event> FindByUser(string name) {
			DbSet<Event> table = _ctx.Set<Event>();
			IQueryable<Event> query = table.Where(e => e.UserEvents.Any(ue => ue.User.Name.Equals(name)));
				// .Include(e => e.Organization)
				// .Include(e => e.UserEvents)
				// .ThenInclude(ue => ue.User);
			if (query.Count() == 0) {
				return null;
			}
			return query.ToList();
		}
		public List<Event> FindByOrg(string email) 
    {
			DbSet<Event> table = _ctx.Set<Event>();
			IQueryable<Event> query = table.Where(e => e.Organization.Email.Equals(email));
				// .Include(e => e.Organization)
				// .Include(e => e.UserEvents)
				// .ThenInclude(ue => ue.User);
			if (query.Count() == 0) {
				return null;
			}
			return query.ToList();
		}
    
	}
}
