using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq.Expressions;
using WebApi.OutputCache.V2;
using System.ComponentModel;

namespace WebApi.Controllers.EMT
{
    [Description("运转卡管理")]
    public partial class YZKController : EMTApiController
    {
        [Description("查询")]
        public object GetList([FromUri]dynamic query)
        {
            try
            {
                #region Linq查询
                var results = (from yzk in db.t_DY_YZK
                               join user in db.t_User on yzk.FBiller equals user.FUserID into g1
                               from a in g1.DefaultIfEmpty()
                               join user1 in db.t_User on yzk.FChecker equals user1.FUserID into g2
                               from b in g2.DefaultIfEmpty()
                               join item in db.t_ICItem on yzk.FItemid equals item.FItemID into g3
                               from c in g3.DefaultIfEmpty()
                               join bos123 in db.t_BOS200000123Entry2 on yzk.FAmountType equals bos123.FID into g4
                               from d in g4.DefaultIfEmpty()
                               join org in db.t_Organization on yzk.FOrganization equals org.FItemID into g5
                               from e in g5.DefaultIfEmpty()
                               join bos121 in db.t_BOS200000121Entry2 on yzk.FBase1 equals bos121.FID into g6
                               from f in g6.DefaultIfEmpty()
                               join bos124 in db.t_BOS200000124Entry2 on yzk.FBase equals bos124.FID into g7
                               from g in g7.DefaultIfEmpty()
                               join submessage in db.t_SubMessage on yzk.FBrandInfo equals submessage.FInterID into g8
                               from h in g8.DefaultIfEmpty()
                               join amountType in db.t_DY_AmountType on yzk.FAmountType equals amountType.FID into g9
                               from i in g9.DefaultIfEmpty()
                               where yzk.FBiller != 0
                               select new
                               {
                                   fid = yzk.FID,
                                   ppsx = h.FName,//品牌属性
                                   sflx = i.FName,//收费类型
                                   gh = yzk.FGH,//缸号
                                   billNo = yzk.FBillNo,//单据编号
                                   biller = a.FName,//制单人
                                   date = yzk.FDate,//单据日期
                                   orderNo = yzk.FOrderNo,//订单号
                                   customerName = e.FName,//客户名称
                                   organizationNumber = e.FNumber,
                                   customerNo = yzk.FCustomerNo,//客户单号
                                   fundNo = yzk.FFundNo,//款号
                                   qty = yzk.FQty,//开单匹数
                                   weight = yzk.FWeight,//重量
                                   pbNote = yzk.FPBNote,//配布备注
                                   checker = b.FName,//审核人
                                   itemNo = c.FNumber,//布类编码
                                   itemName = c.FName,//布类名称
                                   itemModel = c.FModel,//规格型号
                                   sumPZ = yzk.FSumPZ,//原缸总坯重
                                   note = yzk.FNOTE,//备注
                                   closed = yzk.FClosed,
                                   noteForCP = yzk.FNoteForCP,//备注(成长打印)
                                   deliveryDate = yzk.FDeliveryDate,//交货日期
                                   ddNote = yzk.FDDBZ,//订单备注
                                   workFlow = yzk.FWorkFlow,//工艺流程
                                   fdd = yzk.FDD,//对色光源
                                   fsd = yzk.FSD,//色对
                                   fsl = yzk.FSL,//牢度要求
                                   fsg = yzk.FSG,//手感要求
                                   dxfk = yzk.FDXFK,//成品要求
                                   fkz = yzk.fkz,//成品克重
                                   fjb = yzk.FJB,//浆边
                                   fqb = yzk.FQB,//切边
                                   fjsz = yzk.FJSZ,//加树脂
                                   fjry = yzk.FJRY,//加软油
                                   facross = yzk.FAcross,//横
                                   fstraight = yzk.FStraight,//直
                                   ftweak = yzk.FTweak,//扭度
                                   fzt = yzk.FZT,//纸筒
                                   fjz = yzk.fjz,//加重
                                   fjd = yzk.FJD,//胶带
                                   addr = yzk.FAddress//收货地址
                               });
                int page = 1;
                int size = 10;



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
                            results = results.Where(p => p.date >= startDate.Date && p.date <= endDate.Date);
                        }
                        else if (DateTime.TryParse(queryJson["daterange"][0].ToString(), out startDate))
                        {
                            results = results.Where(p => p.date >= startDate.Date);
                        }
                        else if (DateTime.TryParse(queryJson["daterange"][1].ToString(), out endDate))
                        {
                            results = results.Where(p => p.date <= endDate.Date);
                        }

                    }
                    if (queryJson["billNo"] != null)
                    {
                        var billno = queryJson["billNo"].ToString();//单据编号
                        if (!string.IsNullOrEmpty(billno))
                            results = results.Where(p => p.billNo.Contains(billno));
                    }
                    if (queryJson["fgh"] != null)
                    {
                        var fgh = queryJson["fgh"].ToString();//缸号
                        if (!string.IsNullOrEmpty(fgh))
                            results = results.Where(p => p.gh.Contains(fgh));
                    }
                    if (queryJson["orderNo"] != null)
                    {
                        var orderNo = queryJson["orderNo"].ToString();//订单号
                        if (!string.IsNullOrEmpty(orderNo))
                            results = results.Where(p => p.orderNo.Contains(orderNo));
                    }
                    if (queryJson["customerNo"] != null)
                    {
                        var customerNo = queryJson["customerNo"].ToString();//客户单号
                        if (!string.IsNullOrEmpty(customerNo))
                            results = results.Where(p => p.customerNo.Contains(customerNo));
                    }
                    if (queryJson["fundNo"] != null)
                    {
                        var fundNo = queryJson["fundNo"].ToString();//款号
                        if (!string.IsNullOrEmpty(fundNo))
                            results = results.Where(p => p.fundNo.Contains(fundNo));
                    }
                    if (queryJson["itemNo"] != null)
                    {
                        var itemNo = queryJson["itemNo"].ToString();//布种
                        if (!string.IsNullOrEmpty(itemNo))
                            results = results.Where(p => p.itemNo == itemNo);
                    }
                    if (queryJson["cstCode"] != null)
                    {
                        var cstCode = queryJson["cstCode"].ToString();//客户
                        if (!string.IsNullOrEmpty(cstCode))
                            results = results.Where(p => p.organizationNumber == cstCode);
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
                int total = results.Count();
                //分页查询
                results = results.OrderByDescending(p => p.date).Skip((page - 1) * size).Take(size);
                var data = new SuccessResult();
                data.Data = new { page = page, size = size, tableData = results.ToList(), total = total };
                #endregion

                return Json(data);
            }
            catch (Exception ex)
            {
                return new FaildResult(ex.InnerException.Message + "\r\n" + ex.Source);
            }
        }

