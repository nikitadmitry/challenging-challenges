using System;
using System.Collections.Generic;
using System.IO;
using Data.Challenges.Entities;
using Data.Challenges.Enums;

namespace Challenging_Challenges.Helpers
{
    public class ChallengeGenerator
    {
        public Challenge GetChallenge(Guid authorId)
        {
            string answer = GetRandomString();
            Random random = new Random();
            Challenge challenge = new Challenge{
                Answers = new List<Answer>
                {
                    new Answer { Value = $"{GetRandomString()}" },
                    new Answer { Value = answer }
                },
                AuthorId = authorId,
                Condition = $"<p>This is a generated challenge, added in demonstration purposes.</p><p>One of the answers is {answer}</p>",
                PreviewText = "<p>A generated demonstration challenge.</p>",
                Title = "Demonstration challenge" + random.NextDouble(),
                Language = random.NextDouble() >= 0.5 ? Language.English : Language.Russian,
                Difficulty = (byte)random.Next(1, 5)
            };

            return challenge;
        }

        public string GetRandomTags()
        {
            return $"{GetRandomString()} {GetRandomString()}";
        }

        private string GetRandomString()
        {
            return Path.GetRandomFileName().Replace(".", "");
        }

    }
}
