using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Business.Challenges.ViewModels;
using Data.Challenges.Entities;
using Shared.Framework.Automapper;

namespace Business.Challenges.Mappings
{
    public class ChallengesMapProfile: Profile
    {
        protected override void Configure()
        {
            ConfigureChallengeMap();
            ConfigureSolversMap();
            ConfigureCommentsMap();
        }
        
        private void ConfigureChallengeMap()
        {
            CreateMap<Challenge, ChallengeViewModel>()
                .IgnoreAllUnmapped()
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
                .IgnoreAllUnmapped()
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
                .IgnoreAllUnmapped()
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
                .ForMember(t => t.Solvers, o => o.MapFrom(s => s.Solvers))
                .ForMember(t => t.TimesSolved, o => o.MapFrom(s => s.TimesSolved));

            CreateMap<ChallengeFullViewModel, Challenge>()
                .IgnoreAllUnmapped()
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
                .ForMember(t => t.Solvers, o => o.MapFrom(s => s.Solvers))
                .ForMember(t => t.TimesSolved, o => o.MapFrom(s => s.TimesSolved));

            CreateMap<Challenge, ChallengesDescriptionViewModel>()
                .IgnoreAllUnmapped()
                .ForMember(t => t.Id, o => o.MapFrom(s => s.Id))
                .ForMember(t => t.Title, o => o.MapFrom(s => s.Title))
                .ForMember(t => t.PreviewText, o => o.MapFrom(s => s.PreviewText));

            CreateMap<Challenge, ChallengeInfoViewModel>()
                .IgnoreAllUnmapped()
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
                .IgnoreAllUnmapped()
                .ForMember(t => t.UserId, o => o.MapFrom(s => s.Id))
                .ForMember(t => t.HasSolved, o => o.MapFrom(s => s.HasSolved));


            CreateMap<SolverViewModel, Solver>()
                .IgnoreAllUnmapped()
                .ForMember(t => t.Id, o => o.MapFrom(s => s.UserId))
                .ForMember(t => t.HasSolved, o => o.MapFrom(s => s.HasSolved));
        }

        private void ConfigureCommentsMap()
        {
            CreateMap<Comment, CommentViewModel>()
                .IgnoreAllUnmapped()
                .ForMember(t => t.Id, o => o.MapFrom(s => s.Id))
                .ForMember(t => t.Value, o => o.MapFrom(s => s.Value))
                .ForMember(t => t.UserName, o => o.MapFrom(s => s.UserName));


            CreateMap<CommentViewModel, Comment>()
                .IgnoreAllUnmapped()
                .ForMember(t => t.Id, o => o.MapFrom(s => s.Id))
                .ForMember(t => t.Value, o => o.MapFrom(s => s.Value))
                .ForMember(t => t.UserName, o => o.MapFrom(s => s.UserName));
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