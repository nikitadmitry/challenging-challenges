using System.Collections.Generic;
using Shared.Framework.DataSource.Rules;

namespace Shared.Framework.DataSource.Settings
{
    /// <summary>
    /// Contains parameters to specify filtering conditions
    /// </summary>
    public class FilterSettings
    {
        /// <summary>
        /// Gets or sets the filter rules.
        /// </summary>
        /// <value>
        /// The filter rules.
        /// </value>
        public IList<FilterRule> Rules
        {
            get;
            set;
        }

        public bool HasFilterRules()
        {
            return !Rules.IsNullOrEmpty();
        }
    }
}