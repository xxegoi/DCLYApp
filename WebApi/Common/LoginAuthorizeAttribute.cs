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
using WebApi.Models.DYModels;

namespace WebApi.Common
{
    //
    public class LoginAuthorizeAttribute : AuthorizationFilterAttribute
    {
        DYContainer db = new DYContainer();

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            //验证当前请求是否允许匿名请求
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

            //验证是否具有相应操作权限
            if (!CheckPermission(actionContext))
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, new HttpError("没有操作权限"));
            }

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
        private bool CheckPermission(HttpActionContext actionContext)
        {
            var token = actionContext.Request.Headers.Authorization;
            var ticket = FormsAuthentication.Decrypt(token.ToString());
            var userName = ticket.Name;

            if (userName == "admin")
            {
                return true;
            }
            //控制器名称
            var controllerName = actionContext.ControllerContext.ControllerDescriptor.ControllerName;
            //Action名称
            var actionName = actionContext.ActionDescriptor.ActionName;
            //返回ControllerAction在数据库中的记录
            var ca = db.ControllerActionSet.SingleOrDefault(p => p.ControllerName == controllerName && p.Name == actionName);
            //用户所有角色
            var roles = (from role in db.t_UserRoleSet
                        join user in db.t_Users on role.User equals user.FUserCode
                        where user.FUserCode == userName
                        select role).ToList();
                      
            if (ca != null)
            {
               foreach(var r in roles)
                {
                    var car = db.ControllerActionRoleSet.SingleOrDefault(p => p.ControllerActionId == ca.Id && p.RoleId == r.Role&&p.IsAllowed);
                    if (car != null)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

    }
}