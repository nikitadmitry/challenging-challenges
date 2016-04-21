using System.Collections.Generic;

namespace Shared.Framework.Utilities
{
    internal class ByKeyComparer : EqualityComparer<KeyValuePair<string, object>>
    {
        public override bool Equals(KeyValuePair<string, object> x, KeyValuePair<string, object> y)
        {
            return x.Key == y.Key;
        }

        public override int GetHashCode(KeyValuePair<string, object> obj)
        {
            return obj.Key.GetHashCode();
        }
    }
}