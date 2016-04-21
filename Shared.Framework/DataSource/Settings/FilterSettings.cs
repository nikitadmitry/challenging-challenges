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

        /// <summary>
        /// Gets or sets the complex filter rules.
        /// </summary>
        /// <value>
        /// The complex filter rules.
        /// </value>
        public IList<IList<FilterRule>> ComplexRules
        {
            get; 
            set;
        }

        /// <summary>
        /// Gets a value indicating whether this instance contains complex rules.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance contains complex rules; otherwise, <c>false</c>.
        /// </value>
        public bool IsComplexRules
        {
            get
            {
                return !ComplexRules.IsNullOrEmpty();
            }
        }

        public bool HasFilterRules()
        {
            return !Rules.IsNullOrEmpty() || IsComplexRules;
        }


    }
}