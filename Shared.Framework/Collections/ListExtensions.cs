using System;
using System.Collections.Generic;

namespace Shared.Framework.Collections
{
    /// <summary>
    /// Class which provides extension methods for IList members.
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        /// Run provided Action for each item in Ilist collection
        /// </summary>
        /// <typeparam name="T">Type of entity </typeparam>
        /// <param name="items">Ilist collection </param>
        /// <param name="action">Action to run on each item of collection</param>
        public static void ForEach<T>(this IList<T> items, Action<T> action)
        {
            foreach (T local in items)
            {
                action(local);
            }
        }

    }
}