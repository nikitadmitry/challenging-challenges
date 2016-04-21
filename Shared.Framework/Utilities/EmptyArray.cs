namespace Shared.Framework.Utilities
{
    /// <summary>
    /// Helper class that does a memory efficient way of initializing empty arrays. The empty instances are reused across the application.
    /// </summary>
    /// <typeparam name="T">Any type</typeparam>
    public class EmptyArray<T>
    {
        /// <summary>
        /// The instance
        /// </summary>
        public static readonly T[] Instance = {};
    }
}
