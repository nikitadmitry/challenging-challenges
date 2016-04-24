﻿using System;
using System.Collections.Generic;
using Data.Common;
using Microsoft.AspNet.Identity;

namespace Data.Identity.Entities
{
    public class User : Entity, IUser<Guid>
    {
        public string UserName
        {
            get;
            set;
        }

        public string Email
        {
            get;
            set;
        }

        public double Rating
        {
            get;
            set;
        }

        public int SolvedTasksQuantity
        {
            get;
            set;
        }

        public int PostedTasksQuantity
        {
            get;
            set;
        }

        private List<Achievement> achievements;

        public IList<Achievement> Achievements
        {
            get
            {
                return achievements ?? (achievements = new List<Achievement>());
            }
        }

        public string About
        {
            get;
            set;
        }

        public virtual string PasswordHash
        {
            get;
            set;
        }

        public virtual string SecurityStamp
        {
            get;
            set;
        }
    }
}