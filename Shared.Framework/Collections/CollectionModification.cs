using System;
using System.Collections.Generic;
using System.Linq;

namespace Shared.Framework.Collections
{
    /// <summary>
    /// Utility class that resolves a collection change based on original and new collection. 
    /// </summary>
    public class CollectionModificationResolver
    {
        private class CollectionModification<TViewModel, TEntity> : ICollectionModification<TViewModel, TEntity>
        {
            public CollectionModification(IEnumerable<Modification<TViewModel, TEntity>> modified, IEnumerable<TViewModel> added, IEnumerable<TEntity> deleted)
            {
                Modified = modified;
                Added = added;
                Deleted = deleted;
            }

            public IEnumerable<Modification<TViewModel, TEntity>> Modified
            {
                get;
                private set;
            }

            public IEnumerable<TViewModel> Added
            {
                get;
                private set;
            }

            public IEnumerable<TEntity> Deleted
            {
                get;
                private set;
            }
        }


        /// <summary>
        /// Resolves the modification being applied to the original collection on UI.
        /// </summary>
        /// <typeparam name="TViewModel">The type of the view model.</typeparam>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="viewModels">The view models.</param>
        /// <param name="orginalCollection">The orginal collection.</param>
        /// <param name="compareFunction">The compare function.</param>
        /// <returns></returns>
        public static ICollectionModification<TViewModel, TEntity> Resolve<TViewModel, TEntity>(IEnumerable<TViewModel> viewModels, IEnumerable<TEntity> orginalCollection, Func<TEntity, TViewModel, bool> compareFunction)
        {
            var updatedEntities = viewModels.Select(viewModel =>
                                                    new {
                                                        ViewModel = viewModel, 
                                                        Entities = orginalCollection.Where(entity => compareFunction(entity, viewModel)).Take(1)
                                                    }).Where(item => item.Entities.Any()).Select(item => Modification.Create(item.ViewModel, item.Entities.First())).ToList();

            var deletedEntities = orginalCollection.Where(entity => !viewModels.Any(viewModel => compareFunction(entity, viewModel))).ToList();

            var addedEntities = viewModels.Where(viewModel => !orginalCollection.Any(entity => compareFunction(entity, viewModel))).ToList();
            return new CollectionModification<TViewModel, TEntity>(updatedEntities, addedEntities, deletedEntities);
        }
    }
}