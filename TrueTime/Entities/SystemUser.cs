using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrueTime
{
    /// <summary>
    /// Holds the types of users the system distinguishes between
    /// </summary>
    public enum UserType
    {
        Consultant = 1,
        Administrator
    }
    /// <summary>
    /// Models the various users in the system
    /// </summary>
    public class SystemUser : EntityBase
    {
        public string Name { get; set; }
        public string Pwd { get; set; }
        public string Salt { get; set; }
        public DateTime LastLogin { get; set; }
        public UserType TypeOfUser { get; set; }
        public double AccumulatedHours { get; set; }
        public bool Deleted { get; set; }
    }
}