using Data.Challenges.Entities;
using Data.Common;

namespace Data.Challenges.Context
{
    public interface IChallengesUnitOfWork: IUnitOfWork
    {
        IRepository<Challenge> ChallengesRepository
        {
            get;
        }

        IRepository<Answer> AnswerRepository
        {
            get;
        }

        IRepository<Comment> CommentsRepository
        {
            get;
        }

        IRepository<Solver> SolversRepository
        {
            get;
        }

        IRepository<Tag> TagsRepository
        {
            get;
        }
    }
}