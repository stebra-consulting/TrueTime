using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace TrueTime
{
    /// <summary>
    /// Holds the types of users the system distinguishes between
    /// </summary>
    public enum UserType
    {
        Consultant = 1,
        Administrator = 2
    }
    /// <summary>
    /// Models the various users in the system
    /// </summary>
    public class AzureUser : TableEntity
    {
        public string Pwd { get; set; }
        public DateTime LastLogin { get; set; }
        /// <summary>
        /// One of the integer values of the enum UserType above
        /// </summary>
        public int TypeOfUser { get; set; }
        public double AccumulatedHours { get; set; }
        public bool Deleted { get; set; }

        public void fromUser(Models.User u)
        {
            RowKey = u.Name;
            Pwd = u.Pwd;
            LastLogin = u.LastLogin;
            TypeOfUser = u.TypeOfUser;
            AccumulatedHours = u.AccumulatedHours;
            Deleted = u.Deleted;
        }
        public void toUser(Models.User u)
        {
            u.Name = RowKey;
            u.Pwd = Pwd;
            u.LastLogin = LastLogin;
            u.TypeOfUser = TypeOfUser;
            u.AccumulatedHours = AccumulatedHours;
            u.Deleted = Deleted;
        }
        
    }
}