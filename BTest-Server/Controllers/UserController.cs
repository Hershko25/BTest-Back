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
    public class UserController : ApiController
    {
        private static Logger logger = LogManager.GetLogger("MyAppLoggerRules");

        // GET api/<controller>
        BTestDbContext DB = new BTestDbContext();

        // POST api/<controller> NEW USER
        public IHttpActionResult Post([FromBody] Registered_User value)
        {
            try
            {

                if (value.PDF == false)
                {
                    value.SetJSONIndexs();
                }

                if (value.PDF == true)
                {
                    Report_Index[] TruePDFIndexs = new Report_Index[0];
                    try
                    {
                        TruePDFIndexs = value.ReadFromPDF(1);
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

                logger.Info("Create new not regi user- Success");

                return Created(new Uri(Request.RequestUri.AbsoluteUri), value.JsonIndexs);
            }
            catch (Exception ex)

            {
                logger.Info("Create not regi user- Fail" + ex.Message);
                return BadRequest(ex.InnerException.Message);
            }

        } // Returns user report

        private IHttpActionResult ReturnUser(Registered_User User)
        {
            try
            {
                List<Report_Index> UserIndexs = User.UserIndexs.ToList();
                List<Index> AllsystemIndexs = DB.Indices.ToList(); // כל האינדקסים שקיימים בדאטה בייס
                List<Index> UserJSONIndexs = new List<Index>(); // כל האינדקסים והפירוש שלהם
                string JsonIndexs = ""; // גייסון שיחזור לאחר מכן



                foreach (Report_Index UserReportIndex in UserIndexs) // בדיקה של כל הגייסונים והמשמעות שלהם
                {
                    foreach (Index SystemIndex in AllsystemIndexs)
                    {
                        if (UserReportIndex.Index_Number == SystemIndex.Index_Number)
                        {

                            if (UserReportIndex.Value < SystemIndex.Index_Min_Number)
                            {
                                Index BelowIndex = new Index()
                                {
                                    Index_Number = SystemIndex.Index_Number,
                                    Index_Name_En = SystemIndex.Index_Name_En,
                                    Index_Name_He = SystemIndex.Index_Name_He,
                                    The_purpose_of_the_test = SystemIndex.The_purpose_of_the_test,
                                    Index_Explanation_below_norm = SystemIndex.Index_Explanation_below_norm,
                                    Index_Max_Number = SystemIndex.Index_Max_Number,
                                    Index_Min_Number = SystemIndex.Index_Min_Number,
                                    IndexStatus = "Below"
                                };
                                UserJSONIndexs.Add(BelowIndex);
                            }

                            if (UserReportIndex.Value > SystemIndex.Index_Max_Number)
                            {
                                Index AboveIndex = new Index()
                                {
                                    Index_Number = SystemIndex.Index_Number,
                                    Index_Name_En = SystemIndex.Index_Name_En,
                                    Index_Name_He = SystemIndex.Index_Name_He,
                                    The_purpose_of_the_test = SystemIndex.The_purpose_of_the_test,
                                    Index_Explanation_above_norm = SystemIndex.Index_Explanation_above_norm,
                                    Index_Max_Number = SystemIndex.Index_Max_Number,
                                    Index_Min_Number = SystemIndex.Index_Min_Number,
                                    IndexStatus = "Above"
                                };
                                UserJSONIndexs.Add(AboveIndex);
                            }

                            if (UserReportIndex.Value < SystemIndex.Index_Max_Number && UserReportIndex.Value > SystemIndex.Index_Min_Number || SystemIndex.Index_Min_Number == null && SystemIndex.Index_Max_Number == null)
                            {
                                Index AboveIndex = new Index()
                                {
                                    Index_Number = SystemIndex.Index_Number,
                                    Index_Name_En = SystemIndex.Index_Name_En,
                                    Index_Name_He = SystemIndex.Index_Name_He,
                                    The_purpose_of_the_test = SystemIndex.The_purpose_of_the_test,
                                    Index_Max_Number = SystemIndex.Index_Max_Number,
                                    Index_Min_Number = SystemIndex.Index_Min_Number,
                                    IndexStatus = "Fine"
                                };
                                UserJSONIndexs.Add(AboveIndex);
                            }
                        }
                    }
                   

                }

                JsonIndexs = JsonConvert.SerializeObject(new
                {
                    Indexs = UserJSONIndexs
                });
                logger.Info("Not regi User Login - Success");

                return Ok(JsonIndexs);
            }
            catch (Exception ex)
            {
                logger.Info("Not regi User Login - Fail" + ex.Message);

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

    }
}