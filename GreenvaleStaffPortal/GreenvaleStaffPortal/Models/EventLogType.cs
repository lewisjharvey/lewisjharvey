using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace GreenvaleStaffPortal.Models
{
    [Table("Portal_EventLogType")]
    public class EventLogType
    {
        [Key]
        [Column(Name = "Id")]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}