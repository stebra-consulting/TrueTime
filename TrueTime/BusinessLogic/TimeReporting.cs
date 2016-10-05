using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrueTime
{
    /// <summary>
    /// Class responsible for time reporting
    /// </summary>
    public class TimeReporting
    {
        /// <summary>
        /// Reports time for the specified consultant on the specified day.
        /// </summary>
        /// <returns>Note that there may only be one time reported per consultant, day and project</returns>
        public bool ReportTime(AzureUser theConsultant, DateTime aDay, double timeSpent, Project theProject)
        {
            return true;
        }
        /// <summary>
        /// Answers yes if there exists time reported for the specified consultant on the given day, else false
        /// </summary>
        public bool ExistsTimeReported(AzureUser theConsultant, DateTime aDay)
        {
            return false;
        }



    }
}