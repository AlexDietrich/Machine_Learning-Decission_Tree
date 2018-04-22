using System;
using System.Collections.Generic;

namespace Decission_Tree.Decission_Tree
{
    internal class MessageClassifier
    {
        //Liste von Accuracy jedes Durchgangs um Accuracy Gesamt zu berechnen
        private readonly List<double> _accuracy = new List<double>();
        private readonly List<ConfusionMatrix> _confusionMatrices = new List<ConfusionMatrix>();

        //Liste von allen verfügbaren Attributen
        private readonly List<Properties> _porpertyList = new List<Properties>
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

        public MessageClassifier(FoldCrossValidation messages)
        {
            this.Messages = messages.DataSetPackages;
        }


        public void ExecuteTenFoldCrossValidation()
        {
            for (var i = 0; i < 10; i++)
            {
                for (var j = 0; j < 10; j++)
                {
                    if (i == j) _validateMessages = Messages[j];
                    else _learnMessages.Add(Messages[j]);
                }
                Console.WriteLine("Building Tree ...");
                LearnDecissionTree();
                Console.WriteLine("Classification of Block " + (i + 1) + " is running ...");
                ValidateMessages();
            }
        }

        private void LearnDecissionTree()
        {
            var modifiedList = new Attributes(_learnMessages);
            var frequenceTable = new FrequenceTable(modifiedList.Entries);
            frequenceTable.CreateFrequenceTables();
            DecissionTree = new CalculateDecissionTree(frequenceTable, _porpertyList);
        }

        private void ValidateMessages()
        {
            var countAllMessages = 0;
            var confusionMatrix = new ConfusionMatrix();
            var modifiedList = new Attributes(_validateMessages);

            foreach (var list in modifiedList.Entries)
            {
                foreach (var message in list)
                {
                    countAllMessages++;
                    var spam = false;
                    DecissionTree.ClassifyMessage(message, ref spam);
                    switch (message.Classified)
                    {
                        case "spam":
                            if(spam) confusionMatrix.PredictionSpamRealSpam++;
                            if (!spam) confusionMatrix.PredictionHamRealSpam++;
                            break;
                        case "ham":
                            if(!spam) confusionMatrix.PredictionHamRealHam++;
                            if (spam) confusionMatrix.PredictionSpamRealHam++;
                            break;
                    }
                }
            }

            var countCorrectValidate = confusionMatrix.PredictionHamRealHam + confusionMatrix.PredictionSpamRealSpam;
            _accuracy.Add(Math.Round((double)countCorrectValidate / countAllMessages * 100, 2));
            _confusionMatrices.Add(confusionMatrix);
        }

        public void PrintAccuracyAndConfusionMatrix()
        {
            Console.WriteLine("Create Confusion Matrix and calculate accuracy of all Packages ...");
            double sum = 0;
            foreach (var value in _accuracy)
            {
                sum += value;
            }
            var confusionMatrix = new ConfusionMatrix();
            confusionMatrix.PrintConfusionMatrix(_confusionMatrices);
            Console.WriteLine("Accuracy: " + Math.Round(sum / _accuracy.Count, 2) + "%");
        }

    }
}
