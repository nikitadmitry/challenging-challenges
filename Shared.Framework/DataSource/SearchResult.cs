using System;
using System.Collections.Generic;
using System.Linq;

namespace Shared.Framework.DataSource
{
    /// <summary>
    /// Represents subset of data.
    /// </summary>
    /// <typeparam name="T">Data type for result.</typeparam>
    [Serializable]
    public class SearchResult<T>
    {
        /// <summary>
        /// Gets or sets page search result data.
        /// </summary>
        public IList<T> Items
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets total items in search result.
        /// </summary>
        public int Total
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the searh criteria text the was search for
        /// </summary>
        public string SearchText
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or Sets the search type represented as a string
        /// </summary>
        public string SearchType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or Sets the localaized search type represented as a string
        /// </summary>
        public string SearchTypeLocalized
        {
            get;
            set;
        }

        /// <summary>
        /// Builds new instance of <see cref="SearchResult{T}"/>.
        /// </summary>
        public static SearchResult<T> Build(params T[] list)
        {
            return Build((IEnumerable<T>)list);
        }

        /// <summary>
        /// Builds new instance of <see cref="SearchResult{T}"/>.
        /// </summary>
        public static SearchResult<T> Build(IEnumerable<T> entities, int? count = null)
        {
            List<T> list = ConvertToList(entities);

            return new SearchResult<T>
            {
                Items = list,
                Total = count ?? list.Count
            };
        }

        /// <summary>
        /// Returns empty search result.
        /// </summary>
        /// <returns>Search result.</returns>
        public static SearchResult<T> Empty()
        {
            return new SearchResult<T>
            {
                Items = new List<T>(0)
            };
        }

        private static List<T> ConvertToList(IEnumerable<T> entities)
        {
            if (entities == null)
            {
                entities = new List<T>(0);
            }
            return entities is List<T>
                ? (List<T>)entities
                : entities.ToList();
        }
    }
}