using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrueTime.Models
{
    public class User
    {
        public string Name { get; set; }
        public string Pwd { get; set; }
        public DateTime LastLogin { get; set; }
        public double AccumulatedHours { get; set; }
        public bool Deleted { get; set; }
        public int TypeOfUser { get; set; }
        public User()
        {
            TypeOfUser = (int)UserType.Consultant;
            AccumulatedHours = 0.0;
            LastLogin = new DateTime(1601, 01, 01);
            Deleted = false;
        }
        
        public void fromAzure(AzureUser a)
        {
            Name = a.RowKey;
            Pwd = a.Pwd;
            LastLogin = a.LastLogin;
            TypeOfUser = a.TypeOfUser;
            AccumulatedHours = a.AccumulatedHours;
            Deleted = a.Deleted;
        }

        public void toAzure(AzureUser a)
        {
            a.RowKey = Name;
            a.Pwd = Pwd;
            a.LastLogin = LastLogin;
            a.TypeOfUser = TypeOfUser;
            a.AccumulatedHours = AccumulatedHours;
            a.Deleted = Deleted;
        }
    }
}