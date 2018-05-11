using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Common;

namespace WebApi.Controllers.EMT
{
    [Description("色号管理")]
    public class ColorNoController : EMTApiController
    {
        [Description("查询")]
        public object GetList([FromUri]dynamic query)
        {
            int page = 1; int size = 10;
            var results = from colorNo in db.t_DY_ColorNo
                          join item in db.t_ICItem on colorNo.FItemID equals item.FItemID into g1
                          from a in g1.DefaultIfEmpty()
                          join colorNoType in db.t_SubMessage on colorNo.FColorNoType equals colorNoType.FInterID into g2
                          from b in g2.DefaultIfEmpty()
                          join colorSeries in db.t_SubMessage on colorNo.FColor equals colorSeries.FInterID into g3
                          from c in g3.DefaultIfEmpty()
                          join coloerCustSeries in db.t_SubMessage on colorNo.FColorForCust equals coloerCustSeries.FInterID into g4
                          from d in g4.DefaultIfEmpty()
                          join biller in db.t_User on colorNo.FBiller equals biller.FUserID into g5
                          from e in g5.DefaultIfEmpty()
                          join checker in db.t_User on colorNo.FChecker equals checker.FUserID into g6
                          from f in g6.DefaultIfEmpty()
                          join cust in db.t_Organization on colorNo.FCustID equals cust.FItemID into g7
                          from g in g7.DefaultIfEmpty()
                          join shapi in db.t_DY_ShaPi on colorNo.FShapi equals shapi.FID into g8
                          from h in g8.DefaultIfEmpty()
                          orderby colorNo.FDate descending
                          select new
                          {
                              colorNo,//色号表全字段
                              ItemName = a.FName,//布类名称
                              ItemNumber = a.FNumber,//布类编号
                              ShaPiName = h.FName,//纱胚名称
                              ColorTypeName = b.FName, //色号类别名称
                              ColorSeriesName = c.FName,//色系名称
                              ColorCustSeriesName = d.FName,//客户色系名称
                              CustomerNumber=g.FNumber,//客户代码
                              CustomerName = g.FName,//客户名称
                              BillerName = e.FName,//制单人名
                              CheckerName = f.FName//审核人名
                          };

            //过滤条件
            if (query != null)
            {
                JObject queryJson = JsonConvert.DeserializeObject(query);
                if (queryJson["daterange"] != null && queryJson["daterange"].Count() > 0)
                {
                    DateTime startDate;//开始日期
                    DateTime endDate;//结束日期

                    if (DateTime.TryParse(queryJson["daterange"][0].ToString(), out startDate) && DateTime.TryParse(queryJson["daterange"][1].ToString(), out endDate))
                    {
                        results = results.Where(p => p.colorNo.FDate >= startDate.Date && p.colorNo.FDate <= endDate.Date);
                    }
                    else if (DateTime.TryParse(queryJson["daterange"][0].ToString(), out startDate))
                    {
                        results = results.Where(p => p.colorNo.FDate >= startDate.Date);
                    }
                    else if (DateTime.TryParse(queryJson["daterange"][1].ToString(), out endDate))
                    {
                        results = results.Where(p => p.colorNo.FDate <= endDate.Date);
                    }

                }
                if (queryJson["billNo"] != null)
                {
                    var billno = queryJson["billNo"].ToString();//单据编号
                    if (!string.IsNullOrEmpty(billno))
                        results = results.Where(p => p.colorNo.FBillNo.Contains(billno));
                }

                if (queryJson["ColorSeries"] != null)
                {
                    int colorSeriesNo;//色系编号
                    if(Int32.TryParse(queryJson["ColorSeries"].ToString(),out colorSeriesNo))
                        results = results.Where(p => p.colorNo.FColor == colorSeriesNo);
                }

                if (queryJson["itemNo"] != null)
                {
                    var itemNo = queryJson["itemNo"].ToString();//布种
                    if (!string.IsNullOrEmpty(itemNo))
                        results = results.Where(p => p.ItemNumber.Contains(itemNo));
                }

                if (queryJson["ColorNo"] != null)
                {
                    var colorNo = queryJson["ColorNo"].ToString();//色号
                    if (!string.IsNullOrEmpty(colorNo))
                        results = results.Where(p => p.colorNo.FColorNo.Contains(colorNo));
                }

                if (queryJson["Customer"] != null)
                {
                    var cstCode = queryJson["Customer"].ToString();//客户
                    if (!string.IsNullOrEmpty(cstCode))
                        results = results.Where(p => p.CustomerName.Contains(cstCode));
                }
                if (queryJson["page"] != null)
                {
                    var pagestr = queryJson["page"].ToString();

                    page = Convert.ToInt32(pagestr);
                }
                if (queryJson["size"] != null)
                {
                    var sizestr = queryJson["size"].ToString();
                    size = Convert.ToInt32(sizestr);
                }
            }

            //分页查询
            int total = results.Count();
            results = results.Skip((page - 1) * size).Take(size);
            SuccessResult data = new SuccessResult();
            data.Data = new { page = page, size = size, total = total, tableData = results.ToList() };
            return Json(data);

        }
        [Description("明细")]
        public object GetEntries([FromUri]int colorId)
        {
            var results = from entry in db.t_DY_ColorNoEntry
                          join item in db.t_ICItem on entry.FMaterialID equals item.FItemID into g1
                          from a in g1.DefaultIfEmpty()
                          join classType in db.t_SubMessage on entry.FItemType equals classType.FInterID into g2
                          from b in g2.DefaultIfEmpty()
                          join work in db.t_dy_workprocedureentry on entry.FWorkProcedure equals work.FID into g3
                          from c in g3.DefaultIfEmpty()
                          where entry.FID == colorId
                          orderby entry.FIndex ascending
                          select new
                          {
                              entry,
                              WorkName=c.FName,//工序名称
                              ItemName= a.FName,//物料名称
                              ItemModel=a.FModel,//物料规格
                              ClassTypeName =b.FName,//物料类别
                          };
            SuccessResult data = new SuccessResult() { Data = results.ToList() };
            return Json(data);
        }
    }
}
