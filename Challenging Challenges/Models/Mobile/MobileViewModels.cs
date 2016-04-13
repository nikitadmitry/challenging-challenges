using Data.Challenges.Entities;
using Data.Common;

namespace Challenging_Challenges.Models.Mobile
{
    public class UserLoginInfoViewModel
    {
        public string LoginProvider { get; set; }
        public string ProviderKey { get; set; }
    }

    public class MobileChallenge : Entity
    {
        public string Title { get; set; }
        public string Text { get; set; }

        public MobileChallenge(Challenge challenge)
        {
            Id = challenge.Id;
            Title = challenge.Title;
            Text = challenge.Condition;
        }
    }
}
