namespace web_payrolls.Models.Product
{
    public class ProductCut
    {
        public int StockInProductCutId { get; set; }
        public int CompanyId { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string ProductType { get; set; }
        public int GradeId { get; set; }
        public int Qty { get; set; }
        public double Price { get; set; }
        public double Total { get; set; }
        public string Desc { get; set; }
    }
}