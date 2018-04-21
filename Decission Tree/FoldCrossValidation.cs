using System;
using System.Collections.Generic;

namespace Decission_Tree.Decission_Tree
{
    internal class FoldCrossValidation
    {
        public List<List<Message>> DataSetPackages { get; private set; } = new List<List<Message>>
        {
            new List<Message>(),
            new List<Message>(),
            new List<Message>(),
            new List<Message>(),
            new List<Message>(),
            new List<Message>(),
            new List<Message>(),
            new List<Message>(),
            new List<Message>(),
            new List<Message>()
        };

        public void CreateDataPackage(List<string> hamMessages, List<string> spamMessages)
        {
            try
            {
                CreateDataPackagesSpam(spamMessages);
                CreateDataPackagesHam(hamMessages);
            }
            catch (Exception)
            {
                //TODO: Fehlerhandling
                // ignored
            }
        }

        private void CreateDataPackagesHam(List<string> hamMessages)
        {
            //Gehe durch alle Ham Messages durch und teile es auf 10 Pakete auf. Daher modulo 10!
            for (var i = 0; i < hamMessages.Count; i++)
            {
                DataSetPackages[i % 10].Add(new Message("ham", hamMessages[i]));
            }
        }

        private void CreateDataPackagesSpam(List<string> spamMessages)
        {
            //Gehe durch alle Spam Messages durch und teile es auf 10 Pakete auf. Daher modulo 10!
            for (var i = 0; i < spamMessages.Count; i++)
            {
                DataSetPackages[i % 10].Add(new Message("spam", spamMessages[i]));
            }
        }
    }
}
