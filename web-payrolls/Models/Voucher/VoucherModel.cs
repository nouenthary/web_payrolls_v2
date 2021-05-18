namespace web_payrolls.Models.Voucher
{
    public class VoucherModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Qty { get; set; }

        public double Discount { get; set; }
        public double Cost { get; set; }
        public double Amount { get; set; }
    }
}