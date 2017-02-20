using System;

namespace Shared.Framework.Dependency
{
    /// <summary>
    /// This attribute is designed to simplify registration of keyed dependencies (http://docs.autofac.org/en/latest/advanced/keyed-services.html).
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class KeyedDependencyAttribute : Attribute
    {
        internal object Key
        {
            get;
            set;
        }

        public KeyedDependencyAttribute(object key)
        {
            Key = key;
        }
    }
}