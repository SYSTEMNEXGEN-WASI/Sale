using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CRM.ADO.ViewModel
{
    public class DailyExpenseDetailVM
    {
      public string DealerCode{get; set;}
      public string ExpInvNo{get; set;}
      public string ExpFor{get; set;}
        public string ExpForDesc { get; set; }
      public string ExpPayTo{get; set;}
        public string ExpPayToDesc { get; set; }
        
      public string ExpRemarks{get; set;}
      public double Amount{get; set;}
        public string AccountCode { get; set; }
    }
}
