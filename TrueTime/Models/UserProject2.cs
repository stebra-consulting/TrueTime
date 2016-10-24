using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrueTime.Models
{
    public class UserProject2
    {
        public List<AzureUserProject> ConsultantProjects { get; set; }
        public List<AzureProject> AllProjects { get; set; }
        public string SelectedProject { get; set; }
        public UserProject2()
        {
            ConsultantProjects = new List<AzureUserProject>();
            AllProjects = new List<AzureProject>();
            SelectedProject = string.Empty;
        }
    }
}