using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using web_payrolls.Helpers;

namespace web_payrolls.Models.Entity
{
    public static class Hod
    {
        private static readonly DB_Connection Connection = new DB_Connection();

        private static readonly ContextProvider Provider = new ContextProvider(new ClHelper(), new DB_Connection());

        private static IEnumerable<tblBoss> GetHod()
        {
            return Connection.tblBosses.Where(boss => boss.Status == "Enable").ToList();
        }

        private static int GetCurrentHod()
        {
            return Provider.ManagerId;
        }

        public static IEnumerable<SelectListItem> GetListHod()
        {
            return GetHod().Select(
                item => new SelectListItem()
                {
                    Value = item.PK_Boss_Id + "",
                    Text = item.Name, 
                    Selected = GetCurrentHod() == item.PK_Boss_Id
                }).ToList();
        }
    }
}