using System;
using System.Collections.Generic;
using Data.Common.Query.QueryParameters;

namespace Data.Common
{
    /// <summary>
    /// Interface for repositories (data access services) of the system.
    /// </summary>
    /// <remarks>
    /// Interface is separated from concrete data access technology implementation.
    /// Interface works only with one type of entity.
    /// </remarks>
    /// <typeparam name="T">Type of entity that repository works with.</typeparam>
    public interface IRepository<T>
        where T : Entity
    {
        /// <summary>
        /// Saves entity in the repository.
        /// </summary>
        /// <param name="entity">Entity to save.</param>
        /// <returns>Saved entity.</returns>
        T InsertOrUpdate(T entity);

        /// <summary>
        /// Delete entity from repository.
        /// </summary>
        /// <param name="entity">Entity to Delete.</param>
        void Delete(T entity);

        /// <summary>
        /// Gets entity of type T from repository by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>Entity by id.</returns>
        T Get(Guid id);

        /// <summary>
        /// Reloads entity of Type T with actual data from database, by id.
        /// </summary>
        /// <param name="id">Entity identity value.</param>
        /// <returns>Reloaded entity</returns>
        T Reload(Guid id);

        /// <summary>
        /// Gets all entities of type T from repository.
        /// </summary>
        /// <param name="queryParameters">Parameters to much conditions for entity.</param>
        /// <returns>Returns all entities of the type that satisfies criteria.</returns>
        T Get(QueryParameters queryParameters);

        /// <summary>
        /// Gets entity of type T from repository.
        /// </summary>
        /// <param name="queryParameters">Parameters to much conditions for entity.</param>
        /// <returns>Returns entity of the type that satisfies criteria or null.</returns>
        T GetSingleOrDefault(QueryParameters queryParameters);

        /// <summary>
        /// Gets first entity of type T from repository.
        /// </summary>
        /// <param name="queryParameters">Parameters to much conditions for entity.</param>
        /// <returns>Returns first entity of the type that satisfies criteria or null.</returns>
        T GetFirstOrDefault(QueryParameters queryParameters);

        /// <summary>
        /// Gets all entities for the repository
        /// </summary>
        /// <param name="parameters">The query.</param>
        /// <returns></returns>
        IList<T> GetAll(QueryParameters parameters);

        /// <summary>
        /// Gets all entities for the repository
        /// </summary>
        /// <returns></returns>
        IList<T> GetAll();

        /// <summary>
        /// Gets Count of entities which match query parameters.
        /// </summary>
        int Count(QueryParameters parameters);

        /// <summary>
        /// Indicates is there any entity matching query parameters.
        /// </summary>
        bool Any(QueryParameters parameters);
    }
}