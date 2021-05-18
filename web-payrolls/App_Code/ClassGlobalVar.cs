using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Xml;
using System.Web;
using System.Web.UI.WebControls;

public static class ClassGlobalVar
{
    // Replace Text For SQL to Prevent Error by User Input Data
    public static string ReplaceTextForSQL(string Text, bool ToDatabase)
    {
        if (ToDatabase == true)
        {
            // Replace Text and insert to Database
            Text = Text.Replace("'", "&#39;").Replace("<", "&#60;").Replace(">", "&#62;").Replace(System.Environment.NewLine, "<br/>");
        }
        else
        {
            // get Text from Database and Replace
            Text = Text.Replace("&#39;", "'").Replace("&#60;", "<").Replace("&#62;", ">").Replace("<br/>", System.Environment.NewLine);
        }
        return Text;
    }
    public static bool IsNumeric(this string s)
    {
        float output;
        return float.TryParse(s, out output);
    }
    public static string CalculateHours(DateTime StartDateTime, DateTime EndDateTime, bool HourFormat)
    {
        // Format : yyyy-MM-dd HH:mm:ss
        TimeSpan TotalTime = EndDateTime.Subtract(StartDateTime);
        string Result;
        if (HourFormat == true)
        {
            double ResultHours = Convert.ToDouble(TotalTime.TotalHours.ToString("0.00"));
            Result = Convert.ToString(ResultHours);
            // True Return as 1.50
        }
        else
        {
            Result = Math.Floor(TotalTime.TotalHours) + " Hour : " + TotalTime.Minutes + " Minutes";
            // False Return as 1 Hour 30 Minutes
        }
        return Result;
    }
    public static double CalculateDays(DateTime StartDateTime, DateTime EndDateTime)
    {
        // Format : yyyy-MM-dd HH:mm:ss  
        TimeSpan TotalDaysWork = EndDateTime.Subtract(StartDateTime);
        double ResultDays = TotalDaysWork.TotalDays;
        return ResultDays;
    }
    public static string ConvertToDateFormat(int Year, int Month, int Day)
    {
        string myYear = Convert.ToString(Year);
        string myMonth = Convert.ToString(Month);
        string myDay = Convert.ToString(Day);
        if (myMonth.Length == 1)
        {
            myMonth = "0" + Month;
        }
        if (myDay.Length == 1)
        {
            myDay = "0" + Day;
        }
        // return as 2018-01-31
        return myYear + "-" + myMonth + "-" + myDay;
    }
    public static byte[] ConverTobyte(string imgFilePath)
    {
        try
        {
            FileStream fs = new FileStream(imgFilePath, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            int ln = Convert.ToInt32(br.BaseStream.Length);
            byte[] mybyte = new byte[ln + 1];
            mybyte = br.ReadBytes(ln);
            return mybyte;
        }
        catch (Exception)
        {
            return null;
        }
    }
    public static void writeSystemType(string SystemType)
    {
        // Write System Type
        try
        {
            var objENcript = new ClassEncryptDecrypt();
            HttpContext.Current.Response.Cookies["SIS"].Expires = DateTime.Now.AddYears(-1);
            HttpContext.Current.Response.Cookies["SIS"]["ST"] = objENcript.EncryptCookies(SystemType);
            HttpContext.Current.Response.Cookies["SIS"].Expires = DateTime.Now.AddYears(1000);
        }
        catch (Exception)
        {

        }
    }
    public static string ReadSystemType()
    {
        string SystemType = "";
        // Write System Type
        try
        {
            var objENcript = new ClassEncryptDecrypt();
            string myCookie = HttpContext.Current.Request.Cookies["SIS"]["ST"].ToString();
            SystemType = objENcript.DecryptCookies(myCookie);
        }
        catch (Exception)
        {
        }
        return SystemType;
    }
    // Check Control Empty in Form ............................................................
    public static bool CheckControlEmpty(params System.Web.UI.Control[] Controls)
    {
        foreach (System.Web.UI.Control C in Controls)
        {
            if (C is DropDownList)
            {
                if (((DropDownList)C).Text == "")
                {
                    // Control ComboBox ...........................................
                    //  MessageBox.Show("You must select dropdown list !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ((DropDownList)C).BackColor = Color.Yellow;
                    C.Focus();
                    return true;
                }
                else
                {
                    ((DropDownList)C).BackColor = Color.White;
                    return false;
                }
            }
            else
            {
                if (((TextBox)C).Text == "")
                {
                    // Control is text box ........................................
                    //  MessageBox.Show("The text field cannot be empty !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ((TextBox)C).BackColor = Color.Yellow;
                    C.Focus();
                    return true;
                }
                else
                {
                    ((TextBox)C).BackColor = Color.White;
                    return false;
                }
            }
        }
        return false;
    }
    // Clear Text to empty for TextBox and DropDownList
    public static void ClearTextControl(params System.Web.UI.Control[] Controls)
    {
        foreach (System.Web.UI.Control C in Controls)
        {
            try
            {
                if (C is DropDownList)
                {
                    ((DropDownList)C).SelectedIndex = 0;
                }
                else
                {
                    ((TextBox)C).Text = "";
                }
            }
            catch (Exception)
            {
            }
        }
    }
    // Get Random text like hexadecimal
    public static string RandomText()
    {
        string Text = "0123456789ABCDEF";
        Random r = new Random();
        StringBuilder sb = new StringBuilder();
        // is a length of sb
        for (int i = 1; i <= 99; i++)
        {
            // 16 is a length of Text
            int idx = r.Next(0, 16);
            sb.Append(Text.Substring(idx, 1));
        }
        // Return text like hexadecimal
        return sb.ToString();
    }
    public static string SelectedValueDropDownList(DropDownList cboDropDownList)
    {
        string PK_ID = "";
        if (cboDropDownList.SelectedIndex != 0)
        {
            PK_ID = cboDropDownList.SelectedValue;
        }
        else
        { // SelectedIndex = 0 , All
            PK_ID = "";
        }
        return PK_ID;
    }
    // Resize Quality of Photo to Low Quality
    public static string ResizePhoto(FileUpload sFileUpload, string SaveToFolder, int MaxWith, int MaxHeight)
    {
        try
        {
            ClassSQL objSQL = new ClassSQL();

            string myTemp = HttpContext.Current.Server.MapPath("~/TempPhoto");
            string mySave = HttpContext.Current.Server.MapPath(SaveToFolder);
            if (Directory.Exists(myTemp) == false)
            {
                Directory.CreateDirectory(myTemp);
            }
            if (Directory.Exists(mySave) == false)
            {
                Directory.CreateDirectory(mySave);
            }
            string rename = RandomText();
            string myToday = objSQL.GetDateTimeFromSQLServer().ToString("yyyy-MM-dd");
            string mytempPath = myTemp + "/" + myToday + "_" + rename + "_" + sFileUpload.FileName;
            string mySavePath = mySave + "/" + myToday + "_" + rename + ".jpg";
            sFileUpload.PostedFile.SaveAs(mytempPath);
            // Resize Image Size to Low
            Bitmap thumbimage;
            Bitmap originalimage;        
            int width = MaxWith; // # this is the max width of the new image         
            int height = MaxHeight; // # this is the max height of the new image
            int newwidth;
            int newheight;
            originalimage = (Bitmap)System.Drawing.Image.FromFile(mytempPath);
            double oriHeiht = originalimage.Height;
            double oriWidth = originalimage.Width;
            double total;
            if (originalimage.Width > originalimage.Height)
            {
                total = oriHeiht / oriWidth;
                newheight = Convert.ToInt32(total * height);
                newwidth = width;
            }
            else
            {
                total = oriWidth / oriHeiht;
                newwidth = Convert.ToInt32(total * width);
                newheight = height;
            }
            thumbimage = new Bitmap(newwidth, newheight);
            Graphics gr = Graphics.FromImage(thumbimage);
            gr.DrawImage(originalimage, 0, 0, newwidth, newheight);
            thumbimage.Save(mySavePath, System.Drawing.Imaging.ImageFormat.Jpeg);
            originalimage.Dispose();         
            Directory.Delete(myTemp, true);
            return SaveToFolder + "/" + myToday + "_" + rename + ".jpg";
        }
        catch (Exception)
        {
            return "";
        }
    }
    public static void DeleteFiles(string File_Path_From_Database)
    {
        // Use for ASP server Path
        string Del_Path = HttpContext.Current.Server.MapPath(File_Path_From_Database);
        try
        {
            if (File.Exists(Del_Path) == true)
            {
                File.Delete(Del_Path);
            }
        }
        catch (Exception) { }
    }
    // Write Error to Disk
    public static void WriteErrorToDisk(string ClassName, string ErrorSMS)
    {
        try
        {
            string ErrorFolder = @"C:\ErrorSMS\";
            if (Directory.Exists(ErrorFolder) == false)
            {
                Directory.CreateDirectory(ErrorFolder);
            }
            string SysType = ReadSystemType();
            string todayDate = DateTime.Now.ToString("yyyy-MM-dd HH.mm.ss");
            string WritePath = ErrorFolder + SysType + " " + todayDate + ".txt";

            StreamWriter sw = new StreamWriter(WritePath);
            sw.WriteLine("System Type : " + SysType);
            sw.WriteLine("Class Name  : " + ClassName);
            sw.WriteLine("Error Info  : " + ErrorSMS);
            sw.Close();
        }
        catch (Exception)
        {

        }
    }
    
    public static void WriteLoginCount(int LoginCount)
    {
        try
        {
            var objENcript = new ClassEncryptDecrypt();
            HttpContext.Current.Response.Cookies["lxcpp"].Expires = DateTime.Now.AddYears(-1); // clear old cookies
            HttpContext.Current.Response.Cookies["lxcpp"]["xiZ"] = objENcript.EncryptCookies(LoginCount.ToString());
            HttpContext.Current.Response.Cookies["lxcpp"].Expires = DateTime.Now.AddHours(1); // Blocked Login 1 hour
        }
        catch (Exception)
        {
        }
    }
    public static int ReadLoginCount()
    {
       // Allowed Login 5 times only
        int LoginLeft = 5;
        try
        {
            if (HttpContext.Current.Request.Cookies["lxcpp"] != null)
            {
                var objENcript = new ClassEncryptDecrypt();              
                string login = HttpContext.Current.Request.Cookies["lxcpp"]["xiZ"].ToString();
                string decLogin = objENcript.DecryptCookies(login);
                LoginLeft = Convert.ToInt32(decLogin);
                return LoginLeft;
            }
        }
        catch (Exception)
        {        
        }
        return LoginLeft;
    }
}
