using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CRM.ADO.ViewModel
{
    public class GuarantorVM
    {
           public string DealerCode{ get; set; }
           public string CusCode{ get; set; }
           public string GRCode{ get; set; }
           public string GRDesc{ get; set; }
           public string Address1{ get; set; }
           public string NIC{ get; set; }
           public string Phone1{ get; set; }
           public string Phone2{ get; set; }
           public string CellNo{ get; set; }
           public string Remarks{ get; set; }
           public string CountryCode{ get; set; }
           public string StateCode{ get; set; }
           public string CityCode{ get; set; }

        public string CountryDesc { get; set; }
        public string StateDesc { get; set; }
        public string CityDesc { get; set; }
        public string Active { get; set; }
    }

    public class GuarantorVMResponseModel
    {
        public GuarantorVMResponseModel()
        {
            GuarantorList = new List<GuarantorVM>();
        }

        public List<GuarantorVM> GuarantorList { get; set; }

    }
}
