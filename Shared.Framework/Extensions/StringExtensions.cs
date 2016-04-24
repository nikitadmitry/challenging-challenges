using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;

// ReSharper disable once CheckNamespace
namespace System
{
    /// <summary>
    /// Provides extension methods for <see cref="System.String"/>.
    /// </summary>
    public static class StringExtensions
    {
        private static readonly char[] newLineChars = { '\r', '\n' };

        public static string SplitUpperCaseToString(this string source)
        {
            return source == null ? null : String.Join(" ", source.SplitUpperCase());
        }

        public static string[] SplitUpperCase(this string source)
        {
            if (source == null)
            {
                return new string[]{};
            }

            if (source.Length == 0)
            {
                return new[]
                    {
                        String.Empty
                    };
            }

            var words = new StringCollection();
            int wordStartIndex = 0;

            char[] letters = source.ToCharArray();
            char previousChar = Char.MinValue;
            // Skip the first letter. we don't care what case it is.
            for (int i = 1; i < letters.Length; i++)
            {
                if (Char.IsUpper(letters[i]) && !Char.IsWhiteSpace(previousChar))
                {
                    //Grab everything before the current index.
                    words.Add(new String(letters, wordStartIndex, i - wordStartIndex));
                    wordStartIndex = i;
                }
                previousChar = letters[i];
            }
            //We need to have the last word.
            words.Add(new String(letters, wordStartIndex, letters.Length - wordStartIndex));

            //Copy to a string array.
            var wordArray = new string[words.Count];
            words.CopyTo(wordArray, 0);
            return wordArray;
        }

        public static string Truncate(this string source, int maxLength)
        {
            if (String.IsNullOrEmpty(source))
            {
                return source;
            }
            return source.Length <= maxLength ? source : source.Substring(0, maxLength);
        }

        /// <summary>
        /// Method to check if a string Evaluates to a Number
        /// </summary>
        /// <param name="stringToEvaluate"></param>
        /// <returns></returns>
        public static bool IsNumeric(this string stringToEvaluate)
        {
            int isStringToEvaluateNumeric;
            return Int32.TryParse(stringToEvaluate, out isStringToEvaluateNumeric);
        }



        /// <summary>
        /// Method to check if a string Evaluates to a Number
        /// </summary>
        /// <param name="stringToEncode"></param>
        /// <returns></returns>
        public static string EncodeHtml(this string stringToEncode)
        {
            return WebUtility.HtmlEncode(stringToEncode);
        }

        /// <summary>
        /// Checks whether a string equals to any passed value.
        /// </summary>
        public static bool EqualsToAny(this string value, params string[] equalsTo)
        {
            return equalsTo.Contains(value);
        }

        /// <summary>
        ///     Method to check if a string evaluates to a Social Security Number
        /// </summary>
        /// <param name="stringToEvaluate"></param>
        /// <returns></returns>
        public static bool IsSsn(this string stringToEvaluate)
        {
            if (stringToEvaluate.Length == SsnRequiredLengthNoDashes && stringToEvaluate.IsNumeric())
            {
                return true;
            }

            if (stringToEvaluate.Length != SsnRequiredLengthWithDashes || !stringToEvaluate.Contains(SsnDash))
            {
                return false;
            }

            return stringToEvaluate.Substring(0, 3).IsNumeric() && stringToEvaluate.Substring(3, 1) == SsnDash &&
                   stringToEvaluate.Substring(4, 2).IsNumeric() && stringToEvaluate.Substring(6, 1) == SsnDash &&
                   stringToEvaluate.Substring(7, 4).IsNumeric();
        }

        private const String SsnDash = "-";
        private const int SsnRequiredLengthNoDashes = 9;
        private const int SsnRequiredLengthWithDashes = 11;

        private const string AlternateIdStartsWith = "H";
        private const int AlternateIdLength = 9;

        private static IEnumerable<char> CaseInvariant()
        {
            return new List<char>
            {
                AlternateIdStartsWith.ToCharArray().First(), 
                AlternateIdStartsWith.ToLower().ToCharArray().First()
            };
        }
        /// <summary>
        /// Method to check if a string evaluates to AlternateId 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsAlternateId(this string source)
        {
            var isAlternateId = !String.IsNullOrEmpty(source) && source.StartsWith(AlternateIdStartsWith, StringComparison.OrdinalIgnoreCase) &&
               source.Length == AlternateIdLength && source.Except(CaseInvariant()).All(Char.IsDigit);

            return isAlternateId;
        }


        /// <summary>
        ///     Method to remove special characters from a filename string
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns>a filename string with all special characters removed</returns>
        public static string FormatFilename(this string fileName)
        {

            var result = new StringBuilder(fileName);

            result.Replace("\"", "");
            result.Replace("<", "");
            result.Replace(">", "");
            result.Replace(":", "");
            result.Replace("/", "");
            result.Replace("\\", "");
            result.Replace("|", "");
            result.Replace("?", "");
            result.Replace("*", "");

            return result.ToString();

        }

        public static string CutPhaNumber(this string phaNumber)
        {
            if (phaNumber == null)
                return string.Empty;

            return phaNumber.Length > 5
                ? phaNumber.Substring(0, 5)
                : phaNumber;
        }

        public static string ZipStr(this string str)
        {
            using (MemoryStream output = new MemoryStream())
            {
                using (DeflateStream gzip =
                  new DeflateStream(output, CompressionMode.Compress))
                {
                    using (StreamWriter writer =
                      new StreamWriter(gzip, Encoding.UTF8))
                    {
                        writer.Write(str);
                    }
                }

                return Convert.ToBase64String(output.ToArray());
            }
        }

        public static string UnZipStr(this string input)
        {
            byte[] zipBuffer = Convert.FromBase64String(input);
            using (MemoryStream inputStream = new MemoryStream(zipBuffer))
            {
                using (DeflateStream gzip =
                  new DeflateStream(inputStream, CompressionMode.Decompress))
                {
                    using (StreamReader reader =
                      new StreamReader(gzip, Encoding.UTF8))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
        }

        /// <summary>
        /// Return null if the string is null or whitespace.
        /// </summary>
        /// <param name="input">The input string to test.</param>
        /// <returns>Null if the string is null or composed of whitespace, otherwise the string.</returns>
        public static string NullIfWhitespace(this string input)
        {
            return (string.IsNullOrWhiteSpace(input)) ? null : input;
        }

        /// <summary>
        /// Extract the first line of the string (without the overhead of Linq)
        /// </summary>
        /// <param name="input">The input string</param>
        /// <returns>The first line of the input or the entire input if there are no line breaks</returns>
        public static string FirstLine(this string input)
        {
            if (input == null)
                return null;

            var firstCrOrLf = input.IndexOfAny(newLineChars);
            if (firstCrOrLf < 0)
                return input;

            return input.Substring(0, firstCrOrLf);
        }

        /// <summary>
        /// Method to replace line breaks with whitespaces.
        /// </summary>
        /// <param name="input">The input string</param>
        /// <returns>The input string with line breaks replaced with whitespaces.</returns>
        public static string ReplaceLineBreakWithWhitespace(this string input)
        {
            return input.Replace("\r\n", " ");
        }

        public static Guid ToGuid(this string value)
        {
            Guid result;
            Guid.TryParse(value, out result);
            return result;
        }
    }
}