using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace GreenvaleStaffPortal.Models
{
    public class LookupCode
    {
        [Key]
        [Column(Order=0)]
        public string RecType { get; set; }
        [Key]
        [Column(Order = 1)]
        public string System { get; set; }
        [Key]
        [Column(Order = 2)]
        public string SubSystem { get; set; }
        [Key]
        [Column(Order = 3)]
        public string Problem { get; set; }
        public string Description { get; set; }
        [Column(Name = "Telephone No_")]
        public string TelephoneNo { get; set; }
    }
}