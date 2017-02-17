using Lucene.Net.Search;

namespace Business.SearchIndex.ViewModels
{
    public class SearchRequest
    {
        public Sort Sort
        {
            get;
            set;
        }

        public string Term
        {
            get;
            set;
        }

        public string FieldName
        {
            get;
            set;
        }

        public int Page
        {
            get;
            set;
        }

        public int HitsLimit
        {
            get;
            set;
        }
    }
}