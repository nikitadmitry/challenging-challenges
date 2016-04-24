using System;
using System.Collections.Generic;
using System.Linq;
using Data.Challenges.Context;
using Data.Challenges.Entities;
using Data.Common;

namespace Challenging_Challenges.Models.Entities
{
    public class SearchIndex : Entity
    {
        public string AuthorId { get; set; }
        public string Title { get; set; }
        public string PreviewText { get; set; }
        public string Condition { get; set; }
        public byte Difficulty { get; set; }
        public string Section { get; set; }
        public string Language { get; set; }
        public byte Rating { get; set; }
        public string Tags { get; set; }
        public int TimesSolved { get; set; }

        public SearchIndex() { }

        public SearchIndex(Challenge challenge)
        {
            Id = challenge.Id;
            AuthorId = challenge.AuthorId;
            Title = challenge.Title;
            PreviewText = challenge.PreviewText;
            Condition = challenge.Condition;
            Difficulty = challenge.Difficulty;
            Section = challenge.Section.ToString();
            Language = challenge.Language.ToString();
            Rating = Convert.ToByte(challenge.Rating);
            TimesSolved = challenge.TimesSolved;
            Tags = string.Join(" ", challenge.Tags.Select(x => x.Value));
        }
    }

    public static class IndexRepository
    {
        public static SearchIndex Get(Guid id)
        {
            return GetAll().SingleOrDefault(x => x.Id.Equals(id));
        }

        public static List<SearchIndex> GetAll()
        {
            return new List<SearchIndex>();
            //ChallengesContext context = new ChallengesContext();
            //return context.Challenges.ToList().Select(x => new SearchIndex(x)).ToList();
        }
    }
}
