using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using Data.Common.Query.QueryParameters;
using Data.Common.Query.Rules;
using Data.Common.Query.Settings;
using Shared.Framework.DataSource;

namespace Data.Common.Query.Builder
{
    /// <summary>
    /// Represents the builder for the filter settings.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public class FilterSettingsBuilder<TEntity>
    {
        private List<List<IFilterRule>> internalFilters;

        /// <summary>
        /// Prevents a default instance of the <see cref="FilterSettingsBuilder{TEntity}"/> class from being created.
        /// </summary>
        private FilterSettingsBuilder()
        {
            internalFilters = new List<List<IFilterRule>>();
        }

        /// <summary>
        /// Creates new instance of <see cref="FilterSettingsBuilder{TEntity}"/>.
        /// </summary>
        /// <returns></returns>
        public static FilterSettingsBuilder<TEntity> Create()
        {
            return new FilterSettingsBuilder<TEntity>();
        }

        /// <summary>
        /// Adds <see cref="FilterOperator.IsContainedIn"/> rule if <paramref name="values"/> contains more than 1 element.
        /// Otherwise adds <see cref="FilterOperator.IsEqualTo"/> for the first element in <paramref name="values"/>.
        /// Throws an exception if <paramref name="values"/> is null.
        /// </summary>
        public FilterSettingsBuilder<TEntity> AddInFilterRule<TKey>(Expression<Func<TEntity, TKey>> func, ICollection<TKey> values)
        {
            Contract.Requires<ArgumentNullException>(values != null && values.Any(), "values");

            return values.Count > 1
                ? AddFilterRule((LambdaExpression)func, FilterOperator.IsContainedIn, values)
                : AddFilterRule((LambdaExpression)func, FilterOperator.IsEqualTo, values.First());
        }

        /// <summary>
        /// Adds the filter rule to the internal list of filter rules which will be joined by logic AND operator.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="func">The entity member selector expression.</param>
        /// <param name="filterOperator">The filter operator.</param>
        /// <param name="value">The filter value.</param>
        public FilterSettingsBuilder<TEntity> AddFilterRule<TKey>(Expression<Func<TEntity, TKey>> func, FilterOperator filterOperator, object value)
        {
            return AddFilterRule((LambdaExpression)func, filterOperator, value);
        }

        /// <summary>
        /// Adds the filter rule to the internal list of filter rules which will be joined by logic AND operator.
        /// </summary>
        /// <param name="func">The entity member selector expression.</param>
        /// <param name="filterOperator">The filter operator.</param>
        /// <param name="value">The filter value.</param>
        public FilterSettingsBuilder<TEntity> AddFilterRule(LambdaExpression func, FilterOperator filterOperator, object value)
        {
            AddSigleRule(func, filterOperator, value);

            return this;
        }

        private void AddSigleRule(LambdaExpression func, FilterOperator filterOperator, object value)
        {
            var rule = new FilterRule(func, filterOperator, value);
            internalFilters.Add(new List<IFilterRule> { rule });
        }

        /// <summary>
        /// Adds the list of the filter rules which will be joined using OR operator 
        /// and then the result of join will be joined with other filter rules using AND operator.
        /// </summary>
        /// <param name="ruleContainer">The rule list to build complex filter rule.</param>
        public FilterSettingsBuilder<TEntity> AddComplexFilterRule(IEnumerable<FilterSettingRuleContainer<TEntity>> ruleContainer)
        {
            var list = ruleContainer
                .Select(expression => (IFilterRule) new FilterRule(expression.Expression, expression.Operator, expression.Value))
                .ToList();

            internalFilters.Add(list);

            return this;
        }

        /// <summary>
        /// Builds <see cref="SortSettings"/> instance and clears inner collection of added rules.
        /// It means that subsequent invocations of the method will return empty list of rules
        /// unless you build new collection via one of derivations of AscendingBy or DescendingBy.
        /// </summary>
        /// <returns></returns>
        public FilterSettings GetSettings()
        {
            var exportRules = new List<IFilterRule>();

            if (!internalFilters.IsNullOrEmpty())
            {
                foreach (var rules in internalFilters.Where(x => x.Any()))
                {
                    if (rules.Count > 1)
                    {
                        exportRules.Add(new ComplexFilterRule<TEntity>(rules.OfType<FilterRule>()));
                    }
                    else
                    {
                        exportRules.Add(rules.First());
                    }
                }
                internalFilters = new List<List<IFilterRule>>();
            }

            return new FilterSettings
                {
                    Rules = exportRules
                };
        }

        /// <summary>
        /// Converts internal filter rules to the instance of <see cref="BaseQueryParameters"/>
        /// </summary>
        public BaseQueryParameters ToListQueryParameters()
        {
            return ToQueryParameters<BaseQueryParameters>();
        }

        private TParameters ToQueryParameters<TParameters>()
            where TParameters: BaseQueryParameters, new()
        {
            return new TParameters
            {
                FilterSettings = GetSettings()
            };
        }
    }
}