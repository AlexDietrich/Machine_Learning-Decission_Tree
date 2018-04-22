using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Decission_Tree.Decission_Tree;

namespace Decission_Tree
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    [SuppressMessage("ReSharper", "ArrangeTypeModifiers")]
    class Program
    {
        [SuppressMessage("ReSharper", "ArrangeTypeMemberModifiers")]
        static void Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            #region Decission Tree
            var dataReader = new DataReader();
            var foldCrossValidation = new FoldCrossValidation();
            //speicher die Daten vom übergebenen Datensatz in eine Liste ab
            dataReader.ReadDataFromFile("C:\\Users\\Alexa\\Google Drive\\Studium\\4. Semester\\Machine Learning\\Datensätze\\SMS Spam collection Dataset\\SMSSpamCollection");
            //Teile den Input in 10 Listen auf 
            foldCrossValidation.CreateDataPackage(dataReader.HamMessages, dataReader.SpamMessages);
            var messageClassifier = new MessageClassifier(foldCrossValidation);
            messageClassifier.ExecuteTenFoldCrossValidation();
            messageClassifier.PrintAccuracyAndConfusionMatrix();
            #endregion
            
            stopwatch.Stop();
            var elapsedTime = (double) stopwatch.ElapsedMilliseconds / 1000;
            var allMessages = (double) dataReader.SpamMessages.Count + dataReader.HamMessages.Count;
            Console.WriteLine("The Decission-Tree-Algorithm was working for " + Math.Round(elapsedTime, 2) + " seconds to classify Spam or Ham of " + Math.Round(allMessages, 0) + " messages.");
            Console.WriteLine("Each second were classified " + Math.Round(value: allMessages/elapsedTime, digits: 0) + " messages.");
        }
    }
}
