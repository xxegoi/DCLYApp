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
    [Description("客户管理")]
    public class CustomerController : EMTApiController
    {
        [Description("查询")]
        [HttpGet]
        public object GetList(string name)
        {
            try
            {
                var result = (from a in db.t_Organization where a.FName.Contains(name) select new { value = a.FName,code=a.FNumber }).ToList();
                return new SuccessResult() { Data = result };

            }catch(Exception ex)
            {
                return new FaildResult(ex.Message);
            }
        }
    }
}
