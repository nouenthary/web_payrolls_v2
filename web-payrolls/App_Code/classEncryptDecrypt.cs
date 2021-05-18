using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Web;

public class ClassEncryptDecrypt
{ 
    // Password used to Encrypt And Decrypt Password
    private string GetEnPWD()
    {
        string pwd = "Sokleap016690047_VannSavong016676451_SaaNg_ProJect_KBN_Payroll_SKJHGYSAb_Date_Start_2018_10_23_PWDcannotBeChanged";
        return pwd;
    }
    public string EncryptPassword(string TextToEncrypt)
    {
        string key = GetEnPWD(); // Password 
        byte[] keyArray;
        byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(TextToEncrypt);
        MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
        byte[] resultArray;
        try
        {
            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tdes.CreateEncryptor();
            resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return (Convert.ToBase64String(resultArray, 0, resultArray.Length));
        }
        catch (Exception)
        {
            // Get Error if Cannot Encrypt, So We return Nothing
            return "";
        }
    }
    public string DecryptPassword(string TextToDecrypt)
    {
        string key = GetEnPWD(); // Password 
        byte[] keyArray;
        byte[] toEncryptArray;
        MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
        byte[] resultArray;
        try
        {
            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tdes.CreateDecryptor();
            toEncryptArray = Convert.FromBase64String(TextToDecrypt);
            resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return UTF8Encoding.UTF8.GetString(resultArray);
        }
        catch (Exception)
        {
            // Get Error if Cannot Decrypt, So We return Nothing
            return "";
        }
    }
    private string GetCookiesPWD()
    {
        string CookiesPwd = "Sokleap016690047_VannSavong016676451_SaaNg_ProJect_KBN_Payroll_StockDB_SKJHG_IUSDFHSIF3647364723J5745465_YSAb_Date_Start_2018_12_04_PWDcannotBeChanged";
        return CookiesPwd;
    }
    public string EncryptCookies(string TextToEncrypt)
    {
        string key = GetCookiesPWD(); // Password 
        byte[] keyArray;
        byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(TextToEncrypt);
        MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
        byte[] resultArray;
        try
        {
            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tdes.CreateEncryptor();
            resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return (Convert.ToBase64String(resultArray, 0, resultArray.Length));
        }
        catch (Exception)
        {
            // Get Error if Cannot Encrypt, So We return Nothing
            return "";
        }
    }
    public string DecryptCookies(string TextToDecrypt)
    {
        string key = GetCookiesPWD(); // Password 
        byte[] keyArray;
        byte[] toEncryptArray;
        MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
        byte[] resultArray;
        try
        {
            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tdes.CreateDecryptor();
            toEncryptArray = Convert.FromBase64String(TextToDecrypt);
            resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return UTF8Encoding.UTF8.GetString(resultArray);
        }
        catch (Exception)
        {
            // Get Error if Cannot Decrypt, So We return Nothing, Or User try to edit Cookies
            HttpContext.Current.Response.Redirect("~/Error404/AccessDenied.html");         
            return "";
        }
    }
}