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
		private readonly HttpService service;
		private readonly HttpClient client;
		public UserController() {
			service = HttpService.Service;
			client = HttpService.Client;
		}
		static public async Task<User> GetCurrentUser(HttpContext context, HttpClient client) {
			string acct_id = context.Session.GetString("AcctID");
			if (acct_id == null) {
				return null;
			}
			if (acct_id.Length == 0) {
				return null;
			}
			long ID = 0;
			if (!long.TryParse(acct_id, out ID)) {
				return null;
			}
			HttpResponseMessage response = await client.GetAsync($"/api/user/{ID}");
			if (!response.IsSuccessStatusCode) {
				return null;
			}
			return await response.Content.ReadAsAsync<User>();
		}
		static public async Task<User> FindUserByName(HttpClient client, string name) {
			HttpResponseMessage response = await client.GetAsync($"/api/user/name/{name}");
			if (!response.IsSuccessStatusCode) {
				return null;
			}
			return await response.Content.ReadAsAsync<User>();
		}
		static public async Task<long> CreateNewUser(HttpClient client, string name, string password) {
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
			System.Console.WriteLine(content.ToString());
			HttpResponseMessage response = await client.PostAsync("/api/user", content);
			if (!response.IsSuccessStatusCode) {
				return 0;
			}
			return UserID;
		}
		[HttpGet]
		public IActionResult Index() {
			Task<User> user_task = GetCurrentUser(HttpContext, client);
			user_task.Wait();
			User user = user_task.Result;
			if (user == null) {
				return Redirect("/home/index");
			}
			return View(new UserViewModel(user));
		}
		[HttpGet]
		public IActionResult Login() {
			return View();
		}
		[HttpPost]
		public IActionResult Login(UserViewModel uvm) {
			if (!ModelState.IsValid) {
				return View(uvm);
			}
			Task<User> task = FindUserByName(client, uvm.Name);
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
			return View();
		}
		[HttpPost]
		public IActionResult Register(UserViewModel uvm) {
			ViewData["RegErr"] = "";
			if (!ModelState.IsValid) {
				return View(uvm);
			}
			Task<User> find_user = FindUserByName(client, uvm.Name);
			find_user.Wait();
			User user = find_user.Result;
			if (user != null) {
				ViewData["RegErr"] = "Name is already taken!";
				return View(uvm);
			}
			Task<long> find_id = CreateNewUser(client, uvm.Name, uvm.Password);
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