using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Business.Challenges.ViewModels;
using Data.Challenges.Entities;

namespace Business.Challenges.Mappings
{
    public class ChallengesMapProfile: Profile
    {
        public ChallengesMapProfile()
        {
            ConfigureChallengeMap();
            ConfigureSolversMap();
            ConfigureCommentsMap();
        }
        
        private void ConfigureChallengeMap()
        {
            CreateMap<Challenge, ChallengeViewModel>()
                .ForMember(t => t.Id, o => o.MapFrom(s => s.Id))
                .ForMember(t => t.Answers, o => o.MapFrom(s => s.Answers.Select(x => x.Value).ToList()))
                .ForMember(t => t.AuthorId, o => o.MapFrom(s => s.AuthorId))
                .ForMember(t => t.Condition, o => o.MapFrom(s => s.Condition))
                .ForMember(t => t.Difficulty, o => o.MapFrom(s => s.Difficulty))
                .ForMember(t => t.Language, o => o.MapFrom(s => s.Language))
                .ForMember(t => t.PreviewText, o => o.MapFrom(s => s.PreviewText))
                .ForMember(t => t.Section, o => o.MapFrom(s => s.Section))
                .ForMember(t => t.Tags, o => o.MapFrom(s => s.Tags.Select(x => x.Value).ToList()))
                .ForMember(t => t.Title, o => o.MapFrom(s => s.Title));
                

            CreateMap<ChallengeViewModel, Challenge>()
                .ForMember(t => t.Id, o => o.MapFrom(s => s.Id))
                .ForMember(t => t.Answers, o => o.MapFrom(s => GetEntityAnswers(s.Answers)))
                .ForMember(t => t.AuthorId, o => o.MapFrom(s => s.AuthorId))
                .ForMember(t => t.Condition, o => o.MapFrom(s => s.Condition))
                .ForMember(t => t.Difficulty, o => o.MapFrom(s => s.Difficulty))
                .ForMember(t => t.Language, o => o.MapFrom(s => s.Language))
                .ForMember(t => t.PreviewText, o => o.MapFrom(s => s.PreviewText))
                .ForMember(t => t.Section, o => o.MapFrom(s => s.Section))
                .ForMember(t => t.Tags, o => o.MapFrom(s => GetEntityTags(s.Tags)))
                .ForMember(t => t.Title, o => o.MapFrom(s => s.Title));

            CreateMap<Challenge, ChallengeFullViewModel>()
                .ForMember(t => t.Id, o => o.MapFrom(s => s.Id))
                .ForMember(t => t.Answers, o => o.MapFrom(s => s.Answers.Select(x => x.Value).ToList()))
                .ForMember(t => t.AuthorId, o => o.MapFrom(s => s.AuthorId))
                .ForMember(t => t.Condition, o => o.MapFrom(s => s.Condition))
                .ForMember(t => t.Difficulty, o => o.MapFrom(s => s.Difficulty))
                .ForMember(t => t.Language, o => o.MapFrom(s => s.Language))
                .ForMember(t => t.PreviewText, o => o.MapFrom(s => s.PreviewText))
                .ForMember(t => t.Section, o => o.MapFrom(s => s.Section))
                .ForMember(t => t.Tags, o => o.MapFrom(s => GetViewModelTags(s.Tags.ToList())))
                .ForMember(t => t.Title, o => o.MapFrom(s => s.Title))
                .ForMember(t => t.Comments, o => o.MapFrom(s => s.Comments))
                .ForMember(t => t.Rating, o => o.MapFrom(s => s.Rating))
                .ForMember(t => t.TimesSolved, o => o.MapFrom(s => s.TimesSolved));

            CreateMap<ChallengeFullViewModel, Challenge>()
                .ForMember(t => t.Id, o => o.MapFrom(s => s.Id))
                .ForMember(t => t.Answers, o => o.MapFrom(s => GetEntityAnswers(s.Answers)))
                .ForMember(t => t.AuthorId, o => o.MapFrom(s => s.AuthorId))
                .ForMember(t => t.Condition, o => o.MapFrom(s => s.Condition))
                .ForMember(t => t.Difficulty, o => o.MapFrom(s => s.Difficulty))
                .ForMember(t => t.Language, o => o.MapFrom(s => s.Language))
                .ForMember(t => t.PreviewText, o => o.MapFrom(s => s.PreviewText))
                .ForMember(t => t.Section, o => o.MapFrom(s => s.Section))
                .ForMember(t => t.Tags, o => o.MapFrom(s => GetEntityTags(s.Tags)))
                .ForMember(t => t.Title, o => o.MapFrom(s => s.Title))
                .ForMember(t => t.Comments, o => o.MapFrom(s => s.Comments))
                .ForMember(t => t.Rating, o => o.MapFrom(s => s.Rating))
                .ForMember(t => t.TimesSolved, o => o.MapFrom(s => s.TimesSolved));

            CreateMap<Challenge, ChallengesDescriptionViewModel>()
                .ForMember(t => t.Id, o => o.MapFrom(s => s.Id))
                .ForMember(t => t.Title, o => o.MapFrom(s => s.Title))
                .ForMember(t => t.PreviewText, o => o.MapFrom(s => s.PreviewText));

            CreateMap<Challenge, ChallengeInfoViewModel>()
                .ForMember(t => t.Id, o => o.MapFrom(s => s.Id))
                .ForMember(t => t.Title, o => o.MapFrom(s => s.Title))
                .ForMember(t => t.PreviewText, o => o.MapFrom(s => s.PreviewText))
                .ForMember(t => t.AuthorId, o => o.MapFrom(s => s.AuthorId))
                .ForMember(t => t.Difficulty, o => o.MapFrom(s => s.Difficulty))
                .ForMember(t => t.Language, o => o.MapFrom(s => s.Language))
                .ForMember(t => t.Rating, o => o.MapFrom(s => s.Rating))
                .ForMember(t => t.Section, o => o.MapFrom(s => s.Section))
                .ForMember(t => t.Tags, o => o.MapFrom(s => GetViewModelTags(s.Tags.ToList())))
                .ForMember(t => t.TimesSolved, o => o.MapFrom(s => s.TimesSolved));
        }

