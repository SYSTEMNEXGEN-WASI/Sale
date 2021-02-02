using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CRM.ADO.ViewModel
{
    public class VehicleTaxDetailVM
    {
      public string DealerCode      { get; set; } 
      public string BrandCode       { get; set; } 
      public string ProdCode        { get; set; } 
      public string VersionCode     { get; set; } 
      public string ColorCode       { get; set; }
        public string ColorDesc { get; set; }
        public string SalePrice       { get; set; } 
      public string Version         { get; set; } 
      public string VehicleStatus   { get; set; } 
      public string CostPrice       { get; set; } 
      public string WHTaxPerc       { get; set; } 
      public string WHTaxAmount     { get; set; } 
      public string FurTaxPerc      { get; set; } 
      public string ExtraTaxPerc    { get; set; } 
      public string RegTaxPerc      { get; set; } 
      public string FederalTaxPerc  { get; set; } 
      public string FederalTaxAmount { get; set; }  
      public string SEDTaxPerc      { get; set; } 
      public string SEDTaxAmount    { get; set; } 
      public string UpdUser         { get; set; } 
      public string UpdDate         { get; set; } 
      public string updTerm         { get; set; } 
      public string IsActive        { get; set; } 
      public string VehicleCode     { get; set; } 
      public string RegType { get; set; }

    }

    public class VehicleTaxDetailResponseModel
    {
        public VehicleTaxDetailResponseModel()
        {
            VehicleTaxDetailList = new List<VehicleTaxDetailVM>();
        }

        public List<VehicleTaxDetailVM> VehicleTaxDetailList { get; set; }

    }
}
