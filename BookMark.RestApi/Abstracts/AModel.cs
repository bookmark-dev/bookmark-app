using BookMark.RestApi.Interfaces;

namespace BookMark.RestApi.Abstracts {
	public abstract class AModel : IModel {
		public abstract long GetID();
	}
}