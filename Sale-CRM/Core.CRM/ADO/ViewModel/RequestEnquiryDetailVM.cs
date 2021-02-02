using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CRM.ADO.ViewModel
{
    public class RequestEnquiryDetailVM
    {
        public string EnquiryDetID { get; set; }
        public string Enquiry_ID { get; set; }
        public string BrandCode { get; set; }
        public string ProdCode { get; set; }
            public string BrandDesc { get; set; }
        public string ProdDesc { get; set; }
        public string VersionCode { get; set; }
        public string ColorCode { get; set; }
        public string ColorDesc { get; set; }
        public string Qty { get; set; }
        public string RequiredDate { get; set; }
        public string Remarks { get; set; }
        public string Blocked { get; set; }
        public string BlockedDate { get; set; }
        public string BlockedBy { get; set; }
        public string PrimaryModel { get; set; }
        public string StatusCode { get; set; }
        public string IsDeleted { get; set; }
        public string DealerCode { get; set; }
        public string FurtherContact { get; set; }
        public string FurtherDate { get; set; }
        public string VersionDesc { get; set; }
    }
}
