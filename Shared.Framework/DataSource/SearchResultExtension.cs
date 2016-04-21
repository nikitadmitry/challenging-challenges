using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.Framework.DataSource
{
    /// <summary>
    /// Extension methods for <see cref="SearchResult{T}"/>.
    /// </summary>
    public static class SearchResultExtension
    {
        /// <summary>
        /// Get the total from the search results, or 0 if there are none.
        /// </summary>
        /// <param name="searchResult">The results to get the total from.</param>
        /// <returns>The number of items, or 0 if there are none.</returns>
        public static int SafeTotal<T>(this SearchResult<T> searchResult )
        {
            return searchResult.IsNullOrEmpty() ? 0 : searchResult.Total;
        }

        /// <summary>
        /// Get the items from the search result.
        /// </summary>
        /// <param name="searchResult">The results to get the items from.</param>
        /// <returns>A valid enumeration of items, even if search result is null or empty.</returns>
        public static IEnumerable<T> SafeItems<T>(this SearchResult<T> searchResult)
        {
            return searchResult.IsNullOrEmpty() ? new T[0] : searchResult.Items;
        }

        /// <summary>
        /// Returns value indicating whether <paramref name="searchResult"/> is null
        /// or it's <see cref="SearchResult{T}.Items"/> is null or <see cref="SearchResult{T}.Items"/> is empty collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="searchResult"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<T>(this SearchResult<T> searchResult)
        {
            return searchResult == null || searchResult.Items == null || !searchResult.Items.Any();
        }

        /// <summary>
        /// Converts <see cref="SearchResult{TSource}"/> to <see cref="SearchResult{TDestination}"/>.
        /// For items convertion uses <paramref name="convert"/> function.
        /// </summary>
        /// <typeparam name="TSource">Type to convert from.</typeparam>
        /// <typeparam name="TDestination">Type to convert to.</typeparam>
        /// <param name="searchResults">Source results.</param>
        /// <param name="convert">Convertion function.</param>
        /// <returns>New instance of <see cref="SearchResult{TSource}"/> which <see cref="SearchResult{T}.Total"/>
        /// is equal to Total property of <paramref name="searchResults"/> and <see cref="SearchResult{T}.Items"/>
        /// contains items converted from Items specified by <paramref name="searchResults"/>.
        /// Returns null if <paramref name="searchResults"/> is null.</returns>
        public static SearchResult<TDestination> ConvertTo<TSource, TDestination>(
            this SearchResult<TSource> searchResults, Func<TSource, TDestination> convert)
        {
            if (searchResults == null)
            {
                return null;
            }

            var items = searchResults.Items != null
                ? searchResults.Items.Select(convert).ToList()
                : null;
            
            return new SearchResult<TDestination>
            {
                SearchText = searchResults.SearchText,
                SearchType = searchResults.SearchType,
                SearchTypeLocalized = searchResults.SearchTypeLocalized,
                Total = searchResults.Total,
                Items = items
            };
        }

        public static async Task<SearchResult<TDestination>> ConvertToAsync<TSource, TDestination>(
            this SearchResult<TSource> searchResults, Func<TSource, Task<TDestination>> convert)
        {
            if (searchResults == null)
            {
                return null;
            }

            IEnumerable<Task<TDestination>> items = searchResults.Items != null
                ? searchResults.Items.Select(async item => await convert(item))
                : null;

            List<TDestination> list = null;
            if (items != null)
            {
                list = new List<TDestination>();
                foreach (var item in items)
                {
                    list.Add( await item );   
                }
            }

            return new SearchResult<TDestination>
            {
                Total = searchResults.Total,
                Items = list
            };
        }
    }
}