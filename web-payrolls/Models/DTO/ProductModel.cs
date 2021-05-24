namespace web_payrolls.Models.DTO
{
  public class ProductModel : CompanyModel
  {
    public string ProductTypeId { get; set; }
    public string ProductType { get; set; }
    public string TypeName { get; set; }

    public string ProductId { get; set; }
    public string Product { get; set; }

    public string GradeId { get; set; }
    public string Grade { get; set; }
    public string Color { get; set; }
    public string Size { get; set; }
    public string Barcode { get; set; }
    public string QrCode { get; set; }
    public string Measure { get; set; }
    public string Code { get; set; }

    public string DoubleWhole { get; set; }
    public string Whole { get; set; }
    public string UnitPrice { get; set; }
    public string Cost { get; set; }
    public string Normal { get; set; }
    public string Picture { get; set; }
    public string Discount { get; set; }
    public string StartDate { get; set; }
    public string EndDate { get; set; }
    public string DiscountStatus { get; set; }
    public string Status { get; set; }
    public string UserUpdate { get; set; }
    public string DateUpdate { get; set; }
    public string TimeUpdate { get; set; }
  }
}
