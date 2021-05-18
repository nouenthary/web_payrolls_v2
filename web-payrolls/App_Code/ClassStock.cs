using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
public class ClassStock
{
    ClassSQL objSQL = new ClassSQL();
    public string GetUserLogin_PK_U_ID()
    {
        string PK_U_ID_UserLoginID = "";
        try
        { 
            if (HttpContext.Current.Request.Cookies["SIS"] == null)
            {
                ClassGlobalVar.writeSystemType("Stock");
                HttpContext.Current.Response.Redirect("~/StockSystem/Login.aspx");
            }           
            var objENcript = new ClassEncryptDecrypt();
            string LoginCode = HttpContext.Current.Request.Cookies["opLCx"]["kdBkLi"].ToString();
            LoginCode = objENcript.DecryptCookies(LoginCode);
            PK_U_ID_UserLoginID = objSQL.GetCulumnData("SELECT PK_U_ID FROM tblUser WHERE LoginCode='" + LoginCode + "'");
            if (LoginCode == "" || PK_U_ID_UserLoginID == "")
            { // Logout System
                if (HttpContext.Current.Request.Cookies["opLCx"] != null)
                {
                    HttpContext.Current.Response.Cookies["opLCx"].Expires = DateTime.Now.AddYears(-1); // clear old cookies
                }
                HttpContext.Current.Response.Redirect("~/StockSystem/Login.html");
            }
        } 
        catch (Exception)
        {
            HttpContext.Current.Response.Redirect("~/StockSystem/Login.html");
        }
        return PK_U_ID_UserLoginID; 
    }

    //private string GetLocation_PK_ID(string columnName, string PK_U_ID)
    //{
    //    string sqlQuery = "SELECT " + columnName + " FROM tblProvince P INNER JOIN ";
    //    sqlQuery += "tblDistrict D ON P.PK_Pro_ID = D.FK_Pro_ID INNER JOIN ";
    //    sqlQuery += "tblCommune C ON D.PK_Dis_ID = C.FK_Dis_ID INNER JOIN ";
    //    sqlQuery += "tblStockLocation SL ON C.PK_Com_ID = SL.FK_Com_ID INNER JOIN ";
    //    sqlQuery += "tblUser U ON SL.PK_Sto_Loc_ID = U.FK_Sto_Loc_ID ";
    //    sqlQuery += "WHERE U.PK_U_ID=" + PK_U_ID;

    //    string ColData = objSQL.GetCulumnData(sqlQuery);
    //    return ColData;
    //}
    ThavrithClassUserLogin objUserLogin = new ThavrithClassUserLogin();

    private string GetCompany_PK_ID(string columnName, string PK_U_ID)
    {
        string sqlQuery = "SELECT " + columnName + " FROM tblCompany P INNER JOIN ";
        sqlQuery += "tblDepartment D ON P.PK_Comp_Id = D.FK_Comp_Id INNER JOIN ";
        sqlQuery += "tblPosition C ON D.PK_Dept_Id = C.Fk_Dept_Id INNER JOIN ";
       
        sqlQuery += "tblUser U ON C.PK_Pos_Id = U.FK_Pos_Id ";
        sqlQuery += "WHERE U.PK_U_Id=" + PK_U_ID;

        string ColData = objSQL.GetCulumnData(sqlQuery);
        return ColData;
    }

    //public void CheckPermissionLocation(DropDownList cboProvince, DropDownList cboDistrict, DropDownList cboCommune, DropDownList cboStockLocation)
    //{
    //    string sqlQuery;
    //    string PK_U_ID = GetUserLogin_PK_U_ID();
    //    Check Loctaion Of User Login to System

