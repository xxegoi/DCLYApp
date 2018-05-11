using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Models.DYModels;
using WebApi.Common;
using System.Reflection;
using System.ComponentModel;

namespace WebApi.Controllers.DY
{
    [Description("权限管理")]
    public class ControllerActionController : DYApiController
    {
        [HttpGet]
        [Description("刷新API")]
        [AllowAnonymous]
        public object UpdateApi()
        {
            try
            {
                var asm = Assembly.GetExecutingAssembly();
                //反射获得程序集中的CONTROLLER ACTION，只反射父类为BaseApiController的CONTROLLER
                var types = asm.GetTypes().Where(p => p.BaseType != null && p.BaseType.BaseType != null && p.BaseType.BaseType == typeof(BaseApiController)).ToList();
                //读取数据库中CONTROLLER ACTION列表
                var cas = db.ControllerActionSet.ToList();

                //遍历数据库中的CONTROLLER ACTION列表
                cas.ForEach(p =>
                {
                    //如果程序集不存在此CONTROLLER，则删除这条记录
                    if (types.Where(c => c.Name == p.ControllerName).Count() == 0)
                    {
                        db.ControllerActionSet.Attach(p);
                        db.Entry(p).State = System.Data.Entity.EntityState.Deleted;
                    }
                    else
                    {
                        //如果程序集中存在此CONTROLLER但不存在此ACTION，则删除这条记录
                        var controller = types.SingleOrDefault(o => o.Name == p.ControllerName);
                        if (controller != null)
                        {
                            var methods = controller.GetMethods().Where(m => m.IsPublic && m.ReturnType == typeof(object)).ToList();

                            var action = methods.SingleOrDefault(a => a.Name == p.Name);
                            if (action == null)
                            {
                                db.ControllerActionSet.Attach(p);
                                db.Entry(p).State = System.Data.Entity.EntityState.Deleted;
                            }
                            else
                            {
                                p.ActionDisplayName = action.CustomAttributes.Where(o => o.AttributeType == typeof(DescriptionAttribute)).ToList()[0].ConstructorArguments.ToList()[0].Value.ToString();
                                p.ControllerDisplayName = controller.CustomAttributes.Where(o => o.AttributeType == typeof(DescriptionAttribute)).ToList()[0].ConstructorArguments.ToList()[0].Value.ToString();

                                db.ControllerActionSet.Attach(p);
                                db.Entry(p).State = System.Data.Entity.EntityState.Modified;
                            }
                        }
                    }
                });

                types.ForEach(p =>
                {
                    var methods = p.GetMethods().Where(m => m.IsPublic && m.ReturnType == typeof(object)).ToList();
                    methods.ForEach(a =>
                    {
                        var action = db.ControllerActionSet.SingleOrDefault(obj => obj.ControllerName == p.Name && obj.Name == a.Name);
                        if (action == null)
                        {
                            //如果此CONTROLLER  ACTION不存在于数据库中，则插入新记录到数据库中
                            if (cas.Count(c => c.ControllerName == p.Name && c.Name == a.Name) == 0)
                            {
                                ControllerActionSet ca = new ControllerActionSet()
                                {
                                    ControllerName = p.Name,
                                    Name = a.Name,
                                    //读取ACTION的Description属性值作为ACTION的显示名称
                                    ActionDisplayName = a.CustomAttributes.Where(o => o.AttributeType == typeof(DescriptionAttribute)).ToList()[0].ConstructorArguments.ToList()[0].Value.ToString(),
                                    //读取CONTROLLER的Description属性值作为CONTROLLER的显示名称
                                    ControllerDisplayName = p.CustomAttributes.Where(o => o.AttributeType == typeof(DescriptionAttribute)).ToList()[0].ConstructorArguments.ToList()[0].Value.ToString()
                                };
                                db.ControllerActionSet.Add(ca);
                            }
                        }
                    });

                });
                db.SaveChanges();
                return Json(new SuccessResult());

            }
            catch (Exception ex)
            {
                return Json(new FaildResult(ex.Message));
            }

        }
    }
}
