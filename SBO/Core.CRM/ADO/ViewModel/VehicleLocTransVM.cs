using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CRM.ADO.ViewModel
{
    public class VehicleLocTransVM
    {
      public string DealerCode{get; set;}
      public string TransNo{get; set;}
      public string BrandCode{get; set;}
        public string BrandDesc { get; set; }
        public string ProdCode{get; set;}
        public string ProdDesc { get; set; }
        public string VersionCode{get; set;}
      public string EngineNo{get; set;}
      public string ChasisNo{get; set;}
      public string RegNo{get; set;}       
      public string TransDate{get; set;}
      public string FromLocCode{get; set;}
        public string FromLocDesc { get; set; }
        public string ToLocCode{get; set;}
        public string ToLocDesc { get; set; }
        public string DelFlag{get; set;}
      public string UpdUser{get; set;}
      public string UpdDate{get; set;}
      public string UpdTime{get; set;}
      public string UpdTerm{get; set;}
      public string Remarks{get; set;}
    }

    public class VehLocTransDetailResponseModel
    {
        public VehLocTransDetailResponseModel()
        {
            VehLocTransDetailList = new List<VehicleLocTransVM>();
        }

        public List<VehicleLocTransVM> VehLocTransDetailList { get; set; }

    }
}
