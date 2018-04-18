using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naive_Bayes_DT.Decission_Tree
{
    internal class DataReader
    {
        public List<string> HamMessages { get; private set; } = new List<string>();
        public List<string> SpamMessages { get; private set; } = new List<string>();

        public void ReadDataFromFile(string filePath)
        {
            try
            {
                var dataSet = System.IO.File.ReadAllLines(filePath);
                CheckSpamOrHam(dataSet);
            }
            catch (Exception e)
            {
                //TODO: Fehlerhandling
                // ignored
            }
        }

        private void CheckSpamOrHam(string[] message)
        {
            foreach (var line in message)
            {
                var separatedLine = line.Split(';');
                switch (separatedLine[0])
                {
                    case "spam":
                        AddSpamMessage(separatedLine[1]);
                        break;
                    case "ham":
                        AddHamMessage(separatedLine[1]);
                        break;
                    default:
                        throw new ArgumentException("You've sent wrong dataset!");
                }
            }
        }

        private void AddHamMessage(string message)
        {
            HamMessages.Add(message);
        }

        private void AddSpamMessage(string message)
        {
            SpamMessages.Add(message);
        }
    }
}
