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
    
    public partial class tblProduction_Repair_Closth
    {
        public long PK_Repair_Closth_Id { get; set; }
        public Nullable<long> FK_Grade_Id { get; set; }
        public Nullable<long> FK_From_Com_Id { get; set; }
        public Nullable<long> FK_To_Loc_Id { get; set; }
        public string Repair_Closth_No { get; set; }
        public Nullable<int> QTY { get; set; }
        public string Repaire_Type { get; set; }
        public Nullable<double> Price { get; set; }
        public Nullable<double> Total { get; set; }
        public string Type_Repair { get; set; }
        public Nullable<int> U_Update { get; set; }
        public string Date_Update { get; set; }
        public string Time_Updte { get; set; }
        public string Status { get; set; }
        public Nullable<int> Confirm_User { get; set; }
        public string Confirm_Date { get; set; }
        public string Confirm_Time { get; set; }
    }
}
