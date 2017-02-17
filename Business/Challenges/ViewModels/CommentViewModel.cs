using System;

namespace Business.Challenges.ViewModels
{
    public class CommentViewModel
    {
        public Guid Id
        {
            get;
            set;
        }

        public string UserName
        {
            get;
            set;
        }

        public string Value
        {
            get;
            set;
        }
    }
}