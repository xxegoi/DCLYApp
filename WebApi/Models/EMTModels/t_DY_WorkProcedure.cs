//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApi.Models.EMTModels
{
    using System;
    using System.Collections.Generic;
    
    public partial class t_DY_WorkProcedure
    {
        public int FID { get; set; }
        public string FName { get; set; }
        public string FNumber { get; set; }
        public int FParentID { get; set; }
        public int FLogic { get; set; }
        public bool FDetail { get; set; }
        public bool FDiscontinued { get; set; }
        public int FLevels { get; set; }
        public string FFullNumber { get; set; }
        public string FClassTypeID { get; set; }
    }
}
