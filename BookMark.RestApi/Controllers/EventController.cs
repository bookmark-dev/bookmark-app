using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using BookMark.RestApi.Models;
using BookMark.RestApi.Services;

namespace BookMark.RestApi.Controllers 
{
	[ApiController]
	[EnableCors()]
	public class EventController : ControllerBase 
  {
		private OrmService _srv;
		public EventController(OrmService srv) 
    {
			_srv = srv;
		}

		[HttpGet("/api/event")]
		public IActionResult Get() 
    {
			List<Event> events = _srv.AllEvents();
			if (events.Count > 0) 
      {
				return Ok(events);
			}
			return NotFound("No events exist!");
		}

		[HttpGet("/api/event/{id}")]
		public IActionResult Get(string id)
		{
			long ID = 0;
			if (long.TryParse(id, out ID)) 
			{
				Event ev = _srv.GetEvent(ID);
				if (ev != null) {
					return Ok(ev);
				}
				return NotFound($"Couldn't find event with ID: {ID}!");
			}
			return BadRequest("Couldn't parse event ID!");
		}

		[HttpGet("/api/event/search/{name}")]
		public IActionResult GetSearch(string name) 
    {
			if (name.Length == 0) 
      {
				return BadRequest("Name is invalid!");
			}
			List<Event> events = _srv.SearchEventByName(name);
			if (events == null) 
      {
				return NotFound($"No results found for events when searching for: {name}");
			}
			return Ok(events);
		}
		[HttpPost("/api/event")]
		public IActionResult Post(Event model) {
			if (ModelState.IsValid) {
				/*Event ev = new Event() {
					EventID = model.EventID,
					Name = model.Name,
					DateTime = model.DateTime,
					Location = model.Location,
					Info = model.Info,
					IsPublic = model.IsPublic,
					OrganizationID = model.OrganizationID,
					// Organization = _srv.GetOrg(model.OrganizationID)
				};*/
				//Organization org = _srv.GetOrg(model.OrganizationID);
				//org.Events.Add(model);
				System.Console.WriteLine(model);
				Organization org = _srv.GetOrg(model.OrganizationID);
				System.Console.Write(org);
				if (_srv.PostEvent(model)) {
					// FIXME: return ev for confirmation page or nothing?
					return Ok(model);
				}
				return BadRequest("Creating event failed!");
			}
			return BadRequest("Event model is invalid!");
		}
		[HttpPut("/api/event")]
		public IActionResult Put(Event model) 
		{
			if (ModelState.IsValid) 
			{
				if (_srv.GetEvent(model.EventID) != null) 
				{
					Event UpdatedEvent = model;
					// UpdatedEvent.Name = model.Name;
					// UpdatedEvent.DateTime = model.DateTime;
					// UpdatedEvent.Location = model.Location;
					// UpdatedEvent.Info = model.Info;

					if (_srv.PutEvent(UpdatedEvent)) 
					{
						return Ok();
					}
					return BadRequest("Putting event failed!");
				}
				return NotFound($"Couldn't find event with id: \"{model.EventID}\"!");
			}
			return BadRequest("Event model is invalid!");
		}

		[HttpDelete("/api/event/{id}")]
		public IActionResult Delete(string id) 
    	{
			long ID = 0;
			if (long.TryParse(id, out ID)) {
				Event ev = _srv.GetEvent(ID);
				if (ev != null) 
				{
					if (_srv.DeleteEvent(ev)) 
					{
						return Ok();
					}
					return BadRequest("Deleting user failed!");
				}
				return NotFound($"Couldn't find user with ID: {ID}!");
			}
			return BadRequest("Couldn't parse user ID!");
		}

		// TODO: update when update user email
		[HttpGet("/api/event/user/{name}")]
		public IActionResult FindEventsByUser(string name) 
    {
			if (name.Length == 0) 
      {
				return BadRequest("Name is invalid!");
			}
			List<Event> events = _srv.FindEventsByUser(name);
			if (events == null) 
      {
				return NotFound($"No results found for events when searching for: {name}");
			}
			return Ok(events);
		}

		[HttpGet("/api/event/org/{email}")]
		public IActionResult FindEventsByOrg(string email) 
    {
			if (email.Length == 0) 
      {
				return BadRequest("Email is invalid!");
			}
			List<Event> events = _srv.FindEventsByOrg(email);
			if (events == null) 
      {
				return NotFound($"No results found for events when searching for: {email}");
			}
			return Ok(events);
		}
	}
}
