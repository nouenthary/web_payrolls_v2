using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Services;

/// <summary>
/// Summary description for AutoCompleteData
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class AutoCompleteData : System.Web.Services.WebService
{

    public AutoCompleteData()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string HelloWorld()
    {
        return "Hello World";
    }

    [WebMethod]
    public string[] PopulateTelephone(string prefixText)
    {
        string mainConn = ConfigurationManager.ConnectionStrings["KNA_STOCK_2019ConnectionString"].ConnectionString;
        SqlConnection sqlConn = new SqlConnection(mainConn);
        sqlConn.Open();
        string query = "select Telephone from tblUser where Telephone like N'%" + prefixText + "%' COLLATE SQL_Latin1_General_CP850_BIN" +
            " ORDER BY Telephone  COLLATE SQL_Latin1_General_CP850_BIN";
        SqlCommand cmd = new SqlCommand(query, sqlConn);
        SqlDataAdapter sda = new SqlDataAdapter(cmd);
        DataTable data = new DataTable();
        sda.Fill(data);
        List<string> telephone = new List<string>();
        for (int i = 0; i < data.Rows.Count; i++)
        {
            telephone.Add(data.Rows[i]["Telephone"].ToString());
        }

        return telephone.ToArray();
    }

    [WebMethod]
    public string[] PopulateStaffName(string prefixText)
    {
        string mainConn = ConfigurationManager.ConnectionStrings["KNA_STOCK_2019ConnectionString"].ConnectionString;
        SqlConnection sqlConn = new SqlConnection(mainConn);
        sqlConn.Open();
        string query = "select Staff_Name from tblUser where UPPER(Staff_Name) like N'%" + prefixText.ToUpper() + "%' COLLATE SQL_Latin1_General_CP850_BIN" +
            " ORDER BY Staff_Name  COLLATE SQL_Latin1_General_CP850_BIN"; 
        SqlCommand cmd = new SqlCommand(query, sqlConn);
        SqlDataAdapter sda = new SqlDataAdapter(cmd);
        DataTable data = new DataTable();
        sda.Fill(data);
        List<string> staffName = new List<string>();
        for (int i = 0; i < data.Rows.Count; i++)
        {
            staffName.Add(data.Rows[i]["Staff_Name"].ToString());
        }

        return staffName.ToArray();
    }

    [WebMethod]
    public string[] PopulateStaffIdCard(string prefixText)
    {
        string mainConn = ConfigurationManager.ConnectionStrings["KNA_STOCK_2019ConnectionString"].ConnectionString;
        SqlConnection sqlConn = new SqlConnection(mainConn);
        sqlConn.Open();
        string query = "select Staff_Id_Card from tblUser where UPPER(Staff_Id_Card) like N'%" + prefixText.ToUpper() + "%' COLLATE SQL_Latin1_General_CP850_BIN" +
            " ORDER BY Staff_Id_Card  COLLATE SQL_Latin1_General_CP850_BIN";
        SqlCommand cmd = new SqlCommand(query, sqlConn);
        SqlDataAdapter sda = new SqlDataAdapter(cmd);
        DataTable data = new DataTable();
        sda.Fill(data);
        List<string> staffIdCard = new List<string>();
        for (int i = 0; i < data.Rows.Count; i++)
        {
            staffIdCard.Add(data.Rows[i]["Staff_Id_Card"].ToString());
        }

        return staffIdCard.ToArray();
    }

    [WebMethod]
    public string[] PopulateUsernameLogin(string prefixText)
    {
        string mainConn = ConfigurationManager.ConnectionStrings["KNA_STOCK_2019ConnectionString"].ConnectionString;
        SqlConnection sqlConn = new SqlConnection(mainConn);
        sqlConn.Open();
        string query = "select User_Name from tblUser where UPPER(User_Name) like N'%" + prefixText.ToUpper() + "%' COLLATE SQL_Latin1_General_CP850_BIN" +
            " ORDER BY User_Name  COLLATE SQL_Latin1_General_CP850_BIN";
        SqlCommand cmd = new SqlCommand(query, sqlConn);
        SqlDataAdapter sda = new SqlDataAdapter(cmd);
        DataTable data = new DataTable();
        sda.Fill(data);
        List<string> username = new List<string>();
        for (int i = 0; i < data.Rows.Count; i++)
        {
            username.Add(data.Rows[i]["User_Name"].ToString());
        }

        return username.ToArray();
    }

    [WebMethod]
    public string[] PopulateCustomerTelephone(string prefixText)
    {
        string mainConn = ConfigurationManager.ConnectionStrings["KNA_STOCK_2019ConnectionString"].ConnectionString;
        SqlConnection sqlConn = new SqlConnection(mainConn);
        sqlConn.Open();
        string query = "select Tell from tblCustomerInfo where Tell like N'%" + prefixText + "%' COLLATE SQL_Latin1_General_CP850_BIN" +
            " ORDER BY Tell  COLLATE SQL_Latin1_General_CP850_BIN";
        SqlCommand cmd = new SqlCommand(query, sqlConn);
        SqlDataAdapter sda = new SqlDataAdapter(cmd);
        DataTable data = new DataTable();
        sda.Fill(data);
        List<string> telephone = new List<string>();
        for (int i = 0; i < data.Rows.Count; i++)
        {
            telephone.Add(data.Rows[i]["Tell"].ToString());
        }

        return telephone.ToArray();
    }

    [WebMethod]
    public string[] PopulateProductGradeBarcode(string prefixText)
    {
        string mainConn = ConfigurationManager.ConnectionStrings["KNA_STOCK_2019ConnectionString"].ConnectionString;
        SqlConnection sqlConn = new SqlConnection(mainConn);
        sqlConn.Open();
        string query = "select distinct BarCode from tblGrade where BarCode like N'%" + prefixText + "%' COLLATE SQL_Latin1_General_CP850_BIN";
        SqlCommand cmd = new SqlCommand(query, sqlConn);
        SqlDataAdapter sda = new SqlDataAdapter(cmd);
        DataTable data = new DataTable();
        sda.Fill(data);
        List<string> barcode = new List<string>();
        for (int i = 0; i < data.Rows.Count; i++)
        {
            barcode.Add(data.Rows[i]["BarCode"].ToString());
        }

        return barcode.ToArray();
    }
}
