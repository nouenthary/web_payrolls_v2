using System;
using System.Web.UI.WebControls;
using web_payrolls.Models.DTO;

namespace web_payrolls.View.Transfer
{
  public partial class Transfer : System.Web.UI.Page
  {
    private ProductModel _model = new ProductModel();

    string sqlQuery = "";
    ClassSQL objSQL = new ClassSQL();
    ClassStock ObjStock = new ClassStock();
    ThavrithClassUserLogin objUserLogin = new ThavrithClassUserLogin();
    ThavrithClasses _classes = new ThavrithClasses();

    protected void Page_Load(object sender, EventArgs e)
    {
      Searching();
    }


    protected void Searching()
    {
      sqlQuery = string.Format("SELECT * FROM [dbo].[tblDepartment]");
     // DGView.DataSource = objSQL.GetDataTableFromDB(sqlQuery);
      objSQL.GetDataSource(sqlQuery, DGView);
    }

    protected void DGView_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {

    }

    protected void DGView_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
  }
}
