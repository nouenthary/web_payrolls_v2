using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using web_payrolls.Models;
namespace web_payrolls.Controllers.Api
{
    public class RosterController : ApiController
    {
        DB_Connection db = new DB_Connection();
     
        [HttpGet]
        [Route("api/leave/{sid}/{year}")]
        public IHttpActionResult GetSalaryType(int sid, int year)
        {
            var salaryType = db.GetCanLeave(sid).FirstOrDefault();
            var roster = db.GetSumLeave(sid, year).FirstOrDefault();
                       
            int off = Convert.ToInt32(salaryType.ST_OFF) - (int)roster.R_OFF;
            int ph = Convert.ToInt32(salaryType.ST_PH) - (int)roster.R_PH;
            int sic = Convert.ToInt32(salaryType.ST_SIC) - (int)roster.R_SIC;
            int al = Convert.ToInt32(salaryType.ST_AL) - (int)roster.R_AL;
            int cm = Convert.ToInt32(salaryType.ST_SM) - (int)roster.R_CM;
            int upl = Convert.ToInt32(salaryType.ST_UPL) - (int)roster.R_UPL;
            int ab = Convert.ToInt32(salaryType.ST_AB) - (int)roster.R_AB;

            var rest = new Dictionary<string, object>();
            rest.Add("REST_OFF", off);
            rest.Add("REST_PH", ph);
            rest.Add("REST_SIC", sic);
            rest.Add("REST_AL", al);
            rest.Add("REST_CM", cm);
            rest.Add("REST_UPL", upl);
            rest.Add("REST_AB", ab);

            var col = new Dictionary<string, object>();
            col.Add("all", salaryType);
            col.Add("use", roster);
            col.Add("rest", rest);

            return Json(col);
        }



    }
}
