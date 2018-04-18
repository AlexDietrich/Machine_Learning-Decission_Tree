using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Naive_Bayes_DT.Decission_Tree
{
    internal class Attributes
    {
        private List<List<Message>> _entries;

        private const string MatchPhonePattern = @"\d{4,}";
        private const string MatchMoneyPattern = @"[€?$?£?]";

        public Attributes(List<List<Message>> entries)
        {
            this._entries = entries;
        }

        public void SearchAndGeneralizeAttributes()
        {
            foreach (var list in _entries)
            {
                foreach (var entry in list)
                {
                    SetPhoneNumberAttribute(entry);
                    SetXXXAttribute(entry);
                    SetFreeAttribute(entry);
                    SetMoneyAttribute(entry);
                }
            }
        }

        private static void SetPhoneNumberAttribute(Message message)
        {
            var matchCollection = Regex.Matches(message.Text, MatchPhonePattern);
            if (matchCollection.Count > 0) message.FollowingNumbers = true;
        }

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
            var matchCollection = Regex.Matches(MatchMoneyPattern, message.Text);
            if (matchCollection.Count > 0) message.Money = true;
        }
    }
}
