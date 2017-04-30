﻿using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Business.Challenges.ViewModels;
using Data.Challenges.Entities;
using Data.Challenges.Enums;
using ChallengeType = Business.Challenges.ViewModels.ChallengeType;

namespace Business.Challenges.Mappings
{
    public class ChallengesMapProfile: Profile
    {
        public ChallengesMapProfile()
        {
            ConfigureTestCasesMap();
            ConfigureChallengeMap();
            ConfigureSolversMap();
            ConfigureCommentsMap();
            ConfigureChallengeDetailsModelMap();
        }

        private void ConfigureChallengeDetailsModelMap()
        {
            CreateMap<Challenge, ChallengeDetailsModel>()
                .ForMember(x => x.AuthorId, o => o.MapFrom(s => s.AuthorId))
                .ForMember(x => x.AuthorName, o => o.Ignore())
                .ForMember(x => x.IsAuthor, o => o.Ignore())
                .ForMember(x => x.IsSolved, o => o.Ignore())
                .ForMember(x => x.ChallengeType, o => o.MapFrom(s => s.ChallengeType))
                .ForMember(x => x.Condition, o => o.MapFrom(s => s.Condition))
                .ForMember(x => x.Difficulty, o => o.MapFrom(s => s.Difficulty))
                .ForMember(x => x.Id, o => o.MapFrom(s => s.Id))
                .ForMember(x => x.Rating, o => o.MapFrom(s => s.Rating))
                .ForMember(x => x.Section, o => o.MapFrom(s => s.Section))
                .ForMember(x => x.Title, o => o.MapFrom(s => s.Title))
                .ForMember(x => x.AnswerTemplate, o => o.Ignore());
        }

        private void ConfigureTestCasesMap()
        {
            CreateMap<TestCaseViewModel, TestCase>()
                .ForMember(e => e.Id, o => o.Ignore())
                .ForMember(e => e.State, o => o.Ignore())
                .ForMember(e => e.IsPublic, o => o.MapFrom(wm => wm.IsPublic))
                .ForMember(e => e.CodeParameters, o => o.ResolveUsing(wm =>
                {
                    var codeParameters = wm.InputParameters.Select(inputParameter =>
                        new CodeParameter
                        {
                            Type = CodeParameterType.Input,
                            Value = inputParameter
                        }).ToList();

                    codeParameters.AddRange(wm.OutputParameters.Select(outputParameter =>
                        new CodeParameter
                        {
                            Type = CodeParameterType.Output,
                            Value = outputParameter
                        }));

                    return codeParameters;
                }));

            CreateMap<TestCase, TestCaseViewModel>()
                .ForMember(vm => vm.IsPublic, o => o.MapFrom(e => e.IsPublic))
                .ForMember(vm => vm.InputParameters, o => o.ResolveUsing(e => 
                    e.InputParameters.Select(x => x.Value)))
                .ForMember(vm => vm.OutputParameters, o => o.ResolveUsing(e => 
                    e.OutputParameters.Select(x => x.Value)));
        }

        private void ConfigureChallengeMap()
        {
            CreateMap<Challenge, EditChallengeViewModel>()
                .ForMember(t => t.Id, o => o.MapFrom(s => s.Id))
                .ForMember(t => t.Answers, o => o.MapFrom(s => s.Answers.Select(x => x.Value).ToList()))
                .ForMember(t => t.AuthorId, o => o.MapFrom(s => s.AuthorId))
                .ForMember(t => t.Condition, o => o.MapFrom(s => s.Condition))
                .ForMember(t => t.Difficulty, o => o.MapFrom(s => s.Difficulty))
                .ForMember(t => t.Language, o => o.MapFrom(s => s.Language))
                .ForMember(t => t.PreviewText, o => o.MapFrom(s => s.PreviewText))
                .ForMember(t => t.Section, o => o.MapFrom(s => s.Section))
                .ForMember(t => t.Tags, o => o.MapFrom(s => s.Tags.Select(x => x.Value).ToList()))
                .ForMember(t => t.Title, o => o.MapFrom(s => s.Title))
                .ForMember(t => t.SourceCode, o => o.Ignore());

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
                .ForMember(t => t.Tags, o => o.MapFrom(s => s.Tags.Select(x => x.Value)))
                .ForMember(t => t.TimesSolved, o => o.MapFrom(s => s.TimesSolved));
        }

        private void ConfigureSolversMap()
        {
            CreateMap<Solver, SolverViewModel>()
                .ForMember(t => t.UserId, o => o.MapFrom(s => s.Id))
                .ForMember(t => t.HasSolved, o => o.MapFrom(s => s.HasSolved))
                .ForMember(t => t.ChallengeId, o => o.MapFrom(s => s.ChallengeId));


            CreateMap<SolverViewModel, Solver>()
                .ForMember(t => t.Id, o => o.MapFrom(s => s.UserId))
                .ForMember(t => t.HasSolved, o => o.MapFrom(s => s.HasSolved))
                .ForMember(t => t.HasRated, o => o.Ignore())
                .ForMember(t => t.NumberOfTries, o => o.Ignore())
                .ForMember(t => t.State, o => o.Ignore())
                .ForMember(t => t.ChallengeId, o => o.MapFrom(s => s.ChallengeId));
        }

        private void ConfigureCommentsMap()
        {
            CreateMap<Comment, CommentViewModel>()
                .ForMember(t => t.Id, o => o.MapFrom(s => s.Id))
                .ForMember(t => t.Value, o => o.MapFrom(s => s.Value))
                .ForMember(t => t.UserName, o => o.Ignore());


            CreateMap<CommentViewModel, Comment>()
                .ForMember(t => t.Id, o => o.MapFrom(s => s.Id))
                .ForMember(t => t.Value, o => o.MapFrom(s => s.Value))
                .ForMember(t => t.UserId, o => o.Ignore())
                .ForMember(t => t.State, o => o.Ignore());
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
    }
}