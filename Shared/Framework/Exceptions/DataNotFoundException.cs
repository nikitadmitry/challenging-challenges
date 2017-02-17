using System;

namespace Shared.Framework.Exceptions
{
    /// <summary>
    /// Is used to indicate that data is not found.
    /// </summary>
    [Serializable]
    public class DataNotFoundException : Exception
    {
        /// <summary>
        /// Initializes new class of DataNotFoundException
        /// </summary>
        public DataNotFoundException()
            : base("The data you have requested has not been found.")
        {
        }
    }
}