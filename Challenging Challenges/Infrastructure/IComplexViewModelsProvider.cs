using Challenging_Challenges.Models.ViewModels;

namespace Challenging_Challenges.Infrastructure
{
    public interface IComplexViewModelsProvider
    {
        HomeChallengeViewModel GetHomeChallengeViewModel();
    }
}