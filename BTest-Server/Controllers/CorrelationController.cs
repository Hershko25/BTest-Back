using BTestLibrary;
using MathNet.Numerics.Statistics;
using NLog;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BTest_Server.Controllers
{
    public class CorrelationController : ApiController
    {
        private static Logger logger = LogManager.GetLogger("MyAppLoggerRules");

        BTestDbContext DB = new BTestDbContext();
        // GET api/<controller>
        public IHttpActionResult Get()
        {
            try
            {
                var AllCors = DB.AllCorrelations.Select(x => new { Index_Number = x.Index_Number, Question_Number = x.Q_number, Cor = x.correlation }).ToList();
                logger.Info("Get Correlations - Success");
                return Ok(AllCors);
            }
            catch (Exception ex)
            {
                logger.Info("Get Correlations - fail" + ex.Message);

                return Content(HttpStatusCode.BadRequest, ex);

            }
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public IHttpActionResult Post([FromBody] string value)
        {
            try
            {

                int NumberOfQuestions = 14;
                List<int> Indexs = DB.Indices.Select(x => x.Index_Number).ToList();

                Dictionary<string, Dictionary<int, double>> Questions = new Dictionary<string, Dictionary<int, double>>(); //list of all questions with all the users answer
                Dictionary<int, Dictionary<int, double>> IndexsValues = new Dictionary<int, Dictionary<int, double>>(); //list of all indexs with all the users result
                Dictionary<string, List<double>> Questions_IndexsValues = new Dictionary<string, List<double>>();
                Dictionary<string, List<double>> Questions_Answers = new Dictionary<string, List<double>>(); //

                for (int i = 1; i <= NumberOfQuestions; i++)
                {
                    Questions.Add("q" + i, new Dictionary<int, double>());
                    Questions_IndexsValues.Add("q" + i, new List<double>());
                    Questions_Answers.Add("q" + i, new List<double>());

                }
                foreach (int index in Indexs)
                {
                    IndexsValues.Add(index, new Dictionary<int, double>());

                } //add all the indexs to a dictionary

                foreach (Report_Index report in DB.Report_Index)
                {
                    if (IndexsValues[report.Index_Number].ContainsKey(report.User_Report.User_Id))
                    {
                        IndexsValues[report.Index_Number][report.User_Report.User_Id] = report.Value;
                    }
                    else
                    {
                        IndexsValues[report.Index_Number].Add(report.User_Report.User_Id, report.Value);
                    }
                } // Gets all the values of the indexs


                foreach (Health_Questionnaire Questionnaire in DB.Health_Questionnaires)
                {

                    if (Questionnaire.q1 == "y         ")
                    {
                        Questions["q1"].Add(Questionnaire.User_Id, 2);
                    }
                    if (Questionnaire.q1 == "n         ")
                    {
                        Questions["q1"].Add(Questionnaire.User_Id, 1);
                    }
                    //////////////////////////
                    if (Questionnaire.q2 == "y         ")
                    {
                        Questions["q2"].Add(Questionnaire.User_Id, 2);
                    }
                    if (Questionnaire.q2 == "n         ")
                    {
                        Questions["q2"].Add(Questionnaire.User_Id, 1);
                    }
                    //////////////////////////
                    if (Questionnaire.q3 == "y         ")
                    {
                        Questions["q3"].Add(Questionnaire.User_Id, 2);
                    }
                    if (Questionnaire.q3 == "n         ")
                    {
                        Questions["q3"].Add(Questionnaire.User_Id, 1);
                    }
                    //////////////////////////
                    if (Questionnaire.q4 == "y         ")
                    {
                        Questions["q4"].Add(Questionnaire.User_Id, 2);
                    }
                    if (Questionnaire.q4 == "n         ")
                    {
                        Questions["q4"].Add(Questionnaire.User_Id, 1);
                    }
                    //////////////////////////
                    if (Questionnaire.q5 == "y")
                    {
                        Questions["q5"].Add(Questionnaire.User_Id, 2);
                    }
                    if (Questionnaire.q5 == "n")
                    {
                        Questions["q5"].Add(Questionnaire.User_Id, 1);
                    }
                    //////////////////////////
                    if (Questionnaire.q6 == "y         ")
                    {
                        Questions["q6"].Add(Questionnaire.User_Id, 2);
                    }
                    if (Questionnaire.q6 == "n         ")
                    {
                        Questions["q6"].Add(Questionnaire.User_Id, 1);
                    }
                    //////////////////////////
                    if (Questionnaire.q7 == "y         ")
                    {
                        Questions["q7"].Add(Questionnaire.User_Id, 2);
                    }
                    if (Questionnaire.q7 == "n         ")
                    {
                        Questions["q7"].Add(Questionnaire.User_Id, 1);
                    }
                    //////////////////////////
                    if (Questionnaire.q8 == "y         ")
                    {
                        Questions["q8"].Add(Questionnaire.User_Id, 2);
                    }
                    if (Questionnaire.q8 == "n         ")
                    {
                        Questions["q8"].Add(Questionnaire.User_Id, 1);
                    }
                    //////////////////////////
                    if (Questionnaire.q9 == "y         ")
                    {
                        Questions["q9"].Add(Questionnaire.User_Id, 2);
                    }
                    if (Questionnaire.q9 == "n         ")
                    {
                        Questions["q9"].Add(Questionnaire.User_Id, 1);
                    }
                    //////////////////////////
                    if (Questionnaire.q10 == "y         ")
                    {
                        Questions["q10"].Add(Questionnaire.User_Id, 2);
                    }
                    if (Questionnaire.q10 == "n         ")
                    {
                        Questions["q10"].Add(Questionnaire.User_Id, 1);
                    }
                    //////////////////////////
                    if (Questionnaire.q11 == "y         ")
                    {
                        Questions["q11"].Add(Questionnaire.User_Id, 2);
                    }
                    if (Questionnaire.q11 == "n         ")
                    {
                        Questions["q11"].Add(Questionnaire.User_Id, 1);
                    }
                    //////////////////////////
                    if (Questionnaire.q12 == "y         ")
                    {
                        Questions["q12"].Add(Questionnaire.User_Id, 2);
                    }
                    if (Questionnaire.q12 == "n         ")
                    {
                        Questions["q12"].Add(Questionnaire.User_Id, 1);
                    }
                    //////////////////////////
                    if (Questionnaire.q13 == "y         ")
                    {
                        Questions["q13"].Add(Questionnaire.User_Id, 2);
                    }
                    if (Questionnaire.q13 == "n         ")
                    {
                        Questions["q13"].Add(Questionnaire.User_Id, 1);
                    }
                    //////////////////////////
                    if (Questionnaire.q14 == "y         ")
                    {
                        Questions["q14"].Add(Questionnaire.User_Id, 2);
                    }
                    if (Questionnaire.q14 == "n         ")
                    {
                        Questions["q14"].Add(Questionnaire.User_Id, 1);
                    }

                } //Questions check

                List<double> UsersIndexsValues = new List<double>();
                List<double> QuestionAnswers = new List<double>();
                Dictionary<int, Dictionary<string, dynamic>> AllCors = new Dictionary<int, Dictionary<string, dynamic>>();


                foreach (var index in IndexsValues.Keys)
                {
                    Dictionary<string, dynamic> Question_Cor = new Dictionary<string, dynamic>();
                    RestartDic(Questions_Answers, Questions_IndexsValues, NumberOfQuestions);

                    foreach (var user in IndexsValues[index])
                    {
                        foreach (var question in Questions.Keys)
                        {
                            foreach (var UserAnswer in Questions[question])
                            {
                                if (user.Key == UserAnswer.Key)
                                {
                                    Questions_Answers[question].Add(UserAnswer.Value);
                                    Questions_IndexsValues[question].Add(user.Value);

                                }
                            }
                        }

                    }
                    foreach (var item in Questions_Answers.Keys)
                    {
                        dynamic Cor = Correlation.Pearson(Questions_Answers[item], Questions_IndexsValues[item]);
                        Question_Cor.Add(item, Cor);
                    }
                    AllCors.Add(index, Question_Cor);
                }

                foreach (var item in AllCors.Keys)
                {
                    foreach (var item2 in AllCors[item])
                    {
                        AllCorrelation a = new AllCorrelation();
                        a.correlation = item2.Value;
                        a.Index_Number = item;
                        a.Q_number = item2.Key;
                        AllCorrelation CorToUpdate = DB.AllCorrelations.SingleOrDefault(x => x.Index_Number == a.Index_Number && x.Q_number == a.Q_number);
                        CorToUpdate = a;
                        DB.AllCorrelations.AddOrUpdate(CorToUpdate);
                        checkDBUpdate();
                        AllCorrelation bbb = DB.AllCorrelations.SingleOrDefault(x => x.Index_Number == a.Index_Number && x.Q_number == a.Q_number);
                    }
                }
                DB.SaveChanges();
                logger.Info("Update Correlations - Success");

                return Ok();
            }
            catch (Exception ex)
            {
                logger.Info("Update Correlations - Fail" + ex.Message);

                return Content(HttpStatusCode.Forbidden, ex);
            }


        }

        private void RestartDic(Dictionary<string, List<double>> D, Dictionary<string, List<double>> D2, int NumberOfQuestions)
        {
            D = new Dictionary<string, List<double>>();
            D2 = new Dictionary<string, List<double>>();
            for (int i = 1; i <= NumberOfQuestions; i++)
            {
                D.Add("q" + i, new List<double>());
                D2.Add("q" + i, new List<double>());
            }
        }

        private string checkDBUpdate()
        {
            string errors = "";

            try
            {
                if (DB.SaveChanges() > 0)
                {
                    return errors;
                }
            }
            catch (DbEntityValidationException ex)
            {
                foreach (DbEntityValidationResult res in ex.EntityValidationErrors)
                {
                    foreach (DbValidationError error in res.ValidationErrors)
                    {
                        errors += $"Property Name -  { error.PropertyName}, Error -  {error.ErrorMessage} <br/>";
                    }
                }

                return errors;

            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw ex.InnerException;


            }
            catch (DbUpdateException ex)
            {
                foreach (DbEntityEntry entry in ex.Entries)
                {
                    errors += $"Error in entity : {entry.Entity.GetType().Name} State : {entry.State} ";
                    foreach (string prop in entry.CurrentValues.PropertyNames)
                    {
                        errors += $"for colum - {prop} , value - {entry.CurrentValues[prop]}";
                    }
                    errors += "</br>";
                }
                return errors;
            }
            catch (Exception ex)
            {
                throw ex.InnerException;

            }
            return errors;

        }
    }
}