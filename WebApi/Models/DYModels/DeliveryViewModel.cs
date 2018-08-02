using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.DYModels
{
    public class DeliveryViewModel
    {
        public string FGH { get; set; }
        public Nullable<System.DateTime> FDate { get; set; }
        public string OrgName { get; set; }
        public string ItemName { get; set; }
        public string ItemModel { get; set; }
        public Nullable<System.DateTime> DXPlan { get; set; }
        public Nullable<System.DateTime> DXReality { get; set; }
        public double DXOutDays {
            get
            {
                var result = 0.0;

                if (this.DXPlan == null)
                    return result;
                else
                    if (this.DXReality == null)
                    result = Math.Max(0, (DateTime.Now - this.DXPlan).Value.TotalDays);
                else
                    result = Math.Max(0, (this.DXReality - this.DXPlan).Value.TotalDays);

                return Math.Round(result, 1);
            }
        }
        public Nullable<System.DateTime> YDPlan { get; set; }
        public Nullable<System.DateTime> YDReality { get; set; }
        public double YDOutDays {
            get
            {
                var result = 0.0;

                if (this.YDPlan == null)
                    return result;
                else
                    if (this.YDReality == null)
                    result = Math.Max(0, (DateTime.Now - this.YDPlan).Value.TotalDays);
                else
                    result = Math.Max(0, (this.YDReality - this.YDPlan).Value.TotalDays);

                return Math.Round(result, 1);
            }
        }
        public Nullable<System.DateTime> RSPlan { get; set; }
        public Nullable<System.DateTime> RSReality { get; set; }
        public double RSOutDays {
            get
            {
                var result = 0.0;

                if (this.RSPlan == null)
                    return result;
                else
                    if (this.RSReality == null)
                    result = Math.Max(0, (DateTime.Now - this.RSPlan).Value.TotalDays);
                else
                    result = Math.Max(0, (this.RSReality - this.RSPlan).Value.TotalDays);

                return Math.Round(result, 1);
            }
        }
        public Nullable<System.DateTime> RCPlan { get; set; }
        public Nullable<System.DateTime> RCReality { get; set; }
        public double RCOutDays {
            get
            {
                var result = 0.0;

                if (this.RCPlan == null)
                    return result;
                else
                    if (this.RCReality == null)
                    result= Math.Max(0, (DateTime.Now - this.RCPlan).Value.TotalDays);
                else
                    result= Math.Max(0, (this.RCReality - this.RCPlan).Value.TotalDays);

                return Math.Round(result, 1);
            }
        }
    }
}