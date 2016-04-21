using System;
using System.Linq.Expressions;

namespace Data.Common.Query.Rules
{
    /// <summary>
    /// Describes the common interface for the filter rule.
    /// </summary>
    public interface IFilterRule
    {
        /// <summary>
        /// Creates the filter expression from this instance of filter rule.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Expression<Func<T, bool>> GetExpression<T>();
    }
}