using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrueTime
{
    public enum ProjectType
    {
        CustomerProject = 1,
        InternalProject
    }
    /// <summary>
    /// Models a project that a consultant can participate in
    /// </summary>
    public class Project : EntityBase
    {
        public string Name { get; set; }
        public int TypeOfProject { get; set; }
        public bool Locked { get; set; }
        public bool Deleted { get; set; }
    }
}