using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace GreenvaleStaffPortal.Models
{
    [Table("Portal_EventLog")]
    public class EventLog
    {
        [Key]
        public int Id { get; set; }
        public string Username { get; set; }
        public DateTime Time { get; set; }
        [Column(Name = "EventLogTypeId")]
        public EventLogType Type { get; set; }
        public string Metadata { get; set; }
    }
}