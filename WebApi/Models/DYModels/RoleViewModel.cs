using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApi.Models.DYModels
{
    public class RoleViewModel:t_RoleSet
    {
        public List<string> Members { get; set; }
    }

    public class RoleCreateViewModel
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}