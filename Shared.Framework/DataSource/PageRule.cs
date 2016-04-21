using System;

namespace Shared.Framework.DataSource
{
    /// <summary>
    /// Paging rule including start item index and amount of items per page.
    /// </summary>
    [Serializable]
    public class PageRule
    {
        /// <summary>
        /// Gets or sets start item index.
        /// </summary>
        public int Start
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets items count per page.
        /// </summary>
        public int Count
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a value indicating whether there are valid parameters.
        /// </summary>
        public bool IsValid
        {
            get
            {
                return Start >= 0 && Count > 0;
            }
        }
    }
}