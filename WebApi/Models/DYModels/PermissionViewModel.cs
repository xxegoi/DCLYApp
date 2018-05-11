using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.DYModels
{
    public class PermissionViewModel
    {
        public int RoleId { get; set; }
        public int CaId { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public bool IsAllowed { get; set; }
    }
}