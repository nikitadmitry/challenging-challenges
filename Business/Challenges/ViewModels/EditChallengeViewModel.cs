using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Data.Challenges.Enums;

namespace Business.Challenges.ViewModels
{
    public class EditChallengeViewModel
    {
        public Guid ChallengeId
        {
            get;
            set;
        }

        [Required]
        [StringLength(80, MinimumLength = 4)]
        public string Title
        {
            get;
            set;
        }

        public BusinessSection Section
        {
            get;
            set;
        }

        public bool CodeAnswered
        {
            get;
            set;
        }

        public Difficulty Difficulty
        {
            get;
            set;
        }

        public Language Language
        {
            get;
            set;
        }

        [Required]
        [StringLength(600, MinimumLength = 50)]
        public string PreviewText
        {
            get;
            set;
        }

        [Required]
        [StringLength(3000, MinimumLength = 50)]
        public string Condition
        {
            get;
            set;
        }

        public string SourceCode
        {
            get;
            set;
        }

        public IList<string> Answers
        {
            get;
            set;
        }

        public IList<TestCaseViewModel> TestCases
        {
            get;
            set;
        }

        public IList<string> Tags
        {
            get;
            set;
        }
    }
}