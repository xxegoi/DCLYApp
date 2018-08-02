using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;

namespace WebApi.Common
{
    public class LogHandeler
    {
        private static ILog info = LogManager.GetLogger("Info");
        private static ILog error = LogManager.GetLogger("ExceptionLog");

        public static void Info(string message)
        {
            info.Info(message);
        }

        public static void Error(string message)
        {
            error.Error(message);
        }
    }
}