using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApi.Models.DYModels
{
    public class UserLoginViewModel
    {
        public string UserCode { get; set; }
        public string PassWord { get; set; }
    }

    public class UserViewModel
    {
        [Required]
        public string UserCode { get; set; }
        [Required]
        public string UserName { get; set; }
    }

    public class UserCreateViewModel:UserViewModel
    {
        [Required]
        public string PassWord { get; set; }
    }
}