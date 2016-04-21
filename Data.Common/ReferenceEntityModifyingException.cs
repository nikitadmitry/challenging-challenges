using System;

namespace Data.Common
{
    /// <summary>
    /// Exception occures on trying to modify reference entity.
    /// </summary>
    [Serializable]
    public class ReferenceEntityModifyingException : Exception
    {
        public ReferenceEntityModifyingException(string message)
            : base(message)
        {
        }

        public ReferenceEntityModifyingException(Type referenceEntityType)
            : base($"Cannot modify reference entity state of type {referenceEntityType.FullName}.")
        {
        }
    }
}
