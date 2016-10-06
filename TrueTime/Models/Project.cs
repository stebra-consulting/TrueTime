using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrueTime.Models   
{
    public enum ProjectType
    {
        NotSet = 0,
        Internal = 1,
        External = 2
    }
    public class Project
    {
        public string Name { get; set; }
        public int TypeOfProject { get; set; }
        public bool Hidden { get; set; }

        public Project()
        {
            TypeOfProject = (int)ProjectType.NotSet;
            Hidden = false;
        }

        public void fromAzure(AzureProject a)
        {
            Name = a.RowKey;
            TypeOfProject = a.TypeOfProject;
            Hidden = a.Hidden;
        }

        public void toAzure(AzureProject a)
        {
            a.RowKey = Name;
            a.TypeOfProject = TypeOfProject;
            a.Hidden = Hidden;
        }
    }
}