using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CRM.ADO.ViewModel
{
    public class EnquiryDetailVM
    {
        public int? EnquiryDetID { get; set; }
        public string Enquiry_ID { get; set; }
        public string BrandCode { get; set; }
        public string BrandDesc { get; set; }
        public string ProdCode { get; set; }
        public string ProdDesc { get; set; }
        public string VersionCode { get; set; }
        public string ColorCode { get; set; }
        public string ColorDesc { get; set; }
        public int? Qty { get; set; }
        public string RequiredDate { get; set; }
        public string Remarks { get; set; }
       // public bool? Blocked { get; set; }
        public string BlockedDate { get; set; }
        public string BlockedBy  { get; set; }
        public string PrimaryModel  { get; set; }
        public string StatusCode  { get; set; }
       // public bool? IsDeleted  { get; set; }
        public string DealerCode  { get; set; }
        public string FurtherContact  { get; set; }
        public string FurtherDate { get; set; }
        public string LostDate { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string ResAddress { get; set; }
        public string NextFollowUpDate { get; set; }
        public string EnqDate { get; set; }
        public string NextFollowUpTime { get; set; }
        public string NextFollowupActionPlan { get; set; }
        public string NextFollowUpPurpose { get; set; }
        public string ReasonDesc { get; set; }

    }

    public class EnquiryDetailResponseModel
    {
        public EnquiryDetailResponseModel()
        {
            EnquiryDetailList = new List<EnquiryDetailVM>();
        }

        public List<EnquiryDetailVM> EnquiryDetailList { get; set; }

    }

}
