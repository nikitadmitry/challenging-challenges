using System;

namespace Data.Common.Query.FilterExpressions
{
    /// <summary>
    /// Represents interval between two DateTime objects.
    /// </summary>
    public struct DateInterval
    {
        /// <summary>
        /// Gets or sets the start date of this instance of date interval.
        /// </summary>
        /// <value>
        /// The start date.
        /// </value>
        public DateTime StartDate
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the end date of this instance of date interval.
        /// </summary>
        /// <value>
        /// The end date.
        /// </value>
        public DateTime EndDate
        {
            get;
            set;
        }
    }
}