        [CacheOutput(ClientTimeSpan = 60, ServerTimeSpan = 60)]
        [Description("明细")]
        public object GetEntries([FromUri]string fgh)
        {
            try
            {
                if (string.IsNullOrEmpty(fgh))
                {
                    return new FaildResult("缸号不能为空");
                }

                var entrys = GetXiMa(fgh);

                var gy = GetGY(fgh);

                var dxgy = GetDXGY(fgh);

                var rsd = GetRSD(fgh);

                var lld = GetLLD(fgh);

                var rgcr = GetRGCR(fgh);

                SuccessResult result = new SuccessResult();
                result.Data = new { xm = entrys, gy = gy, dxgy = dxgy, rsd = rsd, lld = lld, rgcr = rgcr };
                return Json(result);
            }
            catch (Exception ex)
            {
                return new FaildResult(ex.TargetSite + ":" + ex.InnerException.Message + "timeout:" + db.Database.Connection.ConnectionTimeout);
            }

        }

        [Description("查询")]
        [HttpPost]
        public object GetList(JObject query)
        {
            #region 查询LINQ
            var results = (from yzk in db.t_DY_YZK
                           join user in db.t_User on yzk.FBiller equals user.FUserID into g1
                           from a in g1.DefaultIfEmpty()
                           join user1 in db.t_User on yzk.FChecker equals user1.FUserID into g2
                           from b in g2.DefaultIfEmpty()
                           join item in db.t_ICItem on yzk.FItemid equals item.FItemID into g3
                           from c in g3.DefaultIfEmpty()
                           join bos123 in db.t_BOS200000123Entry2 on yzk.FAmountType equals bos123.FID into g4
                           from d in g4.DefaultIfEmpty()
                           join org in db.t_Organization on yzk.FOrganization equals org.FItemID into g5
                           from e in g5.DefaultIfEmpty()
                           join bos121 in db.t_BOS200000121Entry2 on yzk.FBase1 equals bos121.FID into g6
                           from f in g6.DefaultIfEmpty()
                           join bos124 in db.t_BOS200000124Entry2 on yzk.FBase equals bos124.FID into g7
                           from g in g7.DefaultIfEmpty()
                           join submessage in db.t_SubMessage on yzk.FBrandInfo equals submessage.FInterID into g8
                           from h in g8.DefaultIfEmpty()
                           join amountType in db.t_DY_AmountType on yzk.FAmountType equals amountType.FID into g9
                           from i in g9.DefaultIfEmpty()
                           where yzk.FBiller != 0
                           select new
                           {
                               fid = yzk.FID,
                               ppsx = h.FName,//品牌属性
                               sflx = i.FName,//收费类型
                               gh = yzk.FGH,//缸号
                               billNo = yzk.FBillNo,//单据编号
                               biller = a.FName,//制单人
                               date = yzk.FDate,//单据日期
                               orderNo = yzk.FOrderNo,//订单号
                               customerName = e.FName,//客户名称
                               organizationNumber = e.FNumber,
                               customerNo = yzk.FCustomerNo,//客户单号
                               fundNo = yzk.FFundNo,//款号
                               qty = yzk.FQty,//开单匹数
                               weight = yzk.FWeight,//重量
                               pbNote = yzk.FPBNote,//配布备注
                               checker = b.FName,//审核人
                               itemNo = c.FNumber,//布类编码
                               itemName = c.FName,//布类名称
                               itemModel = c.FModel,//规格型号
                               sumPZ = yzk.FSumPZ,//原缸总坯重
                               note = yzk.FNOTE,//备注
                               closed = yzk.FClosed,
                               noteForCP = yzk.FNoteForCP,//备注(成长打印)
                               deliveryDate = yzk.FDeliveryDate,//交货日期
                               ddNote = yzk.FDDBZ,//订单备注
                               workFlow = yzk.FWorkFlow,//工艺流程
                               fdd = yzk.FDD,//对色光源
                               fsd = yzk.FSD,//色对
                               fsl = yzk.FSL,//牢度要求
                               fsg = yzk.FSG,//手感要求
                               dxfk = yzk.FDXFK,//成品要求
                               fkz = yzk.fkz,//成品克重
                               fjb = yzk.FJB,//浆边
                               fqb = yzk.FQB,//切边
                               fjsz = yzk.FJSZ,//加树脂
                               fjry = yzk.FJRY,//加软油
                               facross = yzk.FAcross,//横
                               fstraight = yzk.FStraight,//直
                               ftweak = yzk.FTweak,//扭度
                               fzt = yzk.FZT,//纸筒
                               fjz = yzk.fjz,//加重
                               fjd = yzk.FJD,//胶带
                               addr = yzk.FAddress//收货地址
                           });
            #endregion
            int page = string.IsNullOrEmpty(query.GetValue("page").ToString()) ? 1 : Convert.ToInt32(query.GetValue("page").ToString());
            int size = string.IsNullOrEmpty(query.GetValue("size").ToString()) ? 1 : Convert.ToInt32(query.GetValue("size").ToString());
            var begin = query.GetValue("begindate");
            var end = query.GetValue("enddate");
            var bill = query.GetValue("billNo");
            var order = query.GetValue("orderNo");
            var gh = query.GetValue("fgh");
            var cst = query.GetValue("cstCode");
            var cstNo = query.GetValue("customerNo");
            var fun = query.GetValue("fundNo");

