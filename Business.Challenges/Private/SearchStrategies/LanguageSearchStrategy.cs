using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using AutoMapper;
using Business.Challenges.ViewModels;
using Data.Challenges.Context;
using Data.Challenges.Entities;
using Data.Challenges.Enums;

namespace Business.Challenges.Private.SearchStrategies
{
    public class LanguageSearchStrategy : EnumSearchStrategy<Language>
    {
        public LanguageSearchStrategy(IChallengesUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        protected override ChallengeSearchType SearchType => ChallengeSearchType.Language;
        protected override IDictionary<Language, string[]> SearchStrings { get; } = new Dictionary<Language, string[]>
        {
            {Language.English, new []{"eng","анг"}},
            {Language.Russian, new []{"рус","rus"}}
        };
        protected override Expression<Func<Challenge, Language>> PropertyExpression => x => x.Language;
    }
}