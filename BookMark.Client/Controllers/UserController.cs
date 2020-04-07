// TODO: switch login using email
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using BookMark.RestApi.Models;
using BookMark.Client.Models;
using BookMark.Client.Utils;

namespace BookMark.Client.Controllers {
	public class UserController : Controller {
		private readonly HttpService _service;
		public UserController(HttpService service) {
			_service = service;
		}
		private async Task<User> GetCurrentUser() {
			string acct_id = HttpContext.Session.GetString("AcctID");
			if (acct_id == null || acct_id.Length == 0) {
				return null;
			}
			long ID = 0;
			if (!long.TryParse(acct_id, out ID)) {
				return null;
			}
			HttpResponseMessage response = await _service.client.GetAsync($"/api/user/{ID}");
			if (!response.IsSuccessStatusCode) {
				return null;
			}
			return await response.Content.ReadAsAsync<User>();
		}
		private async Task<User> FindUserByName(string name) {
			HttpResponseMessage response = await _service.client.GetAsync($"/api/user/name/{name}");
			if (!response.IsSuccessStatusCode) {
				return null;
			}
			return await response.Content.ReadAsAsync<User>();
		}
		private  async Task<long> CreateNewUser(string name, string password) {
			User user = new User() {
				Name = name,
				Password = password
			};
			long UserID = user.UserID;
			HttpContent content = new StringContent(
				JsonConvert.SerializeObject(user), 
				Encoding.UTF8, 
				"application/json"
			);
			HttpResponseMessage response = await _service.client.PostAsync("/api/user", content);
			if (!response.IsSuccessStatusCode) {
				return 0;
			}
			return UserID;
		}
		[HttpGet]
		public IActionResult Index() {
			Task<User> user_task = GetCurrentUser();
			user_task.Wait();
			User user = user_task.Result;
			if (user == null) {
				return Redirect("/home/index");
			}
			return View(new UserViewModel(user));
		}
		[HttpGet]
		public IActionResult Login() {
			return View(new UserViewModel());
		}
		[HttpPost]
		public IActionResult Login(UserViewModel uvm) {
			if (!ModelState.IsValid) {
				return View(uvm);
			}
			Task<User> task = FindUserByName(uvm.Name);
			task.Wait();
			User user = task.Result;
			if (user == null) {
				return View(uvm);
			}
			if (!user.CheckCredentials(uvm.Password)) {
				return View(uvm);
      		}
			HttpContext.Session.SetString("AcctID", user.UserID.ToString());
			return Redirect("/user/index");
		}
		[HttpGet]
		public IActionResult Register() {
			return View(new UserViewModel());
		}
		[HttpPost]
		public IActionResult Register(UserViewModel uvm) {
			ViewData["RegErr"] = "";
			if (!ModelState.IsValid) {
				return View(uvm);
			}
			Task<User> find_user = FindUserByName(uvm.Name);
			find_user.Wait();
			User user = find_user.Result;
			if (user != null) {
				ViewData["RegErr"] = "Name is already taken!";
				return View(uvm);
			}
			Task<long> find_id = CreateNewUser(uvm.Name, uvm.Password);
			find_id.Wait();
			long ID = find_id.Result;
			if (ID == 0) {
				ViewData["RegErr"] = "Registration unsuccessful!";
				return View(uvm);
			}
			HttpContext.Session.SetString("AcctID", ID.ToString());
			return Redirect("/user/index");
		}
	}
}
