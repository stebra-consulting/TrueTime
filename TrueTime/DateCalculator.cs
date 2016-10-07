using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueTime
{
    /// <summary>
    /// Class for calculating all Swedish holidays
    /// </summary>
    public class DateCalculator
    {
        //All holidays that are valid for Sweden
        List<DateTime> _holidays = new List<DateTime>();

        /// <summary>
        /// Property for accessing the holidays that have been stored
        /// </summary>
        public List<DateTime> Holidays
        {
            get
            {
                return _holidays;
            }
        }

        protected DateCalculator()
        {

        }

        /// <summary>
        /// Constructor that takes a year as a parameter, and calculates all Swedish holidays, storing them in the publicly accessible data structure
        /// </summary>
        public DateCalculator(int year)
        {
            CalculateHolidays(year);
        }

        /// <summary>
        /// Given a date, it returns a list of the enclosing Monday(1st element) and Sunday(2nd element)
        /// </summary>
        List<DateTime> GetDateRange(DateTime aDayOfTheWeek)
        {
            List<DateTime> dateRange = new List<DateTime>();
            DateTime testDate;

            //find Monday
            testDate = aDayOfTheWeek.Date;
            while (testDate.DayOfWeek != DayOfWeek.Monday)
                testDate = testDate.AddDays(-1);
            dateRange.Add(testDate);

            //find Sunday
            testDate = aDayOfTheWeek.Date;
            while (testDate.DayOfWeek != DayOfWeek.Sunday)
                testDate = testDate.AddDays(1);
            dateRange.Add(testDate);

            return dateRange;
        }

        /// <summary>
        /// Calculates all the holidays, and stores them in a publicly accessible data structure
        /// </summary>
        /// <param name="year">The year for which the data calculations should be performed</param>
        public void CalculateHolidays(int year)
        {
            _holidays.Clear(); //in case we've been called already

            //1 ALL SUNDAYS
            DateTime tester = new DateTime(year, 1, 1);

            //first day of the week may not start on a Sunday
            while (tester.DayOfWeek != DayOfWeek.Sunday)
                tester = tester.AddDays(1);
            AddHoliday(year, tester.Month, tester.Day);

            //add the remaining Sundays of the year
            tester = tester.AddDays(7);
            while (tester.Year == year)
            {
                AddHoliday(year, tester.Month, tester.Day);
                tester = tester.AddDays(7);
            }

            //2 FIXED DAYS
            AddHoliday(year, 1, 1);   //New Year's Day ("Nyårsdagen")
            AddHoliday(year, 1, 5);   //The Eve of Epiphany ("Trettondagsafton")
            AddHoliday(year, 1, 6);   //Epiphany ("Trettondedag jul")
            AddHoliday(year, 5, 1);   //First of May ("May Day")
            AddHoliday(year, 6, 6);   //Sweden's National Holiday ("Sveriges nationaldag)
            AddHoliday(year, 12, 24); //Christmas Eve ("Julafton")
            AddHoliday(year, 12, 25); //Christmas Day ("Juldagen")
            AddHoliday(year, 12, 26); //Boxing Day ("Annandag jul")
            AddHoliday(year, 12, 31); //New Year's Eve ("Nyårsafton")

            //3 NON-FIXED DAYS

            //Midsummer Day ("Midsommardagen")
            DateTime midsummerDay = CalculateMidsummerDay(year);
            AddHoliday(year, midsummerDay.Month, midsummerDay.Day);
            //Midsummer Eve ("Midsommarafton")
            DateTime midsummerEve = midsummerDay.AddDays(-1);
            AddHoliday(year, midsummerEve.Month, midsummerEve.Day);
            //All Saints Day ("Alla helgons dag")
            DateTime allSaintsDay = CalculateAllSaintsDay(year);
            AddHoliday(year, allSaintsDay.Month, allSaintsDay.Day);
            //lacking a better translation - "All Saints Eve" ("Allhelgonaafton")
            DateTime allSaintsEve = allSaintsDay.AddDays(-1);
            AddHoliday(year, allSaintsEve.Month, allSaintsEve.Day);

            //Easter ("Påsken")

            //Easter Sunday ("påskdagen")
            DateTime easterSunday = CalculateEasterDay(year);
            AddHoliday(year, easterSunday.Month, easterSunday.Day);
            //easter monday ("annandag påsk")
            DateTime easterMonday = easterSunday.AddDays(1);
            AddHoliday(year, easterMonday.Month, easterMonday.Day);
            //easter eve ("påskafton")
            DateTime easterEve = easterSunday.AddDays(-1);
            AddHoliday(year, easterEve.Month, easterEve.Day);
            //good friday ("långfredagen")
            DateTime goodFriday = easterSunday.AddDays(-2);
            AddHoliday(year, goodFriday.Month, goodFriday.Day);
            //the thursday before easter("skärtorsdagen")
            DateTime theThursdayBeforeEaster = easterSunday.AddDays(-3);
            AddHoliday(year, theThursdayBeforeEaster.Month, theThursdayBeforeEaster.Day);

            //Ascension Day ("kristi himmelfärdsdag")
            DateTime ascensionDay = CalculateAscensionDay(year);
            AddHoliday(year, ascensionDay.Month, ascensionDay.Day);

            //Whitsunday ("pingstdagen")
            DateTime whitsunDay = CalculateWhitsunday(year);
            AddHoliday(year, whitsunDay.Month, whitsunDay.Day);

            //Whitsun Eve ("pingstafton")
            DateTime whitsunEve = CalculateWhitsunEve(year);
            AddHoliday(year, whitsunEve.Month, whitsunEve.Day);
        }

        /// <summary>
        /// Returns the number of working hours, given a DateTime
        /// </summary>
        double GetWorkingHours(DateTime aDate)
        {
            return IsHoliday(aDate.Date) ? 0 : IsDayBeforeHoliday(aDate.Date) ? 5.0 : 8.0;
        }

        /// <summary>
        /// Answers true if the supplied date is a holiday, else false
        /// </summary>
        public bool IsHoliday(DateTime aDate)
        {
            return Holidays.Exists(d => d.Year == aDate.Year && d.Month == aDate.Month && d.Day == aDate.Day);
        }

        /// <summary>
        /// Answers true if the following day is a holiday, else false
        /// </summary>
        public bool IsDayBeforeHoliday(DateTime aDate)
        {
            return IsHoliday(aDate.Date.AddDays(1));
        }
        /// <summary>
        /// Answers true if the four digit parameter is a leap year else false
        /// </summary>
        public bool IsLeapYear(int year)
        {
            /*  (from en.wikipedia.org at 2016-09-23)
                The following pseudocode determines whether a year is a leap year or a common year 
                in the Gregorian calendar(and in the proleptic Gregorian calendar before 1582).
                The year variable being tested is the integer representing the number of the year in the Gregorian calendar, 
                and the tests are arranged to dispatch the most common cases first. 
                Care should be taken in translating mathematical integer divisibility into specific programming languages.

                if (year is not divisible by 4) then(it is a common year)
                else if (year is not divisible by 100) then(it is a leap year)
                else if (year is not divisible by 400) then(it is a common year)
                else (it is a leap year)

            */
            if ((year % 4) != 0)
                return false;
            else if ((year % 100) != 0)
                return true;
            else if ((year % 400) != 0)
                return false;
            else
                return true;
        }
        /// <summary>
        /// Helper function that adds a new clean DateTime object (only Date part is set) to the list of holidays
        /// </summary>
        void AddHoliday(int year, int month, int day)
        {
            DateTime holiday = new DateTime(year, month, day);

            if (!_holidays.Exists(d => year == d.Year && month == d.Month && day == d.Day)) //prevent storing multiple identical dates
                _holidays.Add(holiday);
        }

        #region Functions for calculation of non-fixed holidays
        /// <summary>
        /// Calculates the Midsummer day
        /// </summary>
        /// <remarks>The Midsummer day is the Saturday between the 20th and the 26th of June</remarks>
        DateTime CalculateMidsummerDay(int year)
        {
            DateTime testdate = new DateTime(year, 6, 20);
            DateTime maxdate = new DateTime(year, 6, 26);

            while (testdate.DayOfWeek != DayOfWeek.Saturday && testdate <= maxdate)
                testdate = testdate.AddDays(1);

            if (testdate > maxdate)
            {
                //signal an error
                return DateTime.MinValue;
            }
            else
            {
                return testdate;
            }
        }

        /// <summary>
        /// Calculates the All Saints Day ("Alla helgons dag")
        /// </summary>
        /// <remarks>The All Saints Day is the Saturday between the 30th of October and the 5th of November</remarks>
         DateTime CalculateAllSaintsDay(int year)
        {
            DateTime testdate = new DateTime(year, 10, 30);
            DateTime maxdate = new DateTime(year, 11, 5);

            while (testdate.DayOfWeek != DayOfWeek.Saturday && testdate <= maxdate)
                testdate = testdate.AddDays(1);

            if (testdate > maxdate)
            {
                //signal an error
                return DateTime.MinValue;
            }
            else
            {
                return testdate;
            }

        }

        /// <summary>
        /// Calculates the Easter Day ("Påskdagen")
        /// </summary>
         DateTime CalculateEasterDay(int year)
        {
            /*
                     Code snippet from English version of Wikipedia at September 16, 2016.
                     The URL is https://en.wikipedia.org/wiki/Computus
                     By examining the corresponding code for MS SQL, we find that \ is floor(), i.e. the lower integer part of the division of its operands.

                        BASIC – This is a version with the algorithm from Gauss, Zeller, Lichtenberg et al.:[54]

                    Function Easter(X)                                  ' X = year to compute
                        Dim K, M, S, A, D, R, OG, SZ, OE

                        K  = X \ 100                                    ' Secular number
                        M  = 15 + (3 * K + 3) \ 4 - (8 * K + 13) \ 25   ' Secular Moon shift
                        S  = 2 - (3 * K + 3) \ 4                        ' Secular sun shift
                        A  = X Mod 19                                   ' Moon parameter
                        D  = (19 * A + M) Mod 30                        ' Seed for 1st full Moon in spring
                        R  = D \ 29 + (D \ 28 - D \ 29) * (A \ 11)      ' Calendarian correction quantity
                        OG = 21 + D - R                                 ' Easter limit
                        SZ = 7 - (X + X \ 4 + S) Mod 7                  ' 1st sunday in March
                        OE = 7 - (OG - SZ) Mod 7                        ' Distance Easter sunday from Easter limit in days

                        Easter = DateSerial(X, 3, OG + OE)              ' Result: Easter sunday as number of days in March
                    End Function
            */
            int K, M, S, A, D, R, OG, SZ, OE;
            int month;
            int day;

            K = year / 100;                               // Secular number
            M = 15 + (3 * K + 3) / 4 - (8 * K + 13) / 25; // Secular Moon shift
            S = 2 - (3 * K + 3) / 4;                      // Secular sun shift
            A = year % 19;                                // Moon parameter
            D = (19 * A + M) % 30;                        // Seed for 1st full Moon in spring
            R = D / 29 + (D / 28 - D / 29) * (A / 11);     // Calendarian correction quantity
            OG = 21 + D - R;                              // Easter limit
            SZ = 7 - (year + year / 4 + S) % 7;           // 1st sunday in March
            OE = 7 - (OG - SZ) % 7;                       // Distance Easter sunday from Easter limit in days

            month = 3;
            day = OG + OE;
            if (day > 31)
            {
                day -= 31;
                month++;
            }
            return new DateTime(year, month, day);
        }

        /// <summary>
        /// Calculates the Ascension Day ("Kristi Himmelfärdsdag")
        /// </summary>
         DateTime CalculateAscensionDay(int year)
        {
            DateTime result = CalculateEasterDay(year);

            result = result.AddDays(4); //gives us the closest Thursday, since we know that the Easter Day is on a Sunday
            //now we can jump full weeks ahead
            for (int i = 0; i < 5; i++)
            {
                result = result.AddDays(7);
            }

            return result;
        }

        /// <summary>
        /// Calculates the Whitsunday ("Pingstdagen")
        /// </summary>
         DateTime CalculateWhitsunday(int year)
        {
            DateTime result = CalculateEasterDay(year);

            result = result.AddDays(7 * 7);

            return result;
        }

        /// <summary>
        /// Calculates Whitsun Eve ("Pingstafton") 
        /// </summary>
         DateTime CalculateWhitsunEve(int year)
        {
            DateTime result = CalculateWhitsunday(year);

            return result.AddDays(-1);
        }
        #endregion
    }
}
