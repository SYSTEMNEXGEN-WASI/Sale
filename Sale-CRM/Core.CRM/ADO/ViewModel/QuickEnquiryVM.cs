using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CRM.ADO.ViewModel
{
    public class QuickEnquiryVM
    {
        public string DealerCode { get; set; }
        public string Enquiry_ID { get; set; }
        public DateTime EnqDate { get; set; }
		public string ProspectID { get; set; }
        public string ProspectTitle { get; set; }
        public string EmpCode { get; set; }
        public string CashFinanced { get; set; }
        public string IsFinanced { get; set; }
        public string CreatedBy { get; set; }
		public string UpdUser { get; set; }
		public string UpdTerm { get; set; }
		public string BrandCode { get; set; }
        public string ProdCode { get; set; }
        public string VersionCode { get; set; }
        public string ColorCode { get; set; }
        public int Qty { get; set; }
		public string Name { get; set; }
		public string Mobile { get; set; }
    }
}
