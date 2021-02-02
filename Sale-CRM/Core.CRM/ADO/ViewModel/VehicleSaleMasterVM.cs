using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CRM.ADO.ViewModel
{
    public class VehicleSaleMasterVM
    {
        public string CusInvCode { get; set; }
        public string CusInvDesc { get; set; }
        public string DealerCode { get; set; } //1
        public int AutoTransNo { get; set; } //2
        public string TransCode { get; set; } //3
        public string TransDate { get; set; } //4
        public string TransType { get; set; } //5
        public string SaleType { get; set; } //6
        public string CreditTerms { get; set; } //7
        public string EmpCode { get; set; } //8\
        public string EmpName { get; set; } //8

        public string EmpDesc { get; set; }
        public string CusCode { get; set; } //9 
        public string CusDesc { get; set; } //9 
        public string CustomerType { get; set; } //10
        public string CNICNTN { get; set; }  //11
        public string ContactNo { get; set; } //12
        public string PriceLevel { get; set; } //13
        public string BillTo { get; set; } //14
        public string ShipTo { get; set; } //15
        public bool SameAs { get; set; } //16
        public int TotalQty { get; set; } //17
        public int ServceQty { get; set; } //18
        public float TotalAmount { get; set; } //19
        public string PaymentReceiptCode { get; set; } //20
        public float PaidAmoun { get; set; } //21
        public string DelFlag { get; set; } //22
        public string RefType { get; set; } //23
        public string RefDocumentNo { get; set; } //24
        public string PostFlag { get; set; } //25
        public string VoucherNo { get; set; } //26
        public string VoucherDate { get; set; } //27
        public string UpdUser { get; set; } //28
        public string UpdDate { get; set; } //29
        public string ProdDesc { get; set; }
        public string UpdTime { get; set; } //30
        public string UpdTerm { get; set; } //31
        public string ChassisNo { get; set; } //31
        public string ChasisNo { get; set; } //31
        public string EngineNo { get; set; } //31
        public string Prodtitle { get; set; } //31
        public string RecNo { get; set; }
        public string ProdCode { get; set; }
        public string Color { get; set; }
        public string VersionCode { get; set; }
        public string PlanID { get; set; }

        public string Discount { get; set; }
        public string MarketRate { get; set; }
        public string CityDesc { get; set; }
        public string CountryDesc { get; set; }
        public string FactoryPrice { get; set; }

    }
}
