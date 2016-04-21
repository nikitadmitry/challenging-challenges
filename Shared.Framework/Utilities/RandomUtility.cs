using System;
using System.Text;

namespace Shared.Framework.Utilities
{
    /// <summary>
    /// Generates random simple data.
    /// </summary>
    public static class RandomUtility
    {
        private static readonly Random random = new Random();

        /// <summary>
        /// Generate random date.
        /// </summary>
        public static DateTime GetDateTime()
        {
            return new DateTime(2010, 1, 1).AddDays(GetInt(0, 1000));
        }

        /// <summary>
        /// Generates random enum value of specified type.
        /// </summary>
        /// <typeparam name="T">Type of enum.</typeparam>
        /// <returns></returns>
        public static T GetEnumValue<T>()
            where T : struct, IConvertible
        {
            Array values = Enum.GetValues(typeof(T));
            return (T)values.GetValue(GetInt(0, values.Length));
        }

        /// <summary>
        /// Generates random int number.
        /// </summary>
        public static int GetInt(int? minValue = null, int? maxValue = null)
        {
            return random.Next(minValue ?? int.MinValue, maxValue ?? int.MaxValue);
        }

        /// <summary>
        /// Returns a nonnegative random number less than the specified maximum.
        /// </summary>
        public static int NextInt(int maxValue)
        {
            return random.Next(maxValue);
        }

        /// <summary>
        /// Generates random byte number.
        /// </summary>
        public static byte GetByte(byte? minValue = null, byte? maxValue = null)
        {
            return (byte) random.Next(minValue ?? byte.MinValue, maxValue ?? byte.MaxValue);
        }       
        
        /// <summary>
        /// Generates random byte number.
        /// </summary>
        public static short GetShort(short? minValue = null, short? maxValue = null)
        {
            return (short) random.Next(minValue ?? short.MinValue, maxValue ?? short.MaxValue);
        }

        /// <summary>
        /// Generates random decimal number.
        /// </summary>
        public static decimal GetDecimal(short? minValue = null, short? maxValue = null)
        {
            var primePart = GetInt(minValue ?? 1, maxValue - 1);
            var decimalPart = (decimal)GetInt(1, 99) / 100;
            return primePart + decimalPart;
        }

        public static decimal GetRoundedDecimal(short? minValue = null, short? maxValue = null)
        {
            return GetDecimal(minValue, maxValue).Round();
        }

        public static decimal GetPercentageDecimal()
        {
            return GetDecimal(0, 100).Round();
        }


        public static decimal GetPositiveDecimalLessThenOne()
        {
           return (decimal) GetInt(1, 99) / 100;
        }

        static decimal NextDecimal(bool sign)
        {
            byte scale = (byte) GetInt(0, 29);
            return new decimal( GetInt(0),
                                GetInt(0),
                                GetInt(0),
                                sign,
                                scale);
        }

        static decimal NextNonNegativeDecimal()
        {
            return NextDecimal(false);
        }

        public static decimal NextDecimal(decimal maxValue)
        {
            return (NextNonNegativeDecimal() / Decimal.MaxValue) * maxValue;
        }

        public static decimal NextDecimal(decimal minValue, decimal maxValue)
        {
            if (minValue >= maxValue)
            {
                throw new InvalidOperationException();
            }
            decimal range = maxValue - minValue;
            return NextDecimal(range) + minValue;
        }

        public static double GetDouble()
        {
            return random.NextDouble();
        }

        /// <summary>
        /// Generates random boolean value.
        /// </summary>
        public static bool GetBool()
        {
            return GetInt(0, 2) == 0;
        }

        /// <summary>
        /// Generates list of random identifiers.
        /// </summary>
        /// <param name="count">Length of list.</param>
        public static Guid[] GetIds(int? count = null)
        {
            if (count <= 0)
            {
                return EmptyArray<Guid>.Instance;
            }

            var ids = new Guid[count ?? GetInt(2, 10)];
            for (int i = 0; i < ids.Length; i++)
            {
                ids[i] = Guid.NewGuid();
            }
            return ids;
        }

        /// <summary>
        /// Generates string.
        /// </summary>
        /// <param name="length">Length of required string.</param>
        public static string GetString(int? length = null)
        {
            var initial = Guid.NewGuid().ToString();
            return length.GetValueOrDefault() <= 0
                ? initial
                // ReSharper disable once PossibleInvalidOperationException
                : GetStringOfRequiredLength(initial, length.Value);
        }

        private static string GetStringOfRequiredLength(string initial, int length)
        {
            var builder = new StringBuilder(initial);

            while (builder.Length < length)
            {
                builder.Append(Guid.NewGuid());
            }

            return builder.ToString().Substring(0, length);
        }

        /// <summary>
        /// Gets the random unique identifier.
        /// </summary>
        public static Guid GetGuid()
        {
            return Guid.NewGuid();
        }
    }
}