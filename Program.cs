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
            var dataReader = new DataReader();
            var foldCrossValidation = new FoldCrossValidation();
            //speicher die Daten vom übergebenen Datensatz in eine Liste ab
            dataReader.ReadDataFromFile("C:\\Users\\Alexa\\Google Drive\\Studium\\4. Semester\\Machine Learning\\Datensätze\\SMS Spam collection Dataset\\SMSSpamCollection");
            //Teile den Input in 10 Listen auf 
            foldCrossValidation.CreateDataPackage(dataReader.HamMessages, dataReader.SpamMessages);
            var messageClassifier = new MessageClassifier(foldCrossValidation, dataReader.SpamMessages.Count, dataReader.HamMessages.Count);
            stopwatch.Stop();
            var AllMessages = dataReader.SpamMessages.Count + dataReader.HamMessages.Count;
            Console.WriteLine("The Decission-Tree-Algorithm needed " + Math.Round((double)stopwatch.ElapsedMilliseconds/1000, 2) + " Seconds to validate spam or ham of " + AllMessages + " messages.");
        }
    }
}
