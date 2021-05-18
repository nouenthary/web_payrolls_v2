using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using web_payrolls.Helpers;

namespace web_payrolls.Models.Entity
{
    public static class Company
    {
        private static readonly DB_Connection Connection = new DB_Connection();

        private static readonly ContextProvider Provider = new ContextProvider(new ClHelper(), new DB_Connection());

        private static IEnumerable<tblCompany> GetCompanies()
        {
            return Connection.tblCompanies.ToList();
        }
        
        public static IEnumerable<SelectListItem> GetListHod()
        {
            return GetCompanies()
                .Where(company => company.FK_Boss_Id == Provider.ManagerId)
                .Select(item => new SelectListItem()
                {
                    Value = item.PK_Comp_Id + "",
                    Text = item.Comp_Name, 
                    Selected = Provider.CompanyId == item.PK_Comp_Id
                }).ToList();
        }
    }
}