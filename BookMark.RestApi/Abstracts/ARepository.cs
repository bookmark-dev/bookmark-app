using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using BookMark.RestApi.Interfaces;
using BookMark.RestApi.Databases;

namespace BookMark.RestApi.Abstracts {
	public abstract class ARepository<T> : IRepository<T> where T : AModel {
		protected BookMarkDbContext _ctx;
		public ARepository(BookMarkDbContext ctx) {
			_ctx = ctx;
		}
		public abstract List<T> All();
		public abstract T Get(long ID);
		public virtual bool Post(T model) {
			DbSet<T> table = _ctx.Set<T>();
			table.Add(model);
			return _ctx.SaveChanges() >= 1;
		}
		public virtual bool Put(T model) {
			T found = this.Get(model.GetID());
			if (found != null) {
				found = model;
				return _ctx.SaveChanges() >= 1;
			}
			return false;
		}
		public virtual bool Delete(T model) {
			DbSet<T> table = _ctx.Set<T>();
			table.Remove(model);
			return _ctx.SaveChanges() >= 1;
		}
	}
}