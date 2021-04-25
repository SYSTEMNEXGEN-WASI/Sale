using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CRM.ADO.ViewModel
{
   public class BuyingPaymentVM
    {

        public string PaymentModeCode { get; set; }
        public string PaymentModeDesc { get; set; }
        public string BankCode { get; set; }
        public string BankDesc { get; set; }
        public string CityCode { get; set; }
        public string CityDesc { get; set; }
    }
}
