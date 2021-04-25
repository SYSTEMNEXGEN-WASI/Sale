using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CRM.ADO.ViewModel
{
    public class ReversalVM
    {
      public string DealerCode{ get; set; }
      public string ReversalID{ get; set; }
      public string ReversalDate{ get; set; }
      public string TransactionNo{ get; set; }
      public string TrType{ get; set; }
      public string TransactionAmount{ get; set; }
      public string Remarks{ get; set; }
      public string UpdUser{ get; set; }
      public string UpdDate{ get; set; }
      public string UpdTime{ get; set; }
      public string UpdTerminal{ get; set; }
    }
}
