using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using Data.Common.Query.QueryParameters;
using Data.Common.Query.Rules;
using Data.Common.Query.Settings;
using Shared.Framework.Exceptions;
using Shared.Framework.Utilities;
using Shared.Framework.Validation;

namespace Data.Common
{
    public class Repository<T> : IRepository<T>
        where T : Entity
    {
        private const string OrderByDescendingMethod = "OrderByDescending";
        private const string OrderByMethod = "OrderBy";
        private const string ThenByDescendingMethod = "ThenByDescending";
        private const string ThenByMethod = "ThenBy";

        private readonly DbContext context;

        public Repository(DbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Expose dbContext to subclasses.
        /// </summary>
        protected DbContext Context
        {
            get
            {
                return context;
            }
        }

        /// <summary>
        /// DbSet for current entity type.
        /// </summary>
        protected virtual IDbSet<T> DbSet
        {
            get
            {
                return context.Set<T>();
            }
        }

        public T InsertOrUpdate(T entity)
        {
            return InsertOrUpdateInternal(entity);
        }

        private static void CheckEntityModifiable(T entity)
        {
            if (!MultiMappingEntityAttribute.IsEntityModifiable(entity))
            {
                throw new ReferenceEntityModifyingException(entity.GetType());
            }
        }

        /// <summary>
        /// Delete entity from repository.
        /// </summary>
        /// <param name="entity">Entity to Delete.</param>
        public void Delete(T entity)
        {
            CheckEntityModifiable(entity);

            if (IsDetached(entity))
            {
                DbSet.Attach(entity);
            }

            MarkAsDeleted(entity);
            DbSet.Remove(entity);
        }

        /// <summary>
        /// Gets entity of type T from repository by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>Entity by id.</returns>
        public T Get(Guid id)
        {
            Contract.Requires<ArgumentNullException>(id != default(Guid), "id should not be empty");

            return OnGet(id);
        }

        protected virtual T OnGet(Guid id)
        {
            if (Context.IsNotNull())
            {
                Context.Configuration.AutoDetectChangesEnabled = false;
            }
            try
            {
                var entity = DbSet.Find(id);
                if (entity == null)
                {
                    throw new DataNotFoundException();
                }
                return entity;
            }
            finally
            {
                if (Context.IsNotNull())
                {
                    Context.Configuration.AutoDetectChangesEnabled = true;
                }
            }
        }

        public virtual T Reload(Guid id)
        {
            var entity = context.Set<T>().Find(id);

            context.Entry(entity).Reload();

            return entity;
        }

        /// <summary>
        /// Gets entity of type T from repository that satisfy parameters.
        /// </summary>
        /// <param name="queryParameters">Parameters to much conditions for entity.</param>
        /// <returns>Returns all entities of the type that satisfies criteria.</returns>
        public T Get(BaseQueryParameters queryParameters)
        {
            T entity = GetSingleOrDefault(queryParameters);

            if (entity == null)
            {
                throw new DataNotFoundException();
            }

            return entity;
        }

        /// <summary>
        /// Gets entity of type T from repository.
        /// </summary>
        /// <param name="queryParameters">Parameters to much conditions for entity.</param>
        /// <returns>Returns entity of the type that satisfies criteria or null.</returns>
        public T GetSingleOrDefault(BaseQueryParameters queryParameters)
        {
            return GetSingleQuery(queryParameters).SingleOrDefault();
        }

        public T GetFirstOrDefault(BaseQueryParameters queryParameters)
        {
            return GetListQuery(queryParameters).FirstOrDefault();
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <param name="parameters">The query.</param>
        /// <returns></returns>
        protected virtual IQueryable<T> GetAllQuery(BaseQueryParameters parameters)
        {
            var query = GetListQuery(parameters);
            return ApplyPageRule(query, parameters);
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <param name="parameters">The query.</param>
        /// <returns></returns>
        public virtual IList<T> GetAll(BaseQueryParameters parameters)
        {
            IQueryable<T> queryable = GetListQuery(parameters);
            return ExecuteQuery(queryable, parameters);
        }

        /// <summary>
        /// Gets all entities for the repository
        /// </summary>
        /// <returns></returns>
        public virtual IList<T> GetAll()
        {
            return GetAll(BaseQueryParameters.Empty);
        }

        /// <summary>
        /// Gets Count of entities which match query parameters.
        /// </summary>
        public int Count(BaseQueryParameters parameters)
        {
            IQueryable<T> queryable = GetListQuery(parameters);
            return queryable.Count();
        }

        /// <summary>
        /// Indicates is there any entity matching query parameters.
        /// </summary>
        public bool Any(BaseQueryParameters parameters)
        {
            IQueryable<T> queryable = GetListQuery(parameters);
            return queryable.Any();
        }

        private void MarkAsDeleted(T entity)
        {
            context.Entry(entity).State = EntityState.Deleted;
        }

        private void BeforeInsertOrUpdate(T entity)
        {
        }

        private bool IsDetached(T entity)
        {
            return context.Entry(entity).State == EntityState.Detached;
        }

        private void MarkAsModified(T entity)
        {
            context.Entry(entity).State = EntityState.Modified;
        }

        private IQueryable<T> ApplySingleQueryParameters(IQueryable<T> queryable,
                                                                   BaseQueryParameters parameters)
        {
            if (parameters == null)
            {
                return queryable;
            }

            if (parameters.FilterSettings != null)
            {
                queryable = ApplyFilterParameters(queryable, parameters);
            }
            else
            {
                if (!parameters.Ids.IsNullOrEmpty())
                {
                    Contract.Requires<ArgumentNullException>(!parameters.Ids.IsNullOrEmpty(), "id should be provided");
                    Contract.Requires<ArgumentNullException>(parameters.Ids.Count() == 1, "only one id can be provided");
                    var id = parameters.Ids.Single();
                    Contract.Requires<ArgumentNullException>(id != Guid.Empty, "id value should be provided");

                    queryable = queryable.Where(GetSingleEntityExpression(id));
                }
            }
            queryable = ApplySortSettingsIfExist(queryable, parameters);
            return queryable;
        }

        private IQueryable<T> GetListQuery(BaseQueryParameters parameters)
        {
            IQueryable<T> queryable = DbSet.AsQueryable();
            return ApplyListQueryParameters(queryable, parameters);
        }

        private Expression<Func<T, bool>> GetSingleEntityExpression(Guid id)
        {
            return x => x.Id == id;
        }

        /// <summary>
        /// Method applies sorting, filtering, localization and type specific parameters to queryable
        /// </summary>
        /// <param name="queryable">Queryable to apply parameters to</param>
        /// <param name="parameters">Parameters which should be applied to queryable</param>
        /// <returns>queryable with applied parameters</returns>
        private IQueryable<T> ApplyListQueryParameters(IQueryable<T> queryable, BaseQueryParameters parameters)
        {
            if (parameters != null)
            {
                if (parameters.Ids != null && parameters.Ids.Any())
                {
                    queryable = queryable.Where(GetEntityListExpression(parameters));
                }
                queryable = ApplySortSettingsIfExist(queryable, parameters);

                queryable = ApplyFilterParameters(queryable, parameters);
            }

            return ApplyListQueryParametersTypeSpecific(queryable, parameters);
        }

        /// <summary>
        /// This method may be overridden to provide custom Entity.Id expression
        /// </summary>
        /// <returns></returns>
        private Expression<Func<T, bool>> GetEntityListExpression(BaseQueryParameters parameters)
        {
            return x => parameters.Ids.Contains(x.Id);
        }

        /// <summary>
        /// Type specific query parameters should be applied here
        /// </summary>
        /// <param name="queryable">Queryable to apply parameters to</param>
        /// <param name="parameters">Parameters which should be applied to queryable</param>
        /// <returns>queryable with applied parameters</returns>
        private IQueryable<T> ApplyListQueryParametersTypeSpecific(IQueryable<T> queryable,
                                                                             BaseQueryParameters parameters)
        {
            return queryable;
        }

        private IQueryable<TEntity> ApplySortSettingsIfExist<TEntity>(IQueryable<TEntity> queryable, BaseQueryParameters parameters)
            where TEntity : Entity
        {
            if (parameters.SortSettings != null && parameters.SortSettings.SortRules != null &&
                parameters.SortSettings.SortRules.Any())
            {
                SortRule initialSortingRule = parameters.SortSettings.SortRules.First();
                queryable = ApplySortingRule(queryable, initialSortingRule, true);

                queryable = parameters.SortSettings.SortRules.Skip(1)
                    .Aggregate(queryable,
                        (current, sortRule) => ApplySortingRule(current, sortRule));
            }
            return queryable;
        }

        private IQueryable<TEntity> ApplyFilterParameters<TEntity>(IQueryable<TEntity> queryable, BaseQueryParameters parameters)
            where TEntity : Entity
        {
            if (parameters.FilterSettings != null && !parameters.FilterSettings.Rules.IsNullOrEmpty())
            {
                queryable = ApplyFilterSettings(queryable, parameters.FilterSettings);
            }

            return queryable;
        }

        private IQueryable<TEntity> ApplyFilterSettings<TEntity>(IQueryable<TEntity> queryable, FilterSettings filterSettings)
        {
            return filterSettings.Rules.Aggregate(queryable, ApplyFilterRule);
        }

        private IQueryable<TEntity> ApplyFilterRule<TEntity>(IQueryable<TEntity> queryable, IFilterRule filterRule)
        {
            var lambda = filterRule.GetExpression<TEntity>();

            MethodCallExpression result = Expression.Call(
                typeof(Queryable), "Where",
                new[]
                    {
                        queryable.ElementType
                    },
                queryable.Expression,
                lambda);

            return queryable.Provider.CreateQuery<TEntity>(result);
        }

        private IQueryable<TEntity> ApplySortingRule<TEntity>(IQueryable<TEntity> queryable, SortRule sortRule,
                                                                bool initialSorting = false)
            where TEntity : Entity
        {
            string methodName = GetSortingMethodName(sortRule, initialSorting);

            MethodCallExpression resultExp = Expression.Call(typeof(Queryable), methodName, new[]
                {
                    typeof(TEntity),
                    sortRule.Expression
                            .ReturnType
                }, queryable.Expression,
                                                             Expression.Quote(sortRule.Expression));
            return queryable.Provider.CreateQuery<TEntity>(resultExp);
        }

        private static string GetSortingMethodName(SortRule sortRule, bool initialSorting)
        {
            if (initialSorting)
            {
                return sortRule.SortDirection == Shared.Framework.DataSource.SortOrder.Desc ? OrderByDescendingMethod : OrderByMethod;
            }

            return sortRule.SortDirection == Shared.Framework.DataSource.SortOrder.Desc ? ThenByDescendingMethod : ThenByMethod;
        }

        private IQueryable<T> ApplyPageRule(IQueryable<T> queryable, BaseQueryParameters parameters)
        {
            var pageRule = parameters.PageRule;

            if (pageRule != null && pageRule.IsValid)
            {
                if (!queryable.IsOrdered())
                {
                    queryable = queryable.OrderBy(GetApplyPageRuleKeySelector());
                }

                if (pageRule.Start > 0)
                {
                    queryable = queryable.Skip(() => pageRule.Start);
                }

                parameters.TrackChanges = false;

                return queryable.Take(() => pageRule.Count);
            }

            return queryable;
        }

        private Expression<Func<T, Guid>> GetApplyPageRuleKeySelector()
        {
            return x => x.Id;
        }

        /// <summary>
        /// Executes query over database, returns SearchResult.
        /// </summary>
        /// <param name="wholeDatasetQueryable">Custom query to be executed</param>
        /// <param name="parameters">Parameters to apply.</param>
        /// <returns></returns>
        private IList<T> ExecuteQuery(IQueryable<T> wholeDatasetQueryable, BaseQueryParameters parameters)
        {
            if (parameters == null)
            {
                return wholeDatasetQueryable.ToList();
            }

            var pageViewQueryable = ApplyPageRule(wholeDatasetQueryable, parameters);

            var searchResultList = parameters.TrackChanges ? pageViewQueryable.ToList() : pageViewQueryable.AsNoTracking().ToList();

            return searchResultList;
        }

        private bool IsNew(T entity)
        {
            return entity.IsNew;
        }

        private IQueryable<T> GetSingleQuery(BaseQueryParameters queryParameters)
        {
            IQueryable<T> queryable = DbSet.AsQueryable();

            queryable = ApplySingleQueryParameters(queryable, queryParameters);

            return queryable;
        }

        private T InsertOrUpdateInternal(T entity, bool skipTrackingProperties = false)
        {
            BeforeInsertOrUpdate(entity);

            CheckEntityModifiable(entity);

            if (IsNew(entity))
            {
                return DbSet.Add(entity);
            }

            DbSet.Attach(entity);
            if (!skipTrackingProperties)
            {
                MarkAsModified(entity);
            }
            return entity;
        }
    }
}