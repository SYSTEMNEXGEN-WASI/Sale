using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CRM.ADO.ViewModel
{
    public class VehicleSaleDetailVM
    {
        public string DealerCode { get; set; } //1
        public string TransCode { get; set; } //2
        public string BrandCode { get; set; } //3
        public string ProdCode { get; set; } //4
        public string VersionCode { get; set; } //5
        public string ColorCode { get; set; } //6
        public string ColorDesc { get; set; } //7
        public string ChassisNo { get; set; } //8
        public string EngineNo { get; set; } //9
        public int Qty { get; set; } //10
        public decimal InstallmentPlan { get; set; } //11
        public float FactoryPrice { get; set; } //12
        public float SalePrice { get; set; } //13
        public float Discount { get; set; } //14
        public float FreightCharges { get; set; } //15
        public float MarketRate { get; set; } //16
        public float Advance { get; set; } //17
        public float TotalAmount { get; set; } //18
        public string StockType { get; set; } //19
        public string RecNo { get; set; } //20
        public int SNO { get; set; }
        public string BrandDesc { get; set; } //3
    }
}
