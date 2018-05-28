using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Common;

namespace WebApi.Controllers.DY
{
    [Description("同步数据")]
    public class HKPgDataController : DYApiController
    {
        [Description("取同步数据")]
        public object Get()
        {
            try
            {
                var data = db.t_HK_PGData.Where(p => !p.IsSync).ToList();
                return Json(new SuccessResult() { Data = data });
            }
            catch (Exception ex)
            {
                return Json(new FaildResult(ex.Message));
            }
        }

        [HttpPost]
        [Description("同步回写")]
        public object Update(JObject obj)
        {
            try
            {
                var fghs = obj.GetValue("fghs").ToString();
                if (!string.IsNullOrEmpty(fghs.ToString()))
                {
                    var rows = (from row in db.t_HK_PGData
                                where fghs.Contains(row.batch_no)
                                select row).ToList();

                    if (rows != null || rows.Count() != 0)
                    {
                        rows.ForEach(p =>
                        {
                            p.IsSync = true;
                            db.t_HK_PGData.Attach(p);
                            db.Entry(p).State = System.Data.Entity.EntityState.Modified;
                        });

                        return db.SaveChanges();
                    }
                }
                return Json(new FaildResult("传入值不能为空"));
            }
            catch(Exception ex)
            {
                return Json(new FaildResult(ex.Message));
            }
        }
    }
}
