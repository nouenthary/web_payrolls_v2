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
    
    public partial class GetAllExchange_Result
    {
        public int PK_Exchang_Id { get; set; }
        public Nullable<int> FK_Com_Id { get; set; }
        public string Name { get; set; }
        public Nullable<double> Rate { get; set; }
        public string User_Update { get; set; }
        public string Time_Update { get; set; }
        public string Comp_Name { get; set; }
    }
}