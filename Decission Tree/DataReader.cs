﻿using System;
using System.Collections.Generic;

namespace Decission_Tree.Decission_Tree
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
            catch (Exception)
            {
                //TODO: Fehlerhandling
                // ignored
            }
        }

        private void CheckSpamOrHam(IEnumerable<string> message)
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
