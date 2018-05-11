using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using WebApi.Models.DYModels;
using WebApi.Models.EMTModels;
using System.Text;
using System.Web.Security;

namespace WebApi.Common
{
    public class GYLCLogFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            var obj = JObject.Parse(actionExecutedContext.ActionContext.ActionArguments["obj"].ToString());
            //修改后的工艺流程
            var newworkflow = obj["workflow"];
            //缸号
            var fgh = obj["fgh"].ToString();
            //修改前工艺流程
            var workflow = obj["oldwf"];
            var token = actionExecutedContext.ActionContext.Request.Headers.Authorization;
            var ticket = FormsAuthentication.Decrypt(token.ToString());
            //当前操作用户
            string user = ticket.UserData.Split('&')[0].ToString();
            string after = printWorkFlow(newworkflow.ToList());
            string before = printWorkFlow(workflow.ToList());

            GYLCLogSet log = new GYLCLogSet()
            {
                Fgh = fgh,
                LogTime = DateTime.Now,
                Before = before,
                After=after,
                User = user
            };

            EMTContainer db = new EMTContainer();
            db.GYLCLogSet.Add(log);
            db.SaveChanges();

            base.OnActionExecuted(actionExecutedContext);
        }

        private string printWorkFlow(List<JToken> list)
        {
            StringBuilder sb = new StringBuilder();
            list.ForEach(p =>
            {
                sb.Append(list.IndexOf(p) + 1+" ");
                sb.Append(p["FName"]);
                if (list.Last() != p)
                {
                    sb.Append(">>");
                }
            });
            return sb.ToString();
        }
    }
}