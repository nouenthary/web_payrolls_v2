using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

public class ClassSQL
{
    private readonly string conn = ClassConnectionString.GetConnectionString();
    // Call Store Procedure SELECT
    public GridView GetDataSource(string sql, GridView DGView)
    {
        try
        {
            SqlConnection con = new SqlConnection(conn);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = sql;
            cmd.Connection = con;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            DGView.DataSource = dt;
            DGView.DataBind();
            da.Dispose();
            con.Close();
            GC.Collect();
            return DGView;
        }
        catch (Exception ex)
        {
            ClassGlobalVar.WriteErrorToDisk(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
        }
        return null;
    }
    // Get DateTime from Server Machine
    public DateTime GetDateTimeFromSQLServer()
    {
        DateTime myDateTime = DateTime.Now;
        try
        {
            SqlConnection con = new SqlConnection(conn);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlDataAdapter da = new SqlDataAdapter("SELECT GETDATE() As 'CurrentTime'", con);
            DataTable dt = new DataTable();
            DataRow dr;
            da.Fill(dt);
            con.Close();
            da.Dispose();
            if (dt.Rows.Count >= 1)
            {
                dr = dt.Rows[0];
                myDateTime = Convert.ToDateTime(dr[0]);
                return myDateTime;
            }
            dt.Clear();
        }
        catch (Exception ex)
        {
            ClassGlobalVar.WriteErrorToDisk(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
        }
        return myDateTime;
    }
    // Used to Insert Update Delete, EXECUTE QUERY 
    public void ExecuteSQLQuery(string sqlQueryCmd)
    {
        try
        {
            SqlConnection con = new SqlConnection(conn);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand(sqlQueryCmd, con);
            cmd.ExecuteNonQuery();
            con.Close();
        }
        catch (Exception ex)
        {
            ClassGlobalVar.WriteErrorToDisk(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
        }
    }
    // Get Data Only 1 row and 1 column from DB
    public string GetCulumnData(string sqlQueryCmd)
    {
        string ColumnData;
        try
        {
            SqlConnection con = new SqlConnection(conn);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = sqlQueryCmd;
            cmd.Connection = con;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            da.Dispose();
            con.Close();
            DataRow dr;
            if (dt.Rows.Count >= 1)
            {
                dr = dt.Rows[0];
                ColumnData = dr[0].ToString();
                return ColumnData;
            }
        }
        catch (Exception ex)
        {
            ClassGlobalVar.WriteErrorToDisk(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            return "";
        }
        return ""; // return when incurrect sql syntax
    }
    // Check Exist Record in Database
    public bool CheckExistRecord(string TableName, string FieldName, string dataToSearchInLowerCase)
    {
        string sql = "SELECT " + FieldName + " FROM " + TableName + " WHERE LOWER(" + FieldName + ")=N'" + ClassGlobalVar.ReplaceTextForSQL(dataToSearchInLowerCase, true).ToLower() + "' collate SQL_Latin1_General_CP850_BIN";
        string exist = GetCulumnData(sql);
        if (exist != "")
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    // Get DayInMonth
    public int GetDayInMonthFromSQL()
    {
        int DaysInMonth = 0;
        try
        {
            string sql = "DECLARE @ADate DATETIME SET @ADate = GETDATE() SELECT DAY(EOMONTH(@ADate)) AS DaysInMonth";
            DaysInMonth = Convert.ToInt32(GetCulumnData(sql));
            return DaysInMonth;
        }
        catch (Exception ex)
        {
            ClassGlobalVar.WriteErrorToDisk(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
        }
        return DaysInMonth;
    }
    // Bind PK_ID and Name to DropDownList
    public void Bind_ID_Name_ToDropDownList(string sqlQuery, DropDownList myDropDownList)
    {
        try
        {
            SqlConnection con = new SqlConnection(conn);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = sqlQuery;
            cmd.Connection = con;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            da.Dispose();
            con.Close();
            DataRow dr;
            myDropDownList.Items.Clear();
            myDropDownList.Items.Add("All");
            if (dt.Rows.Count >= 1)
            {
                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    dr = dt.Rows[i];
                    // Example sqlQuery SELECT FK_STA_ID,STA_NAME FROM tblStaff
                    myDropDownList.Items.Add(new ListItem(ClassGlobalVar.ReplaceTextForSQL(dr[1].ToString(), false), dr[0].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            ClassGlobalVar.WriteErrorToDisk(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
        }
    }
}