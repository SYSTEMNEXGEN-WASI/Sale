using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CRM.ADO.ViewModel
{
    public class BookOrdVehDetailVM
    {
           public string DealerCode{ get; set; }
           public string BookRefNo{ get; set; }
           public string BrandCode{ get; set; }
        public string BrandDesc { get; set; }
        public string ProdCode{ get; set; }
        public string ProdDesc { get; set; }
        public string VersionCode{ get; set; }
           public string EngineNo { get; set; }
           public string ChasisNo { get; set; }
           public string ExFactPrice{ get; set; }
           public string Qty{ get; set; }
           public string ColorCode1{ get; set; }
        public string ColorDesc { get; set; }
        public string AdvanceTaxAmt{ get; set; }
           public string SpecialDiscount{ get; set; }
           public string TotalAmt{ get; set; }
		   public string ReceiptNo{ get; set; }
           public string Warranty { get; set; }
        public string InstalmentPlan { get; set; }
        public string EnquiryNo { get; set; }
        public string BookingNo { get; set; }
        public string CusCode { get; set; }
        public string VersionDesc { get; set; }
        public string LocCode { get; set; }
        public string LocDesc { get; set; }
        public string FID { get; set; }
        public string FDesc { get; set; }
        public float Frieght { get; set; }

    }

    public class BODetailResponseModel
    {
        public BODetailResponseModel()
        {
            BOVehDetailList = new List<BookOrdVehDetailVM>();
        }

        public List<BookOrdVehDetailVM> BOVehDetailList { get; set; }

    }
}
