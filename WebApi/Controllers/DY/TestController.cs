using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebApi.Common;
using WebApi.Models.DYModels;

namespace WebApi.Controllers.DY
{
    public class TestController : DYApiController
    {
        /// <summary>
        /// json 格式 {page:0,size:1,begindate:'2018-5-1',enddate:'2018-6-1'}
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [Route("test/query")]
        [HttpPost]
        [Description("测试查询页")]
        public object Query(JObject obj)
        {
            int page = int.Parse(obj.GetValue("page").ToString());
            int size = int.Parse(obj.GetValue("size").ToString());
            string begindate = obj.GetValue("begindate").ToString();
            string enddate = obj.GetValue("enddate").ToString();

            string sql = @"SELECT CONVERT(VARCHAR(10), REPLACE(REPLACE(REPLACE([fd],'年','-'),'月','-'),'日',' '),120) AS FinishDate ,
                        RTRIM([fd]) AS[fd] , RTRIM([lot]) AS[lot], RTRIM([kf]) AS[kf] , RTRIM([BN]) AS[BN] , RTRIM([CN]) AS[CN] , RTRIM([bur]) AS[bur] ,
                        RTRIM([tfl]) AS[tfl] , rtrim([tfw]) AS[tfw] , RTRIM([fn]) AS[fn] , RTRIM([l]) AS[l] , RTRIM([g]) AS[g] , RTRIM([pl]) AS[pl] ,
                        RTRIM([pg]) AS[pg] , RTRIM([sm]) AS[sm] , RTRIM([Tno]) AS Tno, RTRIM( [fl]) AS[fl], RTRIM([fw]) AS[fw] ,RTRIM( [n]) AS[n]
                        FROM t_DYJXC_TestData
                        WHERE CONVERT(VARCHAR(10), REPLACE(REPLACE(REPLACE([fd], '年', '-'), '月', '-'), '日', ' '),120) >= @begindate AND
                        CONVERT(VARCHAR(10), REPLACE(REPLACE(REPLACE([fd], '年', '-'), '月', '-'), '日', ' '),120) <= @enddate";

            SqlParameter begindateParam = new SqlParameter("@begindate", begindate);
            SqlParameter enddateParam = new SqlParameter("@enddate", enddate);
            object[] parameters = new object[] { begindateParam, enddateParam };

            var data = db.Database.SqlQuery<TestQueryModel>(sql, parameters).OrderBy(p=>p.fd).Skip((page-1)*size).Take(size).ToList();

            return new SuccessResult() { Data = new { total= data.Count, list=data } };
        }

        /// <summary>
        /// json 格式 {begindate:'2018-5-1',enddate:'2018-6-1'}
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [Route("test/export")]
        [HttpPost]
        [Description("测试页导出")]
        public IHttpActionResult Export(JObject obj)
        {
            string begindate = obj.GetValue("begindate").ToString();
            string enddate = obj.GetValue("enddate").ToString();

            string sql = @"SELECT CONVERT(VARCHAR(10), REPLACE(REPLACE(REPLACE([fd],'年','-'),'月','-'),'日',' '),120) AS FinishDate ,
                        RTRIM([fd]) AS[fd] , RTRIM([lot]) AS[lot], RTRIM([kf]) AS[kf] , RTRIM([BN]) AS[BN] , RTRIM([CN]) AS[CN] , RTRIM([bur]) AS[bur] ,
                        RTRIM([tfl]) AS[tfl] , rtrim([tfw]) AS[tfw] , RTRIM([fn]) AS[fn] , RTRIM([l]) AS[l] , RTRIM([g]) AS[g] , RTRIM([pl]) AS[pl] ,
                        RTRIM([pg]) AS[pg] , RTRIM([sm]) AS[sm] , RTRIM([Tno]) AS Tno, RTRIM( [fl]) AS[fl], RTRIM([fw]) AS[fw] ,RTRIM( [n]) AS[n]
                        FROM t_DYJXC_TestData
                        WHERE CONVERT(VARCHAR(10), REPLACE(REPLACE(REPLACE([fd], '年', '-'), '月', '-'), '日', ' '),120) >= @begindate AND
                        CONVERT(VARCHAR(10), REPLACE(REPLACE(REPLACE([fd], '年', '-'), '月', '-'), '日', ' '),120) <= @enddate";

            SqlParameter begindateParam = new SqlParameter("@begindate", begindate);
            SqlParameter enddateParam = new SqlParameter("@enddate", enddate);
            object[] parameters = new object[] { begindateParam, enddateParam };

            var data = db.Database.SqlQuery<TestQueryModel>(sql, parameters).ToList();

            DataTable dt = DataTableExtensions.ToDataTable<TestQueryModel>(data);

            string filePath = string.Format("{0}/{1}", HttpContext.Current.Server.MapPath("~/App_Data"),"Export.xls");

            NPOIHelper.ExportToFile(dt, filePath);

            return new FileResult(filePath);
        }

    }
}