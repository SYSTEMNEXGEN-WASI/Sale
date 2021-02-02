using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CRM.ADO.ViewModel
{
    public class ProdReceiptVM
    {
        public string DealerCode { get; set; }
        public string RecNo { get; set; }
        public string RecDate { get; set; }
        public string DocumentNo { get; set; }
        public string DelFlag { get; set; }
        public string UpdUser { get; set; }
        public string UpdDate { get; set; }
        public string UpdTime { get; set; }
        public string UpdTerm { get; set; }
        public string VehTypeCode { get; set; }
        public string VehRecCode { get; set; }
        public string Segment { get; set; }
        public string Category { get; set; }
        public string Usage { get; set; }
        public string VendorCode { get; set; }
        public string Remarks { get; set; }
        public string OEMAnnounceDate { get; set; }
        public string TransportID { get; set; }
        public float TotalVehicleAmount { get; set; }

    }
}
