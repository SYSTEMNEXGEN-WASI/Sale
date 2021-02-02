using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CRM.ADO.ViewModel
{
    public class CustomerInstallmentScheduleVM
    {
      public string DealerCode         {get; set;}
      public string RecNo              {get; set;}
      public string RecDate            {get; set;}
      public string CusCode            {get; set;}
      public string CusName            {get; set;}
      public string ProdCode           {get; set;}
      public string VersionCode        {get; set;}
      public string Color              {get; set;}
      public string InstallmentDate    {get; set;}
      public decimal PrincipleAtBegin   {get; set;}
      public decimal MonthlyInstallment {get; set;}
      public decimal Profit             {get; set;}
      public decimal PrincipleRepayment {get; set;}
      public decimal Receiveable        {get; set;}
      public decimal NoOfInstallment    {get; set;}
      public string ScheduleCreateDate {get; set;}
      public string SoftwareVersion    {get; set;}
      public string TransferStatus     {get; set;}
      public string ReturnNo           {get; set;}
      public string PlanID             {get; set;}
      public string ColorCode          {get; set;}
      public string PlanCode           {get; set;}
    }
}
