using System.Collections.Generic;
using Data.Common.Query.Rules;

namespace Data.Common.Query.Settings
{
    public class FilterSettings
    {
        /// <summary>
        /// Gets or sets the sort rules.
        /// </summary>
        /// <value>
        /// The sort rules.
        /// </value>
        public IList<IFilterRule> Rules
        {
            get;
            set;
        }
    }
}