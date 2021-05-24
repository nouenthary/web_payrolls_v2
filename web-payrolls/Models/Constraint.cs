using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Antlr.Runtime.Misc;

namespace web_payrolls.Models
{
    public static class Constraint
    {
        public static readonly int PageSize = 20;
        public static readonly string Date = DateTime.Now.ToString("yyyy-MM-dd");
        public static readonly string Time = DateTime.Now.ToString("hh:mm:ss");
        public static readonly List<SelectListItem> PerPage = new List<SelectListItem>()
        {
            new SelectListItem() { Value="10", Text= "10" },
            new SelectListItem() { Value="20", Text= "20" },
            new SelectListItem() { Value="50", Text= "50" },
            new SelectListItem() { Value="100", Text= "100" },
            new SelectListItem() { Value="5000", Text= "ALL" },
        };

        public static readonly string[] DeductMoneyType = { "OFF", "PH", "AL", "SIC", "AB", "CM", "UPL" };

        public static string GetTime() {
            return DateTime.Now.ToString("hh:mm:ss");
        }

        public static string GetDate()
        {
            return DateTime.Now.ToString("yyyy-MM-dd");
        }

        public static string StatusColor(string status)
        {
          switch (status)
          {
            case "Pending":
              return "info";
            case "Done":
              return "success";
            default:
              return "danger";
          }
        }
    }
}
