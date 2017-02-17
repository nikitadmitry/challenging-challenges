using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Business.Challenges.ViewModels;
using Business.SearchIndex;
using Data.Challenges.Context;
using Data.Challenges.Entities;
using Lucene.Net.Search;
using Shared.Framework.Utilities;

namespace Business.Challenges.Private.SearchStrategies
{
    public abstract class IndexedSearchStrategyBase : ISearchStrategy
    {
        private readonly ISearchIndexService searchIndexService;
        private readonly IChallengesUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        protected abstract ChallengeSearchType SearchType { get; }

        protected IndexedSearchStrategyBase(ISearchIndexService searchIndexService,
            IChallengesUnitOfWork unitOfWork, IMapper mapper)
        {
            this.searchIndexService = searchIndexService;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public bool IsApplicable(ChallengeSearchType searchType)
        {
            return searchType == SearchType;
        }

        public List<ChallengeInfoViewModel> Search(ChallengesPageRule pageRule)
        {
            var list = searchIndexService.Search(new Sort(new SortField("Id", SortField.STRING, true)),
                new[] {SearchType.ToString()}, pageRule.Keyword, pageRule.Start == 0 ? 0 : pageRule.Start / pageRule.Count, 
                pageRule.Count);

            if (list.IsNotNull())
            {
                return mapper.Map<List<ChallengeInfoViewModel>>(
                    list.Select(searchIndex => unitOfWork.Get<Challenge>(searchIndex.Id)));
            }

            return new List<ChallengeInfoViewModel>();
        }
    }
}