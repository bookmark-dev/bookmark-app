
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using BookMark.RestApi.Models;
using BookMark.RestApi.Services;

namespace BookMark.RestApi.Controllers {
	[ApiController]
	[EnableCors()]
	public class OrganizationController : ControllerBase {
		private OrmService _srv;
		public OrganizationController(OrmService srv) 
    	{
			_srv = srv;
		}

		[HttpGet("/api/org")]
		public IActionResult Get() 
    	{
			List<Organization> orgs = _srv.AllOrgs();
			if (orgs.Count > 0) {
				return Ok(orgs);
			}
			return NotFound("No organizations exist!");
		}

		[HttpGet("/api/org/{id}")]
		public IActionResult Get(string id) 
    	{
			long ID = 0;
			if (long.TryParse(id, out ID)) {
				Organization org = _srv.GetOrg(ID);
				if (org != null) {
					return Ok(org);
				}
				return NotFound($"Couldn't find user with ID: {ID}!");
			}
			return BadRequest("Couldn't parse user ID!");
		}

		[HttpGet("/api/org/email/{email}")]
		public IActionResult GetEmail(string email) 
    	{
			if (email.Length == 0) {
				return BadRequest("Email is invalid!");
			}
			Organization org = _srv.FindOrgByEmail(email);
			if (org == null) {
				return NotFound($"Couldn't find organization with email: {email}");
			}
			return Ok(org);
		}

		[HttpPost("/api/org")]
		public IActionResult Post(Organization model) 
		{
			if (ModelState.IsValid) 
      		{
				if (!_srv.CheckOrgExists(model.Name)) 
        		{
					Organization org = new Organization() 
          			{
						Name = model.Name,
						Password = model.Password
					};
					if (_srv.PostOrg(org)) 
          			{
						return Ok();
					}
					return BadRequest("Posting organization failed!");
				}
				return BadRequest($"Organization with name \"{model.Name}\" already exists!");
			}
			return BadRequest("Organization model is invalid!");
		}
		[HttpPut("/api/org")]
		public IActionResult Put(Organization model) {
			if (ModelState.IsValid) {
				if (_srv.CheckOrgExists(model.Name)) {
					Organization newOrganization = model;
					
					// Organization org = new Organization() 
          // {
					// 	OrganizationID = model.OrganizationID,
					// 	Name = model.Name,
					// 	Password = model.Password
					// };
					if (_srv.PutOrg(newOrganization)) {
						return Ok();
					}
					return BadRequest("Putting organization failed!");
				}
				return NotFound($"Couldn't find organization with name: \"{model.Name}\"!");
			}
			return BadRequest("Organization model is invalid!");
		}
		[HttpDelete("/api/org/{id}")]
		public IActionResult Delete(string id) 
    {
			long ID = 0;
			if (long.TryParse(id, out ID)) {
				Organization org = _srv.GetOrg(ID);
				if (org != null) {
					if (_srv.DeleteOrg(org)) 
          {
						return Ok();
					}
					return BadRequest("Deleting organization failed!");
				}
				return NotFound($"Couldn't find organization with ID: {ID}!");
			}
			return BadRequest("Couldn't parse organization ID!");
		}
	}
}
