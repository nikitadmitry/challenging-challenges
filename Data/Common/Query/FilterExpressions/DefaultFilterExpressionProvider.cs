using System;
using System.Linq.Expressions;
using System.Reflection;
using Data.Common.FullTextSearch;
using Shared.Framework.DataSource;

namespace Data.Common.Query.FilterExpressions
{
    /// <summary>
    /// Provides default filtering expressions
    /// </summary>
    public class DefaultFilterExpressionProvider : BaseFilterExpressionProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultFilterExpressionProvider"/> class.
        /// </summary>
        public DefaultFilterExpressionProvider()
        {
            RegisterExpressions();
        }

        private void RegisterExpressions()
        {
            ExpressionCollection.Add(FilterOperator.Contains, (memberExpression, propertyInfo, value) =>
            {
                var constant = GetConstantWithNullableCastHandle(memberExpression, propertyInfo, value);

                MethodInfo method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                Expression compExpr = Expression.Call(memberExpression, method, constant);
                return compExpr;
            });

            ExpressionCollection.Add(FilterOperator.DoesNotContain, (memberExpression, propertyInfo, value) =>
            {
                var constant = GetConstantWithNullableCastHandle(memberExpression, propertyInfo, value);

                MethodInfo method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                Expression compExpr = Expression.Not(Expression.Call(memberExpression, method, constant));
                compExpr = HandleNullComparison(compExpr, memberExpression, propertyInfo, value);

                return compExpr;
            });

            ExpressionCollection.Add(FilterOperator.IsContainedIn, (memberExpression, propertyInfo, value) =>
            {
                var constant = GetConstantWithNullableCastHandle(memberExpression, propertyInfo, value);

                Expression compExpr = Expression.Call(constant, "Contains", new Type[] { }, memberExpression);
                return compExpr;
            });

            ExpressionCollection.Add(FilterOperator.IsNotContainedIn, (memberExpression, propertyInfo, value) =>
            {
                var constant = GetConstantWithNullableCastHandle(memberExpression, propertyInfo, value);

                Expression compExpr = Expression.Not(Expression.Call(constant, "Contains", new Type[] { }, memberExpression));

                return compExpr;
            });

            ExpressionCollection.Add(FilterOperator.StartsWith, (memberExpression, propertyInfo, value) =>
            {
                var constant = GetConstantWithNullableCastHandle(memberExpression, propertyInfo, value);

                MethodInfo method = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });
                Expression compExpr = Expression.Call(memberExpression, method, constant);
                return compExpr;
            });

            ExpressionCollection.Add(FilterOperator.EndsWith, (memberExpression, propertyInfo, value) =>
            {
                var constant = GetConstantWithNullableCastHandle(memberExpression, propertyInfo, value);

                MethodInfo method = typeof(string).GetMethod("EndsWith", new[] { typeof(string) });
                Expression compExpr = Expression.Call(memberExpression, method, constant);
                return compExpr;
            });

            ExpressionCollection.Add(FilterOperator.IsEqualTo, (memberExpression, propertyInfo, value) =>
            {
                var constant = GetConstantWithNullableCastHandle(memberExpression, propertyInfo, value);

                Expression compExpr = Expression.Equal(memberExpression, constant);

                return compExpr;
            });

            ExpressionCollection.Add(FilterOperator.IsEqualToOrNull, (memberExpression, propertyInfo, value) =>
            {
                var constant = GetConstantWithNullableCastHandle(memberExpression, propertyInfo, value);
                Expression compExpr = Expression.Equal(memberExpression, constant);

                var nullConstant = GetConstantWithNullableCastHandle(memberExpression, propertyInfo, null);
                Expression nullCompExpr = Expression.Equal(memberExpression, nullConstant);

                return Expression.Or(compExpr, nullCompExpr);
            });

            ExpressionCollection.Add(FilterOperator.IsNotEqualTo, (memberExpression, propertyInfo, value) =>
            {
                var constant = GetConstantWithNullableCastHandle(memberExpression, propertyInfo, value);

                Expression compExpr = Expression.NotEqual(memberExpression, constant);
                compExpr = HandleNullComparison(compExpr, memberExpression, propertyInfo, value);

                return compExpr;
            });

            ExpressionCollection.Add(FilterOperator.IsGreaterThan, (memberExpression, propertyInfo, value) =>
            {
                var constant = GetConstantWithNullableCastHandle(memberExpression, propertyInfo, value);

                Expression compExpr = Expression.GreaterThan(memberExpression, constant);
                return compExpr;
            });

            ExpressionCollection.Add(FilterOperator.IsGreaterThanOrEqualTo, (memberExpression, propertyInfo, value) =>
            {
                var constant = GetConstantWithNullableCastHandle(memberExpression, propertyInfo, value);

                Expression compExpr = Expression.GreaterThanOrEqual(memberExpression, constant);
                return compExpr;
            });

            ExpressionCollection.Add(FilterOperator.IsLessThan, (memberExpression, propertyInfo, value) =>
            {
                var constant = GetConstantWithNullableCastHandle(memberExpression, propertyInfo, value);

                Expression compExpr = Expression.LessThan(memberExpression, constant);
                return compExpr;
            });

            ExpressionCollection.Add(FilterOperator.IsGreaterThanOrNull, (memberExpression, propertyInfo, value) =>
            {
                var constant = GetConstantWithNullableCastHandle(memberExpression, propertyInfo, value);
                Expression compExpr = Expression.GreaterThan(memberExpression, constant);

                var nullConstant = GetConstantWithNullableCastHandle(memberExpression, propertyInfo, null);
                Expression nullCompExpr = Expression.Equal(memberExpression, nullConstant);

                return Expression.Or(compExpr, nullCompExpr);
            });

            ExpressionCollection.Add(FilterOperator.IsGreaterThanOrEqualToOrNull, (memberExpression, propertyInfo, value) =>
            {
                var constant = GetConstantWithNullableCastHandle(memberExpression, propertyInfo, value);
                Expression compExpr = Expression.GreaterThanOrEqual(memberExpression, constant);

                var nullConstant = GetConstantWithNullableCastHandle(memberExpression, propertyInfo, null);
                Expression nullCompExpr = Expression.Equal(memberExpression, nullConstant);

                return Expression.Or(compExpr, nullCompExpr);
            });

            ExpressionCollection.Add(FilterOperator.FullText, (memberExpression, propertyInfo, value) =>
            {
                var ftsValue = FtsInterceptor.Fts(Convert.ToString(value));

                var constant = GetConstantWithNullableCastHandle(memberExpression, propertyInfo, ftsValue);

                MethodInfo method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                Expression compExpr = Expression.Call(memberExpression, method, constant);
                return compExpr;
            });
        }
    }
}
