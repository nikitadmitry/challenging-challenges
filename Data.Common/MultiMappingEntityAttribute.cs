using System;
using System.Reflection;

namespace Data.Common
{
    [AttributeUsage(AttributeTargets.Class)]
    public class MultiMappingEntityAttribute : Attribute
    {
        public MultiMappingEntityAttribute(MultiMappingEntityRole entityRole)
        {
            MultiMappingEntityRole = entityRole;
        }

        public MultiMappingEntityRole MultiMappingEntityRole
        {
            get;
            private set;
        }

        public static bool IsEntityModifiable<T>(T entity) where T : Entity
        {
            var referenceEntityAttribute = entity.GetType().GetCustomAttribute<MultiMappingEntityAttribute>();

            return referenceEntityAttribute == null ||
                   referenceEntityAttribute.MultiMappingEntityRole == MultiMappingEntityRole.Basic;
        }
    }
}
