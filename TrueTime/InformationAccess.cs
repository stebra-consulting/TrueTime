using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrueTime
{
    /// <summary>
    /// Class that acts as an abstraction layer between the storage and the application logic
    /// </summary>
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

        List<SystemUser> GetAllUsers(UserType typeOfUser, bool includeDeleted = false)
        {
            return null;
        }
        SystemUser GetUser(string name)
        {
            return null;
        }
        bool UpdateUser(SystemUser user)
        {
            return false;
        }
        bool DeleteUser(SystemUser user)
        {
            return false;
        }
        bool CreateUser(SystemUser user)
        {
            return false;
        }

        //TODO: this function should not be here
        /// <summary>
        /// This submits a weekly timesheet to the system. 
        /// </summary>
        /// <returns></returns>
        bool SubmitWeeklyTimeSheet(SystemUser aConsultant, DateTime weekForSubmission)
        {
            return false;
        }
    }
}