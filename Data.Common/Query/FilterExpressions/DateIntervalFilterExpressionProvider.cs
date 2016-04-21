using System.Linq.Expressions;
using Shared.Framework.DataSource;

namespace Data.Common.Query.FilterExpressions
{
    /// <summary>
    /// Provides filter expressions for the values of the DateInterval type.
    /// </summary>
    public class DateIntervalFilterExpressionProvider : BaseFilterExpressionProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DateIntervalFilterExpressionProvider"/> class.
        /// </summary>
        public DateIntervalFilterExpressionProvider()
        {
            RegisterExpressions();
        }

        private void RegisterExpressions()
        {
            ExpressionCollection.Add(FilterOperator.IsEqualTo, (memberExpression, propertyInfo, value) =>
            {
                var interval = (DateInterval)value;

                var startConstant = GetConstantWithNullableCastHandle(memberExpression, propertyInfo, interval.StartDate);
                var endConstant = GetConstantWithNullableCastHandle(memberExpression, propertyInfo, interval.EndDate);

                Expression startExpression = Expression.GreaterThanOrEqual(memberExpression, startConstant);
                Expression endExpression = Expression.LessThan(memberExpression, endConstant);

                return Expression.AndAlso(startExpression, endExpression);
            });

            ExpressionCollection.Add(FilterOperator.IsNotEqualTo, (memberExpression, propertyInfo, value) =>
            {
                var interval = (DateInterval)value;

                var startConstant = GetConstantWithNullableCastHandle(memberExpression, propertyInfo, interval.StartDate);
                var endConstant = GetConstantWithNullableCastHandle(memberExpression, propertyInfo, interval.EndDate);

                Expression startExpression = Expression.LessThan(memberExpression, startConstant);
                Expression endExpression = Expression.GreaterThanOrEqual(memberExpression, endConstant);

                Expression compExpr = Expression.OrElse(startExpression, endExpression);
                compExpr = HandleNullComparison(compExpr, memberExpression, propertyInfo, value);

                return compExpr;
            });

            ExpressionCollection.Add(FilterOperator.IsGreaterThan, (memberExpression, propertyInfo, value) =>
            {
                var interval = (DateInterval)value;
                var endConstant = GetConstantWithNullableCastHandle(memberExpression, propertyInfo, interval.EndDate);
                return Expression.GreaterThanOrEqual(memberExpression, endConstant);
            });

            ExpressionCollection.Add(FilterOperator.IsGreaterThanOrEqualTo, (memberExpression, propertyInfo, value) =>
            {
                var interval = (DateInterval)value;
                var startConstant = GetConstantWithNullableCastHandle(memberExpression, propertyInfo, interval.StartDate);
                return Expression.GreaterThanOrEqual(memberExpression, startConstant);
            });

            ExpressionCollection.Add(FilterOperator.IsLessThan, (memberExpression, propertyInfo, value) =>
            {
                var interval = (DateInterval)value;
                var startConstant = GetConstantWithNullableCastHandle(memberExpression, propertyInfo, interval.StartDate);

                return Expression.LessThan(memberExpression, startConstant);
            });

            ExpressionCollection.Add(FilterOperator.IsLessThanOrEqualTo, (memberExpression, propertyInfo, value) =>
            {
                var interval = (DateInterval)value;
                var endConstant = GetConstantWithNullableCastHandle(memberExpression, propertyInfo, interval.EndDate);

                return Expression.LessThan(memberExpression, endConstant);
            });
        }
    }
}
