using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CRM.ADO.ViewModel
{
    public class ISIInstallmentReceiptVM
    {
        public string TransCode { get; set; }
        public string TransDate { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public string EngineNo { get; set; }
        public string ChasisNo { get; set; }
        public string CusCode { get; set; }
        public string CusDesc { get; set; }
        public decimal MonthlyInstallment { get; set; }
        public int NoOfInstallment { get; set; }
        public string VersionCode { get; set; }

        public string ProdCode { get; set; }
        public string CellNo { get; set; }

        public string TotalReceiveable { get; set; }
        public string TotalActualReceived { get; set; }
        public string ActualTransDate { get; set; }

        public string BrandCode { get; set; }
        public string BrandDesc { get; set; }
        public string ColorDesc { get; set; }

        public string Discount { get; set; }

        public string Penalty { get; set; }



    }
}
