using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CRM.ADO.ViewModel
{
    public class CSIVoucherVM
    {
        public string AccountCode { get; set; }
        public string AccountTitle { get; set; }
        public string Narration { get; set; }
        public string VouchDate { get; set; }
        public string Debit { get; set; }
        public string Credit { get; set; }
        public string InstrumentNo { get; set; }
        public string InstrumentDate { get; set; }
        public string VendorPayable { get; set; }
        public string VehicleCode { get; set; }

        public string Journalno { get; set; }


    }

    //public class VoucherVMResponseModel
    //{
    //    public VoucherVMResponseModel()
    //    {
    //        VoucherList = new List<VoucherVM>();
    //    }

    //    public List<VoucherVM> VoucherList { get; set; }

    //}
}
