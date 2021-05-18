namespace web_payrolls.Models.DTO
{
    public class ProductModel
    {
        public int PK_Boss_Id { get; set; }
        public string Name { get; set; }
        public long PK_ProType_Id { get; set; }
        public string Pro_Type { get; set; }
        public string ProType_Name { get; set; }
        public long PK_Pro_Id { get; set; }
        public string Pro_Name { get; set; }
        public string Picture_Path { get; set; }
        public string User_Update { get; set; }
        public string Date_Update { get; set; }
        public string Time_Update { get; set; }
        public string Descr { get; set; }
    }
}