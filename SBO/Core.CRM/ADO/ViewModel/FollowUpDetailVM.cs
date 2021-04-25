using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CRM.ADO.ViewModel
{
    public class FollowUpDetailVM
    {
        public string FollowupId { get; set; }
        public string DealerCode { get; set; }
        public string EnquiryID { get; set; }
        public string PlanFollowupDate { get; set; }
        public string PlanFollowupTime { get; set; }
        public string PlanFollowupAction { get; set; }
        public string PlanFollowupPurpose { get; set; }
        public string PlanFollowupAssignTo { get; set; }
        public bool Schedule { get; set; }
        public string ActualFollowupDate { get; set; }
        public string ActualFollowupTime { get; set; }
        public string ActualFollowupAction { get; set; }
        public string ActualFollowupBy { get; set; }
        public string AfterFollowupEnqStatus { get; set; }
        public string ActualFollowupRemarks { get; set; }
        public bool IsLostEnq { get; set; }
        public string LostByDealer { get; set; }
        public string LostByModel { get; set; }
        public string LostReasonId { get; set; }
        public string LostDate { get; set; }
        public string NextFollowUpDate { get; set; }
        public string NextFollowUpTime { get; set; }
        public string NextFollowUpActionPlan { get; set; }
        public string NextFollowUpPurpose { get; set; }
        public string Createdby { get; set; }
        public bool IsDeleted { get; set; }
        public string TransferStatus { get; set; }
        public bool PartiallyLost { get; set; }
        public string EmpCode { get; set; }
        public string StatusTypeId { get; set; }
    }
}
