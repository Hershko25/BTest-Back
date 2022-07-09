using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using ConvertApiDotNet;
using BTestLibrary;
using System.IO;
using System.Text.RegularExpressions;
using System.Web.Http.Results;
using System.Web;
using NLog;

namespace BTest_Server.Controllers
{
    public class PdfFileController : ApiController
    {
        private static Logger logger = LogManager.GetLogger("MyAppLoggerRules");

        // GET api/<controller>
        public IHttpActionResult Get()
        {
            return BadRequest();
        }



        // GET api/<controller>/5
        public IHttpActionResult Get(string id)
        {
            return BadRequest();
        }

        // POST api/<controller>
        public async Task<HttpResponseMessage> PostAsync()
        {
            try
            {
                var httpRequest = HttpContext.Current.Request;
                if (httpRequest.Files.Count < 1)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }
                string ApiToken = "BneS6Ibiw3HkA5fp";
                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];
                    var filePath = HttpContext.Current.Server.MapPath("~/" + postedFile.FileName);
                    postedFile.SaveAs(filePath);
                    var convertApi = new ConvertApi(ApiToken);

                    var convertToPdf = await convertApi.ConvertAsync("pdf", "txt", new ConvertApiFileParam(filePath));
                    //Save TXT file 
                    var files = await convertToPdf.SaveFilesAsync(HttpContext.Current.Server.MapPath("~/")); //CHANGE TO THE SERVER
                }

                logger.Info("User PDF Upload - Success");
                return Request.CreateResponse(HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                logger.Info("User PDF Upload - Fail"+ ex.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest,ex.InnerException);
            }
        }

        // PUT api/<controller>/5
        public IHttpActionResult Put(int id, [FromBody] string value)
        {
            return BadRequest();

        }

        // DELETE api/<controller>/5
        public IHttpActionResult Delete(int id)
        {
            return BadRequest();
        }
    }
}