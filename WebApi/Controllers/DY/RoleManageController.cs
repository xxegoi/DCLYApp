using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Common;
using WebApi.Models;
using WebApi.Models.DYModels;

namespace WebApi.Controllers.DY
{
    [Description("角色管理")]
    public class RoleManageController : DYApiController
    {
        [HttpPost]
        [Description("新增")]
        public object Put(JObject obj)
        {
            if (obj == null)
            {
                return new FaildResult("提交数据异常");
            }

            try
            {
                string roleName = obj["Name"].ToString();
                string roleDescription = obj["Description"].ToString();

                t_RoleSet entity = new t_RoleSet() { Name = roleName, Description = roleDescription };
                db.t_RoleSet.Add(entity);
                db.SaveChanges();
                return new SuccessResult();

            }
            catch (Exception ex)
            {
                return new FaildResult(ex.Message);
            }
        }
        [HttpGet]
        [Description("删除")]
        public object Remove(string Id)
        {
            try
            {
                int roleId = Convert.ToInt32(Id);
                var entity = db.t_RoleSet.Find(roleId);

                if (entity == null)
                {
                    return new FaildResult("角色不存在");
                }
                //删除角色
                db.t_RoleSet.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Deleted;

                //删除角色成员
                db.t_UserRoleSet.Where(p => p.Role == roleId).ToList().ForEach(e =>
                   {
                       db.t_UserRoleSet.Attach(e);
                       db.Entry(e).State = System.Data.Entity.EntityState.Deleted;
                   });

                //删除角色权限
                db.ControllerActionRoleSet.Where(p => p.RoleId == roleId).ToList().ForEach(p =>
                {
                    db.ControllerActionRoleSet.Attach(p);
                    db.Entry(p).State = System.Data.Entity.EntityState.Deleted;
                });

                db.SaveChanges();
                return new SuccessResult();

            }
            catch (Exception ex)
            {
                return new FaildResult(ex.Message);
            }
        }
        [HttpPost]
        [Description("修改")]
        public object Update(JObject obj)
        {
            if (obj == null)
            {
                return new FaildResult("提交数据异常");
            }

            try
            {
                int Id = Convert.ToInt32(obj["Id"]);
                var entity = db.t_RoleSet.Find(Id);
                db.t_RoleSet.Attach(entity);
                entity.Description = obj["Description"].ToString();
                //传入成员列表
                var members = obj["Members"];

                //数据库现有成员列表
                var oldmembers = (from mb in db.t_UserRoleSet
                                  where mb.Role == Id
                                  select mb).ToList();

                if (members != null)
                {
                    var memberList = members.ToList();
                    memberList.ForEach(p =>
                    {
                        //传入成员不存在于数据库中时，该成员为新添加成员，插入到数据库中
                        if (oldmembers.Count(m => m.User == p.ToString()) == 0)
                        {
                            db.t_UserRoleSet.Add(new t_UserRoleSet() { Role = Id, User = p.ToString() });
                        }
                    });

                    oldmembers.ForEach(p =>
                    {
                        //如果数据库中成员不存在于传入成员列表中，说明该成员被删除，删除数据库中该记录
                        if (!memberList.Contains(p.User))
                        {
                            db.t_UserRoleSet.Attach(p);
                            db.Entry(p).State = System.Data.Entity.EntityState.Deleted;
                        }
                    });
                }
                else
                {
                    //成员列表为空，删除数据库中所有成员
                    oldmembers.ForEach(p =>
                    {
                        db.t_UserRoleSet.Attach(p);
                        db.Entry(p).State = System.Data.Entity.EntityState.Deleted;
                    });
                }

                db.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return new SuccessResult();

            }
            catch (Exception ex)
            {
                return new Exception(ex.Message);
            }
        }

        [HttpGet]
        [Description("明细")]
        public object GetMembers(string Id)
        {
            try
            {
                var entity = db.t_RoleSet.Find(Convert.ToInt32(Id));
                if (entity == null)
                {
                    return new FaildResult("找不到此角色");
                }

                var members = from m in db.t_UserRoleSet
                              where m.Role == entity.Id
                              select m.User;

                return new SuccessResult() { Data = members };

            }
            catch (Exception ex)
            {
                return new FaildResult(ex.Message);
            }
        }

        [Description("列表")]
        public object GetList(int page = 1, int size = 10)
        {
            try
            {
                var roles = db.t_RoleSet.OrderBy(p => p.Id).Skip((page - 1) * size).Take(size).ToList();

                var total = db.t_RoleSet.Count();

                return new SuccessResult() { Data =new { roles,total } };
            }
            catch (Exception ex)
            {
                return new FaildResult(ex.Message);
            }
        }
    }
}
