using System;
using System.Collections.Generic;
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
        }

        private void ConfigureTestCasesMap()
        {
            CreateMap<TestCaseViewModel, TestCase>()
                .ForMember(e => e.Id, o => o.Ignore())
                .ForMember(e => e.State, o => o.Ignore())
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
                .ForMember(vm => vm.InputParameters, o => o.ResolveUsing(e => 
                    e.InputParameters.Select(x => x.Value)))
                .ForMember(vm => vm.OutputParameters, o => o.ResolveUsing(e => 
                    e.OutputParameters.Select(x => x.Value)));
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
                .ForMember(t => t.Title, o => o.MapFrom(s => s.Title))
                .ForMember(t => t.SourceCode, o => o.Ignore());
                
            CreateMap<ChallengeViewModel, Challenge>()
                .ForMember(t => t.Id, o => o.MapFrom(s => s.Id))
                .ForMember(t => t.Answers, o =>
                {
                    o.Condition(s => s.ChallengeType == ChallengeType.TextAnswered);
                    o.MapFrom(s => GetEntityAnswers(s.Answers));
                })
                .ForMember(t => t.AuthorId, o => o.MapFrom(s => s.AuthorId))
                .ForMember(t => t.Condition, o => o.MapFrom(s => s.Condition))
                .ForMember(t => t.Difficulty, o => o.MapFrom(s => s.Difficulty))
                .ForMember(t => t.Language, o => o.MapFrom(s => s.Language))
                .ForMember(t => t.PreviewText, o => o.MapFrom(s => s.PreviewText))
                .ForMember(t => t.Section, o => o.MapFrom(s => s.Section))
                .ForMember(t => t.Tags, o => o.MapFrom(s => GetEntityTags(s.Tags)))
                .ForMember(t => t.Title, o => o.MapFrom(s => s.Title))
                .ForMember(t => t.TestCases, o =>
                {
                    o.Condition(s => s.ChallengeType == ChallengeType.CodeAnswered);
                    o.MapFrom(s => s.TestCases);
                })
                .ForMember(t => t.SolutionSourceCode, o =>
                {
                    o.Condition(s => s.ChallengeType == ChallengeType.CodeAnswered);
                    o.MapFrom(s => s.SourceCode);
                })
                .ForMember(t => t.Rating, o => o.Ignore())
                .ForMember(t => t.NumberOfVotes, o => o.Ignore())
                .ForMember(t => t.TimeCreated, o => o.Ignore())
                .ForMember(t => t.TimesSolved, o => o.Ignore())
                .ForMember(t => t.State, o => o.Ignore());

            CreateMap<Challenge, ChallengeFullViewModel>()
                .ForMember(t => t.Id, o => o.MapFrom(s => s.Id))
                .ForMember(t => t.Answers, o => o.MapFrom(s => s.Answers.Select(x => x.Value).ToList()))
                .ForMember(t => t.AuthorId, o => o.MapFrom(s => s.AuthorId))
                .ForMember(t => t.Condition, o => o.MapFrom(s => s.Condition))
                .ForMember(t => t.Difficulty, o => o.MapFrom(s => s.Difficulty))
                .ForMember(t => t.Language, o => o.MapFrom(s => s.Language))
                .ForMember(t => t.PreviewText, o => o.MapFrom(s => s.PreviewText))
                .ForMember(t => t.Section, o => o.MapFrom(s => s.Section))
                .ForMember(t => t.Tags, o => o.MapFrom(s => s.Tags.Select(x => x.Value)))
                .ForMember(t => t.Title, o => o.MapFrom(s => s.Title))
                .ForMember(t => t.Comments, o => o.MapFrom(s => s.Comments))
                .ForMember(t => t.Rating, o => o.MapFrom(s => s.Rating))
                .ForMember(t => t.TimesSolved, o => o.MapFrom(s => s.TimesSolved))
                .ForMember(t => t.SourceCode, o => o.Ignore());

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
                .ForMember(t => t.TimesSolved, o => o.MapFrom(s => s.TimesSolved))
                .ForMember(t => t.NumberOfVotes, o => o.Ignore())
                .ForMember(t => t.TimeCreated, o => o.Ignore())
                .ForMember(t => t.TimesSolved, o => o.Ignore())
                .ForMember(t => t.State, o => o.Ignore())
                .ForMember(t => t.SolutionSourceCode, o => o.Ignore());

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
                .ForMember(t => t.HasSolved, o => o.MapFrom(s => s.HasSolved));


            CreateMap<SolverViewModel, Solver>()
                .ForMember(t => t.Id, o => o.MapFrom(s => s.UserId))
                .ForMember(t => t.HasSolved, o => o.MapFrom(s => s.HasSolved))
                .ForMember(t => t.HasRated, o => o.Ignore())
                .ForMember(t => t.NumberOfTries, o => o.Ignore())
                .ForMember(t => t.State, o => o.Ignore());
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
    }
}