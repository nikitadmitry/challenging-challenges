using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using Shared.Framework.Collections;

// ReSharper disable once CheckNamespace
namespace System.Collections.Generic
{
    /// <summary>
    /// Provid3es extension methods for <see cref="IEnumerable{T}"/>
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Converts to collection of list items <see cref="ListItem"/>
        /// </summary>
        /// <typeparam name="T">Element type.</typeparam>
        /// <param name="collection">The source.</param>
        /// <param name="key">The key accessor.</param>
        /// <param name="text">The text accessor.</param>
        /// <returns></returns>
        public static IEnumerable<ListItem> ToItemsList<T>(this IEnumerable<T> collection, Func<T, string> text,
                                                           Func<T, string> key = null)
        {
            var keyAccessor = key ?? (x => null);
            return collection.Select(x => new ListItem(keyAccessor(x), text(x)));
        }

        /// <summary>
        /// Replaces the specified <paramref name="source"/> with the <paramref name="replacement"/>.
        /// </summary>
        /// <typeparam name="T">Element type.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="source">The source.</param>
        /// <param name="replacement">The replacement.</param>
        /// <returns>The collection with replaced element.</returns>
        public static IEnumerable<T> Replace<T>(this IEnumerable<T> collection, T source, T replacement)
        {
            IEnumerable<T> collectionWithout = collection;
            if (source != null)
            {
                collectionWithout = collectionWithout.Except(new[]
                    {
                        source
                    });
            }
            return collectionWithout.Union(new[]
                {
                    replacement
                });
        }

        /// <summary>
        /// Gets the index of the first element matching the predicate.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public static int FirstIndexOf<T>(this IList<T> list, Func<T, bool> predicate)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (predicate(list[i]))
                    return i;
            }
            return -1;
        }

        /// <summary>
        /// Replaces the first element matching the predicate.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list.</param>
        /// <param name="replacement">The replacement.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public static bool ReplaceFirst<T>(this IList<T> list, T replacement, Func<T, bool> predicate)
        {
            int index = list.FirstIndexOf(predicate);
            if (index >= 0)
            {
                list[index] = replacement;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Adds the range of elements to the collection.
        /// </summary>
        /// <typeparam name="T">Element type.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="items">The items to add.</param>
        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                collection.Add(item);
            }
        }

        /// <summary>
        /// Determines whether collection is null or empty.
        /// </summary>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection)
        {
            return collection == null || !collection.Any();
        }

        /// <summary>
        /// Splits enumerable into chunks
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="chunkSize"></param>
        /// <returns></returns>
        public static IEnumerable<IEnumerable<T>> Chunk<T>(
            this IEnumerable<T> list, int chunkSize)
        {
            int i = 0;
            var chunks = from name in list
                         group name by i++ / chunkSize into part
                         select part.AsEnumerable();
            return chunks;
        }

        /// <summary>
        /// Splits an array into specific number of smaller arrays.
        /// </summary>
        /// <typeparam name="T">The type of an array.</typeparam>
        /// <param name="array">The array to split.</param>
        /// <param name="countOfArrays">The number of smaller arrays the intital array will be splitted to.</param>
        /// <returns>An array containing smaller arrays.</returns>
        public static IEnumerable<IEnumerable<T>> Split<T>(this T[] array, int countOfArrays)
        {
            Contract.Assert(countOfArrays > 0, "Number of splitted arrays should be greater than zero.");

            var size = (int) Math.Ceiling((double) array.Length / countOfArrays);
            for (var i = 0; i < countOfArrays; i++)
            {
                yield return array.Skip(i * size).Take(size);
            }
        }

        /// <summary>
        /// If collection is null then returns empty, immutable collection otherwise returns the same collection.
        /// </summary>
        /// <typeparam name="T">Type of item inside collection.</typeparam>
        public static IEnumerable<T> NotNull<T>(this IEnumerable<T> collection)
        {
            return collection ?? new T[0];
        }

        /// <summary>
        /// Uses the Description attribute to access text to an enum object
        /// </summary>
        public static string Description(this Enum enumValue)
        {
            var enumType = enumValue.GetType();
            var field = enumType.GetField(enumValue.ToString());
            var attributes = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length == 0
                ? enumValue.ToString()
                : ((DescriptionAttribute)attributes[0]).Description;
        }

        public static void InsertEmpty<T>(this IEnumerable<T> collection, int position = 0, string text = " ",
            string key = "")
            where T : ListItem, new()
        {

            ((List<T>) collection).Insert(position, new T
            {
                Key = key,
                Text = text
            });
        }

        /// <summary>
        /// Concatenates sequences.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the input sequences.</typeparam>
        /// <param name="collection">The first sequence to concatenate.</param>
        /// <param name="arrays">The sequences to concatenate.</param>
        /// <returns>
        /// An <see cref="IEnumerable"/> that contains the concatenated elements of the input sequences.
        /// </returns>
        public static IEnumerable<T> Concat<T>(this IEnumerable<T> collection, params IEnumerable<T>[] arrays)
        {
            return arrays.Aggregate(collection, Enumerable.Concat);
        }

        /// <summary>
        /// Determines whether a sequence only contains specified element.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the input sequences.</typeparam>
        /// <param name="collection">The sequence to check.</param>
        /// <param name="value">Element to check whether persists in the sequence.</param>
        /// <returns></returns>
        public static bool ContainsOnly<T>(this IEnumerable<T> collection, T value)
        {
            return collection.ContainsOnly(value, null);
        }

        /// <summary>
        /// Determines whether a sequence only contains specified element.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the input sequences.</typeparam>
        /// <param name="collection">The sequence to check.</param>
        /// <param name="value">Element to check whether persists in the sequence.</param>
        /// <param name="comparer">An equality comparer to compare values.</param>
        /// <returns></returns>
        public static bool ContainsOnly<T>(this IEnumerable<T> collection, T value, IEqualityComparer<T> comparer)
        {
            var enumerable = collection as IList<T> ?? collection.ToList();
            return enumerable.Count() == 1
                && (comparer == null ? enumerable.Contains(value) : enumerable.Contains(value, comparer));
        }

        /// <summary>
        /// Determines whether a sequence only contains specified element.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the input sequences.</typeparam>
        /// <param name="collection">The sequence to check.</param>
        /// <param name="predicate">A function to test element for a condition.</param>
        public static bool ContainsOnly<T>(this IEnumerable<T> collection, Func<T, bool> predicate)
        {
            Contract.Requires<ArgumentNullException>(collection != null, "collection");

            var enumerable = collection as IList<T> ?? collection.ToList();
            return enumerable.Count() == 1 && enumerable.All(predicate);
        }

        public static bool MoreThanSingle<T>(this IEnumerable<T> collection)
        {
            return collection.Any() && collection.Skip(1).Any();
        }
    }
}