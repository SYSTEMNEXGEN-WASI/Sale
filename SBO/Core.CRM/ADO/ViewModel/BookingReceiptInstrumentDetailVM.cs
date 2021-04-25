using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CRM.ADO.ViewModel
{
    public class BookingReceiptInstrumentDetailVM
    {
        public string InstrumentDetailID { get; set; }
        public string DealerCode{ get; set; }
        public string ReceiptNo{ get; set; }
        public string ReceiptDate { get; set; }
        public string InstrumentNo{ get; set; }
           public string InstrumentDate{ get; set; }
           public string InstrumentAmount{ get; set; }
           public string CityCode{ get; set; }
        public string CityDesc { get; set; }
        public string BankCode{ get; set; }
        public string BankDesc { get; set; }
        public string Branch{ get; set; }
           public string PaymentMode{ get; set; }
        public string PaymentModeDesc { get; set; }
        public string PaymentType{ get; set; }
        public string BookRefNo { get; set; }
    }

    public class BOPaymentDetailResponseModel
    {
        public BOPaymentDetailResponseModel()
        {
            BOPaymentDetailList = new List<BookingReceiptInstrumentDetailVM>();
        }

        public List<BookingReceiptInstrumentDetailVM> BOPaymentDetailList { get; set; }

    }
}
