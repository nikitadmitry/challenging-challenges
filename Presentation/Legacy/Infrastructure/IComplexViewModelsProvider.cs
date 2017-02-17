using Presentation.Legacy.Models.ViewModels;

namespace Presentation.Legacy.Infrastructure
{
    public interface IComplexViewModelsProvider
    {
        HomeChallengeViewModel GetHomeChallengeViewModel();
    }
}