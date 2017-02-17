using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Data.Common.Query.Rules;
using Data.Common.Query.Settings;
using Shared.Framework.DataSource;

namespace Data.Common.Query.Builder
{
    /// <summary>
    /// Provides helpfull methods for <see cref="SortSettings"/> building
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class SortSettingsBuilder<TEntity>
    {
        private IList<SortRule> sortRules;

        private SortSettingsBuilder()
        {
            sortRules = new List<SortRule>();
        }

        /// <summary>
        /// Creates new instance of <see cref="SortSettingsBuilder{TEntity}"/>.
        /// </summary>
        /// <returns></returns>
        public static SortSettingsBuilder<TEntity> Create()
        {
            return new SortSettingsBuilder<TEntity>();
        }

        /// <summary>
        /// Adds sort by a property which specifies by <paramref name="func"/> in ascending order.
        /// </summary>
        public SortSettingsBuilder<TEntity> AscendingBy<TKey>(Expression<Func<TEntity, TKey>> func)
        {
            return AddOrderBy(func, SortOrder.Asc);
        }

        /// <summary>
        /// Adds sort by a property which specifies by <paramref name="func"/> in descending order.
        /// </summary>
        public SortSettingsBuilder<TEntity> DescendingBy<TKey>(Expression<Func<TEntity, TKey>> func)
        {
            return AddOrderBy(func, SortOrder.Desc);
        }

        /// <summary>
        /// Adds sort by a property which specifies by <paramref name="expression"/> in ascending order.
        /// </summary>
        public SortSettingsBuilder<TEntity> AscendingBy(LambdaExpression expression)
        {
            return AddOrderBy(expression, SortOrder.Asc);
        }

        /// <summary>
        /// Adds sort by a property which specifies by <paramref name="expression"/> in descending order.
        /// </summary>
        public SortSettingsBuilder<TEntity> DescendingBy(LambdaExpression expression)
        {
            return AddOrderBy(expression, SortOrder.Desc);
        }

        /// <summary>
        /// Adds sort by a property which name is <paramref name="propertyName"/> in ascending order.
        /// </summary>
        public SortSettingsBuilder<TEntity> AscendingBy(string propertyName)
        {
            var expression = BuildExpressionByPropertyName(propertyName);
            return AddOrderBy(expression, SortOrder.Asc);
        }

        /// <summary>
        /// Adds sort by a property which name is <paramref name="propertyName"/> in descending order.
        /// </summary>
        public SortSettingsBuilder<TEntity> DescendingBy(string propertyName)
        {
            var expression = BuildExpressionByPropertyName(propertyName);
            return AddOrderBy(expression, SortOrder.Desc);
        }

        private LambdaExpression BuildExpressionByPropertyName(string propertyName)
        {
            var type = typeof(TEntity);
            var property = type.GetProperty(propertyName);
            if (property == null)
            {
                throw new ArgumentException("Invalid property name.");
            }
            var parameter = Expression.Parameter(type, "orderbyproperty");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExp = Expression.Lambda(propertyAccess, parameter);

            return orderByExp;
        }

        /// <summary>
        /// Adds sort by a property which describes by <paramref name="func"/> in <paramref name="direction"/> order.
        /// </summary>
        public SortSettingsBuilder<TEntity> AddOrderBy(LambdaExpression func, SortOrder direction)
        {
            var rule = new SortRule(direction, func);
            sortRules.Add(rule);
            return this;
        }

        /// <summary>
        /// Builds <see cref="SortSettings"/> instance and clears inner collection of added rules.
        /// It means that subsequent invocations of the method will return empty list of rules
        /// unless you build new collection via one of derivations of AscendingBy or DescendingBy.
        /// </summary>
        /// <returns></returns>
        public SortSettings GetSettings()
        {
            var rulesRef = sortRules;
            sortRules = new List<SortRule>();
            return new SortSettings
                {
                    SortRules = rulesRef
                };
        }
    }
}