using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Security;
using WebApi.Common;
using WebApi.Models.DYModels;

namespace WebApi.Controllers.DY
{
    [Description("帐号管理")]
    public class AccountController : DYApiController
    {
        [HttpPost]
        [AllowAnonymous]
        [Description("登录")]
        public object Login(JObject model)
        {
            try
            {
                if (model["UserName"]==null || model["PassWord"]==null)
                {
                    return Json(new FaildResult("用户名和密码都不能为空"));
                }

                var userName = model["UserName"].ToString();
                var passWord = model["PassWord"].ToString();

                var user = db.t_Users.Find(userName);
                if (user == null)
                {
                    return Json(new FaildResult("用户不存在"));
                }
                if (user.FPassWord != passWord)
                {
                    return Json(new FaildResult("密码错误"));
                }

                var ticket = new FormsAuthenticationTicket(0, userName, DateTime.Now, DateTime.Now.AddHours(2),
                    true, string.Format("{0}&{1}", user.FUserName, passWord));

                var token = FormsAuthentication.Encrypt(ticket);
                FormsAuthentication.SetAuthCookie(userName, true);
                HttpContext.Current.Session["token"] = token;
                return Json(new SuccessResult() { Data = token });
            }
            catch(Exception ex)
            {
                return new FaildResult(ex.InnerException.Message);
            }
           
        }

        [HttpGet]
        [Description("注销")]
        [AllowAnonymous]
        public object Logout()
        {
            FormsAuthentication.SignOut();
            return new SuccessResult();
        }

        [HttpPost]
        [Description("修改密码")]
        public object ChangePassword(JObject obj)
        {
            if (obj == null)
            {
                return new FaildResult("数据异常");
            }
            try
            {
                string user = obj["user"].ToString();
                string pass = obj["newpassword"].ToString();
                var userEntry = db.t_Users.Find(user);
                if (userEntry == null)
                {
                    return new FaildResult("用户不存在");
                }
                //修改用户密码 
                userEntry.FPassWord = pass;
                db.t_Users.Attach(userEntry);
                db.Entry<t_Users>(userEntry).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return new SuccessResult();
            }catch(Exception ex)
            {
                return new FaildResult(ex.Message);
            }
        }

        
       
    }
}
