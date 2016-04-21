using System;
using System.Linq.Expressions;
using Data.Common.Query.FilterExpressions;
using Shared.Framework.DataSource;

namespace Data.Common.Query.Rules
{
    /// <summary>
    /// Represenst the simple filter rule which will be used 
    /// during the ejecting entities from the DB.
    /// </summary>
    [Serializable]
    public class FilterRule : IFilterRule
    {
        internal FilterRule(LambdaExpression expression, FilterOperator filterOperator, object value)
        {
            Expression = expression;
            Operator = filterOperator;
            Value = value;
        }


        /// <summary>
        /// Gets the entity member selector expression.
        /// </summary>
        /// <value>
        /// The entity member selector expression.
        /// </value>
        public LambdaExpression Expression
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the filter operator.
        /// </summary>
        /// <value>
        /// The filter operator.
        /// </value>
        public FilterOperator Operator
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the filter value.
        /// </summary>
        /// <value>
        /// The filter value.
        /// </value>
        public object Value
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates the filter expression from this instance of filter rule.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public Expression<Func<T, bool>> GetExpression<T>()
        {
            var typeToCompare = Value != null ? Value.GetType() : typeof (object);
            var provider = FilterExpressionProviderFactory.GetProvider(typeToCompare, Operator);
            return provider.CreateFilterExpression<T>(Expression, Operator, Value);
        }
    }
}
