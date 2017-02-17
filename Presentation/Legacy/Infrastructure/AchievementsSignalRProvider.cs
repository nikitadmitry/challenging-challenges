using System;
using System.Globalization;
using Business.Achievements.ViewModels;
using Challenging_Challenges.Hubs;
using Shared.Framework.Resources;

namespace Challenging_Challenges.Infrastructure
{
    public class AchievementsSignalRProvider : IAchievementsSignalRProvider
    {
        public void ShowAchievementMessage(AchievementType? achievementType, Guid userId)
        {
            if (achievementType == null)
            {
                return;
            }

            var resourceSet = Achievements.ResourceManager.GetResourceSet(CultureInfo.CurrentCulture, true, true);
            var context = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
            foreach (var connectionId in NotificationHub.Connections.GetConnections(userId.ToString()))
            {
                context.Clients.Client(connectionId).showAchievement(resourceSet.GetString(achievementType.ToString()));
            }
        }
    }
}