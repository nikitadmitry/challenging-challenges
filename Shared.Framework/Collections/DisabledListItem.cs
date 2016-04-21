namespace Shared.Framework.Collections
{
    public class DisabledListItem: ListItem
    {
        public bool Disabled
        {
            get;
            set;
        }

        public long LongVersion
        {
            get;
            set;
        }

        public DisabledListItem(string key, string text, bool enabled = false):base(key, text)
        {
            Disabled = enabled;
        }

        public DisabledListItem()
        {
        }
    }
}