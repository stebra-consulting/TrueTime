using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrueTime.Models
{
    public class UserProject
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTime Timestamp { get; set; }

        public bool Deleted { get; set; }
        
        public void fromAzure(AzureUserProject a)
        {
            PartitionKey = a.PartitionKey;
            RowKey = a.RowKey;
            Timestamp = a.Timestamp.DateTime;
            Deleted = a.Deleted;
        }

        public void toAzure(AzureUserProject a)
        {
            a.PartitionKey = PartitionKey;
            a.RowKey = RowKey;
            a.Timestamp = new DateTimeOffset(Timestamp);
            a.Deleted = Deleted;
        }
    }
}