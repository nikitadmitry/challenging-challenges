namespace Shared.Framework.Utilities
{
    /// <summary>
    /// Supports cloning, which creates a new instance of a
    /// class with the same value as an existing instance.
    /// Implementer should decide,
    /// whether the cloning operation performs a deep copy,
    /// a shallow copy, or something in between.
    /// </summary>
    /// <typeparam name="T">Type of class which implements cloning.</typeparam>
    public interface ICloneable<out T>
    {
        /// <returns>
        /// A new object of type <typeparamref name="T" />
        /// clone with full graph deep clone(copy) of this instance.
        /// </returns>
        T DeepClone();

        /// <summary>
        /// A new object of type <typeparamref name="T" />
        /// which is a simple properties clone(copy),
        /// without full graph clone of this instance.
        /// </summary>
        T SimpleClone();
    }
}
