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
    
    public partial class tblProduction_Stock_In_Product_Cut_No
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblProduction_Stock_In_Product_Cut_No()
        {
            this.tblProduction_PK_Stock_In_Product_Cut = new HashSet<tblProduction_PK_Stock_In_Product_Cut>();
        }
    
        public long PK_Stock_In_Product_Cut_No_Id { get; set; }
        public Nullable<int> Stock_In_Product_Cut_No { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblProduction_PK_Stock_In_Product_Cut> tblProduction_PK_Stock_In_Product_Cut { get; set; }
    }
}