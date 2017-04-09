using System;
using System.Collections.Generic;
using Business.Common.ViewModels;
using Data.Identity.Entities;

namespace Business.Identity.ViewModels
{
    public class UserModel
    {
        public Guid Id
        {
            get;
            set;
        }

        public string UserName
        {
            get;
            set;
        }

        public string About
        {
            get;
            set;
        }

        public string Email
        {
            get;
            set;
        }

        public int Level
        {
            get;
            set;
        }

        public int SolvedChallenges
        {
            get;
            set;
        }

        public int PostedChallenges
        {
            get;
            set;
        }

        public IList<AchievementType> Achievements
        {
            get;
            set;
        }

        public bool IsReadOnly
        {
            get;
            set;
        }
    }
}