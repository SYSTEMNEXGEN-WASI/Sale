using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CRM.ADO.ViewModel
{
    public class DaliyExpenseMasterVM
    {
      public string DealerCode{get; set;}
      public string ExpInvNo{get; set;}
      public string ExpInvDate{get; set;}
      public string Remarks{get; set;}
      public string CBCode{get; set;}
      public string CBDesc { get; set; }        
      public string PayModeCode{get; set;}
      public string PayModeDesc { get; set; }
      public string InsNo{get; set;}
      public string InsDate{get; set;}
      public double InsAmount{get; set;}
      public string BankCode{get; set;}
      public string BankDesc { get; set; }
      public string Branch{get; set;}
      public double ExpTotalInvAmount {get; set;}
      public string DelFlag{get; set;}
      public string PostFlag{get; set;}
      public string VoucherNo{get; set;}
      public string VoucherFlag{get; set;}
      public string UpdUser{get; set;}
      public string UpdDate{get; set;}
      public string UpdTime{get; set;}
      public string UpdTerm{get; set;}
      public string CSCode{get; set;}
      public string CSDesc { get; set; }
    }
}
