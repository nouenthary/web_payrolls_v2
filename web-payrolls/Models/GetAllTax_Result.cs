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
    
    public partial class GetAllTax_Result
    {
        public int PK_Comp_Id { get; set; }
        public string Comp_Name { get; set; }
        public int PK_Tax_Id { get; set; }
        public Nullable<int> FK_Comp_Id { get; set; }
        public Nullable<double> Tax_Percent { get; set; }
        public Nullable<double> Start_Rank_Salary { get; set; }
        public Nullable<double> End_Rank_Salary { get; set; }
        public Nullable<double> Decrease_Tax { get; set; }
        public Nullable<double> Decrease_Tax_From_Fammily { get; set; }
        public string User_Update { get; set; }
        public Nullable<int> Decrease_Tax_Foreign { get; set; }
        public string Date_Update { get; set; }
        public string Time_Update { get; set; }
    }
}
