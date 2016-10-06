using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using TrueTime.Models;

namespace TrueTime
{
    /// <summary>
    /// Models the time spent on a project for a specific consultant at a specific day
    /// </summary>
    public class UserTime : TableEntity
    {
        public AzureUser ProjectUser { get; set; }
        public Project CurrentProject { get; set; }
        public DateTime WorkDate { get; set; }
        public double TimeSpent { get; set; }
        public string TimeComment { get; set; }
        public bool Locked { get; set; }
    }
}