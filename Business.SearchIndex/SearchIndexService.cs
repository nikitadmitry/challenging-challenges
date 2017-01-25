using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Data.Challenges.Context;
using Data.Challenges.Entities;
using Lucene.Net.Search;
using System.ServiceModel;

namespace Business.SearchIndex
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true, InstanceContextMode = InstanceContextMode.PerCall)]
    public class SearchIndexService: ISearchIndexService
    {
        private readonly IChallengesUnitOfWork challengesUnitOfWork;
        private readonly IMapper mapper;

        public SearchIndexService(IChallengesUnitOfWork challengesUnitOfWork, IMapper mapper)
        {
            this.challengesUnitOfWork = challengesUnitOfWork;
            this.mapper = mapper;
        }

        public void UpdateIndex()
        {
            var challenges = challengesUnitOfWork.GetAll<Challenge>();

            var searchIndices = mapper.Map<List<ViewModels.SearchIndex>>(challenges);

            LuceneSearch.AddUpdateLuceneIndex(searchIndices);
        }

        public void RemoveRecords(IEnumerable<Guid> ids)
        {
            foreach (var id in ids)
            {
                LuceneSearch.ClearLuceneIndexRecord(id);
            }
        }

        public void Optimize()
        {
            LuceneSearch.Optimize();
        }

        public IEnumerable<string> GetTagsByTerm(string term, int limit)
        {
            var searchIndices = LuceneSearch.Search(Sort.RELEVANCE, term, "Tags", 0, limit);

            List<string> tags = new List<string>();

            foreach (var searchIndex in searchIndices)
            {
                tags.AddRange(searchIndex.Tags.Split(' '));
            }

            return tags.Where(tag => tag.StartsWith(term) && !tag.Equals(string.Empty));
        }

        public IEnumerable<string> GetTags(int limit)
        {
            return GetTagsByTerm(string.Empty, limit);
        }

        public IEnumerable<ViewModels.SearchIndex> Search(Sort sort, string input, string fieldName, int page, int limit)
        {
            return LuceneSearch.Search(sort, input, fieldName, page, limit);
        }
    }
}