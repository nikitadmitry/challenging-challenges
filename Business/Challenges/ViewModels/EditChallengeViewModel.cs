using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Data.Challenges.Enums;
using Shared.Framework.Extensions;

namespace Business.Challenges.ViewModels
{
    public class EditChallengeViewModel
    {
        public Guid? Id
        {
            get;
            set;
        }

        public bool IsNew => Id.IsNullOrEmpty();

        [Required]
        [StringLength(80, MinimumLength = 4)]
        public string Title
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

        public List<string> Tags
        {
            get;
            set;
        }

        public List<string> Answers
        {
            get;
            set;
        }

        public IList<TestCaseViewModel> TestCases
        {
            get;
            set;
        }

        public Difficulty Difficulty
        {
            get;
            set;
        }

        public BusinessSection Section
        {
            get;
            set;
        }

        public Language Language
        {
            get;
            set;
        }

        public ChallengeType ChallengeType => CodeAnswered ? ChallengeType.CodeAnswered 
            : ChallengeType.TextAnswered;

        public bool CodeAnswered
        {
            get;
            set;
        }

        public string SourceCode
        {
            get;
            set;
        }

        public Guid AuthorId
        {
            get;
            set;
        }
    }
}