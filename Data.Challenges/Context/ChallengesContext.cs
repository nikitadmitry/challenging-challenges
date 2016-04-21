using System.Data.Entity;
using Data.Challenges.Entities;

namespace Data.Challenges.Context
{
    public class ChallengesContext : DbContext, ITaggableContext
    {
        public ChallengesContext()
            : this("ChallengesConnection")
        {
            
        }

        public ChallengesContext(string connectionName) : base(connectionName)
        {
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