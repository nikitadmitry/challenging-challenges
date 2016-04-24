using System;
using System.ServiceModel;
using Business.Challenges.ViewModels;

namespace Business.Challenges
{
    [ServiceContract]
    public interface IChallengesService
    {
        [OperationContract]
        ChallengeViewModel GetChallengeViewModel(Guid id);
    }
}