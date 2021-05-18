using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.WebControls; 

public class ClassStartDateEndDate : System.Web.UI.Page
{
    public void Today(TextBox textBoxStartDate, TextBox textBoxEndDate)
    {
        DateTime StartDate;
        DateTime EndDate;
        StartDate = DateTime.Now.AddDays(0);
        EndDate = DateTime.Now.AddDays(0);
        textBoxStartDate.Text = StartDate.ToString("yyyy-MM-dd");
        textBoxEndDate.Text = EndDate.ToString("yyyy-MM-dd");
    }
    public void Yesterday(TextBox textBoxStartDate, TextBox textBoxEndDate)
    {
        DateTime StartDate;
        DateTime EndDate;
        StartDate = DateTime.Now.AddDays(-1);
        EndDate = DateTime.Now.AddDays(-1);
        textBoxStartDate.Text = StartDate.ToString("yyyy-MM-dd");
        textBoxEndDate.Text = EndDate.ToString("yyyy-MM-dd");
    }
    public void ThisWeek(TextBox textBoxStartDate, TextBox textBoxEndDate)
    {
        DateTime StartDate;
        DateTime EndDate;
        StartDate = DateTime.Now.AddDays(-7);
        EndDate = DateTime.Now.AddDays(0);
        textBoxStartDate.Text = StartDate.ToString("yyyy-MM-dd");
        textBoxEndDate.Text = EndDate.ToString("yyyy-MM-dd");
    }
    public void LastWeek(TextBox textBoxStartDate, TextBox textBoxEndDate)
    {
        DateTime StartDate;
        DateTime EndDate;
        StartDate = DateTime.Now.AddDays(-14);
        EndDate = DateTime.Now.AddDays(-7);
        textBoxStartDate.Text = StartDate.ToString("yyyy-MM-dd");
        textBoxEndDate.Text = EndDate.ToString("yyyy-MM-dd");
    }
    public void ThisMonth(TextBox textBoxStartDate, TextBox textBoxEndDate)
    {      
        string S_month = "";
        string S_Day = "";
        S_month = Convert .ToString( DateTime.Now.Month);
        S_Day = Convert.ToString(DateTime.Now.Day);
        if (S_month.Length < 2)
        {
            string s = "0";
            S_month = s + S_month;
        }
        if (S_Day.Length < 2)
        {
            string s = "0";
            S_Day = s + S_Day;
        }
        textBoxStartDate.Text = DateTime.Now.Year + "-" + S_month + "-01";
        textBoxEndDate.Text = DateTime.Now.Year + "-" + S_month + "-" + System.DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
    }
    public void LastMonth(TextBox textBoxStartDate, TextBox textBoxEndDate)
    {
        int year;
        string month = "";
        string day = "";
        year = DateTime.Now.Year;
        month = Convert.ToString(DateTime.Now.Month);
        if (month.Length < 2)
            month = "0" + month;
        if (day.Length < 2)
            day = "0" + day;
        if (month == "02")
            month = "01";
        else if (month == "03")
            month = "02";
        else if (month == "04")
            month = "03";
        else if (month == "05")
            month = "04";
        else if (month == "06")
            month = "05";
        else if (month == "07")
            month = "06";
        else if (month == "08")
            month = "07";
        else if (month == "09")
            month = "08";
        else if (month == "10")
            month = "09";
        else if (month == "11")
            month = "10";
        else if (month == "12")
            month = "11";
        else if (month == "01")
        {
            month = "12";
            year -= 1;
        }
        switch (month)
        {
            case "01":
            case "03":
            case "05":
            case "07":
            case "08":
            case "10":
            case "12":
                {
                    day = "31";
                    break;
                }
            case "04":
            case "06":
            case "09":
            case "11":
                {
                    day = "30";
                    break;
                }

            case "02":
                {
                    day = Convert.ToString(DateTime.DaysInMonth(year, 2));
                    break;
                }
        }
        textBoxStartDate.Text = year + "-" + month + "-01";
        textBoxEndDate.Text = year + "-" + month + "-" + day;
    }
    public void ThisYear(TextBox textBoxStartDate, TextBox textBoxEndDate)
    {
        int Year = DateTime.Now.Year;
        textBoxStartDate.Text = Year + "-01-01";
        textBoxEndDate.Text = Year + "-12-31";
    }
    public void LastYear(TextBox textBoxStartDate, TextBox textBoxEndDate)
    {
        string Year = DateTime.Now.AddYears(-1).ToString("yyyy");
        textBoxStartDate.Text = Year + "-01-01";
        textBoxEndDate.Text = Year + "-12-31";
    }
    public void WriteSelectedDate(string StartDate, string EndDate, string title)
    {
        try 
        {
            var objENcript = new ClassEncryptDecrypt();
            HttpContext.Current.Response.Cookies["gpk"].Expires = DateTime.Now.AddYears(-1); // clear old cookies
            HttpContext.Current.Response.Cookies["gpk"]["SD"] = objENcript.EncryptCookies(StartDate);
            HttpContext.Current.Response.Cookies["gpk"]["ED"] = objENcript.EncryptCookies(EndDate);
            HttpContext.Current.Response.Cookies["gpk"]["Dz"] = objENcript.EncryptCookies(title);
            HttpContext.Current.Response.Cookies["gpk"].Expires = DateTime.Now.AddYears(1000);
        }
        catch (Exception)
        {
        }         
    }
    public string ReadSelectedDate(TextBox textBoxStartDate, TextBox textBoxEndDate)
    {
        string title = "";
        try
        {
            var objENcript = new ClassEncryptDecrypt();
            string StartDate = HttpContext.Current.Request.Cookies["gpk"]["SD"].ToString();
            string EndDate = HttpContext.Current.Request.Cookies["gpk"]["ED"].ToString();
            string Dayz = HttpContext.Current.Request.Cookies["gpk"]["Dz"].ToString();
            string SD = objENcript.DecryptCookies(StartDate);
            string ED = objENcript.DecryptCookies(EndDate);
            title = objENcript.DecryptCookies(Dayz);
            textBoxStartDate.Text = SD;
            textBoxEndDate.Text = ED;
        }
        catch (Exception)
        {
        }
        return title;
    }
}
