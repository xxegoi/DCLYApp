using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.EMTModels
{
    public class YzkTrackViewModel
    {
        public string FGH { get; set; }
        public DateTime? FDate { get; set; }
        public string ItemName { get; set; }
        public decimal FQty { get; set; }
        public string OrgName { get; set; }
        public DateTime? FDeliveryDate { get; set; }
        public List<YzkTrackItem> WorkFlow { get; set; }
    }
}