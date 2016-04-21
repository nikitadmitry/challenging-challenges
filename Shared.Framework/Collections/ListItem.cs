using System;
using System.Runtime.Serialization;

namespace Shared.Framework.Collections
{
    /// <summary>
    /// Represents the item with Key and appropriate Value.
    /// </summary>
    [Serializable]
    [KnownType(typeof(DisabledListItem))]
    [KnownType(typeof(CheckBoxItem))]
    public class ListItem
    {
        public const string KeyPropertyName = "Key";
        public const string TextPropertyName = "Text";

        public ListItem()
        {
        }

        public ListItem(object key, string text)
            : this(key.ToString(), text)
        {
        }

        public ListItem(object key, object text)
            : this(key.ToString(), text.ToString())
        {
        }

        public ListItem(string key, string text)
        {
            Key = key;
            Text = text;
        }

        public string Key
        {
            get;
            set;
        }

        public string Text
        {
            get;
            set;
        }

        public static ListItem CreateEmpty()
        {
            return new ListItem { Key = null, Text = String.Empty };
        }

        public static ListItem CreateDefault<TKey>()
        {
            return new ListItem (default(TKey), String.Empty);
        }
    }
}