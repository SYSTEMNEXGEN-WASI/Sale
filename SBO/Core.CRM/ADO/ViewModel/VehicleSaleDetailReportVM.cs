using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CRM.ADO.ViewModel
{
   public class VehicleSaleDetailReportVM
    {
        public string DealerCode { get; set;} //1
        
        public string SaleType { get; set; } //5
        public string CreditTerms { get; set; } //6
        public string EmpCode { get; set; } //7
        public string EmpName { get; set; }
        public string EngineNo { get; set; }
        public decimal InstallmentPlan { get; set; }
        public string CusCode { get; set; } //8 
        public string CusDesc { get; set; }
        public string CustomerType { get; set; } //9
        public string CNICNTN { get; set; }  //10
        public string Color { get; set; }
        public string ColorCode { get; set; } //22
        public string ColorDesc { get; set; } //23
        public string ContactNo { get; set; } //11
        public string PriceLevel { get; set; } //12
        public string ProdCode { get; set; } //20
        public string ProdDesc { get; set; }
        public int Qty { get; set; } 
        public string BillTo { get; set; } //13
        public string BrandCode { get; set; } //19
        public string BrandDesc { get; set; }
        public string ChassisNo { get; set; } //24
        public string ShipTo { get; set; } //14
        public int TotalQty { get; set; }
        public string TransCode { get; set; } //2
        public DateTime TransDate { get; set; } //3
        public string TransType { get; set; } //4
        public bool SameAs { get; set; } //15
         //16
        public string VersionCode { get; set; } //21
        
         

         
        
        
    }
}
