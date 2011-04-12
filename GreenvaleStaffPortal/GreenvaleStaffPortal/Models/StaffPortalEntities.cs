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
        public DbSet<LookupCode> LookupCodes { get; set; }
        public DbSet<EventLog> EventLog { get; set; }
        public DbSet<EventLogType> EventLogType { get; set; }
    }
}