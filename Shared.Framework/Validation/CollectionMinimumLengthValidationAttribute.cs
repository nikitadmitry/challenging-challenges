using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Shared.Framework.Resources;

namespace Shared.Framework.Validation
{
    /// <summary>
    /// Require a minimum length, and optionally a maximum length, for any IEnumerable
    /// </summary>
    public sealed class CollectionMinimumLengthValidationAttribute : ValidationAttribute
    {
        readonly int minLength;
        int? maxLength;

        public CollectionMinimumLengthValidationAttribute(int min)
        {
            minLength = min;
            maxLength = null;
        }

        public CollectionMinimumLengthValidationAttribute(int min, int max)
        {
            minLength = min;
            maxLength = max;
        }

        public override string FormatErrorMessage(string name)
        {
            if (maxLength != null)
            {
                return string.Format(Localization.MaxLengthValidation, name, minLength, maxLength.Value);
            }
            return string.Format(Localization.MinLengthValidation, name, minLength);
        }

        public override bool IsValid(object value)
        {
            IEnumerable<object> list = value as IEnumerable<object>;

            if (list != null && list.Count(o => (string)o != string.Empty) >= minLength 
                && (maxLength == null || list.Count() <= maxLength))
            {
                return true;
            }
            return false;
        }
    }
}
