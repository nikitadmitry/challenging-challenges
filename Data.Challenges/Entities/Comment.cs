using Data.Common;

namespace Data.Challenges.Entities
{
    public class Comment : Entity
    {
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
