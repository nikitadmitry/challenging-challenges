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
    public class SectionSearchStrategy : EnumSearchStrategy<Section>
    {
        public SectionSearchStrategy(IChallengesUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        protected override ChallengeSearchType SearchType => ChallengeSearchType.Section;

        protected override IDictionary<Section, string[]> SearchStrings { get; } = new Dictionary<Section, string[]>
        {
            {Section.CSharp, new []{"c#","csharp","sharp","#",".net"}},
            {Section.Java, new []{"java","oracle"}},
            {Section.Python, new []{"python","py","питон"}},
            {Section.Ruby, new []{"ruby"}},
            {Section.Other, new []{"other","другое"}}
        };

        protected override Expression<Func<Challenge, Section>> PropertyExpression => x => x.Section;
    }
}