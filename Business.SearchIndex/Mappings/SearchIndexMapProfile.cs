using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Data.Challenges.Entities;

namespace Business.SearchIndex.Mappings
{
    public class SearchIndexMapProfile: Profile
    {
        public SearchIndexMapProfile()
        {
            ConfigureSearchIndexMap();
        }

        public void ConfigureSearchIndexMap()
        {
            CreateMap<Challenge, ViewModels.SearchIndex>()
                .ForMember(t => t.Id, o => o.MapFrom(s => s.Id))
                .ForMember(t => t.Condition, o => o.MapFrom(s => s.Condition))
                .ForMember(t => t.PreviewText, o => o.MapFrom(s => s.PreviewText))
                .ForMember(t => t.Tags, o => o.MapFrom(s => GetViewModelTags(s.Tags.ToList())));
        }

        private string GetViewModelTags(List<Tag> tags)
        {
            StringBuilder tagsAsString = new StringBuilder();

            foreach (var tag in tags)
            {
                tagsAsString.Append($"{tag.Value} ");
            }

            return tagsAsString.ToString();
        }
    }
}