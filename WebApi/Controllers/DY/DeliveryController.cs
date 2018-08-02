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
            var start = obj.GetValue("start") == null ? string.Empty : obj.GetValue("start").ToString();//开始时间
            var end = obj.GetValue("end") == null ? string.Empty : obj.GetValue("end").ToString();//结束时间
            var fgh = obj.GetValue("fgh") == null ? string.Empty : obj.GetValue("fgh").ToString();//缸号
            var org = obj.GetValue("org") == null ? string.Empty : obj.GetValue("org").ToString();//客户名

            var page = obj.GetValue("page") == null ? 1 : Convert.ToInt32(obj.GetValue("page").ToString());//当前页
            var size = obj.GetValue("size") == null ? 20 : Convert.ToInt32(obj.GetValue("size").ToString());//每页返回记录数
            var over = obj.GetValue("over") == null ? false : true;//是否只查超期记录

            var query = from r in db.v_DeliveryTrack  //select * from v_DeliveryTrack
                        select r;

            if (start != string.Empty)
            {
                var startDate = Convert.ToDateTime(start);
                query = query.Where(p => p.FDate >= startDate);//等同于加上  where FDate>=@startDate
            }

            if (end != string.Empty)
            {
                var endDate = Convert.ToDateTime(end);
                query = query.Where(p => p.FDate <= endDate);//等同于在上面SQL语句基础上再加上 AND FDate<=@endDate
            }

            if (fgh != string.Empty)
            {
                query = query.Where(p => p.FGH == fgh); //等同于在上面SQL语句基础上再加上 AND FGH=@FGH
            }

            if (org != string.Empty)
            {
                query = query.Where(p => p.OrgName.Contains(org));//等同于在上面SQL语句基础上再加上 AND OrgName Like @OrgName
            }

            if (over)
            {
                var now = DateTime.Now;

                query = from a in query
                        where a.DXOutDays>0||a.RCOutDays>0||a.RSOutDays>0||a.YDOutDays> 0//等同于在上面SQL语句基础上再加上 AND (DXOutDays>0 OR RCOutDays>0 OR RSOutDays>0 OR YDOutDays>0)
                        select a;
            }
            var total = query.Count();
            query = query.OrderByDescending(p => p.FDate).Skip((page - 1) * size).Take(size);//分页查询，先按日期倒序排序，然后跳过  (当前面-1)X每页返回条数，最后返回记录

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
