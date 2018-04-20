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
            var dataReader = new DataReader();
            var foldCrossValidation = new FoldCrossValidation();
            //speicher die Daten vom übergebenen Datensatz in eine Liste ab
            dataReader.ReadDataFromFile("C:\\Users\\Alexa\\Google Drive\\Studium\\4. Semester\\Machine Learning\\Datensätze\\SMS Spam collection Dataset\\SMSSpamCollection");
            //Teile den Input in 10 Listen auf 
            foldCrossValidation.CreateDataPackage(dataReader.HamMessages, dataReader.SpamMessages);

            var modifiedList = new Attributes(foldCrossValidation.DataSetPackages);
            modifiedList.SearchAndGeneralizeAttributes();

            var frequenceTable = new FrequenceTable(modifiedList.Entries);

            var decissionTree = new CalculateDecissionTree(frequenceTable, dataReader.HamMessages.Count, dataReader.SpamMessages.Count, porpertyList);
        }
    }
}
