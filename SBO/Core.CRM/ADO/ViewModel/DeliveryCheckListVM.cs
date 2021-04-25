using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CRM.ADO.ViewModel
{
    public class DeliveryCheckListVM
    {
        public string DealerCode { get; set; }
        public string DelChkListCode { get; set; }
        public string DelChkListDesc { get; set; }
        public string BookChkListCode { get; set; }
        public string Type { get; set; }
        public string IsActive { get; set; }
        public string OptFlag { get; set; }
    }
}
