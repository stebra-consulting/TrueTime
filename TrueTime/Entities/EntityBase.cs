using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrueTime
{
    /// <summary>
    /// All records in Azure tables have these columns, hence this base class
    /// </summary>
    public class EntityBase
    {
        public string PartitionKey
        {
            get
            {
                return "stebratruetime";
            }
            set
            {

            }
        }
        public string RowKey { get; set; }
        public DateTime Timestamp { get; set; }
    }
}