using System;

namespace Shared.Framework.Validation
{
    /// <summary>
    /// Contains useful methods to ensure method contracts.
    /// </summary>
    public static class Contract
    {
        /// <summary>
        /// Intended to check input method parameters.
        /// </summary>
        /// <typeparam name="TException">Type of exception to throw.</typeparam>
        /// <param name="condition">Condition to check.</param>
        /// <param name="parameterName">name of input parameter</param>
        [ContractAnnotation("condition:false => halt")]
        public static void Requires<TException>(bool condition, string parameterName = null) where TException : ArgumentException
        {
            Assert<TException>(condition, parameterName);
        }
            
        /// <summary>
        /// Intended to check conditions inside method body.
        /// </summary>
        /// <typeparam name="T">Type of exception to throw.</typeparam>
        /// <param name="condition">Condition to check.</param>
        /// <param name="message">Exception message.</param>
        [ContractAnnotation("condition:false => halt")]
        public static void Assert<T>(bool condition, string message = null) where T : Exception
        {
            if (condition)
            {
                return;
            }

            var exception = CreateException<T>(message);
            throw exception;
        }

        /// <summary>
        /// Intended to check null reference
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="message"></param>
        [ContractAnnotation("source:null => halt")]
        public static void NotNull<T>(object source, string message = null) where T : Exception
        {
            if (source != null)
            {
                return;
            }

            throw CreateException<T>(message);
        }

        /// <summary>
        /// Throw exception of type <typeparamref name="T"/>
        /// if <paramref name="source"/> is the default value
        /// of type <typeparamref name="TSource"/>.
        /// </summary>
        /// <typeparam name="T">Type of the exception to throw.</typeparam>
        /// <typeparam name="TSource">Type of the object to check.</typeparam>
        /// <param name="source">Value to check.</param>
        /// <param name="propertyName">
        /// Name of the property (used to construct exception message).
        /// </param>
        public static void NotDefault<T, TSource>(TSource source, string propertyName) where T : Exception
        {
            if (Equals(source, default(TSource)))
            {
                throw CreateException<T>(String.Format("{0} is not specified", propertyName));
            }
        }

        private static TException CreateException<TException>(string parameter)
        {
            if (parameter == null)
            {
                return Activator.CreateInstance<TException>();
            }

            // ArgumentException has specific constructor
            if (typeof(TException) == typeof(ArgumentException))
            {
                return (TException) Activator.CreateInstance(typeof(TException), string.Format("Parameter '{0}' didn't pass validation.", parameter), parameter);
            }

            return (TException)Activator.CreateInstance(typeof(TException), parameter);
        }
    }
}
