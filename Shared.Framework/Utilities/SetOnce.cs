using System;

namespace Shared.Framework.Utilities
{
    /// <summary>
    /// Allows to set value once only.
    /// </summary>
    public class SetOnce<T>
    {
        private bool hasValue;
        private T value;

        public T Value
        {
            get
            {
                return value;
            }
            set
            {
                if (hasValue)
                {
                    throw new InvalidOperationException(String.Format("Value '{0}' can't be set because there is another value already.", value));
                }
                hasValue = true;
                this.value = value;
            }
        }

        public static implicit operator T(SetOnce<T> valueToConvert)
        {
            return valueToConvert.value;
        }
    }
}