            if (begin!=null&&!string.IsNullOrEmpty(begin.ToString()))
            {
                var begindate = DateTime.Parse(begin.ToString()).Date;
                results = results.Where(p => p.date >= begindate);
            }

            if (end!=null&&!string.IsNullOrEmpty(end.ToString()))
            {
                var enddate = DateTime.Parse(end.ToString()).Date;
                results = results.Where(p => p.date <= enddate);
            }

            if (bill!=null&&!string.IsNullOrEmpty(bill.ToString()))
            {
                var billNo = bill.ToString();
                results = results.Where(p => p.billNo == billNo);
            }

            if (order!=null&&!string.IsNullOrEmpty(order.ToString()))
            {
                var orderNo = order.ToString();
                results = results.Where(p => p.orderNo == orderNo);
            }

            if (gh!=null&&!string.IsNullOrEmpty(gh.ToString()))
            {
                var fgh =gh.ToString();
                results = results.Where(p => p.gh == fgh);
            }

            if (cst!=null&&!string.IsNullOrEmpty(cst.ToString()))
            {
                var cstCode =cst.ToString();
                results = results.Where(p => p.customerName == cstCode);
            }

            if (cstNo!=null&&!string.IsNullOrEmpty(cstNo.ToString()))
            {
                var customerNo =cstNo.ToString();
                results = results.Where(p => p.customerNo == customerNo);
            }

