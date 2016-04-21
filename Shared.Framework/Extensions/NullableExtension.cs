using System.ComponentModel;

namespace Shared.Framework.Extensions
{
    public static class ConvertExtension
    {
        public static T? ToNullable<T>(this string s) where T : struct
        {
            try
            {
                if (!string.IsNullOrEmpty(s) && s.Trim().Length > 0)
                {
                    TypeConverter conv = TypeDescriptor.GetConverter(typeof(T));
                    return (T) conv.ConvertFrom(s);
                }
            }
            catch
            {
            }

            return new T?();
        }
    }
}