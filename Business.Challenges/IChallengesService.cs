using System;
using System.ServiceModel;
using Business.Challenges.ViewModels;

namespace Business.Challenges
{
    [ServiceContract(ConfigurationName = "ChallengesService")]
    public interface IChallengesService
    {
        ChallengeViewModel GetChallengeViewModel(Guid id);
    }
}