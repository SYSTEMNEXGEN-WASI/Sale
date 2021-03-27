using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CRM.ADO.ViewModel
{
   public class PaymentReceiptTaxDetailVM
    {
        public string DealerCode { get; set; }
        
        public string ReceiptNo { get; set; }
        public string ReceiptHead { get; set; }
        public string AccountCode { get; set; }
        public string Amount { get; set; }
        public string TaxID { get; set; }
        public string TaxPerc { get; set; }
    }
}
