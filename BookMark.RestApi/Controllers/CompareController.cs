// TODO: switch login using email
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using BookMark.RestApi.Models;
using BookMark.RestApi.Services;

namespace BookMark.RestApi.Controllers {
	[ApiController]
	[EnableCors("CorsPolicy")]
	public class CompareController : ControllerBase {
		private OrmService _srv;
		public CompareController(OrmService srv) {
			_srv = srv;
		}
		[HttpPost("/api/compare")]
		public IActionResult Compare(Compare model) {
			if (ModelState.IsValid) {
				if (model.Check()) {
					return Ok(true);
				}
				return Ok(false);
			}
			return BadRequest("Compare model is invalid!");
		}
	}
}