        private void ConfigureSolversMap()
        {
            CreateMap<Solver, SolverViewModel>()
                .ForMember(t => t.UserId, o => o.MapFrom(s => s.Id))
                .ForMember(t => t.HasSolved, o => o.MapFrom(s => s.HasSolved));


            CreateMap<SolverViewModel, Solver>()
                .ForMember(t => t.Id, o => o.MapFrom(s => s.UserId))
                .ForMember(t => t.HasSolved, o => o.MapFrom(s => s.HasSolved));
        }

        private void ConfigureCommentsMap()
        {
            CreateMap<Comment, CommentViewModel>()
                .ForMember(t => t.Id, o => o.MapFrom(s => s.Id))
                .ForMember(t => t.Value, o => o.MapFrom(s => s.Value));


            CreateMap<CommentViewModel, Comment>()
                .ForMember(t => t.Id, o => o.MapFrom(s => s.Id))
                .ForMember(t => t.Value, o => o.MapFrom(s => s.Value));
        }

        private List<Tag> GetEntityTags(string tags)
        {
            return tags?.Trim().Split(new[] { ",", " " }, StringSplitOptions.RemoveEmptyEntries).Select(tag => new Tag { Value = tag }).ToList();
        }

        private List<Answer> GetEntityAnswers(List<string> answers)
        {
            List<Answer> answersList = new List<Answer>();

            int i = 0;
            foreach (string answer in answers.TakeWhile(answer => i != 5).Where(answer => !answer.Equals(string.Empty)))
            {
                answersList.Add(new Answer { Value = answer });
                i++;
            }

            return answersList;
        }

        private string GetViewModelTags(List<Tag> tags)
        {
            StringBuilder tagsAsString = new StringBuilder();

            foreach (var tag in tags)
            {
                tagsAsString.Append($"{tag.Value} ");
            }

            return tagsAsString.ToString();
        }
    }
}