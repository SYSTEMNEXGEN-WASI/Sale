using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CRM.ADO.ViewModel
{
    [Serializable()]
    public class VehicleExchangeVM
    {
        public string DealerCode { get; set; }
        public string BuyingCode { get; set; }
        public string LocationCode { get; set; }

        public string BuyingDate { get; set; }
        public string BuyingMode { get; set; }
        public string RegNo { get; set; }
        public string KM { get; set; }

        public string BuyingPrice { get; set; }
        public string EvaluationCode { get; set; }
        public string EvaluationDate { get; set; }
        public string DelFlag { get; set; }
        public string UpdUser { get; set; }
        public string UpdDate { get; set; }
        public string UpdTime { get; set; }
        public string UpdTerm { get; set; }

        public string tdDate { get; set; }
        public string ChassisNo { get; set; }
        public string EngineNo { get; set; }
        public string BrandCode { get; set; }
        public string BrandDesc { get; set; }
        public string ProdCode { get; set; }
        public string ProdDesc { get; set; }
        public string ColorCode { get; set; }
        public string ColorDesc { get; set; }
        public string CusCode { get; set; }
        public string CellNo { get; set; }
        public string CusDesc { get; set; }
        public string VersionCode { get; set; }
        public string FatherHusName { get; set; }
        public string Address1 { get; set; }
        public string Phone1 { get; set; }
        public string VehicleCode { get; set; }
        public string SalesPerson { get; set; }
        public string BuyingPriceOffer { get; set; }
        public string RegDate { get; set; }
        public string CusTypeDesc { get; set; }
        public string CurrentMilage { get; set; }
        public string CustomerExpectedPrice { get; set; }

        public string ExchangeCode { get; set; }
        public string ExchangeDate { get; set; }
        public string ExchangeRemarks { get; set; }
        public string TypeOfBuying { get; set; }
        public string NewVehicleType { get; set; }
    }
}
