using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data.Entity;

namespace GreenvaleStaffPortal.Models
{
    public class StaffPortalEntities : DbContext
    {
        public DbSet<LoggedCall> LoggedCalls { get; set; }
    }
}