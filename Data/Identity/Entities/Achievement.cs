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

        public virtual User User
        {
            get;
            set;
        }

        [NotMapped]
        public DataAchievementType AchievementEnum
        {
            get
            {
                DataAchievementType achievement;
                Enum.TryParse(Value, out achievement);
                return achievement;
            }
        }

        public static Achievement Create(string value)
        {
            return new Achievement
            {
                Value = value
            };
        }
    }
}