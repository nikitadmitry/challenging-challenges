using System;
using System.Collections.Generic;
using Data.Challenges.Enums;

namespace Business.Challenges.ViewModels
{
    public class ChallengeInfoViewModel
    {
        public Guid Id
        {
            get;
            set;
        }

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

        public double Rating
        {
            get;
            set;
        }

        public int TimesSolved
        {
            get;
            set;
        }

        public Difficulty Difficulty
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

        public List<string> Tags
        {
            get;
            set;
        }
    }
}