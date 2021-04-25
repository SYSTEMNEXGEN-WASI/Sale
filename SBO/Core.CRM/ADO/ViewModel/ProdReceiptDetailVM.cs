using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CRM.ADO.ViewModel
{
    public class ProdReceiptDetailVM
    {
        public string DealerCode { get; set; }
        public string RecNo { get; set; }
        public string BrandCode { get; set; }
        public string ProdCode { get; set; }
        public string VersionCode { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public string RegNo { get; set; }
        public string RegDate { get; set; }
        public string ChasisNo { get; set; }
        public string EngineNo { get; set; }
        public string ColorCode { get; set; }
        public string StockType { get; set; }
        public string Insurance { get; set; }
        public string Milage { get; set; }
        public string CusCode { get; set; }
        public string CusDesc { get; set; }
        public string BookingNo { get; set; }
        public string LocCode { get; set; }
        public string BookRefNo { get; set; }
        public string ModelYear { get; set; }
        public string Amount { get; set; }
        public string RecDate { get; set; }
        public string VouchNo { get; set; }
        public string VoucherFlag { get; set; }
    }

    public class ProdRecDetailResponseModel
    {
        public ProdRecDetailResponseModel()
        {
            ProdRecDetailList = new List<ProdReceiptDetailVM>();
        }

        public List<ProdReceiptDetailVM> ProdRecDetailList { get; set; }

    }
}
