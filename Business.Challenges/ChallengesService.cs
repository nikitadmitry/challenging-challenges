using System;
using System.ServiceModel;
using Business.Challenges.ViewModels;
using Data.Challenges.Context;
using Data.Challenges.Entities;

namespace Business.Challenges
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true/*, InstanceContextMode = InstanceContextMode.PerCall*/)]
    public class ChallengesService: IChallengesService
    {
        private readonly IChallengesUnitOfWork unitOfWork;

        public ChallengesService(IChallengesUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public ChallengeViewModel GetChallengeViewModel(Guid id)
        {
            var challenge = unitOfWork.Get<Challenge>(id);

            return new ChallengeViewModel(challenge);
        }
    }
}