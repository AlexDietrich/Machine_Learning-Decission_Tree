using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Decission_Tree.Decission_Tree
{
    internal class Attributes
    {
        // ReSharper disable once MemberCanBePrivate.Global
        public List<List<Message>> Entries { get; private set; } = new List<List<Message>>();

        /// <summary>
        /// Regex Patterns zum finden der Attribute in den Nachrichten
        /// </summary>
        private const string MatchPhonePattern = @"\d{4,}";
        private const string MatchMoneyPattern = @"[€?$?£?]";
        // ReSharper disable once InconsistentNaming
        private const string MatchURLPattern = @"/^(https?:\/\/)?([\da-z\.-]+)\.([a-z\.]{2,6})([\/\w \.-]*)*\/?$/";
        private const string MatchUpperCasePattern = @"(\b*[A-Z][A-Z]*\b)";

        public Attributes(List<List<Message>> entries)
        {
            this.Entries = entries;
            SetAttributeToMessage();
        }

        public Attributes(List<Message> messages)
        {
            this.Entries.Add(messages);
            SetAttributeToMessage();
        }

        public void SetAttributeToMessage()
        {
            foreach (var list in Entries)
            {
                foreach (var entry in list)
                {
                    SetPhoneNumberAttribute(entry);
                    SetXXXAttribute(entry);
                    SetFreeAttribute(entry);
                    SetMoneyAttribute(entry);
                    SetURLAttribute(entry);
                    SetApostrophAttribute(entry);
                    SetUpperCaseAttribute(entry);
                }
            }
        }

        private static void SetPhoneNumberAttribute(Message message)
        {
            var matchCollection = Regex.Matches(message.Text, MatchPhonePattern);
            if (matchCollection.Count > 0) message.FollowingNumbers = true;
        }

        // ReSharper disable once InconsistentNaming
        private static void SetXXXAttribute(Message message)
        {
            if (message.Text.ToLower().Contains("xxx")) message.XXX = true; 
        }

        private static void SetFreeAttribute(Message message)
        {
            if (message.Text.ToLower().Contains("free")) message.Free = true;
        }

        private static void SetMoneyAttribute(Message message)
        {
            var matchCollection = Regex.Matches(message.Text, MatchMoneyPattern);
            if (matchCollection.Count > 0) message.Money = true;
        }

        // ReSharper disable once InconsistentNaming
        private static void SetURLAttribute(Message message)
        {
            var matchCollection = Regex.Matches(message.Text, MatchURLPattern);
            if (matchCollection.Count > 0 || message.Text.ToLower().Contains("www")) message.HyperLink = true;
        }

        private static void SetApostrophAttribute(Message message)
        {
            if (message.Text.Contains("'")) message.Apostroph = true; 
        }

        private static void SetUpperCaseAttribute(Message message)
        {
            var matchCollection = Regex.Matches(message.Text, MatchUpperCasePattern);
            if (matchCollection.Count > 0) message.UpperCase = true; 
        }
    }
}
