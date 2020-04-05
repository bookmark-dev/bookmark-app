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
			return table.Include(e => e.UserEvents).ThenInclude(ue => ue.User).ToList();
		}
		public override Event Get(long ID) 
    {
			DbSet<Event> table = _ctx.Set<Event>();
			return table.SingleOrDefault(e => e.EventID == ID);
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
		public List<Event> FindByName(string name) 
    {
			DbSet<Event> table = _ctx.Set<Event>();
			IQueryable<Event> query = table.Where(e => e.Name.Contains(name));
			if (query.Count() == 0) {
				return null;
			}
			return query.ToList();
		}
    
	}
}
