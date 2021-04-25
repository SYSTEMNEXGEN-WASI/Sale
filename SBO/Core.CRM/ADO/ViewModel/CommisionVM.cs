using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CRM.ADO.ViewModel
{
    public class CommisionVM
    {
      public string DealerCode{ get; set; }
      public string CommisionCode{ get; set; }
        public string CommisionDate { get; set; }
        public string EmpCode{ get; set; }
        public string EmpDesc { get; set; }
        public string ProdCode{ get; set; }
      public string Service{ get; set; }
      public string CommPerc{ get; set; }
      public string CommAmount{ get; set; }
      public string UpdUser{ get; set; }
      public string UpdDate{ get; set; }
      public string UpdTerminal{ get; set; }
      public string DelFlag{ get; set; }
      public string Remarks{ get; set; }
        public string DesigCode { get; set; }
        public string DesigDesc { get; set; }
        public string BrandCode { get; set; }
        public string BrandDesc { get; set; }
        public string VersionCode { get; set; }
        public string ProdDesc { get; set; }

        public string ColorCode { get; set; }
        public string ColorDesc { get; set; }
    }
}
