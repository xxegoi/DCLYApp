using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http.Filters;
using WebApi.Common;

namespace WebApi.Common
{
    public class ExceptionLogAttribute: ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {

            string controllerName = actionExecutedContext.ActionContext.ControllerContext.ControllerDescriptor.ControllerName;
            string actionName = actionExecutedContext.ActionContext.ActionDescriptor.ActionName;
            string exceptionMessage = actionExecutedContext.Exception.Message;
            string exceptionObj = actionExecutedContext.Exception.Source;

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("ExceptionMessage：{0}\r\n", exceptionMessage);
            sb.AppendFormat("ControllerName：{0}\r\n", controllerName);
            sb.AppendFormat("ActionName：{0}\r\n", actionName);
            sb.AppendFormat("ExceptionSoruce：{0}\r\n", exceptionObj);

            LogHandeler.Error(sb.ToString());

            base.OnException(actionExecutedContext);
        }
    }
}