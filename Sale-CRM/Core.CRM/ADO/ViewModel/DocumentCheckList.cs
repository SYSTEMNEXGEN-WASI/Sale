using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CRM.ADO.ViewModel
{
    public class DocumentCheckList
    {
        public string DealerCode { get; set; }
        public string RefNo { get; set; }
        public string DocChkListCode { get; set; }
        public string DocChkListDesc { get; set; }
        public byte[] DocImage { get; set; }
        public string Image { get; set; }
    }
}
