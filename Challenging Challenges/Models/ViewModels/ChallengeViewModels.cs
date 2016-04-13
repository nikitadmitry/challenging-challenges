using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Challenging_Challenges.Controllers;
using Challenging_Challenges.Enums;
using Challenging_Challenges.Helpers;
using Challenging_Challenges.Infrastructure;
using Challenging_Challenges.Models.Entities;
using Challenging_Challenges.Resources;
using Data.Challenges.Entities;
using Data.Challenges.Enums;
using Data.Identity.Repositories;
using Newtonsoft.Json;
using PagedList;

namespace Challenging_Challenges.Models.ViewModels
{
    public class HomeChallengeViewModel
    {
        public IPagedList<SearchIndex> LatestChallenges { get; set; }
        public IPagedList<SearchIndex> UnsolvedChallenges { get; set; }
        public IPagedList<SearchIndex> PopularChallenges { get; set; }
        public List<string> Tags { get; set; }
        public List<TopUser> TopUsers { get; set; }

        public HomeChallengeViewModel()
        {
            SearchService searchService = new SearchService();
            LatestChallenges = searchService.GetPagedList(SortType.Latest);
            UnsolvedChallenges = searchService.GetPagedList(SortType.Unsolved);
            PopularChallenges = searchService.GetPagedList(SortType.Popular);
            Tags = (List<string>) JsonConvert.DeserializeObject(JsonConvert.SerializeObject(new ChallengesController().TagSearch("", 50).Data), 
                typeof(List<string>));
            TopUsers = new IdentityContext().Users.OrderByDescending(x => x.Rating)
                        .Take(10)
                        .ToList()
                        .Select(x => new TopUser(x))
                        .ToList();
        }
    }

    public class ChallengeViewModel
    {
        [Key]
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
