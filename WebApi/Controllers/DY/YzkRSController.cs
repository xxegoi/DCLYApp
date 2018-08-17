using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Common;
using WebApi.Models;
using WebApi.Models.DYModels;

namespace WebApi.Controllers.DY
{
    [Description("运转卡染损")]
    public class YzkRSController : DYApiController
    {
        [Description("查询")]
        public object GetList(JObject obj)
        {
            var page = obj.GetValue("page");
            var size = obj.GetValue("size");
            var fgh = obj.GetValue("fgh");
            var start = obj.GetValue("start");
            var end = obj.GetValue("end");

            var p = page == null ? 1 : Convert.ToInt32( page);
            var s = size == null ? 20 : Convert.ToInt32(size);

            var list = from r in db.v_YzkRS
                       select r;

            if (fgh != null)
            {
                var gh = fgh.ToString();
                list = list.Where(r => r.缸号 == gh);
            }

            if (start != null)
            {
                var date = Convert.ToDateTime(start.ToString());
                list = list.Where(r => r.开卡日期 >= date);
            }

            if (end != null)
            {
                var date = Convert.ToDateTime(end.ToString());
                list = list.Where(r => r.开卡日期 <= date);
            }

            var total = list.Count();

            list= list.OrderBy(r => r.开卡日期).Skip((1-p * s)).Take(s);

            return new SuccessResult() { Data = new { total, list = list.ToList() } };

        }

        public object Export(JObject obj)
        {
            var start = obj.GetValue("start");
            var end = obj.GetValue("end");

            if (start == null && end == null)
            {
                return new FaildResult("开始日期和结束日期不能为空");
            }

            var list = from r in db.v_YzkRS
                       select r;

            if (start != null)
            {
                var date = Convert.ToDateTime(start.ToString());
                list = list.Where(r => r.开卡日期 >= date);
            }

            if (end != null)
            {
                var date = Convert.ToDateTime(end.ToString());
                list = list.Where(r => r.开卡日期 <= date);
            }

            return new FileResult(NPOIHelper.ExportToByteArray <v_YzkRS>( list.ToList()),"导出数据");

        }
    }
}
