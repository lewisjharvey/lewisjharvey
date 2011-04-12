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
        // GET: /Summary/
        public ActionResult Summary()
        {
            List<IEnumerable<LoggedCall>> loggedCallsList = new List<IEnumerable<LoggedCall>>();
            DateTime today = DateTime.Now.Date;
            var loggedCalls = callsDB.LoggedCalls.Where(loggedCall => (loggedCall.Solved == 0 && loggedCall.Is24Hour == 0)).ToList();
            loggedCallsList.Add(loggedCalls);
            var solvedCalls = callsDB.LoggedCalls.Where(loggedCall => (loggedCall.Solved == 1 && loggedCall.SolvedDate == today && loggedCall.Is24Hour == 0)).ToList();
            loggedCallsList.Add(solvedCalls);
            var todayCalls = callsDB.LoggedCalls.Where(loggedCall => (loggedCall.DateCalled == today && loggedCall.Is24Hour == 0)).ToList();
            loggedCallsList.Add(todayCalls);
            return View(loggedCallsList);
        }
        
        //
        // GET: /Calls/
        public ActionResult Index()
        {
            // Log the visit
            EventLog eventLog = new EventLog();
            eventLog.Time = DateTime.Now;
            eventLog.Type = callsDB.EventLogType.First(t => t.Name == "Helpdesk - Outstanding Calls - Visit");
            eventLog.Username = User.Identity.Name;
            callsDB.EventLog.Add(eventLog);
            callsDB.SaveChanges();

            var loggedCalls = callsDB.LoggedCalls.Where(loggedCall => loggedCall.Solved == 0 && loggedCall.Is24Hour == 0).ToList();
            return View(loggedCalls);
        }

        //
        // GET: /Calls/Details/5
        public ActionResult Details(string id)
        {
            // Log the visit
            EventLog eventLog = new EventLog();
            eventLog.Time = DateTime.Now;
            eventLog.Type = callsDB.EventLogType.First(t => t.Name == "Helpdesk - Call - Visit");
            eventLog.Username = User.Identity.Name;
            eventLog.Metadata = string.Format("CallNo:{0}", id);
            callsDB.EventLog.Add(eventLog);
            callsDB.SaveChanges();

            string newId = HttpUtility.HtmlEncode(id);
            var call = callsDB.LoggedCalls.First(loggedCall => loggedCall.CallNo == newId);
            return View(call);
        }

    }
}
