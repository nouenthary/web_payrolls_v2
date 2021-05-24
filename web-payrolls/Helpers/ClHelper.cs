using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using web_payrolls.Models;

namespace web_payrolls.Helpers
{
    public class ClHelper
    {
        private DB_Connection db = new DB_Connection();
        // Password used to Encrypt And Decrypt Password
        private string GetEnPWD()
        {
            string pwd = "SK016690047_Group016676451_ProJect_KBN_Payroll_SKJHGYSAb_Date_Start_2020_12_29_PWDTHaVriThcannotBeChanged";
            return pwd;
        }
        private string GetCookiesPWD()
        {
            string CookiesPwd = "SK016690047_Group016676451_ProJect_KBN_Payroll_SKJHGYSAb_Date_Start_2020_12_29_PWDTHaVriThcannotBeChanged";
            return CookiesPwd;
        }
        public IEnumerable<object> GetCompanyByBossId(int boss_id)
        {
            var companies = db
                .tblCompanies
                .Where(C => C.FK_Boss_Id == boss_id)
                .Select(C => new { C.PK_Comp_Id, C.Comp_Name })
                .ToList();

            return companies;
        }
        public IEnumerable<object> GetLocationByCompanyId(int comp_id)
        {
            var locations = db
                .tblLocations
                .Where(L => L.FK_Comp_Id == comp_id)
                .Select(L => new { L.PK_Location_Id, L.Loc_Name })
                .ToList();

            return locations;
        }
        public IEnumerable<object> GetDepartmentByLocationId(int location_id)
        {
            var departments = db
                .tblDepartments
                .Where(D => D.FK_Loc_Id == location_id)
                .Select(D => new { D.PK_Depart_Id, D.Deppart_Name })
                .ToList();

            return departments;
        }
        public IEnumerable<object> GetPositionByDeptId(int dept_id)
        {
            var positions = db
                .tblPositions
                .Where(P => P.FK_Depart_Id == dept_id)
                .Select(P => new { P.PK_Pos_Id, P.Pos_Name })
                .ToList();

            return positions;
        }

        public string EncryptPassword(string TextToEncrypt)
        {
            if (string.IsNullOrEmpty(TextToEncrypt)) return null;
            string key = GetEnPWD(); // Password 
            byte[] keyArray;
            byte[] toEncryptArray = Encoding.UTF8.GetBytes(TextToEncrypt);
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

        // block login
        public int ReadLoginCount()
        {
            // Allowed Login 5 times only
            int LoginLeft = 5;
            try
            {
                if (HttpContext.Current.Request.Cookies["lxcpp"] != null)
                {
                    string login = HttpContext.Current.Request.Cookies["lxcpp"]["xiZ"].ToString();
                    string decLogin = DecryptCookies(login);
                    LoginLeft = Convert.ToInt32(decLogin);
                    return LoginLeft;
                }
            }
            catch (Exception)
            {
            }
            return LoginLeft;
        }
        public void WriteLoginCount(int LoginCount)
        {
            try
            {
                HttpContext.Current.Response.Cookies["lxcpp"].Expires = DateTime.Now.AddYears(-1); // clear old cookies
                HttpContext.Current.Response.Cookies["lxcpp"]["xiZ"] = EncryptCookies(LoginCount.ToString());
                HttpContext.Current.Response.Cookies["lxcpp"].Expires = DateTime.Now.AddHours(1); // Blocked Login 1 hour
            }
            catch (Exception)
            {
            }
        }
        public int GetUserLoginId()
        {
            int PK_U_ID_UserLoginID = 0;
            string LoginCode = "";
            if (HttpContext.Current.Request.Cookies["opLCx"] == null)
            {
                HttpContext.Current.Response.Redirect("~/Account/Login");
            }
            else
            {
              LoginCode = HttpContext.Current.Request.Cookies["opLCx"]["kdBkLi"].ToString();
              if (LoginCode != null)
              {
                bool isTrue = db.tblStaffs.Any(x => x.LoginCodeGeneration == LoginCode);
                if (isTrue)
                {
                  var userId = db.tblStaffs.Where(x => x.LoginCodeGeneration == LoginCode).Select(x => x.PK_Staff_Id).FirstOrDefault();
                  PK_U_ID_UserLoginID = Convert.ToInt32(userId);
                }
                else
                {
                  var userId = DecryptCookies(LoginCode);
                  userId = userId.Remove(userId.Length - 8); // remove the last 2 string

                  PK_U_ID_UserLoginID = int.Parse(userId);
                }
              }
            }

            return PK_U_ID_UserLoginID;
        }

        // end
    }
}
