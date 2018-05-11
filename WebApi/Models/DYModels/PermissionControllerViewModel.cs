using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.DYModels
{
    public class PermissionControllerViewModel
    {
        public int Id { get; set; }

        public int ControllerActionId { get; set; }

        public int RoleId { get; set; }

        public bool IsAllowed { get; set; }

        public string ControllerName { get; set; }

        public string ActionName { get; set; }
    }
}