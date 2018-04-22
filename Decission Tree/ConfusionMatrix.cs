using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decission_Tree.Decission_Tree
{
    internal class ConfusionMatrix
    {
        public int PredictionSpamRealSpam { get; set; } = 0;
        public int PredictionHamRealSpam { get; set; } = 0;
        public int PredictionSpamRealHam { get; set; } = 0;
        public int PredictionHamRealHam { get; set; } = 0;

        public void PrintConfusionMatrix(List<ConfusionMatrix> confusionMatrices)
        {
            if (confusionMatrices == null) return;
            SetAllAttributesFromGivenList(confusionMatrices);
            Console.WriteLine("|--------------------------------------------------------|");
            Console.WriteLine("|-------------- Prediction: Spam ------- Prediction: Ham-|");
            Console.WriteLine("|Real: Spam ----------- " + PredictionSpamRealSpam + " -------------------- " + PredictionHamRealSpam + " ----|");
            Console.WriteLine("|Real: Ham ------------ " + PredictionSpamRealHam + " -------------------- " + PredictionHamRealHam + " ----|");
            Console.WriteLine("|________________________________________________________|");
        }

        private void SetAllAttributesFromGivenList(IEnumerable<ConfusionMatrix> confusionMatrices)
        {
            foreach (var confusionMatrix in confusionMatrices)
            {
                PredictionSpamRealSpam += confusionMatrix.PredictionSpamRealSpam;
                PredictionHamRealHam += confusionMatrix.PredictionHamRealHam;
                PredictionHamRealSpam += confusionMatrix.PredictionHamRealSpam;
                PredictionSpamRealHam += confusionMatrix.PredictionSpamRealHam;
            }
        }
    }
}
