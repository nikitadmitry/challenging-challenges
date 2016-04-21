using System;

namespace Shared.Framework.Extensions
{
    /// <summary>
    /// Represents a class containing extensions that extend the System.Guid object.
    /// </summary>
    public static class GuidExtensions
    {
        /// <summary>
        /// Checks whether <see cref="System.Guid"/> is empty.
        /// </summary>
        public static bool IsEmpty(this Guid guid)
        {
            return guid == Guid.Empty;
        }

        /// <summary>
        /// Checks whether <see cref="System.Guid"/> is not empty.
        /// </summary>
        public static bool IsNotEmpty(this Guid guid)
        {
            return !IsEmpty(guid);
        }
        /// <summary>
        /// Checks whether nullable <see cref="System.Guid"/> is null or empty.
        /// </summary>
        public static bool IsNullOrEmpty(this Guid? guid)
        {
            return !guid.HasValue || IsEmpty(guid.Value);
        }

        /// <summary>
        /// If <paramref name="guid"/> is null returns new <see cref="Guid"/>.
        /// </summary>
        public static Guid NotNull(this Guid? guid)
        {
            return guid.HasValue ? guid.Value : Guid.NewGuid();
        }

        /// <summary>
        /// If the guid is empty, then return null otherwise return the guid.
        /// </summary>
        /// <param name="guid">The guid to test if empty.</param>
        /// <returns>Null if the guid is empty, the guid otherwise.</returns>
        public static Guid? NullIfEmpty(this Guid guid)
        {
            return (guid.IsEmpty()) ? (Guid?)null : guid;
        }
    }
}