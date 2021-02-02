using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CRM.ADO.ViewModel
{
 public class SaleTargetVM
    {
        public string DealerCode { get; set; }
        public string TargetYear { get; set; }
        public string TargetMonth { get; set; }
        public string BrandCode { get; set; }
        public string ProdCode { get; set; }
        public string VersionCode { get; set; }
        public string InvoiceTargetQty { get; set; }
        public string BookingTargetQty { get; set; }
        public string AllocationTargetQty { get; set; }
        public string UpdUser { get; set; }
        public string UpdDate { get; set; }
        public string UpdTime { get; set; }
        public string UpdTerm { get; set; }
        public string AutoNo { get; set; }
        public string BrandDesc { get; set; }
        public string VersionDesc { get; set; }
        public string ProdDesc { get; set; }
    }
}
