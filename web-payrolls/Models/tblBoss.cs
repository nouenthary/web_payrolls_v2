//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace web_payrolls.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblBoss
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblBoss()
        {
            this.tblProduction_Measur = new HashSet<tblProduction_Measur>();
            this.tblCompanies = new HashSet<tblCompany>();
            this.tblType_Of_Inventory = new HashSet<tblType_Of_Inventory>();
        }
    
        public int PK_Boss_Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Code { get; set; }
        public string Address { get; set; }
        public string Picture { get; set; }
        public string Status { get; set; }
        public Nullable<int> User_Update { get; set; }
        public string Date_Update { get; set; }
        public string Time_Update { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblProduction_Measur> tblProduction_Measur { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblCompany> tblCompanies { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblType_Of_Inventory> tblType_Of_Inventory { get; set; }
    }
}
