// TODO: switch login using email
using System;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using BookMark.RestApi.Models;
using BookMark.Client.Models;
using BookMark.Client.Utils;

namespace BookMark.Client.Controllers {
	public class OrganizationController : Controller {
		private readonly HttpService service;
		private readonly HttpClient client;
		public OrganizationController() {
			service = HttpService.Service;
			client = HttpService.Client;
		}
		static public async Task<Organization> GetCurrentOrg(HttpContext context, HttpClient client) 
    {
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

		static public async Task<Organization> FindOrgByEmail(HttpClient client, string email) 
    {
			HttpResponseMessage response = await client.GetAsync($"/api/org/name/{email}");
			if (!response.IsSuccessStatusCode) 
      {
				return null;
			}
			return await response.Content.ReadAsAsync<Organization>();
		}

		static public async Task<long> CreateNewOrg(HttpClient client, string name, string password) 
    {
			Organization org = new Organization() 
      {
				Name = name,
				Password = password
			};
			long OrgID = org.OrganizationID;
			HttpContent content = new StringContent(org.ToString());
			HttpResponseMessage response = await client.PostAsync("/api/org", content);
			if (!response.IsSuccessStatusCode) 
      {
				return 0;
			}
			return OrgID;
		}

		[HttpGet]
		public IActionResult Index() 
    {
			Task<Organization> org_task = GetCurrentOrg(HttpContext, client);
			org_task.Wait();
			Organization org = org_task.Result;
			if (org == null) 
      {
				return Redirect("/home/index");
			}
			return View(new OrganizationViewModel(org));
		}

		[HttpGet]
		public IActionResult Login() 
    {
			return View();
		}

		[HttpPost]
		public IActionResult Login(OrganizationViewModel ovm) 
    {
			if (!ModelState.IsValid) 
      {
				return View(ovm);
			}
			// Console.WriteLine($"Model = (Name: {uvm.Name}, Password: {uvm.Password})");
			Task<Organization> task = FindOrgByEmail(client, ovm.Email);
			task.Wait();
			Organization org = task.Result;
			if (org == null) 
      {
				// Console.WriteLine("Couldn't get user!");
				return View(ovm);
			}
			// Console.WriteLine($"Entity = (Name: {user.Name}, Password {user.Password})");
			if (!org.CheckCredentials(ovm.Password)) 
      {
				return View(ovm);
			}
			HttpContext.Session.SetString("OrgID", org.OrganizationID.ToString());
			return Redirect("/organization/index");
		}

		[HttpGet]
		public IActionResult Register() 
    {
			return View();
		}

		[HttpPost]
		public IActionResult Register(OrganizationViewModel ovm) //TODO: Confirm this works/update to match user register code if different
    {
			ViewData["RegErr"] = "";
			if (!ModelState.IsValid) 
      {
				return View(ovm);
			}
			Task<Organization> find_org = FindOrgByEmail(client, ovm.Email);	//TODO: maybe add check to FindOrgByName, as well
			find_org.Wait();
			Organization org = find_org.Result;
			if (org != null) 
      {
				ViewData["RegErr"] = "Email is already taken!";
				return View(ovm);
			}
			Task<long> find_id = CreateNewOrg(client, ovm.Name, ovm.Password);
			find_id.Wait();
			long ID = find_id.Result;
			if (ID == 0) 
      {
				ViewData["RegErr"] = "Registration unsuccessful!";
				return View(ovm);
			}
			HttpContext.Session.SetString("OrgID", ID.ToString());
			return Redirect("/organization/index");
		}
	}
}