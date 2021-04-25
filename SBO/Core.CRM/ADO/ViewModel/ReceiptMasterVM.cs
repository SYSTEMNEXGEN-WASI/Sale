using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CRM.ADO.ViewModel
{
    public class ReceiptMasterVM
    {
        public string DealerCode { get; set; }
        public string ReceiptNo { get; set; }
        public string ReceiptDate { get; set; }
        public string CusCode { get; set; }
        public string CusDesc { get; set; }
        public string Remarks { get; set; }
        public string ISFullAndFinal { get; set; }
        public string FullAndFinalReceiveable { get; set; }
        public string FullAndFinalPenalty { get; set; }
        public string FullAndFinalTotalReceiveable { get; set; }
        public string FullAndFinalDiscount { get; set; }
        public string CreateDate { get; set; }
        public string CreateTime { get; set; }
        public string CreatedBy { get; set; }
        public string CreateTerm { get; set; }
        public string DelFlag { get; set; }
        public string PostFlag { get; set; }
        public string PostDate { get; set; }
        public string TransferStatus { get; set; }
        public string TransferDate { get; set; }
        public string SoftwareVersion { get; set; }
        public string CommunicationVersion { get; set; }
        public string EmpCode { get; set; }
        public string EmpDesc { get; set; }
        public string ProdDesc { get; set; }
        public string EngineNo { get; set; }
        public string ChasisNo { get; set; }
        public string Type { get; set; }
        public string OldReceiptNo { get; set; }
        public string InvoiceDate { get; set; }
        public string BalanceWithoutPenalty { get; set; }
        public string BalancePenalty { get; set; }

        public decimal CellNo { get; set; }
        public string TotalBalance { get; set; }
        public string LastActualTransDate { get; set; }
        public string LastPenaltyTransDate { get; set; }
        public string PrintCounter { get; set; }
        public string InvoiceNo { get; set; }
        public string PRBNo { get; set; }
        public string SONo { get; set; }
        public string FormName { get; set; }
        public string TransType { get; set; }
        public string SalePrice { get; set; }
        public string TotalAmount { get; set; }
        public string Discount { get; set; }
        public string MarketRate { get; set; }
        public string FreightCharges { get; set; }


        public int ID { get; set; }
        public decimal MonthlyInstallment { get; set; }
        public int NoOfInstallment { get; set; }
        public string VersionCode { get; set; }
        public string SNO { get; set; }

        public string TransDate { get; set; }

        public string ProdCode { get; set; }
        public string BrandCode { get; set; }
        public string BrandDesc { get; set; }
        public string ColorDesc { get; set; }
    }
}
