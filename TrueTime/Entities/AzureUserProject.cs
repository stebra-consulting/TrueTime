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
    /// Models a project that a consultant can participate in
    /// </summary>
    public class AzureUserProject : TableEntity
    {
        public bool Deleted { get; set; }

        public AzureUserProject()
        {
            PartitionKey = string.Empty;
            RowKey = string.Empty;
            Deleted = false;
        }

        public void fromUserProject(UserProject p)
        {
            PartitionKey = p.PartitionKey;
            RowKey = p.RowKey;
            Deleted = p.Deleted;
        }

        public void toProject(UserProject p)
        {
            p.PartitionKey = PartitionKey;
            p.RowKey = RowKey;
            p.Deleted = Deleted;
        }
    }
}