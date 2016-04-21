using System.Collections.Generic;
using Shared.Framework.DataSource.Rules;

namespace Shared.Framework.DataSource.Settings
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
        public IList<SortRule> Rules
        {
            get;
            set;
        }
    }
}
