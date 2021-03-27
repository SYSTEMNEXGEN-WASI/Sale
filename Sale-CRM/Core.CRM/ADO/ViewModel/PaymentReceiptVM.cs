using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CRM.ADO.ViewModel
{
  public  class PaymentReceiptVM
    {
        public string DealerCode { get; set; }
        public string ReceiptNo { get; set; }
        public string ReceiptDate { get; set; }
        public string InvoiceType { get; set; }
        public string CusCode { get; set; }
        public string CusDesc { get; set; }
        public string InsCompCode { get; set; }
        public string BranchCode { get; set; }
        public string InsCusFlag { get; set; }
        public string Remarks { get; set; }
        public string PayModeCode { get; set; }
        public string InsNo { get; set; }
        public string InsDate { get; set; }
        public double AmountPaid { get; set; }
        public string BankCode { get; set; }
        public string Branch { get; set; }
        public string AdvanceAmount { get; set; }
        public double InvTotal { get; set; }
        public double OutSTTotal { get; set; }
        public double InvAdjTotal { get; set; }
        public string DelFlag { get; set; }
        public string UpdUser { get; set; }
        public System.DateTime UpdDate { get; set; }
        public System.DateTime UpdTime { get; set; }
        public string UpdTerm { get; set; }
        public string VoucherNo { get; set; }
        public string VoucherFlag { get; set; }
        public string AdvancePaid { get; set; }
        public string TransType { get; set; }
        public string IsAdjustAdvance { get; set; }
        public string AdvanceReceiptNo { get; set; }
        public string AdvanceAdjustedAmount { get; set; }
        public string AdvanceBalanceAmount { get; set; }
        public string DocumentNo { get; set; }
        public string RefundPayment { get; set; }
        public string RefundAmount { get; set; }
        public string Balance { get; set; }
    }
}
