using System;
using System.Collections.Generic;
using Shared.Framework.DataSource;

namespace Data.Common.Query.FilterExpressions
{
    /// <summary>
    /// Provides filter expression providers by the specified type.
    /// </summary>
    public static class FilterExpressionProviderFactory
    {
        private static readonly IFilterExpressionProvider DefaultProvider;

        private static readonly Dictionary<Type, IFilterExpressionProvider> ExpressionProviders;

        /// <summary>
        /// Initializes the <see cref="FilterExpressionProviderFactory"/> class.
        /// </summary>
        static FilterExpressionProviderFactory()
        {
            DefaultProvider = new DefaultFilterExpressionProvider();

            ExpressionProviders = new Dictionary<Type, IFilterExpressionProvider>
            {
                { typeof(DateInterval), new DateIntervalFilterExpressionProvider() },
            };
        }

        /// <summary>
        /// Gets the provider by the specified filter operator and type.
        /// </summary>
        /// <param name="propertyType">Type of the property.</param>
        /// <param name="filterOperator">The filter operator.</param>
        /// <returns>
        /// The filter expression provider if the specified type has it's own 
        /// filter expression provider and it contains filter expression for
        /// the speciied filter operator;
        /// othewise return default filter expression provider.
        /// </returns>
        public static IFilterExpressionProvider GetProvider(Type propertyType, FilterOperator filterOperator)
        {
            var provider = ExpressionProviders.ContainsKey(propertyType)
                ? ExpressionProviders[propertyType]
                : DefaultProvider;

            if (!provider.ContainsFilterExpressionFor(filterOperator))
            {
                provider = DefaultProvider;
            }

            return provider;
        }
    }
}
