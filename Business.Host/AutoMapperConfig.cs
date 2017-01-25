using Autofac;
using AutoMapper;
using Business.Challenges.Mappings;
using Business.Identity.Mappings;
using Business.SearchIndex.Mappings;

namespace Business.Host
{
    public static class AutoMapperConfig
    {
        public static void RegisterMappings(ContainerBuilder builder)
        {
            var mapperConfig = new MapperConfiguration(AddMappingProfiles);
            var mapper = mapperConfig.CreateMapper();

            builder.RegisterInstance(mapper).As<IMapper>();
        }

        private static void AddMappingProfiles(IMapperConfigurationExpression cfg)
        {
            cfg.AddProfile<IdentityMapProfile>();
            cfg.AddProfile<ChallengesMapProfile>();
            cfg.AddProfile<SearchIndexMapProfile>();
        }
    }
}