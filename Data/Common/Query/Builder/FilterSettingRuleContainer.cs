using System;
using System.Linq.Expressions;
using Shared.Framework.DataSource;

namespace Data.Common.Query.Builder
{
    /// <summary>
    /// Represents the container for the filter rule settings.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public class FilterSettingRuleContainer<TEntity>
    {
        /// <summary>
        /// Gets the member selector expression.
        /// </summary>
        /// <value>
        /// The member selector expression.
        /// </value>
        public LambdaExpression Expression { get; private set; }

        /// <summary>
        /// Gets the filter operator.
        /// </summary>
        /// <value>
        /// The filter operator.
        /// </value>
        public FilterOperator Operator { get; private set; }

        /// <summary>
        /// Gets the filter value.
        /// </summary>
        /// <value>
        /// The filter value.
        /// </value>
        public object Value { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FilterSettingRuleContainer{TEntity}"/> class.
        /// </summary>
        /// <param name="filterOperator">The filter operator.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="value">The value.</param>
        public FilterSettingRuleContainer(FilterOperator filterOperator, Expression<Func<TEntity, bool>> expression, object value)
        {
            Value = value;
            Expression = expression;
            Operator = filterOperator;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FilterSettingRuleContainer{TEntity}"/> class.
        /// </summary>
        /// <param name="filterOperator">The filter operator.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="value">The value.</param>
        public FilterSettingRuleContainer(FilterOperator filterOperator, Expression<Func<TEntity, object>> expression, object value)
        {
            Value = value;
            Expression = expression;
            Operator = filterOperator;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FilterSettingRuleContainer{TEntity}"/> class.
        /// </summary>
        /// <param name="filterOperator">The filter operator.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="value">The value.</param>
        public FilterSettingRuleContainer(FilterOperator filterOperator, LambdaExpression expression, object value)
        {
            Value = value;
            Expression = expression;
            Operator = filterOperator;
        }
    }
}
