using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naive_Bayes_DT.Decission_Tree
{
    internal class FrequenceTable
    {
        public List<List<Message>> Messages { get; private set; }
        /// <summary>
        /// Liste von allen Nachrichten mit den jeweiligen Attributen. 1. Wert = Attribut Ja oder Nein. 2. Wert = Spam Ja oder Nein
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public List<Tuple<bool, bool>> XXXTable { get; private set; } = new List<Tuple<bool, bool>>();

        public List<Tuple<bool, bool>> PhoneNumberTable { get; private set; } = new List<Tuple<bool, bool>>();

        public List<Tuple<bool,bool>> FreeTable { get; private set; } = new List<Tuple<bool, bool>>();

        public List<Tuple<bool, bool>> MoneyTable { get; private set; }  = new List<Tuple<bool, bool>>();

        // ReSharper disable once InconsistentNaming
        public List<Tuple<bool, bool>> URLTable { get; private set; } = new List<Tuple<bool, bool>>();

        public List<Tuple<bool, bool>> ApostrophTable { get; private set; } = new List<Tuple<bool, bool>>();

        public List<Tuple<bool, bool>> UpperCaseTable { get; private set; } = new List<Tuple<bool, bool>>();



        public FrequenceTable(List<List<Message>> messages)
        {
            this.Messages = messages;
            CreateFrequenceTables();
        }

        private void CreateFrequenceTables()
        {
            foreach (var messageList in Messages)
            {
                foreach (var message in messageList)
                {
                    AddAttributeXXX(message);
                    AddAttributeApostroph(message);
                    AddAttributePhoneNumber(message);
                    AddAttributeFree(message);
                    AddAttributeURL(message);
                    AddAttributeMoney(message);
                    AddAttributeUpperCase(message);
                }
            }

        }

        // ReSharper disable once InconsistentNaming
        private void AddAttributeXXX(Message message)
        {
            if (CheckSpamOrHam(message))
            {
                XXXTable.Add(message.XXX ? Tuple.Create(true, true) : Tuple.Create(false, true));
            }
            else
            {
                XXXTable.Add(message.XXX ? Tuple.Create(true, false) : Tuple.Create(false, false));
            }              
        }           
                    
        private void AddAttributeFree(Message message)
        {
            if (CheckSpamOrHam(message))
            {
                FreeTable.Add(message.Free ? Tuple.Create(true, true) : Tuple.Create(false, true));
            }
            else
            {
                FreeTable.Add(message.Free ? Tuple.Create(true, false) : Tuple.Create(false, false));
            }
        }           
                    
        private  void AddAttributePhoneNumber(Message message)
        {
            if (CheckSpamOrHam(message))
            {
                PhoneNumberTable.Add(message.FollowingNumbers ? Tuple.Create(true, true) : Tuple.Create(false, true));
            }
            else
            {
                PhoneNumberTable.Add(message.FollowingNumbers ? Tuple.Create(true, false) : Tuple.Create(false, false));
            }
        }           
                    
        private void AddAttributeMoney(Message message)
        {
            if (CheckSpamOrHam(message))
            {
                MoneyTable.Add(message.Money ? Tuple.Create(true, true) : Tuple.Create(false, true));
            }
            else
            {
                MoneyTable.Add(message.Money ? Tuple.Create(true, false) : Tuple.Create(false, false));
            }
        }           
                    
        private void AddAttributeURL(Message message)
        {
            if (CheckSpamOrHam(message))
            {
                URLTable.Add(message.HyperLink ? Tuple.Create(true, true) : Tuple.Create(false, true));
            }
            else
            {
                URLTable.Add(message.HyperLink ? Tuple.Create(true, false) : Tuple.Create(false, false));
            }
        }           
                    
        private void AddAttributeApostroph(Message message)
        {
            if (CheckSpamOrHam(message))
            {
                ApostrophTable.Add(message.Apostroph ? Tuple.Create(true, true) : Tuple.Create(false, true));
            }
            else
            {
                ApostrophTable.Add(message.Apostroph ? Tuple.Create(true, false) : Tuple.Create(false, false));
            }
        }           
                    
        private void AddAttributeUpperCase(Message message)
        {
            if (CheckSpamOrHam(message))
            {
                UpperCaseTable.Add(message.UpperCase ? Tuple.Create(true, true) : Tuple.Create(false, true));
            }
            else
            {
                UpperCaseTable.Add(message.UpperCase ? Tuple.Create(true, false) : Tuple.Create(false, false));
            }
        }

        /// <summary>
        /// Return true im Fall, dass die nachricht spam ist, false im falle das es ham ist.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private bool CheckSpamOrHam(Message message)
        {
            return message.Classified.Equals("spam");
        }
    }
}
