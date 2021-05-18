using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using web_payrolls.Models;

namespace web_payrolls.Controllers
{
  public class CheckIn
  {
    public string Id { get; set; }
    public string Date { get; set; }
    public string Time { get; set; }
  }

  public class CheckInController : Controller
  {
    private readonly DB_Connection _connection = new DB_Connection();

    private SqlConnection cons;

    private void connection()
    {
      string constring = ConfigurationManager.ConnectionStrings["DB_Connection_Raw"].ToString();
      cons = new SqlConnection(constring);
    }

    // GET: CheckIn
    public ActionResult Index()
    {
      var path = _connection.tblConfigs.FirstOrDefault(x => x.key == "FINGER").value;

      var sql =
        "SELECT CHECKINOUT.USERID, Format (CHECKINOUT.CHECKTIME, 'yyyy-mm-dd') as DATES,Format (CHECKINOUT.CHECKTIME, 'hh:mm:ss') as TIMES FROM CHECKINOUT";

      var con = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path);
      var cmd = new OleDbCommand(sql, con);
      var olda = new OleDbDataAdapter(cmd);
      var dt = new DataTable();
      olda.Fill(dt);


      var data = new List<CheckIn>();

      foreach (DataRow row in dt.Rows)
      {
        data.Add(new CheckIn()
        {
          Id = row["USERID"].ToString(),
          Date = row["DATES"].ToString(),
          Time = row["TIMES"].ToString()
        });

        var useId = row["USERID"].ToString();

        //_connection.Database.ExecuteSqlCommand("EXEC AddCheckIn N'{0}' , N'{1}' , N'{2}', N'{3}' ", "234234", row["DATES"].ToString(), row["TIMES"].ToString(), "Morning");
        //_connection.SaveChanges();

        connection();

        SqlCommand cmds = new SqlCommand("AddCheckIn", cons);
        cmds.CommandType = CommandType.StoredProcedure;

        cmds.Parameters.AddWithValue("@SID", row["USERID"].ToString());
        cmds.Parameters.AddWithValue("@DATE", row["DATES"].ToString());
        cmds.Parameters.AddWithValue("@TIME", row["TIMES"].ToString());
        cmds.Parameters.AddWithValue("@TYPE", "M");

        cons.Open();
        int i = cmds.ExecuteNonQuery();
        cons.Close();
      }

      return View(data);
    }


    public ActionResult Path()
    {
      //  var d = Environment.GetFolderPath(Environment.MachineName);

      //var p = ControllerContext.HttpContext.Server.MapPath("~/C:Windows/iis.log");

      var d = "";


      return Json(d, JsonRequestBehavior.AllowGet);
    }
  }
}
