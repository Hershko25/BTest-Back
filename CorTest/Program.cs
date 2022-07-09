using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BTestLibrary;
using MathNet.Numerics.Statistics;


namespace CorTest
{
    class Program
    {
        static void Main(string[] args)
        {

            BTestDbContext DB = new BTestDbContext();

            Dictionary<string, List<int>> Questions = new Dictionary<string, List<int>>();


            for (int i = 1; i <= 14; i++)
            {
                Questions.Add("q" + i, new List<int>());
            }


            foreach (Health_Questionnaire Questionnaire in DB.Health_Questionnaires)
            {
                for (int i = 1; i <= 14; i++)
                {
                    if (Questionnaire.q1 == "y")
                    {
                        Questions["q" + i].Add(2);
                    }
                    if (Questionnaire.q1 == "n")
                    {
                        Questions["q" + i].Add(1);
                    }
                }

            }
        }
    }
}
