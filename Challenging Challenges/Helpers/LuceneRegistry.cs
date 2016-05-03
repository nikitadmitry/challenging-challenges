using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Autofac;
using Business.Achievements;
using Business.SearchIndex;
using FluentScheduler;

namespace Challenging_Challenges.Helpers
{
    public class LuceneRegistry: Registry
    {
        public static ConcurrentQueue<Guid> StagedToDelete = new ConcurrentQueue<Guid>(); 

        public LuceneRegistry()
        {
            Schedule(() =>
            {
                using (var scope = DependencyRegistration.Container.BeginLifetimeScope())
                {
                    var searchIndexService = scope.Resolve<ISearchIndexService>();
                    
                    searchIndexService.RemoveRecords(GetRemovedRecordIds());
                    searchIndexService.UpdateIndex();
                    searchIndexService.Optimize();

                    scope.Resolve<IAchievementsService>().UpdateTopOne();
                }
            }).ToRunNow().AndEvery(20).Minutes();
        }

        private IEnumerable<Guid> GetRemovedRecordIds()
        {
            Guid id;
            while (StagedToDelete.TryDequeue(out id))
            {
                yield return id;
            }
        } 
    }
}
