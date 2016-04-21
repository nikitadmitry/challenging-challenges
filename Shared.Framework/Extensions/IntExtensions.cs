using System;
using System.Collections.Generic;
using System.Linq;

namespace Shared.Framework.Extensions
{
    public static class IntExtensions
    {
        /// <summary>
        /// Return null if an integer is empty, otherwsise the integer itself.
        /// </summary>
        /// <param name="input">The integer to test.</param>
        /// <returns>Null if the integer is zero, otherwise the integer itself.</returns>
        public static int? NullIfZero(this int input)
        {
            return (input == 0) ? (int?)null : input;
        }

        public static List<T> CreateList<T>(this int times, Func<T> createObject)
        {
            return Enumerable.Range(0, times).Select(counter => createObject()).ToList();
        }
    }
}