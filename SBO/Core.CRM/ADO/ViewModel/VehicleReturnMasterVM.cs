using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CRM.ADO.ViewModel
{
    public class VehicleReturnMasterVM
    {
      public string DealerCode{ get; set; }
      public string VendorCode{ get; set; }
      public string ReturnCode{ get; set; }
      public string ReturnDate{ get; set; }
      public string Remarks{ get; set; }
      public string GatePass{ get; set; }
      public string DelFlag{ get; set; }
      public string UpdUser{ get; set; }
      public string UpdDate{ get; set; }
      public string UpdTime{ get; set; }
      public string UpdTerm{ get; set; }
      public string DealerCustomer{ get; set; }
      public string ReturnType{ get; set; }
        public string DocumentNo { get; set; }

        public string VendorCustomer { get; set; }
        
    }
}
