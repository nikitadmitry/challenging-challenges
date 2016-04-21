using System.Collections.Generic;

namespace Shared.Framework.Collections
{
    /// <summary>
    /// Mofications being applied to the collection
    /// </summary>
    /// <typeparam name="TViewModel">The type of the view model.</typeparam>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface ICollectionModification<TViewModel, TEntity>
    {
        /// <summary>
        /// Gets the modified.
        /// </summary>
        /// <value>
        /// The modified.
        /// </value>
        IEnumerable<Modification<TViewModel, TEntity>> Modified
        {
            get;
        }

        /// <summary>
        /// Gets the added.
        /// </summary>
        /// <value>
        /// The added.
        /// </value>
        IEnumerable<TViewModel> Added
        {
            get;
        }

        /// <summary>
        /// Gets the deleted.
        /// </summary>
        /// <value>
        /// The deleted.
        /// </value>
        IEnumerable<TEntity> Deleted
        {
            get;
        }
    }
}