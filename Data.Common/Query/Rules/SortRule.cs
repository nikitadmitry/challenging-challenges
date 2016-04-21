using System;
using System.Linq.Expressions;
using Shared.Framework.DataSource;

namespace Data.Common.Query.Rules
{
    /// <summary>
    ///     Identify sort rules.
    /// </summary>
    [Serializable]
    public class SortRule
    {
        internal SortRule(SortOrder sortDirection, LambdaExpression sortExpression)
        {
            SortDirection = sortDirection;
            Expression = sortExpression;
        }

        /// <summary>
        ///     Gets or sets sort direction.
        /// </summary>
        /// <value>
        ///     The sort direction.
        /// </value>
        public SortOrder SortDirection
        {
            get;
            private set;
        }

        /// <summary>
        ///     Gets or sets the expression.
        /// </summary>
        /// <value>
        ///     The expression.
        /// </value>
        public LambdaExpression Expression
        {
            get;
            private set;
        }
    }
}