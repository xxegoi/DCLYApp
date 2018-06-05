﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Http;
using WebApi.Common;
using WebApi.Models.EMTModels;
using WebApi.OutputCache.V2;

namespace WebApi.Controllers.EMT
{
    public partial class YZKController : EMTApiController
    {
        [Description("运转卡跟踪")]
        public object YzkTrack(JObject condition)
        {
            var beginDate = condition.GetValue("begindate");
            var endDate = condition.GetValue("enddate");
            var page = condition.GetValue("page") == null ? 1 : int.Parse(condition.GetValue("page").ToString());
            var pagesize = condition.GetValue("pagesize") == null ? 20 : int.Parse(condition.GetValue("pagesize").ToString());
            var orgName = condition.GetValue("orgname");
            var fgh = condition.GetValue("fgh");

            var result = from yzk in db.v_DYJXC_YZKTrack_Head
                         select yzk;

            if (!string.IsNullOrEmpty( beginDate.ToString()))
            {
                var date = DateTime.Parse(beginDate.ToString());
                result = result.Where(p => p.FDate >= date);
            }
            if (!string.IsNullOrEmpty(endDate.ToString()))
            {
                var date = DateTime.Parse(endDate.ToString());
                result = result.Where(p => p.FDate <=date);
            }
            if (!string.IsNullOrEmpty(orgName.ToString()))
            {
                result = result.Where(p => p.OrgName.Contains(orgName.ToString()));
            }
            if (!string.IsNullOrEmpty(fgh.ToString()))
            {
                result = result.Where(p => p.FGH == fgh.ToString());
            }

            var total = result.Count();
            var yzkList = result.OrderByDescending(p => p.FDate).Skip((page - 1) * pagesize).Take(pagesize).AsParallel().ToList();
            List<YzkTrackViewModel> data = null;
            if (yzkList.Count > 0)
            {
                data = new List<YzkTrackViewModel>();
                
                yzkList.ForEach(p =>
                {
                    data.Add(new YzkTrackViewModel()
                    {
                        FGH = p.FGH,
                        FDate = p.FDate,
                        FDeliveryDate = p.FDeliveryDate,
                        FQty = p.FQty,
                        WorkFlow = GetTrackItems(p.FGH),
                        ItemName=p.FName,
                        OrgName=p.OrgName
                    });
                });
            }
            return Json(new SuccessResult() { Data=new { dataList=data,total=total } });
        }

        private List<YzkTrackItem> GetTrackItems(string fgh)
        {

            List<YzkTrackItem> items = new List<YzkTrackItem>();
            if (db.v_DYJXC_CustomYZKTrack.Count(p => p.FGH == fgh) == 0)
            {
                var result = db.v_DYJXC_YZKTrack.Where(p => p.FGH == fgh).AsParallel().ToList();

                YzkTrackItem item = null;

                result.ForEach(p =>
                {
                    if (item == null || item.FWorkProcedure != p.FWorkProcedure)
                    {
                        if (item != null) { items.Add(item); }

                        item = new YzkTrackItem() { FName = p.FName, FIndex = p.FIndex, FWorkProcedure = p.FWorkProcedure };
                    }

                    if (p.FOperType == "S")
                    {
                        item.SendTime = p.FRecDate.Value.Date;
                    }
                    else if (p.FOperType == "J")
                    {
                        item.JieTime = p.FRecDate.Value.Date;
                    }
                });
                if (item != null)
                {
                    items.Add(item);
                }
            }
            else
            {
                var result = db.v_DYJXC_CustomYZKTrack.Where(p => p.FGH == fgh).ToList();

                YzkTrackItem item = null;

                result.ForEach(p =>
                {
                    if (item == null || item.FWorkProcedure != p.FWorkProcedure)
                    {
                        if(item!=null)
                            items.Add(item);
                        item = new YzkTrackItem() { FName = p.FName, FIndex = p.FIndex, FWorkProcedure = p.FWorkProcedure };
                    }

                    if (p.FOperType == "S")
                    {
                        item.SendTime = p.FRecDate.Value;
                    }
                    else if (p.FOperType == "J")
                    {
                        item.JieTime = p.FRecDate.Value;
                    }
                });
            }
            return items.OrderBy(p=>p.FIndex).ToList();
        }
    }
}