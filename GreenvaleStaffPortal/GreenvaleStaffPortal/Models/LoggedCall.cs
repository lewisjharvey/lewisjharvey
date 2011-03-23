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
        public DateTime DateCalled { get; set; }
        public DateTime TimeCalled { get; set; }
        [Column(Name = "Solved?")]
        public byte Solved { get; set; }
        public DateTime CallTime
        {
            get
            {
                return Convert.ToDateTime(DateCalled.ToShortDateString() + " " + TimeCalled.ToLongTimeString());
            }
        }
    }
}