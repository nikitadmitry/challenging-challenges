using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Shared.Framework.Utilities
{
    /// <summary>
    /// Provides utility methods for <see cref="System.DateTime"/>
    /// </summary>
    public static class DateTimeUtility
    {
        /// <summary>
        /// Returns smaller of the specified DateTime objects.
        /// </summary>
        /// <param name="dates">The dates.</param>
        /// <returns></returns>
        public static DateTime Min(params DateTime[] dates)
        {
            if (dates.IsNullOrEmpty())
            {
                throw new InvalidOperationException("Provided dates are invalid.");
            }
            return dates.Min();
        }

        /// <summary>
        /// Returns smaller of the specified DateTime objects.
        /// </summary>
        /// <param name="dates">The dates.</param>
        /// <returns></returns>
        public static DateTime Min(params DateTime?[] dates)
        {
            return Min(dates.Where(x => x.HasValue).Select(x => x.Value).ToArray());
        }

        /// <summary>
        /// Returns larger of the specified DateTime objects.
        /// </summary>
        /// <param name="dates">The dates.</param>
        /// <returns></returns>
        public static DateTime Max(params DateTime[] dates)
        {
            if (dates.IsNullOrEmpty())
            {
                throw new InvalidOperationException("Provided dates are invalid.");
            }
            return dates.Max();
        }

        /// <summary>
        /// Returns larger of the specified DateTime objects.
        /// </summary>
        /// <param name="dates">The dates.</param>
        /// <returns></returns>
        public static DateTime Max(params DateTime?[] dates)
        {
            return Max(dates.Where(x => x.HasValue).Select(x => x.Value).ToArray());
        }

        /// <summary>
        /// Returns last day of month of the specified date.
        /// </summary>
        /// <param name="date">The date.</param>
        public static DateTime LastDayOfMonth(DateTime date)
        {
            var daysInMonth = DateTime.DaysInMonth(date.Year, date.Month);
            return new DateTime(date.Year, date.Month, daysInMonth);
        }

        /// <summary>
        /// Returns last day of month of the specified date.
        /// </summary>
        /// <param name="date">The date.</param>
        public static DateTime? LastDayOfMonth(DateTime? date)
        {
            if (date == null)
            {
                return null;
            }
            return LastDayOfMonth(date.Value);
        }

        /// <summary>
        /// Returns first day of month of the specified date.
        /// </summary>
        /// <param name="date">The date.</param>
        public static DateTime FirstDayOfMonth(DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }

        /// <summary>
        /// Returns first day of month of the specified date.
        /// </summary>
        /// <param name="date">The date.</param>
        public static DateTime FirstDayOfMonth(DateTime? date)
        {
            Contract.Assert(date != null, "Provided date can't be null.");
            return FirstDayOfMonth(date.Value);
        }

        /// <summary>
        /// Determines whether the specified date is first day of month.
        /// </summary>
        /// <param name="date">The date.</param>
        public static bool IsFirstDayOfMonth(DateTime date)
        {
            return date.Day == 1;
        }

        /// <summary>
        /// Creates the date time with days in moths handling.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="month">The month.</param>
        /// <param name="day">The day.</param>
        /// <returns></returns>
        public static DateTime CreateDateTime(int year, int month, int day)
        {
            var actualDay = Math.Min(day, DateTime.DaysInMonth(year, month));
            return new DateTime(year, month, actualDay);
        }

        /// <summary>
        /// Checks wheter two dates are in the same month.
        /// </summary>
        /// <param name="firstDate">The first date.</param>
        /// <param name="secondDate">The second date.</param>
        public static bool SameMonth(DateTime firstDate, DateTime secondDate)
        {
            return firstDate.Year == secondDate.Year && firstDate.Month == secondDate.Month;
        }

        /// <summary>
        /// Returns difference between dates in months.
        /// </summary>
        /// <param name="startDate">Start date.</param>
        /// <param name="endDate">End date.</param>
        /// <returns>Difference in months.</returns>
        public static int DiffMonths(DateTime startDate, DateTime endDate)
        {
            var difference = MonthsInDate(endDate) - MonthsInDate(startDate);
            return  startDate.Date >= endDate.Date
                  ? 0
                  : difference + (startDate.Date.Day > endDate.Date.Day ? 0 : 1);
        }

        private static int MonthsInDate(DateTime date)
        {
            return date.Year * 12 + date.Month;
        }
    }
}