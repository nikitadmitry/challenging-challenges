using System.Collections.Generic;
using System.Linq;

namespace Shared.Framework.Collections
{
    public static class CheckBoxCollectionExtensions
    {
        public static IList<CheckBoxItem> Selected(this IList<CheckBoxItem> collection)
        {
            return collection.Where(item => item.Checked).ToList();
        }

        public static IList<CheckBoxItem> NotSelected(this IList<CheckBoxItem> collection)
        {
            return collection.Where(item => !item.Checked).ToList();
        }
    }
}