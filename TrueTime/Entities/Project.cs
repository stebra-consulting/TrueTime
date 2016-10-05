using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace TrueTime
{
    public enum ProjectType
    {
        CustomerProject = 1,
        InternalProject = 2
    }
    /// <summary>
    /// Models a project that a consultant can participate in
    /// </summary>
    public class Project : TableEntity
    {
        /// <summary>
        /// One of the integer values of the enum ProjectType above
        /// </summary>
        public int TypeOfProject { get; set; }
        public bool Hidden { get; set; }
        public bool Deleted { get; set; }
    }
}