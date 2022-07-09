using ConvertApiDotNet;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace BTestLibrary
{
    [MetadataType(typeof(Registered_UserUserMetaData))]
    public partial class Registered_User
    {
        public List<string> FileName { get; set; }
        public bool PDF { get; set; }
        public Report_Index[] UserIndexs { get; set; }
        public List<Index> UserJSONIndexs { get; set; } = new List<Index>();
        public string JsonIndexs { get; set; }

        public Report_Index[] ReadFromPDF(int ReportId)
        {
            Dictionary<string, string> ReportResult = new Dictionary<string, string>();

            foreach (string file in FileName)
            {
                string pathToFiles = HttpContext.Current.Server.MapPath("~/" + $"{file}.txt");

                string ans = File.ReadAllText(pathToFiles);



                string[] parts = Regex.Split(ans, @"(?=הבדיקה)");
                string Allvalues = parts[2];
                string[] stringSeparators = new string[] { "\r\n\r\n", "\r\n", "LUC%" };
                string[] Splitedstring = Allvalues.Split(stringSeparators, StringSplitOptions.None);
                for (int i = 0; i < Splitedstring.Length; i++)
                {
                    if (Splitedstring[i].EndsWith(" % ") == true)
                    {
                        Splitedstring[i] += "LUC%";
                    }
                }


                for (int i = 1; i < Splitedstring.Length; i++)
                {
                    var firstSpaceIndex = Splitedstring[i].IndexOf(" ");
                    if (Splitedstring[i].Contains("תוצאות") == false)
                    {
                        ReportResult.Add(Splitedstring[i].Substring(firstSpaceIndex + 1), Splitedstring[i].Substring(0, firstSpaceIndex));

                    }
                }
            }

            BTestDbContext DB = new BTestDbContext();
            UserIndexs = new Report_Index[0];
            var allIndexs = DB.Indices;
            foreach (var indexReport in ReportResult)
            {
                foreach (var IndexOriginal in allIndexs)
                {
                    if (indexReport.Key == IndexOriginal.Symbol)
                    {
                        Report_Index NewRecord = new Report_Index();
                        NewRecord.Report_Id = ReportId;
                        NewRecord.Index_Number = IndexOriginal.Index_Number;
                        NewRecord.Value = double.Parse(indexReport.Value);

                        UserJSONIndexs.Add(CheckIndexValues(NewRecord, IndexOriginal));

                        var tempList = UserIndexs.ToList();
                        tempList.Add(NewRecord);
                        UserIndexs = tempList.ToArray();
                    }
                }
            }

            JsonIndexs = JsonConvert.SerializeObject(new
            {
                Indexs = UserJSONIndexs
            });

            return UserIndexs;
        }

        public void SetJSONIndexs()
        {
            BTestDbContext DB = new BTestDbContext();

            var allIndexs = DB.Indices;


            for (int i = 0; i < UserIndexs.Length; i++)
            {
                foreach (Index index in allIndexs)
                {
                    if (UserIndexs[i].Index_Number == index.Index_Number)
                    {
                        UserJSONIndexs.Add(CheckIndexValues(UserIndexs[i], index));
                    }
                }
            }


            JsonIndexs = JsonConvert.SerializeObject(new
            {
                Indexs = UserJSONIndexs
            });


        }

        private Index CheckIndexValues(Report_Index UserReport, Index IndexOriginal)
        {
            Index NewIndex = new Index();

            if (UserReport.Value < IndexOriginal.Index_Min_Number)
            {
                Index BelowIndex = new Index()
                {
                    index_Value = UserReport.Value,
                    Index_Number = IndexOriginal.Index_Number,
                    Index_Name_En = IndexOriginal.Index_Name_En,
                    Index_Name_He = IndexOriginal.Index_Name_He,
                    The_purpose_of_the_test = IndexOriginal.The_purpose_of_the_test,
                    Index_Explanation_below_norm = IndexOriginal.Index_Explanation_below_norm,
                    Index_Max_Number = IndexOriginal.Index_Max_Number,
                    Index_Min_Number = IndexOriginal.Index_Min_Number,
                    IndexStatus = "Below"
                };
                NewIndex = BelowIndex;
            }

            if (UserReport.Value > IndexOriginal.Index_Max_Number)
            {
                Index AboveIndex = new Index()
                {
                    index_Value = UserReport.Value,
                    Index_Number = IndexOriginal.Index_Number,
                    Index_Name_En = IndexOriginal.Index_Name_En,
                    Index_Name_He = IndexOriginal.Index_Name_He,
                    The_purpose_of_the_test = IndexOriginal.The_purpose_of_the_test,
                    Index_Explanation_above_norm = IndexOriginal.Index_Explanation_above_norm,
                    Index_Max_Number = IndexOriginal.Index_Max_Number,
                    Index_Min_Number = IndexOriginal.Index_Min_Number,
                    IndexStatus = "Above"
                };
                NewIndex = AboveIndex;
            }

            if (UserReport.Value < IndexOriginal.Index_Max_Number && UserReport.Value > IndexOriginal.Index_Min_Number || IndexOriginal.Index_Min_Number == null && IndexOriginal.Index_Max_Number == null)
            {
                Index AboveIndex = new Index()
                {
                    index_Value = UserReport.Value,
                    Index_Number = IndexOriginal.Index_Number,
                    Index_Name_En = IndexOriginal.Index_Name_En,
                    Index_Name_He = IndexOriginal.Index_Name_He,
                    The_purpose_of_the_test = IndexOriginal.The_purpose_of_the_test,
                    Index_Max_Number = IndexOriginal.Index_Max_Number,
                    Index_Min_Number = IndexOriginal.Index_Min_Number,
                    IndexStatus = "Fine"
                };
                NewIndex = AboveIndex;
            }

            return NewIndex;
        }
    }

    public class Registered_UserUserMetaData 
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "מייל לא יכול להיות ריק")]
        public string Email;
        [Required(AllowEmptyStrings = false, ErrorMessage = "סיסמא לא יכולה להיות ריקה")]
        public string Password;
        [Required(AllowEmptyStrings = false, ErrorMessage = "שם לא יכול להיות ריק")]
        public string Full_Name;

    }
}
