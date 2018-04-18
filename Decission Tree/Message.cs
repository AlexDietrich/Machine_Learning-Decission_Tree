﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naive_Bayes_DT.Decission_Tree
{
    class Message
    {
        public string Classified { get; private set; } = string.Empty;
        public string Text { get; private set; } = string.Empty;

        public bool XXX { get; set; } = false;
        public bool Free { get; set; } = false;
        public bool Money { get; set; } = false;
        public bool UpperCase { get; set; } = false;
        public bool HyperLink { get; set; } = false;
        public bool FollowingNumbers { get; set; } = false;

        public Message(string classified, string message)
        {
            this.Classified = classified;
            this.Text = message;
        }

    }
}