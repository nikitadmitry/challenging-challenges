using System;
using System.Collections.Generic;
using System.ServiceModel;
using Lucene.Net.Search;

namespace Business.SearchIndex
{
    [ServiceContract]
    public interface ISearchIndexService
    {
        [OperationContract]
        void UpdateIndex();

        [OperationContract]
        void RemoveRecords(IEnumerable<Guid> ids);

        [OperationContract]
        void Optimize();

        [OperationContract]
        IEnumerable<string> GetTagsByTerm(string term, int limit);

        [OperationContract]
        IEnumerable<string> GetTags(int limit);

        [OperationContract]
        IEnumerable<ViewModels.SearchIndex> Search(Sort sort, string[] fieldNames, string input, int page, int limit);
    }
}