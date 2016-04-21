using System;

namespace Shared.Framework.Extensions
{
    public static class NumberToBoolExtensions
    {
        /// <summary>
        /// Extension for int. Converts int number to bool.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool ToBool(this int value)
        {
            return Convert.ToBoolean(value);
        }


        /// <summary>
        /// Extension for byte. Converts int number to bool.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool ToBool(this byte value)
        {
            return Convert.ToBoolean(value);
        }

        /// <summary>
        /// Extension for bool. Converts bool value to int.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ToInt(this bool value)
        {
            return Convert.ToInt32(value);
        }

        /// <summary>
        /// Extension for bool. Converts bool value to byte.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte ToByte(this bool value)
        {
            return Convert.ToByte(value);
        }
    }
}
