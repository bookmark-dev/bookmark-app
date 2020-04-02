using System.Collections.Generic;
using BookMark.RestApi.Interfaces;

namespace BookMark.RestApi.Abstracts {
	public abstract class ARepository<T> : IRepository<T> where T : AModel {
		protected List<T> table;
		public virtual List<T> All() {
			return table;
		}
		public virtual T Get(long ID) {
			return table.Find(m => m.GetID() == ID);
		}
		public virtual void Post(T model) {
			table.Add(model);
		}
		public virtual bool Put(T model) {
			T found = this.Get(model.GetID());
			if (found != null) {
				found = model;
				return true;
			}
			return false;
		}
		public virtual bool Delete(T model) {
			return table.Remove(model);
		}
	}
}