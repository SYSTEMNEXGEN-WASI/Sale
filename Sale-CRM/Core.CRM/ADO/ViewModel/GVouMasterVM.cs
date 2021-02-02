using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CRM.ADO.ViewModel
{
    public class GVouMasterVM
    {

      public string CompCode{get; set;}
      public string Booktype{get; set;}
      public string Journalno{get; set;}
      public string VouchNo{get; set;}
      public string SeqNo{get; set;}
      public string ContAccCode{get; set;}
      public string SubCode{get; set;}
      public string SubSubCode{get; set;}
      public string LocCode{get; set;}
      public string DetailCode{get; set;}
      public string VouchDate{get; set;}
      public string RecPay{get; set;}
      public string Narration01{get; set;}
      public string Narration02{get; set;}
      public string ChqBillNo{get; set;}
      public string ChqBillDate{get; set;}
      public string DebitAmt{get; set;}
      public string CreditAmt{get; set;}
      public string FYear{get; set;}
      public string TYear{get; set;}
      public string DelFlag{get; set;}
      public string BookNo{get; set;}
      public string AutoAcc{get; set;}
      public string Post{get; set;}
      public string Source{get; set;}
      public string AddUser{get; set;}
      public string AddDate{get; set;}
      public string AddTime{get; set;}
      public string AddTerm{get; set;}
      public string CSCode{get; set;}

    }

    public class GVouMasterResponseModel
    {
        public GVouMasterResponseModel()
        {
            GVouList = new List<GVouMasterVM>();
        }

        public List<GVouMasterVM> GVouList { get; set; }

    }
}
