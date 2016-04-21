using System;
using System.ServiceModel;
using Business.Challenges.ViewModels;
using Data.Challenges.Entities;
using Data.Challenges.Repositories;

namespace Business.Challenges
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
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