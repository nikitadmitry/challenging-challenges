using System;
using System.Collections.Generic;
using Data.Challenges.Enums;
using Data.Common;

namespace Data.Challenges.Entities
{
    public class Challenge : Entity, ITaggable
    {
        public Guid AuthorId
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

        public double Rating
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

        public DateTime TimeCreated
        {
            get;
            set;
        }

        private List<Tag> tags; 

        public virtual IList<Tag> Tags
        {
            get
            {
                return tags ?? (tags = new List<Tag>());
            }
            set
            {
                tags = new List<Tag>(value);
            }
        }

        private List<Answer> answers; 

        public virtual IList<Answer> Answers
        {
            get
            {
                return answers ?? (answers = new List<Answer>());
            }
            set
            {
                answers = new List<Answer>(value);
            }
        }

        private List<Comment> comments; 

        public virtual IList<Comment> Comments
        {
            get
            {
                return comments ?? (comments = new List<Comment>());
            }
        }

        private List<Solver> solvers; 

        public virtual IList<Solver> Solvers
        {
            get
            {
                return solvers ?? (solvers = new List<Solver>());
            }
        }
    }

    public interface ITaggable
    {
        IList<Tag> Tags
        {
            get;
        }
    }
}
