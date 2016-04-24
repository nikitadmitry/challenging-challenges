using AutoMapper;
using Business.Identity.Mappings;

namespace Business.Host
{
    public static class AutoMapperConfig
    {
        public static void RegisterMappings()
        {
            Mapper.AddProfile(new IdentityMapProfile());
        }
    }
}