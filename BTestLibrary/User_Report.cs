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
    
    public partial class User_Report
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User_Report()
        {
            this.Report_Index = new HashSet<Report_Index>();
        }
    
        public int Report_Id { get; set; }
        public int User_Id { get; set; }
    
        public virtual Registered_User Registered_User { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Report_Index> Report_Index { get; set; }
    }
}
