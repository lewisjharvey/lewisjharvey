using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace GreenvaleStaffPortal.Models
{
    public class LoggedCall
    {
        [Key]
        public string CallNo { get; set; }
        public string CallNotes { get; set; }
        public string Caller { get; set; }
        public string Company { get; set; }
        public string System { get; set; }
        public string SubSystem { get; set; }
        public string Problem { get; set; }
        public DateTime DateCalled { get; set; }
        public DateTime TimeCalled { get; set; }
        public DateTime? SolvedDate { get; set; }
        [Column(Name = "Solved?")]
        public byte Solved { get; set; }
        [Column(Name = "PassedTo")]
        public string Owner { get; set; }
        [Column(Name = "24Hr Support Call")]
        public byte Is24Hour { get; set; }
        public DateTime CallTime
        {
            get
            {
                return Convert.ToDateTime(DateCalled.ToShortDateString() + " " + TimeCalled.ToLongTimeString());
            }
        }

        StaffPortalEntities callsDB = new StaffPortalEntities();
        public string CallerDescription
        {
            get
            {
                return callsDB.LookupCodes.First(code => code.RecType == "CALLER" && code.SubSystem == Caller).Description;
            }
        }
        public string OwnerDescription
        {
            get
            {
                LookupCode lookupCode = callsDB.LookupCodes.First(code => code.RecType == "STAFF" && code.System == Owner);
                if (lookupCode != null)
                    return string.Format("{0}", lookupCode.Description);
                else
                    return "";
            }
        }
        public string SiteDescription
        {
            get
            {
                return callsDB.LookupCodes.First(code => code.RecType == "COMPANY" && code.System == Company).Description;
            }
        }
        public string SystemDescription
        {
            get
            {
                return callsDB.LookupCodes.First(code => code.RecType == "SYSTEM" && code.System == System).Description;
            }
        }
        public string SubSystemDescription
        {
            get
            {
                return callsDB.LookupCodes.First(code => code.RecType == "SUBSYSTEM" && code.SubSystem == SubSystem).Description;
            }
        }
        public string ProblemDescription
        {
            get
            {
                return callsDB.LookupCodes.First(code => code.RecType == "PROBLEM" && code.Problem == Problem).Description;
            }
        }
    }
}