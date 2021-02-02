using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CRM.ADO.ViewModel
{
    public class MonthlyCommisionVM
    {
        public string DealerCode { get; set; }
        public string TransCode { get; set; }
        public string TransDate { get; set; }
        public string CommisionCode { get; set; }
        public string CommMonth { get; set; }
        public string UpdUser { get; set; }
        public string UpdDate { get; set; }
        public string UpdTerminal { get; set; }
        public string Remarks { get; set; }
        public string Service { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public string ReferenceNo { get; set; }
        public string CommPerc { get; set; }
        public string CommisionAmount { get; set; }
        public string Paid { get; set; }
        public string ProdCode { get; set; }
        public string ProdDesc { get; set; }
        public string BrandCode { get; set; }
        public string BrandDesc { get; set; }
        public string ColorCode { get; set; }
        public string ColorDesc { get; set; }
        public string VersionCode { get; set; }
        public string TotalQty { get; set; }
        public string TotalAmount { get; set; }
    }

    public class MCDetailResponseModel
    {
        public MCDetailResponseModel()
        {
            MCDetailList = new List<MonthlyCommisionVM>();
        }

        public List<MonthlyCommisionVM> MCDetailList { get; set; }

    }
}
