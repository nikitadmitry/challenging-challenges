using System;
using System.Collections.Generic;
using System.Data.Entity;
using Data.Common.Query.QueryParameters;
using Shared.Framework.Utilities;
using Shared.Framework.Validation;

namespace Data.Common
{
    /// <summary>
    /// Represents wrapper above DBContext instance and repositories container.
    /// </summary>
    /// <remarks>
    /// This class should be abstract since only derived types should be created providing proper context.
    /// </remarks>
    public abstract class UnitOfWork : BaseDisposable, IUnitOfWork
    {
        private readonly DbContext context;
        private readonly IDictionary<Type, object> repositories;

        /// <summary>
        /// Initializes new instance using given DBContext.
        /// </summary>
        /// <param name="context">Context to wrap.</param>
        protected UnitOfWork(DbContext context)
        {
            Contract.Requires<ArgumentNullException>(context != null, "context");
            repositories = new Dictionary<Type, object>();
            this.context = context;

            //http://stackoverflow.com/questions/19672799/ef6-sql-generation-for-where-nullable-columns-equals
            this.context.Configuration.UseDatabaseNullSemantics = true;
        }


        /// <summary>
        /// Wrapped DbContext.
        /// </summary>
        protected DbContext Context
        {
            get
            {
                return context;
            }
        }

        /// <summary>
        /// Commits all changes on the DB.
        /// </summary>
        /// <returns>Number of rows affected.</returns>
        public int Commit()
        {
            return context.SaveChanges();
        }

        public T Create<T>() where T : Entity
        {
            return Context.Set<T>().Create();
        }

        /// <summary>
        /// Gets all entities by specified filter.
        /// </summary>
        /// <typeparam name="T">Entity type.</typeparam>
        /// <param name="filter">Query parameters to filter entities.</param>
        /// <returns>List of entities and total number.</returns>
        public IList<T> GetAll<T>(QueryParameters filter) where T : Entity
        {
            return GetRepository<T>().GetAll(filter);
        }

        public IList<T> GetAll<T>() where T : Entity
        {
            return GetAll<T>(QueryParameters.Empty);
        }

        /// <summary>
        /// Gets entity by specified filter.
        /// </summary>
        /// <typeparam name="T">Entity type.</typeparam>
        /// <param name="filter">Query parameters to filter entities.</param>
        /// <returns>Entity <see cref="T"/> instance.</returns>
        public T Get<T>(QueryParameters filter) where T : Entity
        {
            return GetRepository<T>().Get(filter);
        }

        /// <summary>
        /// Gets entity by specified filter.
        /// </summary>
        /// <typeparam name="T">Entity type.</typeparam>
        /// <param name="id">Identifier of an entity.</param>
        /// <returns>Entity <see cref="T"/> instance.</returns>
        public T Get<T>(Guid id) where T : Entity
        {
            return GetRepository<T>().Get(id);
        }

        public T GetSingleOrDefault<T>(QueryParameters parameters) where T : Entity
        {
            return GetRepository<T>().GetSingleOrDefault(parameters);
        }

        public T GetFirstOrDefault<T>(QueryParameters parameters) where T : Entity
        {
            return GetRepository<T>().GetFirstOrDefault(parameters);
        }

        public T InsertOrUpdate<T>(T entity) where T : Entity
        {
            return GetRepository<T>().InsertOrUpdate(entity);
        }

        /// <summary>
        /// Delete entity from repository.
        /// </summary>
        /// <param name="entity">Entity to Delete.</param>
        /// <typeparam name="T">Entity type.</typeparam>
        public void Delete<T>(T entity) where T : Entity
        {
            GetRepository<T>().Delete(entity);
        }

        /// <summary>
        /// Gets Count of entities which match query parameters.
        /// </summary>
        /// <param name="parameters">Base query parameters.</param>
        /// <typeparam name="T">Entity type.</typeparam>
        public int Count<T>(QueryParameters parameters) where T : Entity
        {
            return GetRepository<T>().Count(parameters);
        }

        /// <summary>
        /// Gets the repository for specified entity type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected IRepository<T> GetRepository<T>() where T : Entity
        {
            return (IRepository<T>)repositories[typeof(T)];
        }

        /// <summary>
        /// Registers repository instance.
        /// </summary>
        /// <typeparam name="T">Entity type.</typeparam>
        /// <param name="repository">Repository instance.</param>
        protected void RegisterRepository<T>(IRepository<T> repository) where T : Entity
        {
            repositories[typeof(T)] = repository;
        }

        public T GetReload<T>(Guid id) where T : Entity
        {
            var entity = Get<T>(id);
            context.Entry(entity).Reload();
            return entity;
        }

        public T GetReload<T>(QueryParameters filter) where T : Entity
        {
            var entity = Get<T>(filter);
            context.Entry(entity).Reload();
            return entity;
        }

        protected override void DisposeManaged()
        {
            DisposeContext();
        }

        private void DisposeContext()
        {
            context?.Dispose();
        }
    }
}
