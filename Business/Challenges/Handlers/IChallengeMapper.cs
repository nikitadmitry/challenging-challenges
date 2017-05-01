using Business.Challenges.ViewModels;
using Data.Challenges.Entities;
using Shared.Framework.Dependency;

namespace Business.Challenges.Handlers
{
    public interface IChallengeMapper : IDependency
    {
        void Map(Challenge challenge, EditChallengeViewModel model);
    }
}