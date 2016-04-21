using System;
using System.Collections.Generic;
using System.Linq;
using Data.Challenges.Entities;
using Shared.Framework.Resources;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Data.Challenges.Enums;
using Shared.Framework.Validation;

namespace Business.Challenges.ViewModels
{
    public class ChallengeViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(Localization),
            ErrorMessageResourceName = "IsRequired")]
        [StringLength(100, ErrorMessageResourceType = typeof(Localization),
            ErrorMessageResourceName = "LengthBetweenMessage", MinimumLength = 6)]
        [Display(ResourceType = typeof(Localization), Name = "Title")]
        public string Title { get; set; }

        [StringLength(1000, ErrorMessageResourceType = typeof(Localization),
            ErrorMessageResourceName = "LengthBetweenMessage", MinimumLength = 20)]
        [Required(ErrorMessageResourceType = typeof(Localization),
                            ErrorMessageResourceName = "IsRequired")]
        [Display(ResourceType = typeof(Localization), Name = "PreviewText")]
        [AllowHtml]
        public string PreviewText { get; set; }

        [StringLength(1000, ErrorMessageResourceType = typeof(Localization),
            ErrorMessageResourceName = "LengthBetweenMessage", MinimumLength = 20)]
        [Required(ErrorMessageResourceType = typeof(Localization),
                            ErrorMessageResourceName = "IsRequired")]
        [Display(ResourceType = typeof(Localization), Name = "Condition")]
        [AllowHtml]
        public string Condition { get; set; }

        [StringLength(100, ErrorMessageResourceType = typeof(Localization),
            ErrorMessageResourceName = "LengthBetweenMessage", MinimumLength = 3)]
        [Display(ResourceType = typeof(Localization), Name = "Tags")]
        public string Tags { get; set; }

        [Required]
        [CollectionMinimumLengthValidation(1, 5)]
        [Display(ResourceType = typeof(Localization), Name = "OfAnswers")]
        public List<string> Answers { get; set; }

        [Required]
        [Range(1, 5)]
        [Display(ResourceType = typeof(Localization), Name = "Difficulty")]
        public byte Difficulty { get; set; }

        [Required]
        [Display(ResourceType = typeof(Localization), Name = "Section")]
        public Section Section { get; set; }

        [Required]
        [Display(ResourceType = typeof(Localization), Name = "Language")]
        public Language Language { get; set; }

        public ChallengeViewModel() { }

        public ChallengeViewModel(Challenge challenge)
        {
            Id = challenge.Id;
            Title = challenge.Title;
            PreviewText = challenge.PreviewText;
            Condition = challenge.Condition;
            Tags = "";
            foreach (var tag in challenge.Tags)
                Tags += $"{tag.Value} ";
            Answers = new List<string>();
            foreach (var answer in challenge.Answers)
            {
                Answers.Add(answer.Value);
            }
            Difficulty = challenge.Difficulty;
            Section = challenge.Section;
            Language = challenge.Language;
        }

        public Challenge ToChallenge(string userId)
        {
            var challenge = new Challenge
            {
                Id = Id,
                Section = Section,
                AuthorId = userId,
                PreviewText = PreviewText,
                Condition = Condition,
                Answers = GetAnswers(),
                Difficulty = Difficulty,
                Language = Language,
                Title = Title,
                Tags = GetTags()
            };

            return challenge;
        }

        private List<Tag> GetTags()
        {
            return Tags?.Trim().Split(new[] { ",", " " }, StringSplitOptions.RemoveEmptyEntries).Select(tag => new Tag { Value = tag }).ToList();
        }

        private List<Answer> GetAnswers()
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