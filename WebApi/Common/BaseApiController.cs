using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using WebApi.Models.DYModels;
using WebApi.Models.EMTModels;


namespace WebApi.Common
{
    [LoginAuthorize]
    [ExceptionLog]
    public class BaseApiController : ApiController
    {
        //此类添加控制属性
    }

    /// <summary>
    /// 东岳进销存数据库
    /// </summary>
    public class DYApiController: BaseApiController
    {
        protected DYContainer db = new DYContainer();
    }

    /// <summary>
    /// 东岳EMT数据库
    /// </summary>
    public class EMTApiController : BaseApiController
    {
        protected EMTContainer db = new EMTContainer();
    }
}