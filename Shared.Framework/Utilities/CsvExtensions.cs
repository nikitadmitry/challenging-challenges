using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Shared.Framework.Utilities
{
    /// <summary>
    /// Represents CSV extensions
    /// </summary>
    public static class CsvExtensions
    {
        private const String DefaultDelimiter = ",";

        /// <summary>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="delimiter"></param>
        /// <param name="columns"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static String ToCsv<T>(this IEnumerable<T> source, String delimiter, IEnumerable<String> columns)
        {
            var sb = new StringBuilder();

            foreach (var column in columns)
            {
                sb.Append(column.ToCsv()).Append(delimiter);
            }
            sb.Remove(sb.Length - 1, 1).Append(Environment.NewLine);

            foreach (var item in source)
            {
                foreach (var column in columns)
                {
                    sb.Append(GetPropertyValue(item, column).ToString().ToCsv()).Append(delimiter);
                }
                sb.Remove(sb.Length - 1, 1).Append(Environment.NewLine);
            }

            return sb.ToString();
        }

        /// <summary>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="delimiter"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static String ToCsv<T>(this IEnumerable<T> source, String delimiter)
        {
            return source.ToCsv(delimiter, typeof(T).GetProperties().Select(x => x.Name).ToArray());
        }

        /// <summary>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="columns"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static String ToCsv<T>(this IEnumerable<T> source, IEnumerable<String> columns)
        {
            return source.ToCsv(DefaultDelimiter, columns);
        }

        /// <summary>
        /// </summary>
        /// <param name="source"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static String ToCsv<T>(this IEnumerable<T> source)
        {
            return source.ToCsv(DefaultDelimiter);
        }

        /// <summary>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="delimiter"></param>
        /// <param name="columns"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static MemoryStream ToCsvMemoryStream<T>(this IEnumerable<T> source, String delimiter, String[] columns)
        {
            var memoryStream = new MemoryStream();
            var writer = new StreamWriter(memoryStream);
            int columnsCount = columns.Count();
            for (var i = 0; i < columnsCount - 1; i++)
            {
                writer.Write(columns[i].ToCsv());
                writer.Write(delimiter);
            }
            writer.Write(columns[columnsCount - 1].ToCsv());
            writer.Write(Environment.NewLine);

            foreach (var item in source)
            {
                Object value;
                for (var i = 0; i < columnsCount - 1; i++)
                {
                    value = GetPropertyValue(item, columns[i]);
                    if (value != null)
                    {
                        writer.Write(value.ToString().ToCsv());
                    }
                    writer.Write(delimiter);
                }
                value = GetPropertyValue(item, columns[columnsCount - 1]);
                if (value != null)
                {
                    writer.Write(value.ToString().ToCsv());
                }
                writer.Write(Environment.NewLine);
            }

            writer.Flush();

            memoryStream.Seek(0, SeekOrigin.Begin);

            return memoryStream;
        }

        /// <summary>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="delimiter"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static MemoryStream ToCsvMemoryStream<T>(this IEnumerable<T> source, String delimiter)
        {
            return source.ToCsvMemoryStream(delimiter, typeof(T).GetProperties().Select(x => x.Name).ToArray());
        }

        /// <summary>
        /// </summary>
        /// <param name="source"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static MemoryStream ToCsvMemoryStream<T>(this IEnumerable<T> source)
        {
            return source.ToCsvMemoryStream(DefaultDelimiter);
        }

        /// <summary>
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static String ToCsv(this String source)
        {
            String result = source ?? String.Empty;
            if (source != null && source.Contains(@""""))
            {
                result = result.Replace(@"""", @"""""");
            }

            result = string.Format("\"{0}\"", result);

            return result;
        }

        private static object GetPropertyValue(object root, string propertyName)
        {
            PropertyInfo propertyInfo = root.GetType().GetProperty(propertyName);
            if (propertyInfo == null)
            {
                throw new ArgumentException("Property with specified name doesn't exist in the source object.",
                                            "propertyName");
            }
            root = propertyInfo.GetValue(root, null);

            return root;
        }
    }
}