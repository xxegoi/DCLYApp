using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Common
{
    public abstract class BaseActionResult
    {
        public string Status { get; set; }
        public object Data { get; set; }
    }

    public class SuccessResult:BaseActionResult
    {
        public SuccessResult()
        {
            this.Status = "success";
        }
    }

    public class FaildResult : BaseActionResult
    {
        public FaildResult(string message)
        {
            this.Status = "fail";
            this.Data = message;
        }
    }
}