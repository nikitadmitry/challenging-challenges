﻿using System;
using System.Data.Entity;
using Data.Challenges.Entities;
using Data.Common;

namespace Data.Challenges.Context
{
    public class ChallengesUnitOfWork : UnitOfWork, IFullTextIndexedChallengesUnitOfWork
    {
        public ChallengesUnitOfWork(Func<DbContext, IRepository<Challenge>> challengesRepositoryFunc,
            Func<DbContext, IRepository<Answer>> answersRepositoryFunc,
            Func<DbContext, IRepository<Comment>> commentsRepositoryFunc,
            Func<DbContext, IRepository<Solver>> solversRepositoryFunc,
            Func<DbContext, IRepository<Tag>> tagsRepositoryFunc,
            ChallengesContext context)
            : base(context)
        {
            RegisterRepository(challengesRepositoryFunc(Context));
            RegisterRepository(answersRepositoryFunc(Context));
            RegisterRepository(commentsRepositoryFunc(Context));
            RegisterRepository(solversRepositoryFunc(Context));
            RegisterRepository(tagsRepositoryFunc(Context));
        }

        public IRepository<Challenge> ChallengesRepository
        {
            get
            {
                return GetRepository<Challenge>();
            }
        }

        public IRepository<Answer> AnswerRepository
        {
            get
            {
                return GetRepository<Answer>();
            }
        }

        public IRepository<Comment> CommentsRepository
        {
            get
            {
                return GetRepository<Comment>();
            }
        }

        public IRepository<Solver> SolversRepository
        {
            get
            {
                return GetRepository<Solver>();
            }
        }

        public IRepository<Tag> TagsRepository
        {
            get
            {
                return GetRepository<Tag>();
            }
        }
    }
}