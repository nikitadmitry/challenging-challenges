using System;
using Business.Achievements.ViewModels;

namespace Presentation.Legacy.Infrastructure
{
    public interface IAchievementsSignalRProvider
    {
        void ShowAchievementMessage(AchievementType? achievementType, Guid userId);
    }
}