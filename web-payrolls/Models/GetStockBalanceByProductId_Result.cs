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
    
    public partial class GetStockBalanceByProductId_Result
    {
        public string Pro_Type { get; set; }
        public string ProType_Name { get; set; }
        public long PK_Pro_Id { get; set; }
        public string Pro_Name { get; set; }
        public long PK_Grade_Id { get; set; }
        public string Grade_Name { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public string Picture_Path { get; set; }
        public long PK_Stock_In_Product_Import_Bal_Id { get; set; }
        public Nullable<int> QTY_Balance { get; set; }
    }
}