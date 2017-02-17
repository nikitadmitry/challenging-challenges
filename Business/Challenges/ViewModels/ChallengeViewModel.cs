﻿using System;
using System.Collections.Generic;
using System.Linq;
using Data.Challenges.Entities;
using System.ComponentModel.DataAnnotations;
using Data.Challenges.Enums;

namespace Business.Challenges.ViewModels
{
    public class ChallengeViewModel
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Title { get; set; }

        [StringLength(1000, MinimumLength = 20)]
        [Required]
        public string PreviewText { get; set; }

        [StringLength(1000, MinimumLength = 20)]
        [Required]
        public string Condition { get; set; }

        [StringLength(100, MinimumLength = 3)]
        public string Tags { get; set; }

        public List<string> Answers { get; set; }

        public IList<TestCaseViewModel> TestCases
        {
            get;
            set;
        }

        public Difficulty Difficulty { get; set; }

        public Section Section { get; set; }

        public Language Language { get; set; }

        public ChallengeType ChallengeType { get; set; }

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

        public List<Tag> GetTags()
        {
            return Tags?.Trim().Split(new[] { ",", " " }, StringSplitOptions.RemoveEmptyEntries).Select(tag => new Tag { Value = tag }).ToList();
        }

        public List<Answer> GetAnswers()
        {
            List<Answer> answersList = new List<Answer>();

            int i = 0;
            foreach (string answer in Answers.TakeWhile(answer => i != 5).Where(answer => !answer.Equals(String.Empty)))
            {
                answersList.Add(new Answer { Value = answer });
                i++;
            }

            return answersList;
        }
    }
}