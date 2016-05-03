using System;

namespace Business.SearchIndex.ViewModels
{
    public class SearchIndex
    {
        public Guid Id
        {
            get;
            set;
        }

        public string PreviewText
        {
            get;
            set;
        }

        public string Condition
        {
            get;
            set;
        }

        public string Tags
        {
            get;
            set;
        }
    }
}