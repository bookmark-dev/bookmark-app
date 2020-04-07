using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BookMark.Client.Models;
using Microsoft.AspNetCore.Http;

namespace BookMark.Client.Controllers {
    public class HomeController : Controller {
        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger) {
            _logger = logger;
        }
        public IActionResult Index() {
            return View();
        }
        public IActionResult Privacy() {
            return View();
        }

        public IActionResult Logout() {
            HttpContext.Session.SetString("AcctID", "");
			HttpContext.Session.SetString("OrgID", "");
            return Redirect("/home/index");
        }

        public IActionResult RelIndex() {
            if (HttpContext.Session.GetString("AcctID")!=null && HttpContext.Session.GetString("OrgID")==null){
                return Redirect("/user/index");
            }
            else if (HttpContext.Session.GetString("AcctID")==null && HttpContext.Session.GetString("OrgID")!=null){
                return Redirect("/organization/index");
            }
            else if (HttpContext.Session.GetString("AcctID")==null && HttpContext.Session.GetString("OrgID")==null){
                return Redirect("/home/index");
            }
            else {
                return Redirect("/home/logout");
            }

        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
