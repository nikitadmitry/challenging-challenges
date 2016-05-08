using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Shared.Framework.DataSource.Rules;
using Shared.Framework.DataSource.Settings;

namespace Shared.Framework.DataSource
{
    [DataContract]
    public class ListQueryParameters
    {
        private FilterSettings filterSettings;
        private SortSettings sortSettings;

        /// <summary>
        /// Gets or sets the ids.
        /// </summary>
        /// <value>
        /// The ids.
        /// </value>
        [DataMember]
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
        [DataMember]
        public SortSettings SortSettings
        {
            get
            {
                return sortSettings ?? (sortSettings = new SortSettings
                    {
                        Rules = new List<SortRule>()
                    });
            }
            set
            {
                sortSettings = value;
            }
        }

        /// <summary>
        /// Gets or sets the filter settings
        /// </summary>
        [DataMember]
        public FilterSettings FilterSettings
        {
            get
            {
                return filterSettings ?? (filterSettings = new FilterSettings
                    {
                        Rules = new List<FilterRule>()
                    });
            }
            set
            {
                filterSettings = value;
            }
        }

        /// <summary>
        /// Gets or sets the page rule.
        /// </summary>
        /// <value>
        /// The page rule.
        /// </value>
        [DataMember]
        public PageRule PageRule
        {
            get;
            set;
        }
    }
}