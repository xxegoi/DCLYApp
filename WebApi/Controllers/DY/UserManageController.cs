using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Common;
using WebApi.Models.DYModels;

namespace WebApi.Controllers.DY
{
    [Description("用户管理")]
    public class UserManageController : DYApiController
    {
        [Description("列表")]
        public object GetList(string query="", int page = 1, int size = 10)
        {
            var users = (from u in db.t_Users
                          select new { u.FUserCode, u.FUserName }).OrderBy(p => p.FUserCode);
            var total = db.t_Users.Count(); 

            if (query==null)
            {
                return new SuccessResult() { Data =new { Users=users.Skip((page - 1) * size).Take(size).ToList(),total=total } };
            }

            var users1 = users.Where(p => p.FUserCode.Contains(query) || p.FUserName.Contains(query)).Skip((page - 1) * size).Take(size).ToList();

            return new SuccessResult() { Data =new {Users=users1,total=total} };
        }

        [Description("列出所有")]
        public object GetAll()
        {
            var users = from u in db.t_Users
                        select new { u.FUserCode, u.FUserName };

            return new SuccessResult() { Data = users.ToList() };
        }

        [Description("新增")]
        [HttpPost]
        public object Put(JObject obj)
        {
            if (obj == null)
            {
                return new FaildResult("传入数据异常");
            }

            try
            {
                string userName = obj["UserName"].ToString();
                string userCode = obj["UserCode"].ToString();
                string passWord = obj["PassWord"].ToString();

                var user = db.t_Users.Find(userCode);
                if (user != null)
                {
                    return new FaildResult("用户帐号重复");
                }

                if (db.t_Users.Count(p => p.FUserName.ToLower().Contains(userName.ToLower())) > 0)
                {
                    return new FaildResult(string.Format("系统中已存在 {0} 的用户，请修改，建议：{0}{1}", userName, DateTime.Now.Year));
                }

                user = new t_Users() { FUserCode = userCode, FUserName = userName, FPassWord = passWord };
                db.t_Users.Add(user);
                db.SaveChanges();
                return new SuccessResult();

            }catch(Exception ex)
            {
                return new FaildResult(ex.Message);
            }
        }

        [Description("删除")]
        [HttpGet]
        public object Delete(string userCode)
        {
            try
            {
                var user = db.t_Users.Find(userCode);
                if (user == null)
                {
                    return new FaildResult("用户不存在");
                }
                db.t_Users.Attach(user);
                db.Entry(user).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                return new SuccessResult();
            }
            catch(Exception ex)
            {
                return new FaildResult(ex.Message);
            }
        }

        [Description("修改")]
        [HttpPost]
        public object Update(JObject obj)
        {
            return new FaildResult("此方法未实现");
        }
    }
}
