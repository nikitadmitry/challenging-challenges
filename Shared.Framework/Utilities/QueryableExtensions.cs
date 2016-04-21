using System.Linq;
using System.Linq.Expressions;

namespace Shared.Framework.Utilities
{
    /// <summary>
    ///     Extensions for IQueryable
    /// </summary>
    public static class QueryableExtensions
    {
        /// <summary>
        ///     Method checks if Quearyable was ever ordered.
        /// </summary>
        /// <typeparam name="T">Entity type for IQueryable</typeparam>
        /// <param name="queryable">IQueryable to check</param>
        /// <returns>Return true if queryable is ordered.</returns>
        public static bool IsOrdered<T>(this IQueryable<T> queryable)
        {
            return IsExpressionTreeContainsOrderingMethodsCalls<T>(queryable.Expression);
        }

        /// <summary>
        ///     This method checks if provided expression tree has method Call expression which returns IOrderedQuerable.
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="expression">Expression</param>
        /// <returns>Returns true if expression contains ordering methods calls</returns>
        private static bool IsExpressionTreeContainsOrderingMethodsCalls<T>(Expression expression)
        {
            var methodCallExpression = expression as MethodCallExpression;

            if (methodCallExpression != null &&
                ((methodCallExpression.Arguments[0].Type == (typeof(IOrderedQueryable<T>))) || methodCallExpression.Type == (typeof(IOrderedQueryable<T>))))
            {
                return true;
            }
            if (methodCallExpression != null && methodCallExpression.Arguments[0].Type == (typeof(IQueryable<T>)))
            {
                return IsExpressionTreeContainsOrderingMethodsCalls<T>(methodCallExpression.Arguments[0]);
            }
            return false;
        }
    }
}