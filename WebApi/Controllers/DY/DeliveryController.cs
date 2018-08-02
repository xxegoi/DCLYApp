using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Common;
using WebApi.Models.DYModels;

namespace WebApi.Controllers.DY
{
    [Description("交期跟踪")]
    public class DeliveryController : DYApiController
    {
        [HttpPost]
        [Description("查询")]
        public object DeliveryTrack(JObject obj)
        {
            var start = obj.GetValue("start") == null ? string.Empty : obj.GetValue("start").ToString();
            var end = obj.GetValue("end") == null ? string.Empty : obj.GetValue("end").ToString();
            var fgh = obj.GetValue("fgh") == null ? string.Empty : obj.GetValue("fgh").ToString();
            var org = obj.GetValue("org") == null ? string.Empty : obj.GetValue("org").ToString();

            var page = obj.GetValue("page") == null ? 1 : Convert.ToInt32(obj.GetValue("page").ToString());
            var size = obj.GetValue("size") == null ? 20 : Convert.ToInt32(obj.GetValue("size").ToString());
            var over = obj.GetValue("over") == null ? false : true;

            var query = from r in db.v_DeliveryTrack
                        select r;

            if (start != string.Empty)
            {
                var startDate = Convert.ToDateTime(start);
                query = query.Where(p => p.FDate >= startDate);
            }

            if (end != string.Empty)
            {
                var endDate = Convert.ToDateTime(end);
                query = query.Where(p => p.FDate <= endDate);
            }

            if (fgh != string.Empty)
            {
                query = query.Where(p => p.FGH == fgh);
            }

            if (org != string.Empty)
            {
                query = query.Where(p => p.OrgName.Contains(org));
            }

            if (over)
            {
                var now = DateTime.Now;

                query = from a in query
                        where a.DXOutDays>0||a.RCOutDays>0||a.RSOutDays>0||a.YDOutDays>0
                        select a;
            }
            var total = query.Count();
            query = query.OrderByDescending(p => p.FDate).Skip((page - 1) * size).Take(size);

            //List<DeliveryViewModel> dataList = new List<DeliveryViewModel>();

            //var t = typeof(DeliveryViewModel);
            //var epis = typeof(v_DeliveryTrack).GetProperties();

            //data.ForEach(p =>
            //{
            //    var item= DeliveryEntityToViewModel(p);
            //    dataList.Add(item);
            //});

            //dataList = dataList.Where(p => p.DXOutDays >= 1 || p.RCOutDays >= 1 || p.RSOutDays >= 1 || p.YDOutDays >= 1).ToList();

            return new SuccessResult() { Data = new { total = total, dataList = query.ToList() } };

        }

        private DeliveryViewModel DeliveryEntityToViewModel(v_DeliveryTrack entity)
        {
            var t = typeof(DeliveryViewModel);
            var epis = typeof(v_DeliveryTrack).GetProperties();

            DeliveryViewModel item = new DeliveryViewModel();
            foreach (var pi in epis)
            {
                var value = pi.GetValue(entity);
                var vpi = t.GetProperty(pi.Name);
                vpi.SetValue(item, value);
            }

            return item;
        }
    }
}
