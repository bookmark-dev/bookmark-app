
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
	public class OrganizationController : Controller {
		private readonly HttpService _service;
		public OrganizationController(HttpService service) {
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
		private async Task<Organization> FindOrgByEmail(string email) {
			HttpResponseMessage response = await _service.client.GetAsync($"/api/org/email/{email}");
			if (!response.IsSuccessStatusCode) {
				return null;
			}
			return await response.Content.ReadAsAsync<Organization>();
		}
		private async Task<long> CreateNewOrg(string name, string email, string password) {
			Organization org = new Organization() {
				Name = name,
        		Email = email,
				Password = password
			};
			long OrgID = org.OrganizationID;
			HttpContent content = new StringContent(
				JsonConvert.SerializeObject(org),
				Encoding.UTF8,
				"application/json"
			);
			HttpResponseMessage response = await _service.client.PostAsync("/api/org", content);
			if (!response.IsSuccessStatusCode) {
				return 0;
			}
			return OrgID;
		}
		
		private async Task<long> UpdateOrg(string name, string email, string password) {
			string org_id = HttpContext.Session.GetString("OrgID");
			if (org_id == null || org_id.Length == 0) {
				return 0;
			}
			long ID = 0;
			if (!long.TryParse(org_id, out ID)) {
				return 0;
			}
			Organization org = new Organization() {
				Name = name,
        Email = email,
				Password = password,
				OrganizationID = ID
			};
			long OrgID = org.OrganizationID;
			HttpContent content = new StringContent(
				JsonConvert.SerializeObject(org),
				Encoding.UTF8,
				"application/json"
			);
			HttpResponseMessage response = await _service.client.PutAsync("/api/org", content);
			if (!response.IsSuccessStatusCode) {
				return 0;
			}
			return OrgID;
		}
		
		[HttpGet]
		public IActionResult Index() {
			Task<Organization> org_task = GetCurrentOrg();
			org_task.Wait();
			Organization org = org_task.Result;
			if (org == null) {
				return Redirect("/home/index");
			}
			return View(new OrganizationViewModel(org));
		}
		[HttpGet]
		public IActionResult Login() {
			return View();
		}
		[HttpPost]
		public IActionResult Login(OrganizationViewModel ovm) {
			if (!ModelState.IsValid) {
				return View(ovm);
			}
			ovm.Email = ovm.Email.ToLower().Replace(" ","");
			ovm.Password = ovm.Password.Replace(" ", ""); //FIXME: ???

			Task<Organization> task = FindOrgByEmail(ovm.Email);
			task.Wait();
			Organization org = task.Result;
			if (org == null) {
				return View(ovm);
			}
			if (!org.CheckCredentials(ovm.Password)) {
				return View(ovm);
			}
			HttpContext.Session.SetString("OrgID", org.OrganizationID.ToString());
			return Redirect("/organization/index");
		}
		[HttpGet]
		public IActionResult Register() {
			return View();
		}
		[HttpPost]
		public IActionResult Register(OrganizationViewModel ovm) {
			ViewData["RegErr"] = "";
			if (!ModelState.IsValid) {
				return View(ovm);
			} 
			ovm.Email=ovm.Email.ToLower().Replace(" ","");
			// FIXME: 
			ovm.Password = ovm.Password.Replace(" ","");
			if (ovm.Name.Length == 0 || ovm.Password.Length < 6) {
				return View(ovm);
			}

			
			Task<Organization> find_org = FindOrgByEmail(ovm.Email);
			find_org.Wait();
			Organization org = find_org.Result;
			if (org != null) {
				ViewData["RegErr"] = "Email is already taken!";
				return View(ovm);
			}
			Task<long> find_id = CreateNewOrg(ovm.Name, ovm.Email, ovm.Password);
			find_id.Wait();
			long ID = find_id.Result;
			if (ID == 0) {
				ViewData["RegErr"] = "Registration unsuccessful!";
				return View(ovm);
			}
			HttpContext.Session.SetString("OrgID", ID.ToString());
			return Redirect("/organization/index");
		}

		[HttpGet]
		public IActionResult UpdateInfo()
		{
			return View();
		}

		[HttpPut]
		public IActionResult UpdateInfo(OrganizationViewModel ovm)
		{
			ViewData["RegErr"] = "";
			
			if (!ModelState.IsValid) {
				return View(ovm);
			} 
			ovm.Email=ovm.Email.ToLower().Replace(" ","");
			ovm.Password = ovm.Password.Replace(" ","");	//FIXME: ???
			if (ovm.Name.Length == 0 || ovm.Password.Length < 6) {
				return View(ovm);
			}

			Task<Organization> find_org = FindOrgByEmail(ovm.Email);
			find_org.Wait();
			Organization org = find_org.Result;
			if (org != null ) {
				long tempID;
				if (!long.TryParse(HttpContext.Session.GetString("OrgID"), out tempID)) {
					return null;
				}
				if (org.OrganizationID!=tempID){
					ViewData["RegErr"] = "Email is already taken!";
					return View(ovm);
				}
			}
			Task<long> find_id = UpdateOrg(ovm.Name, ovm.Email, ovm.Password);
			find_id.Wait();
			long ID = find_id.Result;
			if (ID == 0) {
				ViewData["RegErr"] = "Registration unsuccessful!";
				return View(ovm);
			}
			return Redirect("/organization/index");
		}
	}
}