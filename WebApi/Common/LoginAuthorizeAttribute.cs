using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Security;

namespace WebApi.Common
{
    //
    public class LoginAuthorizeAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            //验证当前请求是否匿名请求
            if (actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any())
            {
                return;
            }

            var token = actionContext.Request.Headers.Authorization;
            //验证是否登录
            if (!CheckLogin(token))
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, new HttpError("登录超时或未登录"));
                return;
            }

            var ticket = FormsAuthentication.Decrypt(token.ToString());
            var user = ticket.Name;

        }

        /// <summary>
        /// 验证登录状态
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private bool CheckLogin(System.Net.Http.Headers.AuthenticationHeaderValue token)
        {
            if (token != null)
            {
                var ticket = FormsAuthentication.Decrypt(token.ToString());
                if (ticket.Expired)
                {
                    //登录超时
                    return false;
                }
                
                return true;
            }
            //没有token信息，未登录
            return false;
        }

        /// <summary>
        /// 权限验证
        /// </summary>
        /// <param name="token"></param>
        /// <param name="actionContext"></param>
        /// <returns></returns>
        private bool CheckPermission(AuthenticationHeaderValue token,HttpActionContext actionContext)
        {


            return false;
        }

    }
}