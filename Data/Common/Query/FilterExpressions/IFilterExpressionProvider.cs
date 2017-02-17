using System;
using System.Linq.Expressions;
using Shared.Framework.DataSource;

namespace Data.Common.Query.FilterExpressions
{
    /// <summary>
    /// Describes the common interface of the filter expression provider.
    /// </summary>
    public interface IFilterExpressionProvider
    {
        /// <summary>
        /// Creates the part of the full filter expression which can be joined with other parts by logic operator.
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="property">The property.</param>
        /// <param name="filterOperator">The filter operator.</param>
        /// <param name="value">The value.</param>
        Expression CreateConditionExpression(ParameterExpression argument, LambdaExpression property,
            FilterOperator filterOperator, object value);

        /// <summary>
        /// Creates the filter expression from the specified member expression, comparison operator and filter value.
        /// </summary>
        /// <typeparam name="T">Type of entity.</typeparam>
        /// <param name="memberExpression">The member expression.</param>
        /// <param name="filterOperator">The filter operator.</param>
        /// <param name="value">The filter value.</param>
        Expression<Func<T, bool>> CreateFilterExpression<T>(LambdaExpression memberExpression, FilterOperator filterOperator,
            object value);

        /// <summary>
        /// Determines whether this filter expression provider 
        /// contains filter expression for the specified filter operator.
        /// </summary>
        /// <param name="filterOperator">The filter operator.</param>
        /// <returns>
        ///   <c>true</c> if this filter expression provider contains 
        /// filter expression for the specified filter operator; 
        /// otherwise, <c>false</c>.
        /// </returns>
        bool ContainsFilterExpressionFor(FilterOperator filterOperator);
    }
}
