using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CRM.ADO.ViewModel
{
   public class InstallmentPlanVM
    {
        public string DealerCode { get; set; }
        public int ID { get; set; }
        public string PlanID { get; set; }
        public string PlanType { get; set; }
        public string BrandCode { get; set; }
        public string ProdCode { get; set; }
        public string VersionCode { get; set; }
        public string ColorCode { get; set; }
        public string Color { get; set; }
        public string MonthlyInstallment { get; set; }
        public string DownPayment { get; set; }
        public int NoOfInstallment { get; set; }
        public decimal InstallmentPercentage { get; set; }
        public DateTime StartEffectiveDate { get; set; }
        public DateTime EndEffectiveDate { get; set; }
        public int Active { get; set; }
        public string TransferStatus { get; set; }
        public string Remarks { get; set; }
    }
}
