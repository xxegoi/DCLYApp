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
    
    public partial class ICStockBill
    {
        public string FBrNo { get; set; }
        public int FInterID { get; set; }
        public Nullable<short> FTranType { get; set; }
        public Nullable<System.DateTime> FDate { get; set; }
        public string FBillNo { get; set; }
        public string FUse { get; set; }
        public string FNote { get; set; }
        public Nullable<int> FDCStockID { get; set; }
        public Nullable<int> FSCStockID { get; set; }
        public Nullable<int> FDeptID { get; set; }
        public Nullable<int> FEmpID { get; set; }
        public Nullable<int> FSupplyID { get; set; }
        public Nullable<int> FPosterID { get; set; }
        public Nullable<int> FCheckerID { get; set; }
        public Nullable<int> FFManagerID { get; set; }
        public Nullable<int> FSManagerID { get; set; }
        public Nullable<int> FBillerID { get; set; }
        public Nullable<int> FReturnBillInterID { get; set; }
        public string FSCBillNo { get; set; }
        public Nullable<int> FHookInterID { get; set; }
        public Nullable<int> FVchInterID { get; set; }
        public short FPosted { get; set; }
        public Nullable<short> FCheckSelect { get; set; }
        public Nullable<int> FCurrencyID { get; set; }
        public Nullable<int> FSaleStyle { get; set; }
        public Nullable<int> FAcctID { get; set; }
        public short FROB { get; set; }
        public string FRSCBillNo { get; set; }
        public short FStatus { get; set; }
        public bool FUpStockWhenSave { get; set; }
        public bool FCancellation { get; set; }
        public int FOrgBillInterID { get; set; }
        public Nullable<int> FBillTypeID { get; set; }
        public Nullable<int> FPOStyle { get; set; }
        public Nullable<int> FMultiCheckLevel1 { get; set; }
        public Nullable<int> FMultiCheckLevel2 { get; set; }
        public Nullable<int> FMultiCheckLevel3 { get; set; }
        public Nullable<int> FMultiCheckLevel4 { get; set; }
        public Nullable<int> FMultiCheckLevel5 { get; set; }
        public Nullable<int> FMultiCheckLevel6 { get; set; }
        public Nullable<System.DateTime> FMultiCheckDate1 { get; set; }
        public Nullable<System.DateTime> FMultiCheckDate2 { get; set; }
        public Nullable<System.DateTime> FMultiCheckDate3 { get; set; }
        public Nullable<System.DateTime> FMultiCheckDate4 { get; set; }
        public Nullable<System.DateTime> FMultiCheckDate5 { get; set; }
        public Nullable<System.DateTime> FMultiCheckDate6 { get; set; }
        public Nullable<int> FCurCheckLevel { get; set; }
        public Nullable<int> FTaskID { get; set; }
        public Nullable<int> FResourceID { get; set; }
        public bool FBackFlushed { get; set; }
        public int FWBInterID { get; set; }
        public int FTranStatus { get; set; }
        public Nullable<int> FZPBillInterID { get; set; }
        public Nullable<int> FRelateBrID { get; set; }
        public int FPurposeID { get; set; }
        public System.Guid FUUID { get; set; }
        public int FRelateInvoiceID { get; set; }
        public byte[] FOperDate { get; set; }
        public Nullable<int> FImport { get; set; }
        public Nullable<int> FSystemType { get; set; }
        public int FMarketingStyle { get; set; }
        public int FPayBillID { get; set; }
        public Nullable<System.DateTime> FCheckDate { get; set; }
        public string FExplanation { get; set; }
        public string FFetchAdd { get; set; }
        public Nullable<System.DateTime> FFetchDate { get; set; }
        public int FManagerID { get; set; }
        public int FRefType { get; set; }
        public int FSelTranType { get; set; }
        public int FChildren { get; set; }
        public short FHookStatus { get; set; }
        public int FActPriceVchTplID { get; set; }
        public int FPlanPriceVchTplID { get; set; }
        public int FProcID { get; set; }
        public int FActualVchTplID { get; set; }
        public int FPlanVchTplID { get; set; }
        public Nullable<int> FBrID { get; set; }
        public int FVIPCardID { get; set; }
        public decimal FVIPScore { get; set; }
        public decimal FHolisticDiscountRate { get; set; }
        public string FPOSName { get; set; }
        public int FWorkShiftId { get; set; }
        public int FCussentAcctID { get; set; }
        public bool FZanGuCount { get; set; }
        public string FPOOrdBillNo { get; set; }
        public int FLSSrcInterID { get; set; }
        public Nullable<System.DateTime> FSettleDate { get; set; }
        public Nullable<int> FManageType { get; set; }
        public Nullable<int> FOrderAffirm { get; set; }
        public string FHeadSelfB0434 { get; set; }
        public string FHeadSelfA0140 { get; set; }
        public Nullable<int> FHeadSelfB0435 { get; set; }
        public string FHeadSelfA0139 { get; set; }
        public string FAddress { get; set; }
        public string FCarNo { get; set; }
        public string FAuditDate { get; set; }
        public string FHeadSelfA9736 { get; set; }
        public Nullable<int> FBillType { get; set; }
        public Nullable<int> FIsMeOut { get; set; }
        public string FHeadSelfb0152 { get; set; }
        public string FHeadSelfA0230 { get; set; }
        public string FHeadSelfA9737 { get; set; }
        public string FHeadSelfB0936 { get; set; }
        public Nullable<decimal> FPBWeight { get; set; }
        public string FBLCode { get; set; }
        public string FGH { get; set; }
        public Nullable<int> FCustid { get; set; }
        public string FColorNo { get; set; }
        public Nullable<int> FTS { get; set; }
        public Nullable<decimal> FWeight { get; set; }
        public string FSB { get; set; }
        public string FGYTJ { get; set; }
        public Nullable<decimal> FSL { get; set; }
        public Nullable<int> FColor { get; set; }
        public Nullable<int> FOutStockType { get; set; }
        public string FGYH1 { get; set; }
        public string FGYH2 { get; set; }
        public string FOrderNo { get; set; }
        public string FFundNo { get; set; }
        public string FZT { get; set; }
        public string FJD { get; set; }
        public string FYzkNote { get; set; }
        public string FBillRemark { get; set; }
        public Nullable<int> FCustomer { get; set; }
        public string FProcessType { get; set; }
        public string FColorNameOrID { get; set; }
        public string FChargeType { get; set; }
        public string FPBName { get; set; }
        public string FItemNameForPrint { get; set; }
        public Nullable<decimal> FLost { get; set; }
        public string FJZ { get; set; }
        public Nullable<int> FInEmployee { get; set; }
    }
}
