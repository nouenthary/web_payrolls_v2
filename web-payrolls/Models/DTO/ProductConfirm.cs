namespace web_payrolls.Models.DTO
{
  public class ProductConfirm : ProductModel
  {
    public string InDate { get; set; }
    public string InTime { get; set; }
    public string UserConfirm { get; set; }
    public string DateConfirm { get; set; }
    public string TimeConfirm { get; set; }
    public string StatusConfirm { get; set; }
  }
}
