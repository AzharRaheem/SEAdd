using SEAdd.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SEAdd.Controllers
{
    [HandleError]
    [Authorize]
    public class DashboardController : Controller
    {
        [Authorize(Roles ="Admin")]
        public ActionResult AdminDashboard()
        {
            return View();
        }
        [Authorize(Roles ="User")]
        public ActionResult UserDashboard()
        {
            return View();
        }
    }
}