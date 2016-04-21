using System;

namespace Shared.Framework.DataSource.Rules
{
    /// <summary>
    /// Filter rules 
    /// </summary>
    [Serializable]
    public class FilterRule
    {
        /// <summary>
        /// Model property name
        /// </summary>
        public string PropertyName
        {
            get;
            set;
        }
        /// <summary>
        /// The type of model property
        /// </summary>
        public FilterOperator Operator
        {
            get;
            set;
        }
        /// <summary>
        /// Property value
        /// </summary>
        public object Value
        {
            get;
            set;
        }
    }
}
