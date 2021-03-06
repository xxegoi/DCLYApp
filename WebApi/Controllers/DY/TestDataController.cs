﻿using Newtonsoft.Json.Linq;
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
    [Description(@"测试数据查询\导出")]
    public class TestDataController : DYApiController
    {
        /// <summary>
        /// 传入的 json 格式 {page:0,size:1,begindate:'2018-5-1',enddate:'2018-6-1'}
        /// </summary>
        //[Route("testdata/query")]
        [HttpPost]
        [Description("测试查询页")]
        public object Query(JObject obj)
        {
            int page = int.Parse(obj.GetValue("page").ToString());
            int size = int.Parse(obj.GetValue("size").ToString());
            var begindate = obj.GetValue("begindate").ToString();
            var enddate = obj.GetValue("enddate").ToString();

            //增加过滤条件，缸号
            var fgh = obj.GetValue("fgh").ToString();

            string sql = @"SELECT CONVERT(VARCHAR(10), REPLACE(REPLACE(REPLACE([fd],'年','-'),'月','-'),'日',' '),120) AS FinishDate ,
                        RTRIM([fd]) AS[fd] , RTRIM([lot]) AS[lot], RTRIM([kf]) AS[kf] , RTRIM([BN]) AS[BN] , RTRIM([CN]) AS[CN] , RTRIM([bur]) AS[bur] ,
                        RTRIM([tfl]) AS[tfl] , rtrim([tfw]) AS[tfw] , RTRIM([fn]) AS[fn] , RTRIM([l]) AS[l] , RTRIM([g]) AS[g] , RTRIM([pl]) AS[pl] ,
                        RTRIM([pg]) AS[pg] , RTRIM([sm]) AS[sm] , RTRIM([Tno]) AS Tno, RTRIM( [fl]) AS[fl], RTRIM([fw]) AS[fw] ,RTRIM( [n]) AS[n]
                        FROM t_DYJXC_TestData
                        WHERE 1=1 ";

            string totalSql = @"select count(*) from t_DYJXC_TestData WHERE 1=1 ";

            var dateCondition = @"AND CONVERT(VARCHAR(10), REPLACE(REPLACE(REPLACE([fd], '年', '-'), '月', '-'), '日', ' '),120) >= @begindate AND
                        CONVERT(VARCHAR(10), REPLACE(REPLACE(REPLACE([fd], '年', '-'), '月', '-'), '日', ' '),120) <= @enddate";

            SqlParameter begindateParam = new SqlParameter("@begindate", begindate);
            SqlParameter enddateParam = new SqlParameter("@enddate", enddate);

            //object[] parameters = new object[] {begindateParam,enddateParam };

            //用LIST更容易操作-----青
            List<SqlParameter> parameters = new List<SqlParameter>();
            List<SqlParameter> totalParmeters = new List<SqlParameter>();

            var condition = "";

            if (!string.IsNullOrEmpty(begindate) || !string.IsNullOrEmpty(enddate))
            {
                sql += dateCondition;
                totalSql += dateCondition;
                totalParmeters.Add(new SqlParameter("@begindate", begindate));
                totalParmeters.Add(new SqlParameter("@enddate", enddate));
                parameters.Add(begindateParam);
                parameters.Add(enddateParam);
            }

            //如果缸号不为空，则添加过滤条件----青
            if (!string.IsNullOrEmpty(fgh))
            {
                //如果日期值为空则通过缸号全表查询，否则查询日期区间内与缸号匹配的记------青
               
               condition =  " AND lot=@lot";

                sql += condition;

                totalSql += condition;

                parameters.Add(new SqlParameter("@lot", fgh));
                totalParmeters.Add(new SqlParameter("@lot", fgh));
            }

            var total = db.Database.SqlQuery<int>(totalSql,
                        totalParmeters.ToArray());

            //按照分页要求查询SQL的结果
            //查询条件参数LIST要ToArray转换为数组，不然报错---------青
            var data = db.Database.SqlQuery<TestQueryModel>(sql, parameters.ToArray()).OrderBy(p => p.fd).Skip((page - 1) * size).Take(size).ToList();

            return new SuccessResult() { Data = new { total = total, list = data } };
        }

        //[Route("test/query2")]
        //[HttpPost]
        //[Description("测试查询页")]
        //public object Query2(JObject obj)
        //{
        //    //int page = int.Parse(obj.GetValue("page").ToString());
        //    //int size = int.Parse(obj.GetValue("size").ToString());

        //    string begindate = obj.GetValue("begindate").ToString();
        //    string enddate = obj.GetValue("enddate").ToString();


        //    string sql = @"SELECT CONVERT(VARCHAR(10), REPLACE(REPLACE(REPLACE([fd],'年','-'),'月','-'),'日',' '),120) AS FinishDate ,
        //                RTRIM([fd]) AS[fd] , RTRIM([lot]) AS[lot], RTRIM([kf]) AS[kf] , RTRIM([BN]) AS[BN] , RTRIM([CN]) AS[CN] , RTRIM([bur]) AS[bur] ,
        //                RTRIM([tfl]) AS[tfl] , rtrim([tfw]) AS[tfw] , RTRIM([fn]) AS[fn] , RTRIM([l]) AS[l] , RTRIM([g]) AS[g] , RTRIM([pl]) AS[pl] ,
        //                RTRIM([pg]) AS[pg] , RTRIM([sm]) AS[sm] , RTRIM([Tno]) AS Tno, RTRIM( [fl]) AS[fl], RTRIM([fw]) AS[fw] ,RTRIM( [n]) AS[n]
        //                FROM t_DYJXC_TestData
        //                WHERE CONVERT(VARCHAR(10), REPLACE(REPLACE(REPLACE([fd], '年', '-'), '月', '-'), '日', ' '),120) >= @begindate AND
        //                CONVERT(VARCHAR(10), REPLACE(REPLACE(REPLACE([fd], '年', '-'), '月', '-'), '日', ' '),120) <= @enddate";

        //    SqlParameter begindateParam = new SqlParameter("@begindate", begindate);
        //    SqlParameter enddateParam = new SqlParameter("@enddate", enddate);
        //    object[] parameters = new object[] { begindateParam, enddateParam };

        //    //按照分页要求查询SQL的结果
        //    var data = db.Database.SqlQuery<TestQueryModel>(sql, parameters).OrderBy(p => p.fd).ToList();

        //    return new SuccessResult() { Data = new { total = data.Count, list = data } };
        //}

        /// <summary>
        /// 传入 的 json 格式 {begindate:'2018-5-1',enddate:'2018-6-1'}
        /// </summary>
        //[Route("testdata/export")]
        [HttpPost]
        [Description("测试页导出")]
        public object Export(JObject obj)
        {
            var begindate = obj.GetValue("begindate") == null ? DateTime.Now.Date : DateTime.Parse(obj.GetValue("begindate").ToString());
            var enddate = obj.GetValue("enddate") == null ? DateTime.Now.Date : DateTime.Parse(obj.GetValue("enddate").ToString());

            string sql = @"SELECT RTRIM([fd]) AS [完成日期] , RTRIM([lot]) AS [缸号], RTRIM([kf]) AS [客户] , RTRIM([BN]) AS [布种] , RTRIM([CN]) AS [颜色] , RTRIM([bur]) AS [顶破力26mm] ,
                            RTRIM([tfl]) AS [成品直缩] , rtrim([tfw]) AS [成品横缩] , RTRIM([fn]) AS [成品扭度] , RTRIM([l]) AS [成品幅宽] , RTRIM([g]) AS [成品克重] , RTRIM([pl]) AS [要求幅宽] ,
                            RTRIM([pg]) AS [要求克重] , RTRIM([sm]) AS [备注] , RTRIM([Tno]) AS [测试编码] ,RTRIM( [fl]) AS [要求直缩] , RTRIM([fw]) AS [要求横缩] ,RTRIM( [n]) AS [要求扭度]
                            FROM t_DYJXC_TestData 
                            WHERE CONVERT(VARCHAR(10), REPLACE(REPLACE(REPLACE([fd],'年','-'),'月','-'),'日',' '),120) >=@begindate AND 
                            CONVERT(VARCHAR(10), REPLACE(REPLACE(REPLACE([fd],'年','-'),'月','-'),'日',' '),120) <= @enddate";

            SqlParameter begindateParam = new SqlParameter("@begindate", begindate);
            SqlParameter enddateParam = new SqlParameter("@enddate", enddate);
            object[] parameters = new object[] { begindateParam, enddateParam };

            var data = db.Database.SqlQuery<TestExportModel>(sql, parameters).OrderBy(o => o.完成日期).ToList();

            DataTable dt = DataTableExtensions.ToDataTable<TestExportModel>(data);

            List<DataTable> dts = new List<DataTable>();

            dts.Add(dt);

            return new FileResult(NPOIHelper.ExportToByteArray(dts), string.Format("导出文件_{0}_{1}.xls", begindate, enddate));

            //string filePath = string.Format("{0}/{1}", HttpContext.Current.Server.MapPath("~/App_Data"),"Export.xls");

            //NPOIHelper.ExportToFile(dt, filePath);
        }

        //[Route("test/export2")]
        //[HttpGet]
        //[Description("导出测试用")]
        //public IHttpActionResult Export2()
        //{
        //    string sql = @"SELECT RTRIM([fd]) AS [完成日期] , RTRIM([lot]) AS [缸号], RTRIM([kf]) AS [客户] , RTRIM([BN]) AS [布种] , RTRIM([CN]) AS [颜色] , RTRIM([bur]) AS [顶破力26mm] ,
        //                    RTRIM([tfl]) AS [成品直缩] , rtrim([tfw]) AS [成品横缩] , RTRIM([fn]) AS [成品扭度] , RTRIM([l]) AS [成品幅宽] , RTRIM([g]) AS [成品克重] , RTRIM([pl]) AS [要求幅宽] ,
        //                    RTRIM([pg]) AS [要求克重] , RTRIM([sm]) AS [备注] , RTRIM([Tno]) AS [测试编码] ,RTRIM( [fl]) AS [要求直缩] , RTRIM([fw]) AS [要求横缩] ,RTRIM( [n]) AS [要求扭度]
        //                    FROM t_DYJXC_TestData 
        //                    WHERE CONVERT(VARCHAR(10), REPLACE(REPLACE(REPLACE([fd],'年','-'),'月','-'),'日',' '),120) >= '2017-05-01' AND 
        //                    CONVERT(VARCHAR(10), REPLACE(REPLACE(REPLACE([fd],'年','-'),'月','-'),'日',' '),120) <= '2018-09-11'";

        //    var data = db.Database.SqlQuery<TestExportModel>(sql).OrderBy(o => o.完成日期).ToList();

        //    //for(int i=0;i<4;i++)
        //    //{
        //    //    data.AddRange(data);
        //    //}

        //    DataTable dt = DataTableExtensions.ToDataTable<TestExportModel>(data);

        //    List<DataTable> dts = new List<DataTable>();

        //    dts.Add(dt);

        //    return new FileResult(NPOIHelper.ExportToByteArray(dts), "Export.xls", "application/octet-stream");

        //}


    }
}