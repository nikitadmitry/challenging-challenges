using System.Linq;
using AutoMapper;
using Business.Identity.ViewModels;
using Data.Identity.Entities;
using Shared.Framework.Automapper;

namespace Business.Identity.Mappings
{
    public class IdentityMapProfile: Profile
    {
        protected override void Configure()
        {
            ConfigureIdentityUserMap();
            ConfigureIdentityRoleMap();
        }

        private void ConfigureIdentityRoleMap()
        {
            CreateMap<Role, IdentityRole>()
                .IgnoreAllUnmapped()
                .ForMember(r => r.Id, o => o.MapFrom(e => e.Id))
                .ForMember(r => r.Name, o => o.MapFrom(e => e.Name));
        }

        private void ConfigureIdentityUserMap()
        {
            CreateMap<IdentityUser, User>()
                .IgnoreAllUnmapped()
                .ForMember(t => t.Id, o => o.MapFrom(s => s.Id))
                .ForMember(t => t.UserName, o => o.MapFrom(s => s.UserName))
                .ForMember(t => t.Email, o => o.MapFrom(s => s.Email))
                .ForMember(t => t.About, o => o.MapFrom(s => s.About))
                .ForMember(t => t.PostedTasksQuantity, o => o.MapFrom(s => s.PostedTasksQuantity))
                .ForMember(t => t.SolvedTasksQuantity, o => o.MapFrom(s => s.SolvedTasksQuantity))
                .ForMember(t => t.Rating, o => o.MapFrom(s => s.Rating))
                .ForMember(t => t.PasswordHash, o => o.MapFrom(s => s.PasswordHash))
                .ForMember(t => t.SecurityStamp, o => o.MapFrom(s => s.SecurityStamp))
                .ForMember(t => t.EmailConfirmed, o => o.MapFrom(s => s.EmailConfirmed));

            CreateMap<User, IdentityUser>()
                .IgnoreAllUnmapped()
                .ForMember(t => t.Id, o => o.MapFrom(s => s.Id))
                .ForMember(t => t.UserName, o => o.MapFrom(s => s.UserName))
                .ForMember(t => t.Email, o => o.MapFrom(s => s.Email))
                .ForMember(t => t.About, o => o.MapFrom(s => s.About))
                .ForMember(t => t.PostedTasksQuantity, o => o.MapFrom(s => s.PostedTasksQuantity))
                .ForMember(t => t.SolvedTasksQuantity, o => o.MapFrom(s => s.SolvedTasksQuantity))
                .ForMember(t => t.Rating, o => o.MapFrom(s => s.Rating))
                .ForMember(t => t.Achievements, o => 
                    o.MapFrom(s => 
                        s.Achievements.Select(x => x.Value).ToList()))
                .ForMember(t => t.Roles, o =>
                                    o.MapFrom(s =>
                                        s.Roles.Select(x => x.Name).ToList()))
                .ForMember(t => t.PasswordHash, o => o.MapFrom(s => s.PasswordHash))
                .ForMember(t => t.SecurityStamp, o => o.MapFrom(s => s.SecurityStamp))
                .ForMember(t => t.EmailConfirmed, o => o.MapFrom(s => s.EmailConfirmed));

            CreateMap<User, UserTopViewModel>()
                .IgnoreAllUnmapped()
                .ForMember(t => t.UserName, o => o.MapFrom(s => s.UserName))
                .ForMember(t => t.Rating, o => o.MapFrom(s => s.Rating))
                .ForMember(t => t.PostedChallenges, o => o.MapFrom(s => s.PostedTasksQuantity))
                .ForMember(t => t.SolvedChallenges, o => o.MapFrom(s => s.SolvedTasksQuantity));
        }
    }
}