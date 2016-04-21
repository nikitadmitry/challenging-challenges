using System.Globalization;

// ReSharper disable once CheckNamespace
namespace System
{
    /// <summary>
    /// Provides methods which perform conversion of different values (like currency) to string.
    /// </summary>
    public static class StringValueFormatter
    {
        private static readonly CultureInfo CultureInfo = CultureInfo.GetCultureInfo("en-US");

        /// <summary>
        /// Converts decimal to currency string.
        /// </summary>
        public static string FormatCurrency(this decimal? value, string format = "{0:c2}")
        {
            // Now we use "en-US" in order to ensure we have same formatting at business and presentation layers
            // Later we must specify culture from configuration
            return string.Format(CultureInfo, format, value);
        }
        
        /// <summary>
        /// Converts integer to currency string.
        /// </summary>
        public static string FormatCurrency(this int value, string format = "{0:c2}")
        {
            return FormatCurrency((decimal?) value, format);
        }
        
        /// <summary>
        /// Converts decimal to currency string
        /// </summary>
        /// <param name="value"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string FormatCurrency(this decimal value, string format = "{0:c2}")
        {
            return FormatCurrency((decimal?)value, format);
        }

        /// <summary>
        /// Converts double to string.
        /// </summary>
        public static string FormatDouble(this double value, string format)
        {
            return string.Format(CultureInfo, format, value);
        }

        /// <summary>
        /// Converts decimal to string.
        /// </summary>
        public static string FormatDecimal(this decimal value, string format)
        {
            return string.Format(CultureInfo, format, value);
        }

        /// <summary>
        /// Converts double to percentage string.
        /// Returns "-" if value does not exist.
        /// </summary>
        public static string FormatPercent(this double value)
        {
            return string.Format(CultureInfo, "{0:p}", value);
        }

        /// <summary>
        /// Converts month to text
        /// </summary>
        /// <param name="month">Number of month</param>
        /// <returns>Month</returns>
        public static string FormatMonth(this byte? month)
        {
            if (month.HasValue && month.Value > 0 && month.Value < 13)
            {
                return CultureInfo.DateTimeFormat.GetMonthName(month.Value);
            }

            return string.Empty;
        }

        public static string FormatDateTime(this DateTime? dateTime, string format = "MM/dd/yyyy h:mm tt")
        {
            if (!dateTime.HasValue)
                return string.Empty;

            return dateTime.Value.ToString(format, CultureInfo);
        }

        public static string FormatDateTime(this DateTime dateTime, string format = "MM/dd/yyyy h:mm tt")
        {
           return dateTime.ToString(format, CultureInfo);
        }

        public static string FormatDate(this DateTime? date, string format = "MM/dd/yyyy")
        {
            if (!date.HasValue)
                return null;

            return date.Value.FormatDate(format);
        }

        public static string FormatDate(this DateTime date, string format = "MM/dd/yyyy")
        {
            return date.ToString(format, CultureInfo);
        }

        /// <summary>
        /// Converts given <paramref name="boolean"/> boolean value.
        /// </summary>
        /// <param name="boolean">Boolean value to convert.</param>
        /// <param name="nullCase">String to use when provided boolean is null.</param>
        /// <returns>Converted string.</returns>
        public static string FormatBoolean(this bool? boolean, string nullCase = " ")
        {
            if (boolean.HasValue)
            {
                return boolean.Value ? "Y" : "N";
            }
            return nullCase;
        }
    }
}