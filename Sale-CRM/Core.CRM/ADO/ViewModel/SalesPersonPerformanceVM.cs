using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CRM.ADO.ViewModel
{
    public class SalesPersonPerformanceVM
    {
        public int GenerateLeads { get; set; }
        public int Converted { get; set; }
        public int Lost { get; set; }
        public int FollowUp { get; set; }
        public int Task { get; set; }
        public int DeliveryOrder { get; set; }
        public int BookingOrder { get; set; }
        public int Stock { get; set; }
        public int PendingInvoice { get; set; }
        public int Invoice { get; set; }
        public int PendingPayment { get; set; }
        public int TotalHoldVehicle { get; set; }
        
    }
}
