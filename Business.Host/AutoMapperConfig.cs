using AutoMapper;
using Business.Challenges.Mappings;
using Business.Identity.Mappings;
using Business.SearchIndex.Mappings;

namespace Business.Host
{
    public static class AutoMapperConfig
    {
        public static void RegisterMappings()
        {
            Mapper.AddProfile(new IdentityMapProfile());
            Mapper.AddProfile(new ChallengesMapProfile());
            Mapper.AddProfile(new SearchIndexMapProfile());
        }
    }
}