using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CRM.ADO.ViewModel
{
    public class VehicleDeliveryDetailVM
    {
      public string DealerCode  { get; set; }
      public string DeliveryNo  { get; set; }
      public string BrandCode   { get; set; }
      public string ProdCode    { get; set; }
      public string VersionCode { get; set; }
      public string EngineNo    { get; set; }
      public string ChasisNo    { get; set; }
      public string ExFactPrice { get; set; }
      public string ColorCode   { get; set; }
      public string ProdRecNo   { get; set; }
      public string LocCode     { get; set; }
      public string EnquiryNo { get; set; }
        public string StockType { get; set; }
        public string BookingNo { get; set; }
        public string DocumentNo { get; set; }

    }

    public class DODetailResponseModel
    {
        public DODetailResponseModel()
        {
            DODetailList = new List<VehicleDeliveryDetailVM>();
        }

        public List<VehicleDeliveryDetailVM> DODetailList { get; set; }

    }
}
