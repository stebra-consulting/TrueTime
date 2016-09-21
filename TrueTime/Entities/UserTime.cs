using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrueTime
{
    /// <summary>
    /// Models the time spent on a project for a specific consultant at a specific day
    /// </summary>
    public class UserTime : EntityBase
    {
        public SystemUser ProjectUser { get; set; }
        public Project CurrentProject { get; set; }
        public DateTime WorkDate { get; set; }
        public double TimeSpent { get; set; }
        public string TimeComment { get; set; }
        public bool Locked { get; set; }
    }
}