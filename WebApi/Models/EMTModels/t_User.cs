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
    
    public partial class t_User
    {
        public int FUserID { get; set; }
        public string FName { get; set; }
        public string FSID { get; set; }
        public byte[] PasswordHashValue { get; set; }
        public System.Guid ID { get; set; }
        public short FPrimaryGroup { get; set; }
        public string FDescription { get; set; }
        public string FPortUser { get; set; }
        public Nullable<bool> FForbidden { get; set; }
        public byte[] FRight { get; set; }
        public Nullable<int> FEmpID { get; set; }
        public Nullable<int> FDataVokeType { get; set; }
        public Nullable<bool> FIsNeedOffline { get; set; }
        public Nullable<bool> FRightChanged { get; set; }
        public Nullable<bool> FOfflineRefeshEachTime { get; set; }
        public Nullable<int> FSafeMode { get; set; }
        public Nullable<int> FHRUser { get; set; }
        public string FSSOUsername { get; set; }
        public string FSCPwd { get; set; }
        public Nullable<System.Guid> UUID { get; set; }
        public Nullable<int> FAccessUUID { get; set; }
        public Nullable<System.DateTime> FUInValidDate { get; set; }
        public Nullable<System.DateTime> FPwCreateDate { get; set; }
        public Nullable<int> FPwValidDay { get; set; }
        public Nullable<int> FAuthRight { get; set; }
        public Nullable<int> FUserTypeID { get; set; }
        public byte[] FModifyTime { get; set; }
    }
}
