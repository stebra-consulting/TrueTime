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
    public class AzureUserTime : TableEntity
    {
        public string UserRowKey { get; set; }
        public string ProjectRowKey { get; set; }
        public DateTime WorkDate { get; set; }
        public double TimeSpent { get; set; }
        public string TimeComment { get; set; }
        public bool Locked { get; set; }
        public bool Deleted { get; set; }
    }
}