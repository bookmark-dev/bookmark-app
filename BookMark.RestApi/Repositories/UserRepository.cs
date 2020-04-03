using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using BookMark.RestApi.Databases;
using BookMark.RestApi.Abstracts;
using BookMark.RestApi.Models;

namespace BookMark.RestApi.Repositories {
	public class UserRepository : ARepository<User> {
		public UserRepository(BookMarkDbContext ctx) : base(ctx) {

		}
		public override List<User> All() {
			DbSet<User> table = _ctx.Set<User>();
			return table.ToList();
		}
		public override User Get(long ID) {
			DbSet<User> table = _ctx.Set<User>();
			return table.SingleOrDefault(u => u.UserID == ID);
		}
		public override bool Post(User user) {
			user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
			DbSet<User> table = _ctx.Set<User>();
			table.Add(user);
			return _ctx.SaveChanges() >= 1;
		}
		public override bool Put(User user) {
			User found = this.Get(user.UserID);
			if (found != null) {
				found = user;
				found.Password = BCrypt.Net.BCrypt.HashPassword(found.Password);
				return _ctx.SaveChanges() >= 1;
			}
			return false;
		}
		public User FindByName(string name) {
			DbSet<User> table = _ctx.Set<User>();
			IQueryable<User> query = table.Where(u => u.Name == name);
			if (query.Count() == 0) {
				return null;
			}
			return query.First();
		}
		public bool CheckCredentials(string name, string password) {
			User user = this.FindByName(name);
			if (user != null) {
				return user.CheckCredentials(password);
			}
			return false;
		}
	}
}