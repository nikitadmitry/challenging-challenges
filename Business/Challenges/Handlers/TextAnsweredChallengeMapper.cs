using Business.Challenges.ViewModels;
using Data.Challenges.Entities;
using Shared.Framework.Dependency;

namespace Business.Challenges.Handlers
{
    [KeyedDependency(ChallengeType.TextAnswered)]
    public class TextAnsweredChallengeMapper : IChallengeMapper
    {
        public void Map(Challenge challenge, EditChallengeViewModel model)
        {
            throw new System.NotImplementedException();
        }
    }
}