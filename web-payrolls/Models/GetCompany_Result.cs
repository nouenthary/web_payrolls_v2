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
    
    public partial class GetCompany_Result
    {
        public int PK_Comp_Id { get; set; }
        public Nullable<int> FK_Boss_Id { get; set; }
        public string Comp_Name { get; set; }
        public string Picture_Company { get; set; }
        public string Descr { get; set; }
        public Nullable<int> User_Update { get; set; }
        public string Date_Update { get; set; }
        public string Time_Update { get; set; }
        public Nullable<int> Number_Staff_Transaction { get; set; }
    }
}
