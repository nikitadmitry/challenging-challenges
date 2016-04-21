using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace Shared.Framework.Utilities
{
    /// <summary>
    /// Custom converter between long/object and byte array
    /// </summary>
    public class BytesUtility
    {
        /// <summary>
        /// Converts long value to byte array
        /// </summary>
        /// <param name="value">Long value to convert</param>
        /// <returns>Byte array representation of long</returns>
        public static byte[] ToBytes(long value)
        {
            byte[] result = BitConverter.GetBytes(value).ToArray().Reverse().ToArray();
            return result;
        }

        /// <summary>
        /// Converts byte array value to long
        /// </summary>
        /// <param name="bytes">Byte array to convert</param>
        /// <returns>Long representation of byte array</returns>
        public static long ToLong(byte[] bytes)
        {
            long result = bytes != null && bytes.Length > 0 ? BitConverter.ToInt64(bytes.ToArray().Reverse().ToArray(), 0) : 0;
            return result;
        }

        /// <summary>
        /// Converts object value to byte array
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="tObject">Object value to convert</param>
        /// <returns>Byte array representation of object</returns>
        public static byte[] ObjectToBytes<T>(T tObject)
        {
            var stream = new MemoryStream();
            var formatter = new BinaryFormatter();

            formatter.Serialize(stream, tObject);

            return stream.ToArray();
        }

        /// <summary>
        /// Converts byte array value to object
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="bytes">Byte array to convert</param>
        /// <returns>Object representation of byte array</returns>
        public static T ToObject<T>(byte[] bytes)
        {
            var stream = new MemoryStream(bytes);
            var formatter = new BinaryFormatter();

            return (T) formatter.Deserialize(stream);
        }

        /// <summary>
        /// This equal implementation for comparison of two byte arrays was created 
        /// like a quite fast solution for this king of task. In comparison to 
        /// SequenceEqual e.g. One more optimization if using direct access to array 
        /// instead of iterator 
        /// </summary>
        /// <param name="b1">first byte array to compare</param>
        /// <param name="b2">second byte array to compare</param>
        /// <returns>true if all the arrays values are equal</returns>
        public static bool ByteArraysEqual(byte[] b1, byte[] b2)
        {
            if (b1 == b2) return true;
            if (b1 == null || b2 == null) return false;
            if (b1.Length != b2.Length) return false;
            for (int i = 0; i < b1.Length; i++)
            {
                if (b1[i] != b2[i]) return false;
            }
            return true;
        }
    }
}