using AutoMapper;
using Business.Challenges.Mappings;
using Business.Identity.Mappings;
using Business.SearchIndex.Mappings;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Business.Tests
{
    [TestClass]
    public class AutomapperTests
    {
        [TestMethod]
        public void AutomapperConfigurationIsValid()
        {
            var mapperConfig = new MapperConfiguration(AddMappingProfiles);
            var mapper = mapperConfig.CreateMapper();

            mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }

        private static void AddMappingProfiles(IMapperConfigurationExpression cfg)
        {
            cfg.AddProfile<IdentityMapProfile>();
            cfg.AddProfile<ChallengesMapProfile>();
            cfg.AddProfile<SearchIndexMapProfile>();
        }
    }
}