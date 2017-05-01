using System.Linq;
using Data.Challenges.Entities;
using Data.Common.Query.Builder;
using Shared.Framework.DataSource;
using Shared.Framework.Dependency;

namespace Data.Challenges.Context
{
    public interface ITagService : IDependency
    {
        TEntity Tag<TEntity>(TEntity entity, string str) where TEntity : ITaggable;
        TEntity Untag<TEntity>(TEntity entity, string str) where TEntity : ITaggable;
    }

    public class TagService : ITagService
    {
        private readonly IChallengesUnitOfWork unitOfWork;

        public TagService()
        { }

        public TagService(IChallengesUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public TEntity Tag<TEntity>(TEntity entity, string str) where TEntity : ITaggable
        {
            var tag = str.Trim();

            var queryParameters = FilterSettingsBuilder<Tag>.Create()
                .AddFilterRule(t => t.Value, FilterOperator.IsEqualTo, tag)
                .ToListQueryParameters();

            if (entity.Tags.Any(t => t.Value == tag))
            {
                return entity;
            }

            var tagEntity = unitOfWork.TagsRepository.GetFirstOrDefault(queryParameters) ??
                            unitOfWork.TagsRepository.InsertOrUpdate(new Tag { Value = tag });

            entity.Tags.Add(tagEntity);

            return entity;
        }

        public TEntity Untag<TEntity>(TEntity entity, string str) where TEntity : ITaggable
        {
            var tag = entity.Tags.FirstOrDefault(x => x.Value == str);

            if (tag != null)
                entity.Tags.Remove(tag);

            return entity;
        }
    }
}
