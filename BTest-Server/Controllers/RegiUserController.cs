using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BTestLibrary;
using Newtonsoft.Json;
using NLog;


namespace BTest_Server.Controllers
{
    public class RegiUserController : ApiController
    {
        private static Logger logger = LogManager.GetLogger("MyAppLoggerRules");
        // GET api/<controller>
        BTestDbContext DB = new BTestDbContext();

        public IHttpActionResult Get() //GET ALL USERS
        {
            try
            {
                var AllUsers = DB.Registered_Users.Select(x => new { name = x.Full_Name, id = x.User_Id, email = x.Email, date = x.Birthday, password = x.Password }).ToList();
                return Ok(AllUsers);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);

            }
        }
        // GET api/<controller>/EMAIL
        [HttpGet]
        [Route("api/regiuser/{email}")]
        public IHttpActionResult GetUser(string email)
        {
            try
            {
                Registered_User User = DB.Registered_Users.SingleOrDefault(x => x.Email == email);

                if (User != null)
                {
                    return ReturnUser(User);
                }
                else
                    return BadRequest("No user");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException.ToString());

            }
        } //GET ANY USER

        [HttpGet]
        [Route("api/regiuser/{email}/{value}")]
        public IHttpActionResult Get(string email, string value)
        {
            try
            {
                Registered_User User = DB.Registered_Users.SingleOrDefault(x => x.Email == email);
                if (User == null)
                {
                    return BadRequest("No exist user");
                }
                if (User != null & User.Password == value)
                {
                    // PASSWORD OK
                    return ReturnUser(User);
                }
                // PASSWORD WRONG
                return BadRequest("Wrong password");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException.ToString());

            }
        } //USER LOGIN

        [HttpGet]
        [Route("api/regiuser/mailcheck/{email}")]
        public IHttpActionResult CheckEmail(string email)
        {
            try
            {
                Registered_User User = DB.Registered_Users.SingleOrDefault(x => x.Email == email);
                if (User == null)
                {
                    return Ok("User not exist");
                }

                return Ok("User exist");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException.ToString());

            }
        } //CHECK EMAIL

        // POST api/<controller> NEW USER
        public IHttpActionResult Post([FromBody] Registered_User value)
        {
            try
            {
                Registered_User CheckUserEmail = DB.Registered_Users.SingleOrDefault(x => x.Email == value.Email);

                if (CheckUserEmail != null) //בדיקת מייל
                {
                    return BadRequest("Exist email : " + value.Email);
                }
                DB.Registered_Users.Add(value);
                if (checkDBUpdate() != "")
                {
                    return BadRequest(checkDBUpdate());
                }

                int ReportID = DB.User_Reports.Select(x => x.Report_Id).Max();

                if (value.PDF == false)
                {
                    for (int i = 0; i < value.UserIndexs.Length; i++)
                    {
                        Report_Index NewReportIndex = new Report_Index();
                        NewReportIndex.Report_Id = ReportID;
                        NewReportIndex.Index_Number = value.UserIndexs[i].Index_Number;
                        NewReportIndex.Value = value.UserIndexs[i].Value;
                        DB.Report_Index.Add(NewReportIndex);
                    }
                    value.SetJSONIndexs();
                }

                if (value.PDF == true)
                {
                    Report_Index[] TruePDFIndexs = new Report_Index[0];
                    try
                    {
                        TruePDFIndexs = value.ReadFromPDF(ReportID);
                        foreach (Report_Index report in TruePDFIndexs)
                        {
                            DB.Report_Index.Add(report);
                        }
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(ex.InnerException.Message);
                    }
                }

                if (checkDBUpdate() != "")
                {
                    return BadRequest(checkDBUpdate());
                }

                logger.Info("Add user - Success");
                return ReturnUser(value);
            }
            catch (Exception ex)
            {
                logger.Info("Add user - Fail" + ex.Message);
                return BadRequest(ex.InnerException.Message);
            }

        } // MAKE NEW USER

        //POST api/<controller>/{email} NEW Report
        [HttpPost]
        [Route("api/regiuser/{email}")]
        public IHttpActionResult Post([FromBody] Registered_User value, string email)
        {
            try
            {
                Registered_User User = DB.Registered_Users.SingleOrDefault(x => x.Email == email);


                User_Report New_user_Report = new User_Report();
                New_user_Report.Report_Id = DB.User_Reports.Select(x => x.Report_Id).Max() + 1;
                New_user_Report.User_Id = User.User_Id;
                DB.User_Reports.Add(New_user_Report);
                if (checkDBUpdate() != "")
                {
                    return BadRequest(checkDBUpdate());
                }

                if (value.PDF == false)
                {
                    for (int i = 0; i < value.UserIndexs.Length; i++)
                    {
                        Report_Index NewReportIndex = new Report_Index();
                        NewReportIndex.Report_Id = New_user_Report.Report_Id;
                        NewReportIndex.Index_Number = value.UserIndexs[i].Index_Number;
                        NewReportIndex.Value = value.UserIndexs[i].Value;
                        DB.Report_Index.Add(NewReportIndex);
                    }
                    value.SetJSONIndexs();
                }

                if (value.PDF == true)
                {
                    Report_Index[] TruePDFIndexs = new Report_Index[0];
                    try
                    {
                        TruePDFIndexs = value.ReadFromPDF(New_user_Report.Report_Id);
                        foreach (Report_Index report in TruePDFIndexs)
                        {
                            DB.Report_Index.Add(report);
                        }
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(ex.InnerException.Message);
                    }
                }

                if (checkDBUpdate() != "")
                {
                    return BadRequest(checkDBUpdate());
                }

                logger.Info("Add user - Success");
                return ReturnUser(DB.Registered_Users.SingleOrDefault(x => x.Email == email));
            }
            catch (Exception ex)
            {
                logger.Info("Add user - Fail" + ex.Message);
                return BadRequest(ex.InnerException.Message);
            }

        } //New user report
        //PUT api/<controller>/EMAIL, NEW VALUES

        public IHttpActionResult Put(string id, [FromBody] Registered_User value)
        {
            try
            {
                Registered_User UserToUpdate = DB.Registered_Users.SingleOrDefault(x => x.Email == id);
                if (UserToUpdate != null)
                {
                    UserToUpdate = value;
                    checkDBUpdate();
                    logger.Info("Update user - Success");
                    return Ok(UserToUpdate.User_Id);
                }
                return Content(HttpStatusCode.NotFound, "User Not Found");

            }
            catch (Exception ex)
            {
                logger.Info("Update user - Fail" + ex.Message);
                return Content(HttpStatusCode.Forbidden, ex);
            }
        }//UPDATE USER

        // DELETE api/<controller>/EMAIL
        public IHttpActionResult Delete(string id) //CHECK IF WE NEED IT
        {
            try
            {
                Registered_User UserToDelete = DB.Registered_Users.SingleOrDefault(s => s.Email == id);
                if (UserToDelete != null)
                {
                    DB.Registered_Users.Remove(UserToDelete);
                    checkDBUpdate();
                    logger.Info("Delete user - Success");
                    return Ok();
                }
                return Content(HttpStatusCode.NotFound, "User Not Found");

            }
            catch (Exception ex)
            {
                logger.Info("Delete user - Fail" + ex.Message);
                return Content(HttpStatusCode.Forbidden, ex);
            }
        } //NOT READY

        private IHttpActionResult ReturnUser(Registered_User User)
        {
            try
            {

                List<User_Report> ReportIDS = DB.User_Reports.Where(x => x.User_Id == User.User_Id).ToList(); // מוצא את כל הדוחות של היוזר
                User_Report ReportID = ReportIDS.Last(); // מוצא את הדוח האחרון של היוזר
                List<Report_Index> UserIndexs = DB.Report_Index.Where(x => x.Report_Id == ReportID.Report_Id).ToList(); //לוקח את כל האינדקסים שיש בדוח האחרון
                List<Index> AllsystemIndexs = DB.Indices.ToList(); // כל האינדקסים שקיימים בדאטה בייס
                List<Index> UserJSONIndexs = new List<Index>(); // כל האינדקסים והפירוש שלהם
                List<Index_Number_Recommendation_Id> AllSystemRecommendations = DB.Index_Number_Recommendation_Id.ToList(); // כל ההמלצות 
                List<Food_Index> AllDBFoods = DB.Food_Index.ToList();

                string JsonIndexs = ""; // גייסון שיחזור לאחר מכן

                foreach (Report_Index UserReportIndex in UserIndexs) // בדיקה של כל הגייסונים והמשמעות שלהם
                {
                    foreach (Index SystemIndex in AllsystemIndexs)
                    {
                        if (UserReportIndex.Index_Number == SystemIndex.Index_Number)
                        {
                            List<Index_Number_Recommendation_Id> a = AllSystemRecommendations.Where(x => x.Index_Number == SystemIndex.Index_Number).ToList();
                            string AboveRecoommend = "";
                            string BelowRecommend = "";
                            if (a.Count != 0)
                            {
                                foreach (Index_Number_Recommendation_Id recommendation in a)
                                {
                                    if (recommendation.AboveBelow == "Above     ")
                                    {
                                        AboveRecoommend = recommendation.Recommendation.Recommendation1;
                                    }
                                    if (recommendation.AboveBelow == "Below     ")
                                    {
                                        BelowRecommend = recommendation.Recommendation.Recommendation1;
                                    }
                                }
                            }

                            if (UserReportIndex.Value < SystemIndex.Index_Min_Number)
                            {
                                Index BelowIndex = new Index()
                                {
                                    Index_Number = SystemIndex.Index_Number,
                                    index_Value = UserReportIndex.Value,
                                    Index_Name_En = SystemIndex.Index_Name_En,
                                    Index_Name_He = SystemIndex.Index_Name_He,
                                    The_purpose_of_the_test = SystemIndex.The_purpose_of_the_test,
                                    Index_Explanation_below_norm = SystemIndex.Index_Explanation_below_norm,
                                    Index_Max_Number = SystemIndex.Index_Max_Number,
                                    Index_Min_Number = SystemIndex.Index_Min_Number,
                                    IndexStatus = "Below",
                                    Recommend = BelowRecommend,
                                    Foods = checkFood(AllDBFoods.Where(x => x.Index_Id == SystemIndex.Index_Number && x.Index_Type == "LOW       ").ToList())
                                };
                                UserJSONIndexs.Add(BelowIndex);
                            }

                            if (UserReportIndex.Value > SystemIndex.Index_Max_Number)
                            {
                                Index AboveIndex = new Index()
                                {
                                    Index_Number = SystemIndex.Index_Number,
                                    index_Value = UserReportIndex.Value,
                                    Index_Name_En = SystemIndex.Index_Name_En,
                                    Index_Name_He = SystemIndex.Index_Name_He,
                                    The_purpose_of_the_test = SystemIndex.The_purpose_of_the_test,
                                    Index_Explanation_above_norm = SystemIndex.Index_Explanation_above_norm,
                                    Index_Max_Number = SystemIndex.Index_Max_Number,
                                    Index_Min_Number = SystemIndex.Index_Min_Number,
                                    IndexStatus = "Above",
                                    Recommend = AboveRecoommend,
                                    Foods = checkFood(AllDBFoods.Where(x => x.Index_Id == SystemIndex.Index_Number && x.Index_Type == "HIGH       ").ToList())

                                };
                                UserJSONIndexs.Add(AboveIndex);
                            }

                            if (UserReportIndex.Value < SystemIndex.Index_Max_Number && UserReportIndex.Value > SystemIndex.Index_Min_Number || SystemIndex.Index_Min_Number == null && SystemIndex.Index_Max_Number == null)
                            {
                                Index AboveIndex = new Index()
                                {
                                    Index_Number = SystemIndex.Index_Number,
                                    index_Value = UserReportIndex.Value,
                                    Index_Name_En = SystemIndex.Index_Name_En,
                                    Index_Name_He = SystemIndex.Index_Name_He,
                                    The_purpose_of_the_test = SystemIndex.The_purpose_of_the_test,
                                    Index_Max_Number = SystemIndex.Index_Max_Number,
                                    Index_Min_Number = SystemIndex.Index_Min_Number,
                                    IndexStatus = "Fine",

                                };
                                UserJSONIndexs.Add(AboveIndex);
                            }
                        }
                    }
                }

                JsonIndexs = JsonConvert.SerializeObject(new
                {
                    User = User.Email,
                    UserName = User.Full_Name,
                    User.Birthday,
                    User.Health_Questionnaire.LastOrDefault().Height,
                    User.Health_Questionnaire.LastOrDefault().Weight,
                    Indexs = UserJSONIndexs,
                    Cors = checkCors(UserIndexs, User.User_Id),
                    aa = ReturnQestions(User.User_Id)
                });
                logger.Info("User Login - Success");

                return Ok(JsonIndexs);
            }
            catch (Exception ex)
            {
                logger.Info("User Login - Fail" + ex.Message);
                return BadRequest(ex.Message);

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

        private List<AllCorrelation> checkCors(List<Report_Index> indexs, int id)
        {
            List<AllCorrelation> AllCors = DB.AllCorrelations.ToList();
            List<AllCorrelation> UserCors = new List<AllCorrelation>();
            Health_Questionnaire User_health_Questionnaire = DB.Health_Questionnaires.SingleOrDefault(x => x.User_Id == id);
            List<string> AllYesQuestions = new List<string>();
            #region
            if (User_health_Questionnaire.q1.Contains("y"))
            {
                AllYesQuestions.Add("q1");
            }
            if (User_health_Questionnaire.q2.Contains("y"))
            {
                AllYesQuestions.Add("q2");
            }
            if (User_health_Questionnaire.q3.Contains("y"))
            {
                AllYesQuestions.Add("q3");
            }
            if (User_health_Questionnaire.q4.Contains("y"))
            {
                AllYesQuestions.Add("q4");
            }
            if (User_health_Questionnaire.q5.Contains("y"))
            {
                AllYesQuestions.Add("q5");
            }
            if (User_health_Questionnaire.q6.Contains("y"))
            {
                AllYesQuestions.Add("q6");
            }
            if (User_health_Questionnaire.q7.Contains("y"))
            {
                AllYesQuestions.Add("q7");
            }
            if (User_health_Questionnaire.q8.Contains("y"))
            {
                AllYesQuestions.Add("q8");
            }
            if (User_health_Questionnaire.q9.Contains("y"))
            {
                AllYesQuestions.Add("q9");
            }
            if (User_health_Questionnaire.q10.Contains("y"))
            {
                AllYesQuestions.Add("q10");
            }
            if (User_health_Questionnaire.q11.Contains("y"))
            {
                AllYesQuestions.Add("q11");
            }
            if (User_health_Questionnaire.q12.Contains("y"))
            {
                AllYesQuestions.Add("q12");
            }
            if (User_health_Questionnaire.q13.Contains("y"))
            {
                AllYesQuestions.Add("q13");
            }
            if (User_health_Questionnaire.q14.Contains("y"))
            {
                AllYesQuestions.Add("q14");
            }
            #endregion //Check user questions 

            try
            {
                foreach (Report_Index index in indexs)
                {
                    foreach (string question in AllYesQuestions)
                    {
                        List<AllCorrelation> a = AllCors.Where(x => x.Index_Number == index.Index_Number && x.correlation > 0.75 && x.Q_number.Contains(question) || x.Index_Number == index.Index_Number && x.correlation < -0.75 && x.Q_number.Contains(question)).ToList();
                        foreach (AllCorrelation correlation in a)
                        {
                            UserCors.Add(correlation);
                        }
                    }
                }
                logger.Info("Return Cors - Success");
                return UserCors;

            }
            catch (Exception ex)
            {
                logger.Info("Return Cors - Failed" + ex.Message);
                throw ex.InnerException;
            }
        }

        private Dictionary<string,string> ReturnQestions(int id)
        {
            Health_Questionnaire userAnswers = DB.Health_Questionnaires.SingleOrDefault(x => x.User_Id == id);
            Dictionary<string,string> userQuestions = new Dictionary<string, string>();
            #region 
            try
            {
                if (userAnswers.q1 != null)
                {
                    userQuestions.Add("q1", userAnswers.q1);
                }
                if (userAnswers.q2 != null)
                {
                    userQuestions.Add("q2", userAnswers.q2);
                }
                if (userAnswers.q3 != null)
                {
                    userQuestions.Add("q3", userAnswers.q3);
                }
                if (userAnswers.q4 != null)
                {
                    userQuestions.Add("q4", userAnswers.q4);
                }
                if (userAnswers.q5 != null)
                {
                    userQuestions.Add("q5", userAnswers.q5);
                }
                if (userAnswers.q6 != null)
                {
                    userQuestions.Add("q6", userAnswers.q6);
                }
                if (userAnswers.q7 != null)
                {
                    userQuestions.Add("q7", userAnswers.q7);
                }
                if (userAnswers.q8 != null)
                {
                    userQuestions.Add("q8", userAnswers.q8);
                }
                if (userAnswers.q9 != null)
                {
                    userQuestions.Add("q9", userAnswers.q9);
                }
                if (userAnswers.q10 != null)
                {
                    userQuestions.Add("q10", userAnswers.q10);
                }
                if (userAnswers.q11 != null)
                {
                    userQuestions.Add("q11", userAnswers.q11);
                }
                if (userAnswers.q12 != null)
                {
                    userQuestions.Add("q12", userAnswers.q12);
                }
                if (userAnswers.q13 != null)
                {
                    userQuestions.Add("q13", userAnswers.q13);
                }
                if (userAnswers.q14 != null)
                {
                    userQuestions.Add("q14", userAnswers.q14);
                }
                #endregion //questions check

                logger.Info("Return Qestions Log - Success");
                return userQuestions;
            }
            catch (Exception ex)
            {
                logger.Info("Return Qestions - Failed" + ex.Message);
                throw ex.InnerException; 
            }
        }

        private string checkFood(List<Food_Index> list)
        {
            string answer = "";
            foreach (Food_Index food_Index in list)
            {
                answer += food_Index.Food.Food_Name + " | ";
            }

            return answer;
        }


    

    }
}