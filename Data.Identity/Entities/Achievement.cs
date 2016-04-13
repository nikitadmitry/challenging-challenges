using System;
using System.ComponentModel.DataAnnotations.Schema;
using Data.Common;
using Data.Identity.Enums;

namespace Data.Identity.Entities
{
    public class Achievement: Entity
    {
        public string Value
        {
            get;
            set;
        }

        [NotMapped]
        public AchievementTypes AchievementEnum
        {
            get
            {
                AchievementTypes achievement;
                Enum.TryParse(Value, out achievement);
                return achievement;
            }
        }

    }
}