//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApi.Models.DYModels
{
    using System;
    using System.Collections.Generic;
    
    public partial class ControllerActionSet
    {
        public int Id { get; set; }
        public bool IsAllowedNonRoles { get; set; }
        public bool IsAllowedAllRoles { get; set; }
        public string Name { get; set; }
        public string ControllerName { get; set; }
        public string Description { get; set; }
        public string ControllerDisplayName { get; set; }
        public string ActionDisplayName { get; set; }
        public bool IsApi { get; set; }
    }
}