            if (fun!=null&&!string.IsNullOrEmpty(fun.ToString()))
            {
                var fundNo = fun.ToString();
                results = results.Where(p => p.fundNo == fundNo);
            }

            int total = results.Count();
            //分页查询
            results = results.OrderByDescending(p => p.date).Skip((page - 1) * size).Take(size);
            var data = new SuccessResult();
            data.Data = new { tableData = results.ToList(), total = total };

            return Json(data);

        }

        private object GetXiMa(string fgh)
        {
            return (from yzk in db.t_DY_YZK
                    join item in db.t_DY_YZKEntry1 on yzk.FID equals item.FID into g1
                    from a in g1.DefaultIfEmpty()
                    where yzk.FGH == fgh
                    select new
                    {
                        fxm = a.FXM == null ? "" : a.FXM,
                        fpz = a.FPZ.ToString()
                    }).ToList();
        }

        private object GetGY(string fgh)
        {
            return (from item in db.t_DY_YZKEntry2
                    join yzk in db.t_DY_YZK on item.FID equals yzk.FID
                    join bos130 in db.t_BOS200000130Entry2 on item.FJB equals bos130.FID into g1
                    from a in g1.DefaultIfEmpty()
                    join bos128 in db.t_BOS200000128Entry2 on item.FRG equals bos128.FID into g2
                    from b in g2.DefaultIfEmpty()
                    join dep in db.t_Department on item.FDepartment equals dep.FItemID into g3
                    from c in g3.DefaultIfEmpty()
                    join dep1 in db.t_Department on item.FSJDepartment equals dep1.FItemID into g4
                    from d in g4.DefaultIfEmpty()
                    join work in db.t_dy_workprocedureentry on item.FCurWorkProcedure equals work.FID into g5
                    from e in g5.DefaultIfEmpty()
                    join bos128_2 in db.t_BOS200000128Entry2 on item.FSCJT equals bos128_2.FID into g6
                    from f in g6.DefaultIfEmpty()
                    join work2 in db.t_dy_workprocedureentry on item.FXDGX equals work2.FID into g7
                    from g in g7.DefaultIfEmpty()
                    where yzk.FGH == fgh
                    orderby item.FEntryID ascending
                    select new
                    {
                        FEntryID3 = item.FEntryID,
                        FBase3 = item.FJB,
                        FBase3_FNDName = a.FNumber,
                        FBase3_DSPName = a.FName,
                        Finteger = item.FQty,
                        FRG = item.FRG,
                        FRG_FNDName = b.FNumber,
                        FRG_DSPName = b.FName,
                        FLZQty = item.FLZQty,
                        FYCJL = item.FYCJL,
                        FYCQty = item.FYCQty,
                        FDepartment = item.FDepartment,
                        FDepartment_FNDName = c.FNumber,
                        FDepartment_DSPName = c.FName,//计划生产车间
                        FSJDepartment = item.FSJDepartment,
                        FSJDepartment_FNDName = d.FNumber,
                        FSJDepartment_DSPName = d.FName,//实际生产车间
                        FCurWorkProcedure = item.FCurWorkProcedure,
                        FCurWorkProcedure_FNDName = e.FNumber,//当前工序
                        FCurWorkProcedure_DSPName = e.FName,
                        FSCJT_FNDName = f.FNumber,
                        FSCJT_DSPName = f.FName,//生产机台
                        FJSTime = item.FJSTime,//接收时间
                        FKSTiem = item.FKSTime,//开始时间
                        FEndTiem = item.FEndTime,//结束时间
                        FXDGX = item.FXDGX,
                        FXDGX_FNDName = g.FNumber,
                        FXDGX_DSPName = g.FName,//下道工序
                        FOprationer = item.FOprationer,//操作人
                        FCheckBox = item.FCheckBox,//是否完成
                        FBolCurWP = item.FBolCurWP,
                        FFinishTime = item.FFinishTime,//完工时间
                        FID3 = item.FID,
                        FIndex3 = item.FIndex
                    }).ToList();
        }

