using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using GreenvaleStaffPortal.Models;

namespace GreenvaleStaffPortal.Controllers
{
    public class CallsController : Controller
    {
        StaffPortalEntities callsDB = new StaffPortalEntities();
        //
        // GET: /Calls/

        public ActionResult Index()
        {
            var loggedCalls = callsDB.LoggedCalls.Where(loggedCall => loggedCall.Solved == 0).ToList();
            return View(loggedCalls);
        }

        //
        // GET: /Calls/Details/5

        public ActionResult Details(string id)
        {
            string newId = HttpUtility.HtmlEncode(id);
            var call = callsDB.LoggedCalls.First(loggedCall => loggedCall.CallNo == newId);
            return View(call);
        }

    }
}
