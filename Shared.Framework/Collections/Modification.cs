using System;

namespace Shared.Framework.Collections
{
    public static class Modification
    {
        /// <summary>
        /// Creates the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static Modification<TViewModel, TEntity> Create<TViewModel, TEntity>(TViewModel model, TEntity entity)
        {
            return new Modification<TViewModel, TEntity>(model, entity);
        }
    }
    /// <summary>
    /// A pair that represents entity and related view model in the modification context.
    /// </summary>
    /// <typeparam name="TViewModel">The type of the view model.</typeparam>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public class Modification<TViewModel, TEntity> : Tuple<TViewModel, TEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Modification{TViewModel, TEntity}"/> class.
        /// </summary>
        /// <param name="item1">The item1.</param>
        /// <param name="item2">The item2.</param>
        public Modification(TViewModel item1, TEntity item2)
            : base(item1, item2)
        {
        }

        /// <summary>
        /// Gets the view model.
        /// </summary>
        /// <value>
        /// The view model.
        /// </value>
        public TViewModel ViewModel
        {
            get
            {
                return Item1;
            }
        }

        /// <summary>
        /// Gets the entity.
        /// </summary>
        /// <value>
        /// The entity.
        /// </value>
        public TEntity Entity
        {
            get
            {
                return Item2;
            }
        }
    }
}