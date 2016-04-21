using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Data.Common.Query.QueryParameters;

namespace Data.Common
{
    /// <summary>
    /// Contract for DB access unit of work.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Commits all changes in the DB.
        /// </summary>
        /// <returns></returns>
        int Commit();

        /// <summary>
        /// Gets all entities by specified filter.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filter"></param>
        /// <returns></returns>
        IList<T> GetAll<T>(BaseQueryParameters filter) where T: Entity;

        /// <summary>
        /// Gets all entities by specified filter.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IList<T> GetAll<T>() where T : Entity;

        /// <summary>
        /// Gets entity by specified filter.
        /// </summary>
        /// <typeparam name="T">Entity type.</typeparam>
        /// <param name="filter">Query parameters to filter entities.</param>
        /// <returns>Entity <see cref="T"/> instance.</returns>
        T Get<T>(BaseQueryParameters filter) where T : Entity;

        /// <summary>
        /// Gets entity by specified unique identifier.
        /// </summary>
        /// <typeparam name="T">Entity type.</typeparam>
        /// <param name="id">Identifier of an entity.</param>
        /// <returns>Entity <see cref="T"/> instance.</returns>
        T Get<T>(Guid id) where T : Entity;

        /// <summary>
        /// Gets entity by unique identifier from context and reload it from DB.
        /// </summary>
        /// <typeparam name="T">Entity type.</typeparam>
        /// <param name="id">Identifier of an entity.</param>
        /// <returns>Entity <see cref="T"/> instance.</returns>
        T GetReload<T>(Guid id) where T : Entity;

        /// <summary>
        /// Gets entity by unique identifier from context and reload it from DB.
        /// </summary>
        /// <typeparam name="T">Entity type.</typeparam>
        /// <param name="filter">Query parameters to filter entities.</param>
        /// <returns>Entity <see cref="T"/> instance.</returns>
        T GetReload<T>(BaseQueryParameters filter) where T : Entity;

        /// <summary>
        /// Retrieves entity from Data Source. Doesn't throw an exception if there is no entity matched the parameters.
        /// </summary>
        /// <typeparam name="T">Entity to retrieve.</typeparam>
        /// <param name="parameters">Parameters to match.</param>
        /// <returns></returns>
        T GetSingleOrDefault<T>(BaseQueryParameters parameters) where T : Entity;

        /// <summary>
        /// Retrieves first entity that matches specified parameters. Doesn't throw an exception if there is no entity matched the parameters.
        /// </summary>
        /// <typeparam name="T">Entity to retrieve.</typeparam>
        /// <param name="parameters">Parameters to match.</param>
        T GetFirstOrDefault<T>(BaseQueryParameters parameters) where T : Entity;

        /// <summary>
        /// Saves entity in the repository.
        /// </summary>
        /// <param name="entity">Entity to save.</param>
        /// <returns>Saved entity.</returns>
        T InsertOrUpdate<T>(T entity) where T : Entity;

        /// <summary>
        /// Saves entity in the repository and does not mark it as modified.
        /// </summary>
        /// <param name="entity">Entity to save.</param>
        /// <returns>Saved entity.</returns>
        T InsertOrUpdateWithSkipTrackingProperties<T>(T entity) where T : Entity;

        /// <summary>
        /// Saves entity in the repository with AutoDetectChangesEnabled and ValidateOnSaveEnabled options turned off.
        /// </summary>
        /// <param name="entities">Entities to save.</param>
        /// <returns>Saved bulk.</returns>
        IEnumerable<T> BulkInsertOrUpdate<T>(IEnumerable<T> entities) where T : Entity;

        /// <summary>
        /// Delete entity from repository.
        /// </summary>
        /// <typeparam name="T">Entity type.</typeparam>
        /// <param name="entity">Entity to Delete.</param>
        /// <returns>Entity <see cref="T"/> instance.</returns>
        void Delete<T>(T entity) where T : Entity;

        /// <summary>
        /// Gets Count of entities which match query parameters.
        /// </summary>
        /// <param name="parameters">Base query parameters.</param>
        /// <typeparam name="T">Entity type.</typeparam>
        int Count<T>(BaseQueryParameters parameters) where T : Entity;

        /// <summary>
        /// Evict entity from context.
        /// </summary>
        /// <typeparam name="T">Entity type.</typeparam>
        /// <param name="entity">Entity to Evict.</param>
        void Evict<T>(T entity) where T : Entity;

        /// <summary>
        /// Creates new instance of an entity.
        /// It must be used in cases when you want to create entity instance and save it without retrieving it from DB.
        /// </summary>
        T Create<T>() where T : Entity;

        /// <summary>
        /// Load Navigation Property
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="entity"></param>
        /// <param name="navigationProperty"></param>
        void LoadNavigationProperty<TEntity, TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> navigationProperty)
            where TProperty : class
            where TEntity : Entity;
    }
}