using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CRM.ADO.ViewModel
{
   public class CrdSIVM
    {
        public int ServceQty { get; set; }        //-----
        public int TotalQty { get; set; }         //-----
        public string TransCode { get; set; }     //----
        public string TransDate { get; set; }   //----
        public string TransType { get; set; }     //-----
        public string BillTo { get; set; }        //-----
        public string ShipTo { get; set; }        //----
        public string CityDesc { get; set; }      //----
        public string CNICNTN { get; set; }       //----
        public string ContactNo { get; set; }     //-----
        public string CountryDesc { get; set; }   //----
        public string CreditTerms { get; set; }   //-----
        public string CusCode { get; set; }       //----
        public string CusDesc { get; set; }       //----
        public string CustomerType { get; set; }  //----
        public string DealerDesc { get; set; }
        public string EmpCode { get; set; }       //----
        public string EmpName { get; set; }       //----
        public float  GrandTtlAmount { get; set; } //------
        public string PriceLevel { get; set; }    //----
        public string RefDocumentNo { get; set; } //-----
        public string RefType { get; set; }       //----
        public string SaleType { get; set; }      //----
        public bool   SameAs { get; set; }          //------
       
        
        
        
    }
}