    //    Bind Province
    //   sqlQuery = "SELECT PK_Pro_ID,Pro_Name FROM tblProvince";
    //    objSQL.Bind_ID_Name_ToDropDownList(sqlQuery, cboProvince);
    //    cboProvince.SelectedValue = GetLocation_PK_ID("PK_Pro_ID", PK_U_ID);
    //    Bind District
    //    sqlQuery = "SELECT PK_Dis_ID,Dis_Name FROM tblDistrict WHERE FK_Pro_ID=" + cboProvince.SelectedValue;
    //    objSQL.Bind_ID_Name_ToDropDownList(sqlQuery, cboDistrict);
    //    cboDistrict.SelectedValue = GetLocation_PK_ID("PK_Dis_ID", PK_U_ID);
    //    Bind Commune
    //    sqlQuery = "SELECT PK_Com_ID,Com_Name FROM tblCommune WHERE FK_Dis_ID=" + cboDistrict.SelectedValue;
    //    objSQL.Bind_ID_Name_ToDropDownList(sqlQuery, cboCommune);
    //    cboCommune.SelectedValue = GetLocation_PK_ID("PK_Com_ID", PK_U_ID);
    //    Bind Stock Location
    //    sqlQuery = "SELECT PK_Sto_Loc_ID,Loc_Name FROM tblStockLocation WHERE FK_Com_ID=" + cboCommune.SelectedValue;
    //    objSQL.Bind_ID_Name_ToDropDownList(sqlQuery, cboStockLocation);
    //    sqlQuery = "SELECT [FK_Sto_Loc_ID] FROM [tblUser] WHERE [PK_U_ID]=" + PK_U_ID;
    //    cboStockLocation.SelectedValue = objSQL.GetCulumnData(sqlQuery);


    //    Enable or Disable Control
    //     Check Province
    //    sqlQuery = "SELECT ViewProvince FROM tblPermission WHERE FK_U_ID=" + PK_U_ID;
    //    string VIEW_ALL_PROVINCE = objSQL.GetCulumnData(sqlQuery);
    //    if (VIEW_ALL_PROVINCE != "YES")
    //    {
    //        cboProvince.Enabled = false;
    //    }
    //    Check District
    //    sqlQuery = "SELECT ViewDistrict FROM tblPermission WHERE FK_U_ID=" + PK_U_ID;
    //    string VIEW_ALL_DISTRICT = objSQL.GetCulumnData(sqlQuery);
    //    if (VIEW_ALL_DISTRICT != "YES")
    //    {
    //        cboDistrict.Enabled = false;
    //    }
    //    Check Commune
    //    sqlQuery = "SELECT ViewCommune FROM tblPermission WHERE FK_U_ID=" + PK_U_ID;
    //    string VIEW_ALL_COMMUNE = objSQL.GetCulumnData(sqlQuery);
    //    if (VIEW_ALL_COMMUNE != "YES")
    //    {
    //        cboCommune.Enabled = false;
    //    }
    //    Check View Stock Location
    //    sqlQuery = "SELECT ViewStockLocation FROM tblPermission WHERE FK_U_ID=" + PK_U_ID;
    //    string ViewStockLocation = objSQL.GetCulumnData(sqlQuery);
    //    if (ViewStockLocation != "YES")
    //    {
    //        cboStockLocation.Enabled = false;
    //    }

    //    Check Allow All
    //   sqlQuery = "SELECT ALLOW_ALL FROM tblPermission WHERE FK_U_ID=" + PK_U_ID;
    //    string ALLOW_ALL = objSQL.GetCulumnData(sqlQuery);
    //    if (ALLOW_ALL == "YES")
    //    {
    //        cboProvince.Enabled = true;
    //        cboDistrict.Enabled = true;
    //        cboCommune.Enabled = true;
    //        cboStockLocation.Enabled = true;
    //    }
    //}




