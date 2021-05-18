using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ThavrithClassUserLogin
/// </summary>
public class ThavrithClassUserLogin
{
    ClassSQL objSQL = new ClassSQL();
    ClassEncryptDecrypt objEN = new ClassEncryptDecrypt();

    public int GetUserLogin_PK_U_ID()
    {
        int PK_U_ID_UserLoginID = 0;
        try
        {
            if (HttpContext.Current.Request.Cookies["SIS"] == null)
            {
                HttpContext.Current.Response.Redirect("~/Login.aspx");
            }
            var objENcript = new ClassEncryptDecrypt();
            string LoginCode = HttpContext.Current.Request.Cookies["opLCx"]["kdBkLi"].ToString();
            
            if (LoginCode == "")
            { 
                // Logout System
                if (HttpContext.Current.Request.Cookies["opLCx"] != null)
                {
                    HttpContext.Current.Response.Cookies["opLCx"].Expires = DateTime.Now.AddYears(-1); // clear old cookies
                }
                HttpContext.Current.Response.Redirect("~/Login.html");
            }
            else
            {
                bool isTrue = objSQL.CheckExistRecord("tblUser", "GeneratedCodeLogin", LoginCode);
                if (isTrue)
                {
                    var userId = objSQL.GetCulumnData("SELECT PK_U_Id FROM tblUser WHERE GeneratedCodeLogin='" + LoginCode + "'");
                    PK_U_ID_UserLoginID = int.Parse(userId);
                }
                else
                {
                    var userId = objENcript.DecryptCookies(LoginCode);
                    userId = userId.Remove(userId.Length - 2); // remove the last 2 string

                    PK_U_ID_UserLoginID = int.Parse(userId);
                }
            }
        }
        catch (Exception)
        {
            HttpContext.Current.Response.Redirect("~/Login.html");
        }
        return PK_U_ID_UserLoginID;
    }
}