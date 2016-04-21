using System.Runtime.Serialization;

namespace Shared.Framework.DataSource
{
    /// <summary>
    /// Specifies an order in which to return the search results.
    /// </summary>
    public enum SortOrder
    {
        /// <summary>
        /// Ascending sorting.
        /// </summary>
        [EnumMember(Value = "asc")]
        Asc = 0,

        /// <summary>
        /// Descending sorting.
        /// </summary>
        [EnumMember(Value = "desc")]
        Desc = 1
    }
}