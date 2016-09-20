using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrueTime
{
    /// <summary>
    /// Holds the various events that are logged in the audit trail
    /// </summary>
    public enum Activity
    {
        Login = 1,
        Logout
    }

    /// <summary>
    /// Holds the types of users the system distinguishes between
    /// </summary>
    public enum UserType
    {
        Consultant = 1,
        Administrator
    }

    public enum ProjectType
    {
        CustomerProject = 1,
        InternalProject
    }

    #region Entities
    /// <summary>
    /// All records have an integer ID; hence this base class
    /// </summary>
    public class ProjectBase
    {
        public int Id { get; set; }
    }

    /// <summary>
    /// Models a project that a consultant can participate in
    /// </summary>
    public class Project : ProjectBase
    {
        public string Name { get; set; }
        public int TypeOfProject { get; set; }
        public bool Locked { get; set; }
        public bool Deleted { get; set; }
    }

    /// <summary>
    /// Models the various users in the system
    /// </summary>
    public class User : ProjectBase
    {
        public string Name { get; set; }
        public string Pwd { get; set; }
        public string Salt { get; set; }
        public DateTime LastLogin { get; set; }
        public UserType TypeOfUser { get; set; }
        public double AccumulatedHours { get; set; }
        public bool Deleted { get; set; }
    }

    /// <summary>
    /// Models the time spent on a project for a specific consultant at a specific day
    /// </summary>
    public class UserTime : ProjectBase
    {
        public User ProjectUser { get; set; }
        public Project CurrentProject { get; set; }
        public DateTime WorkDate { get; set; }
        public double TimeSpent { get; set; }
        public string TimeComment { get; set; }
        public bool Locked { get; set; }
    }

    /// <summary>
    /// Models the audit trail of the system
    /// </summary>
    public class AuditTrail : ProjectBase
    {
        public User ProjectUser { get; set; }
        public Activity TheActivity { get; set; }
        public string Data1 { get; set; }
        public string Data2 { get; set; }
    }
    #endregion

    public class InformationAccess
    {
        public InformationAccess()
        {
        }

        List<Project> GetAllProjects(bool includeLocked = false, bool includeDeleted = false)
        {
            return null;
        }
        Project GetProject(string name)
        {
            return null;
        }

        List<User> GetAllUsers(UserType typeOfUser, bool includeDeleted = false)
        {
            return null;
        }
        User GetUser(string name)
        {
            return null;
        }

        

        //TODO: this function should not be here
        /// <summary>
        /// This submits a weekly timesheet to the system. 
        /// </summary>
        /// <returns></returns>
        bool SubmitWeeklyTimeSheet(User aConsultant, DateTime weekForSubmission)
        {
            return false;
        }
    }
}