        private object GetDXGY(string fgh)
        {
            return (from gylc in db.t_DY_MonldingGYLC
                    join classType in db.t_SubMessage on gylc.FClassType equals classType.FInterID into g1
                    from a in g1.DefaultIfEmpty()
                    join jt in db.t_BOS200000129Entry2 on gylc.FJT equals jt.FID into g2
                    from b in g2.DefaultIfEmpty()
                    join type in db.t_SubMessage on gylc.FType equals type.FInterID into g3
                    from c in g3.DefaultIfEmpty()
                    join dep in db.t_Department on gylc.FDept equals dep.FItemID into g4
                    from d in g4.DefaultIfEmpty()
                    join cust in db.t_Organization on gylc.FCustermor equals cust.FItemID into g5
                    from e in g5.DefaultIfEmpty()
                    join item in db.t_ICItem on gylc.FBL equals item.FItemID into g6
                    from f in g6.DefaultIfEmpty()
                    where gylc.FGH == fgh
                    select new
                    {
                        gylc,//工艺流表全字段
                        classTypeName = a.FName,//班组名称
                        JTH = b.FName,//机台名称
                        typeName = c.FName,//类型名称
                        department = d.FName,//定型车间
                        customerName = e.FName,//客户名称
                        customerNum = e.FNumber,//客户编号
                        itemName = f.FName,//品名
                        itemNum = f.FNumber,//布类编码
                        itemModel = f.FModel//布类规格
                    }).ToList();
        }

        private object GetRSD(string fgh)
        {
            return (from entry in db.t_TempDSKEntry2
                    join m in db.t_TempDSK on entry.FID equals m.FID
                    join item in db.t_ICItem on entry.FMaterialID equals item.FItemID into g1
                    from a in g1.DefaultIfEmpty()
                    join item in db.t_ICItem on m.FItemID equals item.FItemID into g2
                    from b in g2.DefaultIfEmpty()
                    join user in db.t_User on m.FBiller equals user.FUserID into g3
                    from c in g3.DefaultIfEmpty()
                    join user in db.t_User on m.FChecker equals user.FUserID into g4
                    from d in g4.DefaultIfEmpty()
                    join color in db.t_SubMessage on m.FColor equals color.FInterID into g5
                    from e in g5.DefaultIfEmpty()
                    join cust in db.t_Organization on m.FCustID equals cust.FItemID into g6
                    from f in g6.DefaultIfEmpty()
                    join shapi in db.t_DY_ShaPi on entry.FShaPi equals shapi.FID into g7
                    from g in g7.DefaultIfEmpty()
                    join colorType in db.t_SubMessage on entry.FItemType equals colorType.FInterID into g8
                    from h in g8.DefaultIfEmpty()
                    join work in db.t_dy_workprocedureentry on entry.FWorkProcedure equals work.FID into g9
                    from i in g9.DefaultIfEmpty()
                    where m.FGH == fgh
                    select new
                    {
                        m,//染色单主表全字段
                        entry,//染色单明细全字段
                        entryItemName = a.FName,//明细物料名称
                        entryItemNum = a.FNumber,//明细物料编码
                        entryItemModel = a.FModel,//明细物料规格
                        itemName = b.FName,//主表布种名称
                        itemNum = b.FNumber,//布类编码
                        biller = c.FName,//制单人
                        checker = d.FName,//审核人
                        colorSeri = e.FName,//主表色系
                        custNumber = f.FNumber,//客户代码
                        custName = f.FName,//客户名称
                        entryShapi = g.FName,//明细纱胚名称
                        entryItemType = h.FName,//明细物料类别
                        entryWork = i.FName//明细工序名称
                    }).ToList();
        }

