using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Common;
using WebApi.Models.DYModels;

namespace WebApi.Controllers.DY
{
    public class YzkShapiController : DYApiController
    {
        [HttpPost]
        [Description("测试")]
        public object Query(JObject obj)
        {
            //int page = int.Parse(obj.GetValue("page").ToString());
            //int size = int.Parse(obj.GetValue("size").ToString());

            var fgh = obj.GetValue("fgh").ToString();

            string sql = @"SELECT distinct yzk.FGH,bcl.FMachine,bcl.FColor,shapi.FName as 'shapiname',zhichang.FName as 'zhichangname' FROM EMT.dbo.t_DY_YZK yzk  
                        LEFT JOIN EMT.dbo.t_DY_YZKEntry1 yzke ON yzke.FID = yzk.FID 
                        LEFT JOIN dbo.t_BarCodeLabelDetail bcld ON bcld.FXiMaNo = yzke.FXM 
                        LEFT JOIN dbo.t_BarCodeLabel bcl ON bcl.FRecID = bcld.FPID 
                        LEFT JOIN dbo.t_ShaPi shapi ON shapi.FID = bcl.FShaPiID 
                        LEFT JOIN dbo.t_ZhiChang zhichang ON zhichang.FID = bcl.FZhiChang
                        WHERE yzk.FGH = @gh ";

            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@gh", fgh));

            //var data = db.Database.SqlQuery<Test123Model>(sql, parameters.ToArray()).OrderBy(p => p.fgh).Skip((page - 1) * size).Take(size).ToList();
            var data = db.Database.SqlQuery<YzkShapiViewModel>(sql, parameters.ToArray()).OrderBy(p => p.fgh).ToList();

            return new SuccessResult() { Data = new { list = data } };
        }

    }
}
