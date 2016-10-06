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
    public class AzureProject : TableEntity
    {
        /// <summary>
        /// One of the integer values of the enum ProjectType above
        /// </summary>
        public int TypeOfProject { get; set; }
        public bool Hidden { get; set; }

        public AzureProject()
        {
            TypeOfProject = (int)ProjectType.NotSet;
            Hidden = true;
            RowKey = string.Empty;
        }

        public void fromProject(Project p)
        {
            RowKey = p.Name;
            TypeOfProject = p.TypeOfProject;
            Hidden = p.Hidden;
        }

        public void toProject(Project p)
        {
            p.Name = RowKey;
            p.TypeOfProject = TypeOfProject;
            p.Hidden = Hidden;
        }
    }
}