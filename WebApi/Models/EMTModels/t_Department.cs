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
    
    public partial class t_Department
    {
        public int FItemID { get; set; }
        public string FBrNO { get; set; }
        public Nullable<int> FManager { get; set; }
        public string FPhone { get; set; }
        public string FFax { get; set; }
        public string FNote { get; set; }
        public string FNumber { get; set; }
        public string FName { get; set; }
        public Nullable<int> FParentID { get; set; }
        public int FDProperty { get; set; }
        public Nullable<int> FDStock { get; set; }
        public short FDeleted { get; set; }
        public string FShortNumber { get; set; }
        public int FAcctID { get; set; }
        public int FCostAccountType { get; set; }
        public byte[] FModifyTime { get; set; }
        public int FCalID { get; set; }
        public Nullable<int> FPlanArea { get; set; }
        public int FOtherARAcctID { get; set; }
        public int FOtherAPAcctID { get; set; }
        public int FPreARAcctID { get; set; }
        public int FPreAPAcctID { get; set; }
        public bool FIsCreditMgr { get; set; }
    }
}