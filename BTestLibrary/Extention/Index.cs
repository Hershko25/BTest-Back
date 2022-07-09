namespace BTestLibrary
{
    using System;
    using System.Collections.Generic;

    public partial class Index
    {
        public double index_Value { get; set; }
        public string IndexStatus { get; set; }
        public string Recommend { get; set; }
        public string Foods { get; set; }
        public override string ToString()
        {
            string s = $"Index_Number : {this.Index_Number}\nIndex_Value : {this.index_Value}\nRecommend : {this.Recommend}\nFoods : {this.Foods}\nIndex Status : {this.IndexStatus}\nIndex_Name_En : {this.Index_Name_En}\nIndex_Name_He : {Index_Name_He}\nThe_purpose_of_the_test : {The_purpose_of_the_test}\n" +
                $"Index_Explanation_above_norm:{this.Index_Explanation_above_norm}\nIndex_Explanation_below_norm : {Index_Explanation_below_norm}";
            return s;
         }

    }




}
