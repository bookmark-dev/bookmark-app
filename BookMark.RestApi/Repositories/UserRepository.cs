using System.Collections.Generic;
using BookMark.RestApi.Abstracts;
using BookMark.RestApi.Models;

namespace BookMark.RestApi.Repositories {
	public class UserRepository : ARepository<User> {
		public UserRepository() {
			table = new List<User>() {
				new User() {
					Name = "synaodev",
					Password =  BCrypt.Net.BCrypt.HashPassword("tylercadena")
				}
			};
		}
		public override void Post(User user) {
			user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
			table.Add(user);
		}
		public override bool Put(User user) {
			User found = this.Get(user.UserID);
			if (found != null) {
				found = user;
				found.Password = BCrypt.Net.BCrypt.HashPassword(found.Password);
				return true;
			}
			return false;
		}
		public User FindByName(string name) {
			return table.Find(u => u.Name == name);
		}
		public bool CheckExists(string name) {
			User user = this.FindByName(name);
			return user != null;
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