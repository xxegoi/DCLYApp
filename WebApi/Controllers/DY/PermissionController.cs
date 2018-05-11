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
    [Description("权限管理")]
    public class PermissionController : DYApiController
    {
        [Description("查询")]
        [HttpGet]
        public object GetList([FromUri]string roleId)
        {
            try
            {
                int role = Convert.ToInt32(roleId);
                //获取角色权限列表
                var cars = db.ControllerActionRoleSet.Where(p => p.RoleId == role).ToList();
                //获取所有模块功能列表
                var cas = db.ControllerActionSet.ToList();

                List<PermissionControllerViewModel> result = new List<PermissionControllerViewModel>();

                //遍历所有模块功能
                cas.ForEach(p =>
                {
                    var item = new PermissionControllerViewModel()
                    {
                        ControllerName = p.ControllerDisplayName,
                        ActionName = p.ActionDisplayName,
                        IsAllowed = false,
                        ControllerActionId=p.Id
                    };
                    //如果当前模块功能ID存在于该角色权限列表中，返回数据库中的值
                    var car = cars.SingleOrDefault(c => c.ControllerActionId == p.Id);
                    if (car != null)
                    {
                        item.Id = car.Id;
                        item.IsAllowed = car.IsAllowed;
                    }
                    result.Add(item);
                });

                return Json(new SuccessResult() { Data = result });
            }
            catch (Exception ex)
            {
                return Json(new FaildResult(ex.Message));
            }
        }

        [HttpPost]
        [Description("修改")]
        public object Update([FromBody]JObject obj)
        {
            try
            {
                int roleId = (int)obj["roleId"];
                var permission = obj["permission"].ToList();
                var dbPermission = db.ControllerActionRoleSet.Where(p => p.RoleId == roleId).ToList();

                permission.ForEach(p =>
                {
                    var caId = (int)p["ControllerActionId"];
                    
                    var car = dbPermission.SingleOrDefault(o => o.ControllerActionId== caId&&o.RoleId==roleId);
                    if (car == null)
                    {
                        car = new ControllerActionRoleSet()
                        {
                            ControllerActionId = (int)p["ControllerActionId"],
                            RoleId = roleId,
                            IsAllowed = Convert.ToBoolean(p["IsAllowed"])
                        };
                        db.ControllerActionRoleSet.Add(car);
                    }
                    else
                    {
                        car.IsAllowed = Convert.ToBoolean(p["IsAllowed"]);
                        db.ControllerActionRoleSet.Attach(car);
                        db.Entry(car).State = System.Data.Entity.EntityState.Modified;
                    }
                });

                return Json(new SuccessResult() { Data=db.SaveChanges()});
            }
            catch (Exception ex)
            {
                return new FaildResult(ex.Message);
            }


        }
    }
}
