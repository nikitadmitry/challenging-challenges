using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Shared.Framework.DataSource.Rules
{
    /// <summary>
    /// Identify sort rules.
    /// Expression has higher priority then SortPropertyName
    /// </summary>
    [Serializable]
    public class SortRule
    {
        /// <summary>
        /// Gets or sets sort direction.
        /// </summary>
        /// <value>
        /// The sort direction.
        /// </value>
        [JsonProperty(PropertyName = "dir")]
        [JsonConverter(typeof(StringEnumConverter))]
        public SortOrder SortDirection
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets sort property to use.
        /// Use Expression property instead of SortPropertyName
        /// </summary>
        [JsonProperty(PropertyName = "field")]
        public String SortPropertyName
        {
            get;
            set;
        }
    }
}