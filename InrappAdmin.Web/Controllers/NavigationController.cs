using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace InrappAdmin.Web.Controllers
{
    public class NavigationController : Controller
    {
        [ChildActionOnly]
        public ActionResult Menu()
        {
            if (Roles.IsUserInRole("Admin"))
            {
                return PartialView("_LoginAdmin");

            }

            return PartialView("_Loginpartial");
        }
    }
}