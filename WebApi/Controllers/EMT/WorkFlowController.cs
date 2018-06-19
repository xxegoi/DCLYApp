using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Common;
using WebApi.Models.DYModels;
using Newtonsoft.Json.Linq;
using WebApi.Models.EMTModels;
using System.ComponentModel;

namespace WebApi.Controllers.EMT
{
    [Description("工艺流程管理")]
    public class WorkFlowController : EMTApiController
    {
        DYContainer dydb = new DYContainer();

        [Description("查询")]
        public object GetWorkFlow([FromUri]string fgh)
        {
            if (fgh != null)
            {
                var wps = (from wp in db.t_DY_WorkProcedure
                           select new { WPID = wp.FID, wp.FName }).ToList();

                var gxhb = (from g in dydb.t_DYJXC_GXHB
                            where g.FGH == fgh
                            group g by new { g.FProcedureID, g.FOperType, g.FIndex } into g1
                            select new { WPID = g1.Key.FProcedureID, FType = g1.Key.FOperType, FNum = g1.Sum(p => p.FNum), FIndex = g1.Key.FIndex }).ToList();

                //查找自定义工艺路线
                var result = (from g in db.CustomWorkFlowSet
                              join entry in db.CustomWorkFlowEntrySet on g.Id equals entry.PFID into g1
                              from a in g1.DefaultIfEmpty()
                              join wp in db.t_DY_WorkProcedure on a.FWorkProcedure equals wp.FID into g2
                              from b in g2.DefaultIfEmpty()
                              where g.FGH == fgh && a.IsDeleted == false
                              select new
                              {
                                  FID = a.Id,
                                  WPID = b.FID,
                                  b.FName,
                                  FIndex = a.Index
                              }).OrderBy(p => p.FIndex).ToList();

                //如果自定义工艺路线不存在，则返回标准工艺路线
                if (result.Count == 0)
                {
                    var result1 = (from yzk in db.t_DY_YZK
                                   join gylc in db.t_DY_GYLC on yzk.FWorkFlow equals gylc.FBillNo into g1
                                   from a in g1.DefaultIfEmpty()
                                   join gylce in db.t_DY_GYLCEntry on a.FID equals gylce.FID into g2
                                   from b in g2.DefaultIfEmpty()
                                   join wp in db.t_DY_WorkProcedure on b.FWorkProcedure equals wp.FID into g3
                                   from c in g3.DefaultIfEmpty()
                                   where yzk.FGH == fgh
                                   select new
                                   {
                                       FID = b.FEntryID,
                                       WPID = c.FID,
                                       c.FName,
                                       b.FIndex
                                   }).OrderBy(p => p.FIndex).ToList();
                    return Json(new SuccessResult() { Data = new { GH = fgh, WorkFlow = result1, GXHB = gxhb, wp = wps } });
                }
                //返回自定义工艺路线数据
                return Json(new SuccessResult() { Data = new { GH = fgh, WorkFlow = result, GXHB = gxhb, wp = wps } });
            }

            return new FaildResult("");
        }

        
        [Description("查询2")]
        [HttpPost]
        public object GetWorkFlow(JObject obj)
        {
            var fgh = obj.GetValue("fgh");
            if (fgh != null && !string.IsNullOrEmpty(fgh.ToString()))
            {
                var wps = (from wp in db.t_DY_WorkProcedure
                           select new { WPID = wp.FID, wp.FName }).ToList();

                var gh = fgh.ToString();
                var activeIndex = 0;

                if (db.v_YzkWorkFlowCustomer_GXHB.Count(p => p.FGH == gh) > 0)
                {
                    var result = db.v_YzkWorkFlowCustomer_GXHB.Where(p => p.FGH == gh).OrderBy(p=>p.FIndex).ToList();

                    

                    for(int i = 0; i < result.Count; i++)
                    {
                        if (result[i].GXHB == 1)
                            activeIndex = i+1;
                    }

                    return Json(new SuccessResult()
                    {
                        Data = new { fgh = fgh, workflow = result, activeIndex = activeIndex, wps = wps }
                    });
                }
                else
                {
                    var result = db.v_YzkWorkFlow_GXHB.Where(p => p.FGH == gh).OrderBy(p => p.FIndex).ToList();

                    for (int i = 0; i < result.Count; i++)
                    {
                        if (result[i].GXHB == 1)
                            activeIndex = i+1;
                    }

                    return Json(new SuccessResult()
                    {
                        Data = new { fgh = fgh,workflow = result,activeIndex=activeIndex, wps = wps }
                    });
                }

            }
            return new FaildResult("缸号不能为空");
        }

        [HttpPost]
        [GYLCLogFilter]
        [Description("修改")]
        public object ModifyWorkFlow(JObject obj)
        {
            if (obj != null)
            {
                if (obj["workflow"] != null)
                {
                    CustomWorkFlowSet workflowModel;
                    try
                    {
                        var fgh = obj["fgh"].ToString();
                        //如果自定义工艺流程未有创建则新建并保存到数据库
                        if (db.CustomWorkFlowSet.Count(p => p.FGH == fgh) != 1)
                        {
                            workflowModel = new CustomWorkFlowSet() { FGH = obj["fgh"].ToString(), CreateTime = DateTime.Now, Creator = "admin" };
                            db.CustomWorkFlowSet.Add(workflowModel);
                            db.SaveChanges();
                        }

                        workflowModel = db.CustomWorkFlowSet.Single(p => p.FGH == fgh);


                        //删除未有工序汇报的自定义工艺流程节点
                        var canModifyIndex = (int)obj["canModifyIndex"];
                        var entryList = db.CustomWorkFlowEntrySet.Where(p => p.PFID == workflowModel.Id).ToList();
                        if (entryList.Count > 0)
                        {
                            for (int i = canModifyIndex > 0 ? canModifyIndex : 0; i < entryList.Count; i++)
                            {
                                var p = entryList[i];
                                var entry = db.Entry<CustomWorkFlowEntrySet>(p);
                                entry.State = System.Data.Entity.EntityState.Deleted;
                            }
                        }


                        //重新插入修改后的自定义工艺流程节点
                        var workflow = obj["workflow"];
                        for (int i = 0; i < workflow.Count(); i++)
                        {
                            CustomWorkFlowEntrySet entry;
                            var fid = workflow[i]["FID"] == null ? -1 : (int)workflow[i]["FID"];

                            if (i >= canModifyIndex || entryList.Count(p => p.Id == fid) == 0)
                            {
                                entry = new CustomWorkFlowEntrySet()
                                {
                                    //节点顺序等于节点所在数组索引
                                    Index = i + 1,
                                    PFID = workflowModel.Id,
                                    FWorkProcedure = (int)workflow[i]["WPID"],
                                    IsDeleted = false
                                };
                                db.CustomWorkFlowEntrySet.Add(entry);
                            }


                        }
                        db.SaveChanges();
                        return new SuccessResult();
                    }
                    catch (Exception ex)
                    {
                        return new FaildResult("500，服务端错误: \r\n" + ex.Message);
                    }
                }
            }

            return new FaildResult("数据格式错误");
        }

        [Description("修改日志")]
        public object GetLog([FromUri]string fgh)
        {
            if (string.IsNullOrEmpty(fgh))
            {
                return new FaildResult("缸号不能为空");
            }

            return new SuccessResult()
            {
                Data = db.GYLCLogSet.Where(p => p.Fgh == fgh).OrderBy(p => p.Id).ToList()
            };

        }
    }
}
