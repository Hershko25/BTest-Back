//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BTestLibrary
{
    using System;
    using System.Collections.Generic;
    
    public partial class Index
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Index()
        {
            this.Food_Index = new HashSet<Food_Index>();
            this.Index_Number_Recommendation_Id = new HashSet<Index_Number_Recommendation_Id>();
            this.Report_Index = new HashSet<Report_Index>();
        }
    
        public int Index_Number { get; set; }
        public string Symbol { get; set; }
        public string Index_Name_En { get; set; }
        public string Index_Name_He { get; set; }
        public string The_purpose_of_the_test { get; set; }
        public string Index_Explanation_above_norm { get; set; }
        public string Index_Explanation_below_norm { get; set; }
        public Nullable<double> Index_Min_Number { get; set; }
        public Nullable<double> Index_Max_Number { get; set; }
        public Nullable<double> Index_Min_Number_F { get; set; }
        public Nullable<double> Index_Max_Number_F { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Food_Index> Food_Index { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Index_Number_Recommendation_Id> Index_Number_Recommendation_Id { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Report_Index> Report_Index { get; set; }
    }
}
