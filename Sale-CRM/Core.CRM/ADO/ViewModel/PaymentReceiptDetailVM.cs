using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CRM.ADO.ViewModel
{
  public class PaymentReceiptDetailVM
    {
        public string DealerCode { get; set; }
        public string ReceiptNo { get; set; }
        public string InvoiceType { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public string InvAmount { get; set; }
        public string OutStAmount { get; set; }
        public string AdjAmount { get; set; }
        public string RefNo { get; set; }
        public string RefDate { get; set; }
        public string RefAmount { get; set; }
        public string OutStanding { get; set; }
    }
}
