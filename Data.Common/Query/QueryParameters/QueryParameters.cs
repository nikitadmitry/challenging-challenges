using System;
using System.Collections.Generic;
using Data.Common.Query.Settings;
using Shared.Framework.DataSource;

namespace Data.Common.Query.QueryParameters
{
    /// <summary>
    /// Query parameters class.
    /// </summary>
    [Serializable]
    public class QueryParameters
    {
        public QueryParameters()
        {
            TrackChanges = true;
        }

        /// <summary>
        /// Gets or sets the ids.
        /// </summary>
        /// <value>
        /// The ids.
        /// </value>
        public IEnumerable<Guid> Ids
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the sort settings.
        /// </summary>
        /// <value>
        /// The sort settings.
        /// </value>
        public SortSettings SortSettings
        {
            get;
            set;
        }

        /// <summary>
        /// Represents filter rules to be applied in repository
        /// </summary>
        public FilterSettings FilterSettings
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the page rule.
        /// </summary>
        /// <value>
        /// The page rule.
        /// </value>
        public PageRule PageRule
        {
            get;
            set;
        }

        public string LanguageCode
        {
            get;
            set;
        }

        public static QueryParameters Empty
        {
            get
            {
                return new QueryParameters();
            }
        }

        /// <summary>
        /// Specifies if change tracking for entities should be enabled
        /// </summary>
        public bool TrackChanges
        {
            get;
            set;
        }
    }
}