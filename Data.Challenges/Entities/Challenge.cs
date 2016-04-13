using System.Collections.Generic;
using Data.Challenges.Enums;
using Data.Common;

namespace Data.Challenges.Entities
{
    public class Challenge : Entity, ITaggable
    {
        public string AuthorId
        {
            get;
            set;
        }

        public string Title
        {
            get;
            set;
        }

        public string PreviewText
        {
            get;
            set;
        }

        public string Condition
        {
            get;
            set;
        }

        public byte Difficulty
        {
            get;
            set;
        }

        public Section Section
        {
            get;
            set;
        }

        public Language Language
        {
            get;
            set;
        }

        public float Rating
        {
            get;
            set;
        } = 3;

        public int NumberOfVotes
        {
            get;
            set;
        } = 1;

        public int TimesSolved
        {
            get;
            set;
        }

        public virtual ICollection<Tag> Tags
        {
            get;
            set;
        }

        public virtual ICollection<Answer> Answers
        {
            get;
            set;
        }

        public virtual ICollection<Comment> Comments
        {
            get;
            set;
        }

        public virtual ICollection<Solver> Solvers
        {
            get;
            set;
        }

        public void BindChanges(Challenge challenge)
        {
            Title = challenge.Title;
            Section = challenge.Section;
            PreviewText = challenge.PreviewText;
            Condition = challenge.Condition;
            Answers = challenge.Answers;
            Difficulty = challenge.Difficulty;
            Language = challenge.Language;
        }
    }

    public interface ITaggable
    {
        ICollection<Tag> Tags
        {
            get;
        }
    }
}
