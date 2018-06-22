using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.EMTModels
{
    public class YzkTrackItem
    {
        public int FIndex { get; set; } 
        public string FName { get; set; }
        public DateTime? SendTime { get; set; }
        public DateTime? JieTime { get; set; }
        public int FWorkProcedure { get; set; }

        string state;

        public double Interval
        {
            get
            {
                if (this.SendTime != null && this.JieTime != null)
                    return (this.SendTime.Value - this.JieTime.Value).TotalHours;
                else
                    return 0;
            }
        }

        public string State
        {
            get
            {
                if (this.SendTime == null && this.JieTime == null)
                {
                    return "wait";
                }
                else if(this.SendTime!=null)
                {
                    return "success";
                }
                else 
                {
                    return "process";
                }
            }
        }

        double interval;
    }
}