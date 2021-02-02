using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CRM.ADO.ViewModel
{
    public class RequestFollowUpDetailVM
    {
        public string FollowupId { get; set; }
        public string DealerCode { get; set; }
        public string EnquiryID { get; set; }
        public bool IsLostEnq { get; set; }
        public string LostReasonId { get; set; }
        public string LostDate { get; set; }
        public string NextFollowUpDate { get; set; }
        public string NextFollowUpTime { get; set; }
        public string NextFollowUpActionPlan { get; set; }
        public string NextFollowUpPurpose { get; set; }
        public string ReasonID { get; set; }
        public string EmpCode { get; set; }
        public string StatusType { get; set; }
        public int StatusTypeId { get; set; }
        public string TaskTypeCode { get; set; }
        
    }
}
