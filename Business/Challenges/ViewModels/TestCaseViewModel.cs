﻿using System;
using System.Collections.Generic;

namespace Business.Challenges.ViewModels
{
    public class TestCaseViewModel
    {
        public Guid Id
        {
            get;
            set;
        }

        public List<string> InputParameters { get; set; }

        public List<string> OutputParameters { get; set; }

        public bool IsPublic { get; set; }
    }
}