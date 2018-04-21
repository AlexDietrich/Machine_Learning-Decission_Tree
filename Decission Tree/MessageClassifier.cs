using System;
using System.Collections.Generic;

namespace Decission_Tree.Decission_Tree
{
    internal class MessageClassifier
    {
        private int SpamCount = 0;
        private int HamCount = 0; 
        List<double> Accuracy = new List<double>();

            //Liste von allen verfügbaren Attributen
        List<Properties> porpertyList = new List<Properties>
        {
            Properties.Apostroph,
            Properties.Free,
            Properties.Money,
            Properties.TelephoneNumber,
            Properties.UpperCase,
            Properties.Url,
            Properties.Xxx
        };

        private List<List<Message>> Messages { get; set; }
        private readonly List<List<Message>> _learnMessages = new List<List<Message>>();
        private List<Message> _validateMessages = new List<Message>();
        private CalculateDecissionTree DecissionTree { get; set; }

        public MessageClassifier(FoldCrossValidation messages, int spamCount, int hamCount)
        {
            this.Messages = messages.DataSetPackages;
            ExecuteTenFoldCrossValidation();
            CalculateAccuracy();
        }


        private void ExecuteTenFoldCrossValidation()
        {
            for (var i = 0; i < 10; i++)
            {
                for (var j = 0; j < 10; j++)
                {
                    if (i == j) _validateMessages = Messages[j];
                    else _learnMessages.Add(Messages[j]);
                }
                LearnDecissionTree();
                ValidateMessages();
            }
        }

        private void LearnDecissionTree()
        {

            var modifiedList = new Attributes(_learnMessages);
            var frequenceTable = new FrequenceTable(modifiedList.Entries);
            DecissionTree = new CalculateDecissionTree(frequenceTable, porpertyList);
        }

        private void ValidateMessages()
        {
            int countCorrectValidate = 0, count = 0;
            var modifiedList = new Attributes(_validateMessages);
            foreach (var list in modifiedList.Entries)
            {
                foreach (var message in list)
                {
                    count++;
                    var spam = false;
                    DecissionTree.ClassifyMessage(message, ref spam);
                    switch (message.Classified)
                    {
                        case "spam" when spam:
                            countCorrectValidate++;
                            break;
                        case "ham" when !spam:
                            countCorrectValidate++;
                            break;
                    }
                }
            }
            Accuracy.Add(Math.Round((double)countCorrectValidate / count * 100, 2));
        }

        private void CalculateAccuracy()
        {
            double sum = 0;
            foreach (var value in Accuracy)
            {
                sum += value;
            }
            Console.WriteLine("Accuracy: " + Math.Round(sum / Accuracy.Count, 2) + "%");
        }

    }
}
