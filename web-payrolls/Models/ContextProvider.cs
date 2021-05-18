using System;
using System.Linq;
using web_payrolls.Helpers;

namespace web_payrolls.Models
{
    public class ContextProvider
    { 
        public int ManagerId { get; set; }
        public int CompanyId { get; set; }
        public int LocationId { get; set; }
        public int DepartmentId { get; set; }
        public int PositionId { get; set; }
        public int StaffId { get; set; }
        
        public ContextProvider(ClHelper helper, DB_Connection connection)
        {
            var userId = helper.GetUserLoginId();
            var staffInfo = connection
                .GetStaffInfo(userId).Single();
            
            ManagerId = staffInfo.PK_Boss_Id;
            CompanyId = staffInfo.PK_Comp_Id;
            LocationId = staffInfo.PK_Location_Id;
            DepartmentId = staffInfo.PK_Depart_Id;
            PositionId = staffInfo.PK_Pos_Id;
            StaffId = Convert.ToInt32(staffInfo.PK_Staff_Id);
        }
    }
}