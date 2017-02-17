using System.Linq;
using Data.Challenges.Entities;

namespace Data.Challenges.Context
{
    public interface ITagService
    {
        TEntity Tag<TEntity>(TEntity entity, string str) where TEntity : ITaggable;
        TEntity Untag<TEntity>(TEntity entity, string str) where TEntity : ITaggable;
    }

    public class TagService : ITagService
    {
        private readonly ITaggableContext context;

        public TagService()
        { }

        public TagService(ITaggableContext context)
        {
            this.context = context;
        }

        public TEntity Tag<TEntity>(TEntity entity, string str) where TEntity : ITaggable
        {
            string tag = str.Trim();

            if (entity.Tags.All(t => t.Value != tag))
            {
                var tagEntity = context.Tags.FirstOrDefault(t => t.Value == tag) ??
                                context.Tags.Add(new Tag { Value = tag });

                entity.Tags.Add(tagEntity);
            }

            return entity;
        }

        public TEntity Untag<TEntity>(TEntity entity, string str) where TEntity : ITaggable
        {
            Tag tag = entity.Tags.FirstOrDefault(x => x.Value == str);

            if (tag != null)
                entity.Tags.Remove(tag);

            return entity;
        }
    }
}
