﻿using System;
using System.Collections.Concurrent;
using System.Linq;
using Challenging_Challenges.Infrastructure;
using Challenging_Challenges.Models.Entities;
using Data.Identity.Entities;
using Data.Identity.Repositories;
using FluentScheduler;

namespace Challenging_Challenges.Helpers
{
    public class LuceneRegistry: Registry
    {
        public static ConcurrentQueue<string> StagedToDelete = new ConcurrentQueue<string>(); 

        public LuceneRegistry()
        {
            Schedule(() =>
            {
                string id;
                while (StagedToDelete.TryDequeue(out id))
                    LuceneSearch.ClearLuceneIndexRecord(id);
                LuceneSearch.AddUpdateLuceneIndex(IndexRepository.GetAll());
                LuceneSearch.Optimize();
                UpdateTopOne();
            }).ToRunNow().AndEvery(20).Minutes();
        }

        private void UpdateTopOne()
        {
            IdentityContext usersDb = new IdentityContext();
            ApplicationUser user = usersDb.Users.OrderByDescending(x => x.Rating).Take(1).First();
            new StatisticsWorker(usersDb, user).BecameTopOne();
        }
    }
}
