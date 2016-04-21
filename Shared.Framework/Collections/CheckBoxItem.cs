using System;
using System.Globalization;

namespace Shared.Framework.Collections
{
    [Serializable]
    public class CheckBoxItem : ListItem
    {
        public CheckBoxItem()
        {
            
        }
        public CheckBoxItem(int value, string text, bool selected)
            : this(Convert.ToString(value, CultureInfo.InvariantCulture), text, selected)
        {
        }

        public CheckBoxItem(object key, string text, bool isSelected)
            : base(key, text)
        {
            Checked = isSelected;
        }

        public bool Checked
        {
            get;
            set;
        }
    }
}