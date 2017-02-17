using Business.Achievements;
using Business.SearchIndex;
using Hangfire;

namespace Presentation.Web.Lucene
{
    public class LuceneIndexer
    {
        /// <summary>
        /// Cron interval between index updates.
        /// </summary>
        private static readonly string IndexUpdateInterval = Cron.MinuteInterval(20);

        private readonly ISearchIndexService searchIndexService;
        private readonly IAchievementsService achievementsService;

        public LuceneIndexer(ISearchIndexService searchIndexService, IAchievementsService achievementsService)
        {
            this.searchIndexService = searchIndexService;
            this.achievementsService = achievementsService;
        }

        public void Update()
        {
            searchIndexService.UpdateIndex();
            searchIndexService.Optimize();

            achievementsService.UpdateTopOne();
        }

        public static void LaunchReccuringJob()
        {
            RecurringJob.AddOrUpdate<LuceneIndexer>(x => x.Update(), IndexUpdateInterval);
        }
    }
}