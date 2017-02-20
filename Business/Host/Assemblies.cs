using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Business.Achievements;
using Business.Challenges;
using Business.CodeExecution;
using Business.Host.Modules;
using Business.Identity;
using Data.Challenges.Context;
using Data.Identity.Context;

namespace Business.Host
{
    public static class Assemblies
    {
        public static IEnumerable<Assembly> All()
        {
            return Business().Union(Data());
        }

        public static IEnumerable<Assembly> Business()
        {
            yield return typeof(AchievementsService).Assembly;
            yield return typeof(ChallengesService).Assembly;
            yield return typeof(CodeExecutor).Assembly;
            yield return typeof(IdentityService).Assembly;
        } 

        public static IEnumerable<Assembly> Data()
        {
            yield return typeof(ChallengesContext).Assembly;
            yield return typeof(IdentityContext).Assembly;
        }

        public static IEnumerable<Assembly> Modules()
        {
            yield return typeof(DataModule).Assembly;
        }
    }
}