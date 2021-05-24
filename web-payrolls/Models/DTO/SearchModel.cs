namespace web_payrolls.Models.DTO
{
  public class SearchModel
  {
    public string BossId { get; set; }
    public string CompanyId { get; set; }
    public string LocationId { get; set; }
    public string ProductTypeId { get; set; }
    public string TypeName { get; set; }
    public string ProductId { get; set; }
    public string Color { get; set; }
    public string Size { get; set; }
    public string Grade { get; set; }
    public string Code { get; set; }
    public string Barcode { get; set; }
    public string QrCode { get; set; }
    public string Status { get; set; }
    public string No { get; set; }
    public string StartDate { get; set; }
    public string EndDate { get; set; }
    public int? Page { get; set; }
    public int? PageSize { get; set; }
  }
}
