using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CRM.ADO.ViewModel
{
    public class ReceiptDetailVM
    {

      public string DealerCode { get; set;}
      public string ReceiptNo { get; set;}
      public string ReceiptDate { get; set;}
      public string InstrumentTypeCode { get; set;}
        public string PayModeDesc { get; set; }
        
      public string InstrumentNo { get; set;}
      public string InstrumentDate { get; set;}
      public string CityCode { get; set;}
        public string CityDesc { get; set; }
        
      public string BankCode { get; set;}
        public string BankDesc { get; set; }
        
      public string Branch { get; set;}
      public string DrawnBankCode { get; set;}
      public string DrawnBranch { get; set;}
      public string CusCode { get; set;}
      public string AccountCode { get; set;}
      public string ReceiptAmount { get; set;}
      public string AdjustedAmount { get; set;}
      public string AdjustableAmount { get; set;}
      public string InvoiceNo { get; set;}
      public string ISFullAndFinal { get; set;}
      public string ISPenalty { get; set;}
      public string CreateDate { get; set;}
      public string CreateTime { get; set;}
      public string CreatedBy { get; set;}
      public string CreatedTerm { get; set;}
      public string UpdDate { get; set;}
      public string UpdTime { get; set;}
      public string UpdUser { get; set;}
      public string UpdTerm { get; set;}
      public string BookRefNo { get; set;}
      public string ProdCode { get; set;}
      public string VersionCode { get; set;}
      public string ColorDesc { get; set;}
      public string ProdDesc { get; set;}
      public string EmpCode { get; set;}
      public string EmpDesc { get; set;}
      public string TransferStatus { get; set;}
      public string SNO { get; set;}
      public string DepositSlipNo { get; set;}
      public string RealizeDate { get; set;}
      public string Status { get; set;}
      public string RefundNo { get; set;}
      public string PlanCode { get; set;}
      public string SlipRefNo { get; set;}
      public string ColorCode { get; set;}
      public string ID { get; set;}
      public string VehExpCode { get; set;}
        public string VehExpDesc { get; set; }
        
      public string ServicesAccountCode { get; set;}
      public string BrandCode { get; set;}
        public string TrType { get; set; }
    }

    public class ReceiptResponseModel
    {
        public ReceiptResponseModel()
        {
            ReceiptDetailList = new List<ReceiptDetailVM>();
        }

        public List<ReceiptDetailVM> ReceiptDetailList { get; set; }

    }
}
