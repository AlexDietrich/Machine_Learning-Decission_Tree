using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Naive_Bayes_DT.Decission_Tree
{
    internal class CalculateDecissionTree
    {
        private int SpamCount { get; set; }
        private int HamCount { get; set; }
        private int MessagesCount { get; set; }
        private double EntropyTreeNode { get; set; }
        private Properties ActualProp { get; set; }

        private FrequenceTable FrequenceTable { get; set; }

        private List<List<Message>> ActualMessages { get; set; } = new List<List<Message>>();

        private readonly List<Properties> _availableProperties;

        private readonly IDictionary<Properties, double> _gainValues = new Dictionary<Properties, double>();



        private int NewSpamCountTrue { get; set; }
        private int NewHamCountTrue { get; set; }
        private int NewSpamCountFalse { get; set; }
        private int NewHamCountFalse { get; set; }

        private List<List<Message>> NewMessagesTrue { get; set; } = new List<List<Message>>();
        private List<List<Message>> NewMessagesFalse { get; set; } = new List<List<Message>>();

        private CalculateDecissionTree NewNodeTrue { get; set; }
        private CalculateDecissionTree NewNodeFalse { get; set; }

        private FrequenceTable NewFrequenceTableTrue { get; set; }
        private FrequenceTable NewFrequenceTableFalse { get; set; }



        public CalculateDecissionTree(FrequenceTable table, int spamCount, int hamCount, List<Properties> properties)
        {
            this.FrequenceTable = table;
            this.HamCount = hamCount;
            this.SpamCount = spamCount;
            this.MessagesCount = hamCount + spamCount;
            this.ActualMessages = table.Messages;
            this._availableProperties = properties;
            Controller();
        }

        private void Controller()
        {
            if (_availableProperties.Count <= 0) return;
            CalculateEntropyTreeNode();
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (EntropyTreeNode == 0f) return; 
            GainController();
            FindBestGain();
            DeleteActualProperty();
            SetNewMessageList();
            NewFrequenceTableFalse = new FrequenceTable(NewMessagesFalse);
            NewFrequenceTableTrue = new FrequenceTable(NewMessagesTrue);
            NewNodeFalse  = new CalculateDecissionTree(NewFrequenceTableTrue, NewSpamCountTrue, NewHamCountTrue, _availableProperties);
            NewNodeTrue = new CalculateDecissionTree(NewFrequenceTableFalse, NewSpamCountFalse, NewHamCountFalse, _availableProperties);
        }

        private void CalculateEntropyTreeNode()
        {
            if (MessagesCount == 0)
            {
                EntropyTreeNode = 0;
                return;
            }
            // Entropy = - Wahrscheinlichkeit(yes) * log2(Wahrscheinlichkeit(yes)) - Wahrscheinlichkeit(no) * log2(Wahrscheinlichkeit(no))
            EntropyTreeNode -= ((double)SpamCount/MessagesCount) * (Math.Log((double)SpamCount/MessagesCount, 2));
            EntropyTreeNode -= ((double) HamCount / MessagesCount) * (Math.Log((double) HamCount / MessagesCount, 2));
        }

        private void GainController()
        {
            foreach (var property in _availableProperties)
            {
                double gain = 0;
                switch (property)
                {
                    case Properties.Xxx:
                        gain = CalculateAndSaveGain(FrequenceTable.XXXTable);
                        break;
                    case Properties.Free:
                        gain = CalculateAndSaveGain(FrequenceTable.FreeTable);
                        break;
                    case Properties.Money:
                        gain = CalculateAndSaveGain(FrequenceTable.MoneyTable);
                        break;
                    case Properties.UpperCase:
                        gain = CalculateAndSaveGain(FrequenceTable.UpperCaseTable);
                        break;
                    case Properties.TelephoneNumber:
                        gain = CalculateAndSaveGain(FrequenceTable.PhoneNumberTable);
                        break;
                    case Properties.Url:
                        gain = CalculateAndSaveGain(FrequenceTable.URLTable);
                        break;
                    case Properties.Apostroph:
                        gain = CalculateAndSaveGain(FrequenceTable.ApostrophTable);
                        break;
                    case Properties.NotDefined:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                _gainValues.Add(property, gain);
            }
        }

        private double CalculateAndSaveGain(List<Tuple<bool, bool>> messages)
        {
            int spamYes = 0, spamNo = 0, hamYes = 0, hamNo = 0, AttributeYes = 0, AttributeNo = 0;
            var gain = EntropyTreeNode; 

            foreach (var message in messages)
            {
                //Wenn Nachricht Spam ist und Nachricht besitzt das Attribut von der übergebenen Liste
                if (message.Item1 && message.Item2) spamYes++;
                //Wenn Nachricht das Attribut der übergebenen Liste besitz und kein Spam ist
                if (message.Item1 && !message.Item2) hamYes++;
                //Wenn die Nachricht nicht das Attribut besitzt und Spam ist
                if (!message.Item1 && message.Item2) spamNo++;
                //Wenn die Nachricht nicht das Attribut besitzt und kein Spam ist
                if (!message.Item1 && !message.Item2) hamNo++;
            }

            AttributeYes = hamYes + spamYes;
            AttributeNo = hamNo + spamNo;
            var messageComplete = AttributeNo + AttributeYes;
            var entropyYes = GivenNumbersEntropy(spamYes, hamYes);
            var entropyNo = GivenNumbersEntropy(spamNo, hamNo);

            gain -= (double)AttributeYes / messageComplete * entropyYes;    //spam yes und spam no
            gain -= (double)AttributeNo/messageComplete * entropyNo;
            return gain;
        }

        private static double GivenNumbersEntropy(double yes, double no)
        {
            var sum = yes + no;
            double entropy = 0;

            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (sum == 0) return 0;

            entropy -= (yes / sum) * Math.Log(yes / sum, 2);
            entropy -= (no / sum) * Math.Log(no / sum, 2);

            return entropy;
        }

        /// <summary>
        /// Finds the highest gain of the decission tree node.
        /// </summary>
        private void FindBestGain()
        {
            double maxGain = 0;
            foreach (var gainValue in _gainValues)
            {
                if (!(maxGain < gainValue.Value)) continue;
                maxGain = gainValue.Value;
                ActualProp = gainValue.Key;
            }
        }

        /// <summary>
        /// Löschen der aktuellen Status um den gleichen status nicht nochmal zu berechnen.
        /// </summary>
        private void DeleteActualProperty()
        {
            if (_availableProperties.Contains(ActualProp)) _availableProperties.Remove(ActualProp);
        }

        private void SetNewMessageList()
        {
            var spamMessageTrue = new List<string>();
            var spamMessageFalse = new List<string>();
            var hamMessageTrue = new List<string>();
            var hamMessageFalse = new List<string>();

            foreach (var list in ActualMessages)
            {
                foreach (var message in list)
                {
                    var isSpam = IsSpam(message);
                    switch (ActualProp)
                    {
                        case Properties.Xxx:
                            if (message.XXX && isSpam) spamMessageTrue.Add(message.Text);
                            if (message.XXX && !isSpam) hamMessageTrue.Add(message.Text);
                            if (!message.XXX && isSpam) spamMessageFalse.Add(message.Text);
                            if (!message.XXX && !isSpam) hamMessageFalse.Add(message.Text);
                            break;
                        case Properties.Free:
                            if (message.Free && isSpam) spamMessageTrue.Add(message.Text);
                            if (message.Free && !isSpam) hamMessageTrue.Add(message.Text);
                            if (!message.Free && isSpam) spamMessageFalse.Add(message.Text);
                            if (!message.Free && !isSpam) hamMessageFalse.Add(message.Text);
                            break;
                        case Properties.Money:
                            if (message.Money && isSpam) spamMessageTrue.Add(message.Text);
                            if (message.Money && !isSpam) hamMessageTrue.Add(message.Text);
                            if (!message.Money && isSpam) spamMessageFalse.Add(message.Text);
                            if (!message.Money && !isSpam) hamMessageFalse.Add(message.Text);
                            break;
                        case Properties.UpperCase:
                            if (message.UpperCase && isSpam) spamMessageTrue.Add(message.Text);
                            if (message.UpperCase && !isSpam) hamMessageTrue.Add(message.Text);
                            if (!message.UpperCase && isSpam) spamMessageFalse.Add(message.Text);
                            if (!message.UpperCase && !isSpam) hamMessageFalse.Add(message.Text);
                            break;
                        case Properties.TelephoneNumber:
                            if (message.FollowingNumbers && isSpam) spamMessageTrue.Add(message.Text);
                            if (message.FollowingNumbers && !isSpam) hamMessageTrue.Add(message.Text);
                            if (!message.FollowingNumbers && isSpam) spamMessageFalse.Add(message.Text);
                            if (!message.FollowingNumbers && !isSpam) hamMessageFalse.Add(message.Text);
                            break;
                        case Properties.Url:
                            if (message.HyperLink && isSpam) spamMessageTrue.Add(message.Text);
                            if (message.HyperLink && !isSpam) hamMessageTrue.Add(message.Text);
                            if (!message.HyperLink && isSpam) spamMessageFalse.Add(message.Text);
                            if (!message.HyperLink && !isSpam) hamMessageFalse.Add(message.Text);
                            break;
                        case Properties.Apostroph:
                            if (message.Apostroph && isSpam) spamMessageTrue.Add(message.Text);
                            if (message.Apostroph && !isSpam) hamMessageTrue.Add(message.Text);
                            if (!message.Apostroph && isSpam) spamMessageFalse.Add(message.Text);
                            if (!message.Apostroph && !isSpam) hamMessageFalse.Add(message.Text);
                            break;
                        case Properties.NotDefined:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

            var foldCrossValidation = new FoldCrossValidation();
            foldCrossValidation.CreateDataPackage(spamMessageFalse, hamMessageFalse);
            NewMessagesFalse = foldCrossValidation.DataSetPackages;

            foldCrossValidation = new FoldCrossValidation();
            foldCrossValidation.CreateDataPackage(spamMessageTrue, hamMessageTrue);
            NewMessagesTrue = foldCrossValidation.DataSetPackages;

            NewHamCountFalse = hamMessageFalse.Count;
            NewHamCountTrue = hamMessageTrue.Count;
            NewSpamCountFalse = spamMessageFalse.Count;
            NewSpamCountTrue = spamMessageTrue.Count;
        }

        private static bool IsSpam(Message message)
        {
            return message.Classified == "spam";
        }


    }
}
