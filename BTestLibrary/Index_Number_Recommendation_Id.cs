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
    
    public partial class Index_Number_Recommendation_Id
    {
        public int Recommendation_Id { get; set; }
        public int Index_Number { get; set; }
        public string AboveBelow { get; set; }
    
        public virtual Index Index { get; set; }
        public virtual Recommendation Recommendation { get; set; }
    }
}
