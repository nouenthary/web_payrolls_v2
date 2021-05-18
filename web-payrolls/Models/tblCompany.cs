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
    
    public partial class tblCompany
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblCompany()
        {
            this.tblExchanges = new HashSet<tblExchange>();
            this.tblLocations = new HashSet<tblLocation>();
            this.tblTaxes = new HashSet<tblTax>();
        }
    
        public int PK_Comp_Id { get; set; }
        public Nullable<int> FK_Boss_Id { get; set; }
        public string Comp_Name { get; set; }
        public string Picture_Company { get; set; }
        public string Descr { get; set; }
        public Nullable<int> User_Update { get; set; }
        public string Date_Update { get; set; }
        public string Time_Update { get; set; }
        public Nullable<int> Number_Staff_Transaction { get; set; }
    
        public virtual tblBoss tblBoss { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblExchange> tblExchanges { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblLocation> tblLocations { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblTax> tblTaxes { get; set; }
    }
}
