using System;

namespace Shared.Framework.Utilities
{
    /// <summary>
    /// Provides methods for <see cref="System.Decimal"/>.
    /// </summary>
    public static class CurrencyValueExtensions
    {
        /// <summary>
        /// Default value.
        /// </summary>
        private const decimal DefaultCurrencyValue = 0M;

        /// <summary>
        /// Returns the value of the current <see cref="Nullable{Decimal}"/> object or default value (0M).
        /// </summary>
        /// <param name="value">Value of <see cref="Nullable{T}"/> object.</param>
        /// <returns><see cref="decimal"/> value.</returns>
        public static decimal GetValue(this decimal? value)
        {
            return value.HasValue ? value.Value : DefaultCurrencyValue;
        }
    }
}