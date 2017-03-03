using System;
using Data.Challenges.Enums;

namespace Business.Challenges.ViewModels
{
    public class ChallengeDetailsModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Condition { get; set; }
        public Difficulty Difficulty { get; set; }
        public BusinessSection Section { get; set; }
        public double Rating { get; set; }
        public ChallengeType ChallengeType { get; set; }
        public Guid AuthorId { get; set; }
        public string AuthorName { get; set; }
        public bool IsAuthor { get; set; }
        public bool IsSolved { get; set; }
        public string AnswerTemplate { get; set; }
    }
}