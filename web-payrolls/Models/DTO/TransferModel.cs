namespace web_payrolls.Models.DTO
{
  public class TransferModel : ProductConfirm
  {
    public string Id { get; set; }
    public string Qty { get; set; }
    public string FromCompany { get; set; }
    public string No { get; set; }
  }
}
