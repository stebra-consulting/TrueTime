using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrueTime
{
    public class DataCalculator
    {
        protected InformationAccess InformationAccessor { get; set; }

        public DataCalculator(InformationAccess i)
        {
            InformationAccessor = i;
        }
        /// <summary>
        /// Accesses the time spent on a project by year
        /// </summary>
        double GetTime(Project aProject, int aYear)
        {
            return 0.0;
        }
        /// <summary>
        /// Accesses the time spent on a project by year and month
        /// </summary>
        double GetTime(Project aProject, int aYear, int aMonth)
        {
            return 0.0;
        }
        /// <summary>
        /// Accesses the time spent on a project by year, month and a week range
        /// </summary>
        double GetTime(Project aProject, int aYear, int aMonth, DateTime aDateInAWeek)
        {
            return 0.0;
        }
        /// <summary>
        /// Accesses the time spent on a project by a consultant and year.
        /// </summary>
        /// <returns>
        /// First element is the number of hours spent on external customer projects,
        /// second element is the number of hours spent on internal projects
        /// </returns>
        List<double> GetTime(Project aProject, User aConsultant, int aYear)
        {
            return null;
        }
        /// <summary>
        /// Accesses the time spent on a project by a consultant, year and month
        /// </summary>
        /// <returns>
        /// First element is the number of hours spent on external customer projects,
        /// second element is the number of hours spent on internal projects
        /// </returns>
        List<double> GetTime(Project aProject, User aConsultant, int aYear, int aMonth)
        {
            return null;
        }
        /// <summary>
        /// Accesses the time spent on a project by a consultant, year, month and a week
        /// </summary>
        /// <returns>
        /// First element is the number of hours spent on external customer projects,
        /// second element is the number of hours spent on internal projects
        /// </returns>
        List<double> GetTime(Project aProject, User aConsultant, int aYear, int aMonth, DateTime aDateInAWeek)
        {
            return null;
        }
    }
}