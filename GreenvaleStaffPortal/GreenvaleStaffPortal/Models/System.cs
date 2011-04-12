using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreenvaleStaffPortal.Models
{
    public class System
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public int SolvedCount { get; set; }
        public int TodayCount { get; set; }

        public System(string code, string name, int count, int solvedCount, int todayCount)
        {
            this.Code = code;
            this.Name = name;
            this.Count = count;
            this.SolvedCount = solvedCount;
            this.TodayCount = todayCount;
        }
    }
}