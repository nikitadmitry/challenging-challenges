using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Data.Common.Query.FilterExpressions;

namespace Data.Common.Query.Rules
{
    /// <summary>
    /// Represents complex filter rule and contains batch of the filter rules 
    /// which will be joined using OR operator.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public class ComplexFilterRule<TEntity> : IFilterRule
    {
        private readonly ParameterExpression argument;

        private readonly Expression filterBody;

        /// <summary>
        /// Initializes a new instance of the <see cref="ComplexFilterRule{TEntity}"/> class.
        /// </summary>
        /// <param name="rules">The rules to join using OR operator.</param>
        internal ComplexFilterRule(IEnumerable<FilterRule> rules)
        {
            argument = Expression.Parameter(typeof(TEntity), "x");

            foreach (var filterRule in rules)
            {
                var typeToCompare = filterRule.Value != null ? filterRule.Value.GetType() : typeof(object);
                var provider = FilterExpressionProviderFactory.GetProvider(typeToCompare, filterRule.Operator);
                var condition = provider.CreateConditionExpression(argument, filterRule.Expression, filterRule.Operator, filterRule.Value);

                filterBody = filterBody != null 
                    ? Expression.OrElse(condition, filterBody) 
                    : condition;
            }
        }

        /// <summary>
        /// Creates the filter expression from this instance of filter rule.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public Expression<Func<T, bool>> GetExpression<T>()
        {
            return Expression.Lambda<Func<T, bool>>(filterBody, argument);
        }
    }
}
