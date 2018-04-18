using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Naive_Bayes_DT.Decission_Tree;

namespace Naive_Bayes_DT
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    [SuppressMessage("ReSharper", "ArrangeTypeModifiers")]
    class Program
    {
        [SuppressMessage("ReSharper", "ArrangeTypeMemberModifiers")]
        static void Main(string[] args)
        {
            var dataReader = new DataReader();
            var foldCrossValidation = new FoldCrossValidation();
            //speicher die Daten vom übergebenen Datensatz in eine Liste ab
            dataReader.ReadDataFromFile("C:\\Users\\Alexa\\Google Drive\\Studium\\4. Semester\\Machine Learning\\Datensätze\\SMS Spam collection Dataset\\SMSSpamCollection");
            //Teile den Input in 10 Listen auf 
            foldCrossValidation.CreateDataPackage(dataReader.HamMessages, dataReader.SpamMessages);

            var modifiedList = new Attributes(foldCrossValidation.DataSetPackages);
            modifiedList.SearchAndGeneralizeAttributes();

        }
    }
}
