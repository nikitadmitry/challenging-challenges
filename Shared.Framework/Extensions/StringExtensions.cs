// ReSharper disable once CheckNamespace
namespace System
{
    /// <summary>
    /// Provides extension methods for <see cref="System.String"/>.
    /// </summary>
    public static class StringExtensions
    {
        public static Guid ToGuid(this string value)
        {
            Guid result;
            Guid.TryParse(value, out result);
            return result;
        }
    }
}