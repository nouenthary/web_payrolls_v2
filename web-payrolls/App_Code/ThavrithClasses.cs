using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ThavrithClasses
/// </summary>
public class ThavrithClasses
{

    private readonly string conn = ClassConnectionString.GetConnectionString();
    readonly ClassSQL _objSql = new ClassSQL();
    public ThavrithClasses()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public DataTable GetDataTable(string sqlQuery)
    {
        DataTable dt = new DataTable();

        try
        {
            SqlConnection con = new SqlConnection(conn);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = sqlQuery;
            cmd.Connection = con;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            
            da.Fill(dt);
        }
        catch (Exception ex)
        {
            ClassGlobalVar.WriteErrorToDisk(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
        }
        return dt;
    }

    public string ReturnDataFieldTableByPassingTablePrimaryKey(string TableName, string FieldNameToGetData, string fieldTablePrimaryKey, int id)
    {
        string stringReturn = string.Empty;

        string sql = "SELECT " + FieldNameToGetData + " FROM " + TableName + " WHERE " + fieldTablePrimaryKey + "=" + id;
        DataTable dt = GetDataTable(sql);
        DataRow dr;
        if (dt.Rows.Count >= 1)
        {
            dr = dt.Rows[0];
            stringReturn = dr[0].ToString();
            return stringReturn;
        }
        else
        {
            return stringReturn;
        }
    }
}