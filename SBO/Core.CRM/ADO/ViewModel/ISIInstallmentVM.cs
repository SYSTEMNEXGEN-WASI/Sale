using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CRM.ADO.ViewModel
{
    public class ISIInstallmentVM
    {
        public int NoOfInstallment { get; set; }
        public decimal DownPayment { get; set; }
        public decimal MonthlyInstallment { get; set; }
    }
}
