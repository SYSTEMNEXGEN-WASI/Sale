using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CRM.ADO.ViewModel
{
    public class RequestEventDetailVM
    {
        public string DealerCode { get; set; }
        public string Event_ID { get; set; }
        public string EventDate { get; set; }
        public string EventDesc { get; set; }
        public string AssignID { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int ExpBudget { get; set; }
        public int ActBudget { get; set; }
        public string Status { get; set; }
        public string Feedback { get; set; }
        public string EventType { get; set; }
        public string ProdCode { get; set; }
        public string VersionCode { get; set; }
        public string ColorCode { get; set; }
        public string BrandCode { get; set; }
        
    }
}
