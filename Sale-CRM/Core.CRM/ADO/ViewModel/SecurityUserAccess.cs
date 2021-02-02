using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CRM.ADO.ViewModel
{
    public partial class SecurityUserAccess
    {
        public long ID { get; set; }
        public string UserCode { get; set; }
        public string ApplicationCode { get; set; }
        public string FormCode { get; set; }
        public string FormActionCode { get; set; }
        public string ActionValue { get; set; }
    }
}
