using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using web_payrolls.Models;
using System.IO;
using System.Web;

namespace web_payrolls.Controllers.Api
{
    public class OtherController : ApiController
    {
        private DB_Connection db = new DB_Connection();
       
        [HttpGet]
        [Route("api/position/{did}")]
        public IHttpActionResult GetPositionByDepartment(int? did)
        {
            var position = db.tblPositions.Where(p => p.FK_Depart_Id == did).ToList();
            return Json(position);
        }

        //[HttpGet]
        //[Route("api/bossManage/{bid}")]
        //public IHttpActionResult Get(int? bid)
        //{
        //    var location = db.tblLocations.Where(b=> b.FK_Boss_Id == bid).ToList();
        //    var company = db.tblCompanies.Where(b => b.FK_Boss_Id == bid).ToList();

        //    var data = new Dictionary<string, object>();
        //    data.Add("location", location);
        //    data.Add("company", company);
        //    return Json(data);
        //}
       
        [HttpGet]
        [Route("api/collection")]
        public IHttpActionResult Collection()
        {
            var boss = db.tblBosses.ToList();
            var locations = db.tblLocations.ToList();
            var companys = db.tblCompanies.ToList();
            var departments = db.tblDepartments.ToList();
            var positions = db.tblPositions.ToList();

            var collection = new Dictionary<string, object>();
            collection.Add("boss", boss);
            collection.Add("locations", locations);
            collection.Add("companys", companys);
            collection.Add("departments", departments);
            collection.Add("positions", positions);

            return Json(collection);
        }        
    }
}