    public void CheckPermissionCompany(DropDownList ddlsearchcompany , DropDownList ddlsearchdepartment, DropDownList ddlsearchposition)
    {
        string sqlQuery;
        string PK_U_ID = Convert.ToString (objUserLogin.GetUserLogin_PK_U_ID());
        // Check Loctaion Of User Login to System

        // Bind Province
        if (ddlsearchcompany != null)
        {
            sqlQuery = "SELECT PK_Comp_Id,Comp_Name FROM tblCompany";
            objSQL.Bind_ID_Name_ToDropDownList(sqlQuery, ddlsearchcompany);
            ddlsearchcompany.SelectedValue = GetCompany_PK_ID("PK_Comp_Id", PK_U_ID);
        }
        
        ////Bind District
        if (ddlsearchdepartment != null)
        {
            sqlQuery = "SELECT PK_Dept_Id,Dept_Name FROM tblDepartment WHERE FK_Comp_Id=" + ddlsearchcompany.SelectedValue;
            objSQL.Bind_ID_Name_ToDropDownList(sqlQuery, ddlsearchdepartment);
            ddlsearchdepartment.SelectedValue = GetCompany_PK_ID("PK_Dept_Id", PK_U_ID);
        }
        
        ////Bind Commune
        if (ddlsearchposition != null)
        {
            sqlQuery = "SELECT PK_Pos_Id,Pos_Name FROM tblPosition WHERE Fk_Dept_Id=" + ddlsearchdepartment.SelectedValue;
            objSQL.Bind_ID_Name_ToDropDownList(sqlQuery, ddlsearchposition);
            ddlsearchposition.SelectedValue = GetCompany_PK_ID("PK_Pos_Id", PK_U_ID);
        }
       
        ////Bind Stock Location
        //sqlQuery = "SELECT PK_Sto_Loc_ID,Loc_Name FROM tblStockLocation WHERE FK_Com_ID=" + cboCommune.SelectedValue;
        //objSQL.Bind_ID_Name_ToDropDownList(sqlQuery, cboStockLocation);
        //sqlQuery = "SELECT [FK_Sto_Loc_ID] FROM [tblUser] WHERE [PK_U_ID]=" + PK_U_ID;
        //cboStockLocation.SelectedValue = objSQL.GetCulumnData(sqlQuery);


        // Enable or Disable Control
        // Check Province
        //sqlQuery = "SELECT ViewAllStock FROM tblPermission WHERE FK_U_Id=" + PK_U_ID;
        //string VIEW_ALL_PROVINCE = objSQL.GetCulumnData(sqlQuery);
        //if (VIEW_ALL_PROVINCE != "True")
        //{
            //ddlsearchcompany.Enabled = false; // user is code we cannot get value of dropdownlist
        //}
        // Check District
        //sqlQuery = "SELECT ViewDistrict FROM tblPermission WHERE FK_U_ID=" + PK_U_ID;
        //string VIEW_ALL_DISTRICT = objSQL.GetCulumnData(sqlQuery);
        //if (VIEW_ALL_DISTRICT != "YES")
        //{
        //    cboDistrict.Enabled = false;
        //}
        //// Check Commune
        //sqlQuery = "SELECT ViewCommune FROM tblPermission WHERE FK_U_ID=" + PK_U_ID;
        //string VIEW_ALL_COMMUNE = objSQL.GetCulumnData(sqlQuery);
        //if (VIEW_ALL_COMMUNE != "YES")
        //{
        //    cboCommune.Enabled = false;
        //}
        //// Check View Stock Location
        //sqlQuery = "SELECT ViewStockLocation FROM tblPermission WHERE FK_U_ID=" + PK_U_ID;
        //string ViewStockLocation = objSQL.GetCulumnData(sqlQuery);
        //if (ViewStockLocation != "YES")
        //{
        //    cboStockLocation.Enabled = false;
        //}

        // Check Allow All
        //sqlQuery = "SELECT Admin FROM tblPermission WHERE PK_U_Id=" + PK_U_ID;
        //string Admin = objSQL.GetCulumnData(sqlQuery);
        //sqlQuery = "SELECT Director FROM tblPermission WHERE PK_U_Id=" + PK_U_ID;
        //string Director = objSQL.GetCulumnData(sqlQuery);

        //var sqlQueryShowAll = "SELECT ShowAll FROM tblPermission WHERE FK_U_Id=" + PK_U_ID;
        //string ShowAll = objSQL.GetCulumnData(sqlQueryShowAll);
        //var sqlQueryAllowAll = "SELECT AllowAll FROM tblPermission WHERE FK_U_Id=" + PK_U_ID;
        //string AllowAll = objSQL.GetCulumnData(sqlQueryAllowAll);
        
        //if (Admin == "True" | Director == "True")
        //{
        //    ddlsearchcompany.Enabled = true;
        //    //cboDistrict.Enabled = true;
        //    //cboCommune.Enabled = true;
        //    //cboStockLocation.Enabled = true;
        //}

        //if (ShowAll == "True" | AllowAll == "True")
        //{
            //ddlsearchcompany.Enabled = true;
        //}
        //else
        //{
            //ddlsearchcompany.Enabled = false;
        //}
    }

}