using System;
using Business.Achievements.ViewModels;

namespace Challenging_Challenges.Infrastructure
{
    public interface IAchievementsSignalRProvider
    {
        void ShowAchievementMessage(AchievementType? achievementType, Guid userId);
    }
}