        private object GetLLD(string fgh)
        {
            return (from lld in db.ICStockBill
                    join detail in db.ICStockBillEntry on lld.FInterID equals detail.FInterID
                    join item in db.t_ICItem on detail.FItemID equals item.FItemID
                    join unit in db.t_MeasureUnit on item.FUnitID equals unit.FMeasureUnitID into g1
                    from a in g1.DefaultIfEmpty()
                    join dept in db.t_Department on lld.FDeptID equals dept.FItemID into g2
                    from b in g2.DefaultIfEmpty()
                    where lld.FGH == fgh
                    select new
                    {
                        date = lld.FDate,//日期
                        check = lld.FCheckerID > 0 || lld.FCheckerID < 0 ? "Y" : "",//审核标志
                        cancellation = lld.FCancellation == true ? "Y" : "",//关闭标志
                        billNo = lld.FBillNo,//单据编号
                        itemName = item.FName,//物料名称
                        itemNum = item.FNumber,//物料编码
                        itemModel = item.FModel,//物料规格
                        itemUnit = a.FName,//单位名称
                        itemQty = item.FQtyDecimal,//实发数量
                        deptName = b.FName,//部门名称
                        itemDosage = detail.FDosage//用量
                    }
                    ).ToList();
        }

        private object GetRGCR(string fgh)
        {
            var result = (from header in db.t_DY_RGStockBill
                          join detail in db.t_DY_RGStockBillEntry on header.FID equals detail.FID
                          join user in db.t_User on header.FBiller equals user.FUserID into g1
                          from a in g1.DefaultIfEmpty()
                          join user1 in db.t_User on header.FChecker equals user1.FUserID into g2
                          from b in g2.DefaultIfEmpty()
                          join item in db.t_ICItem on detail.FItemID equals item.FItemID into g3
                          from c in g3.DefaultIfEmpty()
                          join cust in db.t_Organization on detail.FCustomer equals cust.FItemID into g4
                          from d in g4.DefaultIfEmpty()
                          join bos128 in db.t_BOS200000128Entry2 on detail.FJT equals bos128.FID into g5
                          from e in g5.DefaultIfEmpty()
                          join dept in db.t_Department on detail.FDepartment equals dept.FItemID into g6
                          from f in g6.DefaultIfEmpty()
                          join emp in db.t_Emp on detail.FZG equals emp.FItemID into g7
                          from g in g7.DefaultIfEmpty()
                          join emp1 in db.t_Emp on detail.FDS equals emp1.FItemID into g8
                          from h in g8.DefaultIfEmpty()
                          where detail.FGH == fgh
                          select new
                          {
                              date = header.FDate,
                              tranType = header.FTrantype == "1" ? "出缸" : "入缸",
                              biller = a.FName,
                              checker = b.FName,
                              billNo = header.FBillNo,
                              checkDate = header.FCheckDate,
                              detaiDate = detail.FTime,
                              fType = detail.FType == "1" ? "分OK缸" : "OK缸",
                              itemNum = c.FNumber,
                              itemName = c.FName,
                              colorNo = detail.FColorNo,
                              color = detail.FColor,
                              custName = d.FName,
                              qty = detail.FQty,
                              weight = detail.FWeight,
                              rjh = e.FName,
                              deptName = f.FName,
                              jgy = g.FName,
                              ds = h.FName,
                              fbb = detail.FBB == "1" ? "甲班" : "乙班",
                              note = detail.FNote
                          }).ToList();

            return result;
        }

    }


}
