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
    
    public partial class Registered_User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Registered_User()
        {
            this.Health_Questionnaire = new HashSet<Health_Questionnaire>();
            this.User_Report = new HashSet<User_Report>();
        }
    
        public int User_Id { get; set; }
        public string Full_Name { get; set; }
        public string Email { get; set; }
        public Nullable<System.DateTime> Birthday { get; set; }
        public string Password { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Health_Questionnaire> Health_Questionnaire { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<User_Report> User_Report { get; set; }
    }
}
