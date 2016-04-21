using System.Collections.Generic;
using Data.Common.Query.Rules;

namespace Data.Common.Query.Settings
{
    /// <summary>
    /// Contains sorting settings.
    /// </summary>
    public class SortSettings
    {
        /// <summary>
        /// Gets or sets the sort rules.
        /// </summary>
        /// <value>
        /// The sort rules.
        /// </value>
        public IList<SortRule> SortRules
        {
            get;
            set;
        }

        /// <summary>
        /// Indicates if sort settings are empty.
        /// </summary>
        public bool AreEmpty()
        {
            return SortRules.IsNullOrEmpty();
        }
    }
}
