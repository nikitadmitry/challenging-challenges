using System.Linq;
using Data.Challenges.Entities;
using Data.Challenges.Repositories;

namespace Challenging_Challenges.Infrastructure
{
    public interface ITagService
    {
        TEntity Tag<TEntity>(TEntity entity, string str) where TEntity : ITaggable;
        TEntity Untag<TEntity>(TEntity entity, string str) where TEntity : ITaggable;
    }

    public class TagService : ITagService
    {
        private readonly ITaggableContext _context;

        public TagService()
        { }

        public TagService(ITaggableContext context)
        {
            _context = context;
        }

        public TEntity Tag<TEntity>(TEntity entity, string str) where TEntity : ITaggable
        {
            string tag = str.Trim();

            if (entity.Tags.All(t => t.Value != tag))
            {
                var tagEntity = _context.Tags.FirstOrDefault(t => t.Value == tag);

                if (tagEntity == null)
                    tagEntity = _context.Tags.Add(new Tag { Value = tag });
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
