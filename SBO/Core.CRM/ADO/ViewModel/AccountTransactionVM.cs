using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CRM.ADO.ViewModel
{
   public class AccountTransactionVM
    {
        public string DealerCode { get; set; }
        public int ID { get; set; }
        public string TransactionCode { get; set; }
        public string TransactionDate { get; set; }
        public string CusCode { get; set; }
        public string CusDesc { get; set; }
        public string AccountCode { get; set; }
        public string InvType { get; set; }
        public string TrType { get; set; }
        public string Narration { get; set; }
        public string Reference { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal Balance { get; set; }
        public string Remarks { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime CreateTime { get; set; }
        public string CreateUser { get; set; }
        public string CreateTerm { get; set; }
        public DateTime UpdDate { get; set; }
        public DateTime UpdTime { get; set; }
        public string  UpdUser { get; set; }
        public string UpdTerm { get; set; }
        public string EmpDesc { get; set; }
        public string EngineNo { get; set; }
        public string ChasisNo { get; set; }
        public string ProdDesc { get; set; }
    }

    public class AccountResponseModel
    {
        public AccountResponseModel()
        {
            AccTransList = new List<AccountTransactionVM>();
        }

        public List<AccountTransactionVM> AccTransList { get; set; }

    }
}
