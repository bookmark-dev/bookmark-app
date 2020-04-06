using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using BookMark.RestApi.Models;
using BookMark.Client.Models;
using BookMark.Client.Utils;

namespace BookMark.Client.Controllers {
	public class EventController : Controller {
		private readonly HttpService service;
		private readonly HttpClient client;

		public EventController() 
    	{
			service = HttpService.Service;
			client = HttpService.Client;
		}

		static public async Task<Organization> GetCurrentOrg(HttpContext context, HttpClient client) {
			string org_id = context.Session.GetString("OrgID");
			if (org_id == null) 
      		{
				return null;
			}
			if (org_id.Length == 0) 
      		{
				return null;
			}
			long ID = 0;
			if (!long.TryParse(org_id, out ID)) 
      		{
				return null;
			}
			HttpResponseMessage response = await client.GetAsync($"/api/org/{ID}");
			if (!response.IsSuccessStatusCode) 
      		{
				return null;
			}
			return await response.Content.ReadAsAsync<Organization>();
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
			if (!long.TryParse(acct_id, out ID)) 
      		{
				return null;
			}
			HttpResponseMessage response = await client.GetAsync($"/api/user/{ID}");
			if (!response.IsSuccessStatusCode) 
      		{
				return null;
			}
			return await response.Content.ReadAsAsync<User>();
		}

    // Shows the events for current user/org
    	[HttpGet]
		public IActionResult Index() 
    	{
			Task<Organization> org_task = GetCurrentOrg(HttpContext, client);
			org_task.Wait();
			Organization org = org_task.Result;
      Task<User> user_task = GetCurrentUser(HttpContext, client);
			user_task.Wait();
			User user = user_task.Result;
      
      // Get all Events for Current Org or User
      if (user != null) 
      {
        // TODO: update if we update login to user email
				Task<List<Event>> ev_task = FindUserEvents(client, user.Name); // This function takes HttpContext and a string called "email".
			  ev_task.Wait();
			  List<Event> events = ev_task.Result;	
        return View(events);	
			}
			else if (org != null)
			{
				Task<List<Event>> ev_task = FindOrgEvents(client, org.Email);
        List<Event> events = ev_task.Result;	
				return View(events);
			}

			return Redirect("/home/index");
		}
    

		// (you need context to access session stuff)
		// (you need client to make HTTP requests)
    // Get events for a user
		static public async Task<List<Event>> FindUserEvents(HttpClient client, string name) 
    	{
			HttpResponseMessage response = await client.GetAsync($"/api/event/user/{name}");
			if (!response.IsSuccessStatusCode) {
				return null;
			}
			return await response.Content.ReadAsAsync<List<Event>>();
		}

    // Get events for a organization
		static public async Task<List<Event>> FindOrgEvents(HttpClient client, string email) 
    {
			HttpResponseMessage response = await client.GetAsync($"/api/event/org/{email}");
			if (!response.IsSuccessStatusCode) 
      {
				return null;
			}
			return await response.Content.ReadAsAsync<List<Event>>();
		}

    // TODO: Search by Name
		static public async Task<List<Event>> SearchEventByName(HttpClient client, string name) 
    {
			HttpResponseMessage response = await client.GetAsync($"/api/event/search/{name}");
			if (!response.IsSuccessStatusCode) {
				return null;
			}
			return await response.Content.ReadAsAsync<List<Event>>();
		}


    /*
		static public async Task<long> CreateNewEvent(HttpClient client, string name, string location, string info, DateTime datetime) 
    {
			Event ev = new Event() 
      {
				Name = name,	
      	Location = location,
      	DateTime = datetime,
      	Info = info,
      	Organization = GetCurrentOrg(context, client)
			};
			long EventID = ev.EventID;
			HttpContent content = new StringContent(ev.ToString());
			HttpResponseMessage response = await client.PostAsync("/api/event", content);
			if (!response.IsSuccessStatusCode) 
      {
				return 0;
			}
			return EventID;
		}
    */

		// [HttpPost]
		// public IActionResult Login(UserViewModel uvm) {
		// 	if (!ModelState.IsValid) {
		// 		return View(uvm);
		// 	}
		// 	// Console.WriteLine($"Model = (Name: {uvm.Name}, Password: {uvm.Password})");
		// 	Task<User> task = FindUserByName(client, uvm.Name);
		// 	task.Wait();
		// 	User user = task.Result;
		// 	if (user == null) {
		// 		// Console.WriteLine("Couldn't get user!");
		// 		return View(uvm);
		// 	}
		// 	// Console.WriteLine($"Entity = (Name: {user.Name}, Password {user.Password})");
		// 	if (!user.CheckCredentials(uvm.Password)) {
		// 		return View(uvm);
		// 	}
		// 	HttpContext.Session.SetString("AcctID", user.UserID.ToString());
		// 	return Redirect("/user/index");
		// }
    /*
		[HttpGet]
		public IActionResult Create() 
    {
			return View();
		}

		[HttpPost]
		public IActionResult Create(EventViewModel evm) {
			ViewData["RegErr"] = "";
			if (!ModelState.IsValid) {
				return View(evm);
			}
			Task<long> find_id = CreateNewEvent(client, evm.Name, evm.Location, evm.Info, evm.DateTime);
			find_id.Wait();
			long ID = find_id.Result;
			if (ID == 0) {
				ViewData["RegErr"] = "Registration unsuccessful!";
				return View(evm);
			}
			HttpContext.Session.SetString("AcctID", ID.ToString());
			return Redirect("/user/index");
		}
    */
	}
}