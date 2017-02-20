using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using AutoMapper;
using Business.Challenges.ViewModels;
using Data.Challenges.Context;
using Data.Challenges.Entities;
using Data.Challenges.Enums;
using Shared.Framework.Dependency;

namespace Business.Challenges.Private.SearchStrategies
{
    [KeyedDependency(ChallengeSearchType.Difficulty)]
    public class DifficultySearchStrategy : EnumSearchStrategy<Difficulty>
    {
        public DifficultySearchStrategy(IChallengesUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        protected override ChallengeSearchType SearchType => ChallengeSearchType.Difficulty;

        protected override IDictionary<Difficulty, string[]> SearchStrings { get; } = new Dictionary<Difficulty, string[]>
        {
            {Difficulty.VeryEasy, new []{"very easy","очень легк","очень прост"}},
            {Difficulty.Easy, new []{"easy","легк","прост"}},
            {Difficulty.Intermediate, new []{"средн","intermediate","average"}},
            {Difficulty.Hard, new []{"hard","тяжел","тяжёл","сложн"}},
            {Difficulty.VeryHard, new []{"insane","very hard","очень сложн"}}
        };

        protected override Expression<Func<Challenge, Difficulty>> PropertyExpression => x => x.Difficulty;
    }
}