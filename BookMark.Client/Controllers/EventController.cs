using System;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using BookMark.RestApi.Models;
using BookMark.Client.Models;
using BookMark.Client.Utils;

namespace BookMark.Client.Controllers {
	public class EventController : Controller {
		private readonly HttpService _service;
		public EventController(HttpService service) {
			_service = service;
		}
		private async Task<Organization> GetCurrentOrg() {
			string org_id = HttpContext.Session.GetString("OrgID");
			if (org_id == null || org_id.Length == 0) {
				return null;
			}
			long ID = 0;
			if (!long.TryParse(org_id, out ID)) {
				return null;
			}
			HttpResponseMessage response = await _service.client.GetAsync($"/api/org/{ID}");
			if (!response.IsSuccessStatusCode) {
				return null;
			}
			return await response.Content.ReadAsAsync<Organization>();
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
    	// Shows the events for current user/org
			// TODO: Seperate for USER and ORG
    [HttpGet]
		public IActionResult Index() 
		{
			Task<Organization> org_task = GetCurrentOrg();
			org_task.Wait();
			Organization org = org_task.Result;
      Task<User> user_task = GetCurrentUser();
			user_task.Wait();
			User user = user_task.Result;
      // Get all Events for Current Org or User
      if (user != null) 
			{
        // TODO: update if we update login to user email
				Task<List<Event>> ev_task = FindUserEvents(user.Name);
			  ev_task.Wait();
			  List<Event> events = ev_task.Result;	

        return View(CreateEVMList(events));
			} else if (org != null) 
			{
				Task<List<Event>> ev_task = SearchEventByEmail(org.Email);
        List<Event> events = ev_task.Result;	

				return View(CreateEVMList(events));
			}
			return Redirect("/home/index");
		}
		
		

		public List<EventViewModel> CreateEVMList(List<Event> events)
		{
			List<EventViewModel> eventsview = new List<EventViewModel>();
			if(events != null && events.Count > 0)
			{
				foreach (var e in events)
				{
					eventsview.Add(new EventViewModel(e));
				}
			}
			return eventsview;
		}

		// (you need context to access session stuff)
		// (you need client to make HTTP requests)
    	// Get events for a user
		private async Task<List<Event>> FindUserEvents(string name) {
			HttpResponseMessage response = await _service.client.GetAsync($"/api/event/user/{name}");
			if (!response.IsSuccessStatusCode) {
				return null;
			}
			return await response.Content.ReadAsAsync<List<Event>>();
		}
    	// Get events for a organization
		private async Task<List<Event>> SearchEventByEmail(string email) {
			HttpResponseMessage response = await _service.client.GetAsync($"/api/event/org/{email}");
			if (!response.IsSuccessStatusCode) {
				return null;
			}
			return await response.Content.ReadAsAsync<List<Event>>();
		}
		private async Task<List<Event>> SearchEventByName(string name) {
			HttpResponseMessage response = await _service.client.GetAsync($"/api/event/search/{name}");
			if (!response.IsSuccessStatusCode) {
				return null;
			}
			return await response.Content.ReadAsAsync<List<Event>>();
		}
    	[HttpGet]
		public IActionResult Create() {
			return View(new EventViewModel());
		}
		[HttpPost]
		public IActionResult Create(EventViewModel model) {
			if (!ModelState.IsValid) {
				return View(model);
			}
			Task<long> task_long = CreateEvent(model.Name, model.Location, model.Info, model.DateTime);
			task_long.Wait();
			long result = task_long.Result;
			if (result != 0L) {
				// TODO: Change redirect to go Confirmation/Event page?
				Task<Event> event_task = GetEvent(id);
				event_task.Wait();
				Event ev = event_task.Result;
				return View("Confirmation", new EventViewModel(ev));
			}

			// return Redirect("/organization/index");
			return View(model);
		}
		private async Task<long> CreateEvent(string name, string location, string info, DateTime datetime) {
			Task<Organization> task_org = GetCurrentOrg();
			task_org.Wait();
			Organization org = task_org.Result;
			if (org == null) {
				return 0;
			}
			Event ev = new Event() {
				Name = name,	
				Location = location,
				DateTime = datetime,
				Info = info,
				// Organization = org
				OrganizationID = org.OrganizationID
			};
			long EventID = ev.EventID;
			string serial = JsonConvert.SerializeObject(ev);
			System.Console.WriteLine(serial);
			HttpContent content = new StringContent(
				serial,
				Encoding.UTF8,
				"application/json"
			);
			HttpResponseMessage response = await _service.client.PostAsync("/api/event", content);
			/*if (!response.IsSuccessStatusCode) {
				return 0;
			}*/
			return EventID;
		}

		[HttpGet, ActionName("Detail")]
		public IActionResult Detail(long id) 
		{
      Task<Event> event_task = GetEvent(id);
			event_task.Wait();
			Event ev = event_task.Result;

			return View(new EventViewModel(ev));
		}

		private async Task<Event> GetEvent(long id) 
		{
			HttpResponseMessage response = await _service.client.GetAsync($"/api/event/{id}");
			if (!response.IsSuccessStatusCode) {
				return null;
			}
			return await response.Content.ReadAsAsync<Event>();
		}

	}
}