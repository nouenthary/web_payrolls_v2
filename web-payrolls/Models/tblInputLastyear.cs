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
    
    public partial class tblInputLastyear
    {
        public long PK_LastY_Money_Id { get; set; }
        public Nullable<long> FK_Staff_Id { get; set; }
        public Nullable<int> Num_Year { get; set; }
        public Nullable<int> Num_month { get; set; }
        public Nullable<double> Salary { get; set; }
        public Nullable<double> Multiply { get; set; }
        public Nullable<double> Input_LastY_Money { get; set; }
        public Nullable<double> Total_LastYear { get; set; }
        public Nullable<int> User_Update { get; set; }
        public string Date_Update { get; set; }
        public string Time_Update { get; set; }
    
        public virtual tblStaff tblStaff { get; set; }
    }
}
