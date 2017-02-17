using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Interception;
using System.Linq;
using System.Reflection;
using Data.Challenges.Entities;
using Data.Common.FullTextSearch;

namespace Data.Challenges.Context
{
    public class ChallengesContext : DbContext, ITaggableContext
    {
        public virtual DbSet<Challenge> Challenges
        {
            get;
            set;
        }

        public virtual DbSet<Tag> Tags
        {
            get;
            set;
        }

        public ChallengesContext() : this("ChallengesConnection")
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            var res = assembly.GetManifestResourceNames();
        }

        public ChallengesContext(string connectionName) : base(connectionName)
        {
            DbInterception.Add(new FtsInterceptor());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Challenge>()
                        .HasMany(a => a.Comments)
                        .WithOptional()
                        .WillCascadeOnDelete();

            modelBuilder.Entity<Challenge>()
                        .HasMany(a => a.Answers)
                        .WithOptional()
                        .WillCascadeOnDelete();

            modelBuilder.Entity<Challenge>()
                        .HasMany(a => a.Solvers)
                        .WithOptional()
                        .WillCascadeOnDelete();
        }
    }

    public interface ITaggableContext
    {
        DbSet<Tag> Tags
        {
            get;
            set;
        }
    }
}