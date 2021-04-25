using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CRM.ADO.ViewModel
{
    public class CSIVehicleGridDataVM
    {
        
        
        public string BrandCode { get; set; } //3
        public string ProdCode { get; set; } //4
        public string VersionCode { get; set; } //5
        public string ProdDesc { get; set; }
        public string ColorCode { get; set; } //6
        public string Color { get; set; } //7
        public string ChasisNo { get; set; } //8
        public string EngineNo { get; set; } //9
        public int Qty { get; set; } //10
        public decimal InstallmentPlan { get; set; } //11
        public float CostPrice { get; set; } //12
        public float SalePrice { get; set; } //13
        public string Segment { get; set; }
        public float Discount { get; set; } //14
        public float FreightCharges { get; set; } //15
        public float MarketRate { get; set; } //16
        public float Advance { get; set; } //17
        public float TotalAmount { get; set; } //18
        public string Type { get; set; } //19
        public string RecNo { get; set; } //20
        public string BrandDesc { get; set; } //20
        public string FactoryPrice { get; set; } //20
        public string ModelYear { get; set; } //20
        public string CusInvCode { get; set; } //20
        public string CusInvDesc { get; set; } //20

    }
}
