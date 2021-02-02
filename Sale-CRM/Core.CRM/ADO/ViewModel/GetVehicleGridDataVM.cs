using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CRM.ADO.ViewModel
{
   public class GetVehicleGridDataVM
    {
        public string CusDesc { get; set; }
        public string BrandCode { get; set; }
        public string BrandDesc { get; set; }
        public string ProdCode { get; set; }
        public string ProdDesc { get; set; }
        public string VersionCode { get; set; }
        public string ColorCode { get; set; }
        public string Segment { get; set; }
        public string Qty { get; set; }
        public string InstallmentPlan { get; set; }
    }
}
