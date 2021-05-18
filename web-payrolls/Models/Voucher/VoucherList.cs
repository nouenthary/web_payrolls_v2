namespace web_payrolls.Models.Voucher
{
    public class VoucherList
    {
        public int StaffId { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string PhotoVoucher { get; set; }
        public string Desc { get; set; }
        public int VoucherTypeId { get; set; }
    }
}