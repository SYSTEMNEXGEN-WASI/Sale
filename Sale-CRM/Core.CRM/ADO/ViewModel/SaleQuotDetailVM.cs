using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CRM.ADO.ViewModel
{
    public class SaleQuotDetailVM
    {

        public string DealerCode        { get; set; }
	    public string SaleQuotCode      { get; set; }
	    public string BrandCode         { get; set; }
        public string BrandDesc { get; set; }
        public string ProdCode          { get; set; }
        public string ProdDesc { get; set; }
        public string VersionCode       { get; set; }
	    public string ChasisNo          { get; set; }
	    public string EngineNo          { get; set; }
	    public string ColorCode1        { get; set; }
        public string ColorDesc { get; set; }
        public string ReqQty            { get; set; }
	    public string ExFactPrice       { get; set; }
	    public string SpecialDiscount   { get; set; }
        public string TotalAmt          { get; set; }
        public string Warranty { get; set; }
        public string FreightCharges { get; set; }
        public string VersionDesc { get; set; }
    }

    public class SQDetailResponseModel
    {
        public SQDetailResponseModel()
        {
            SQVehDetailList = new List<SaleQuotDetailVM>();
        }

        public List<SaleQuotDetailVM> SQVehDetailList { get; set; }

    }
}
