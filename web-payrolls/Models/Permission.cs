using System.Linq;
using web_payrolls.Helpers;

namespace web_payrolls.Models
{
    public class Permission
    {
        public tblPermission Permissions { get; set; }

        public Permission()
        {
            var provider = new ContextProvider(new ClHelper(),new DB_Connection());
            var db = new DB_Connection();
            Permissions = db.tblPermissions.Single(permission => permission.FK_Staff_Id == provider.StaffId);
        }
    }
}