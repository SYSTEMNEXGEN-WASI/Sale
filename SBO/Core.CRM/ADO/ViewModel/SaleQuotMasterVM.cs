using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CRM.ADO.ViewModel
{
    public class SaleQuotMasterVM
    {
    public string DealerCode    { get; set; }
	public string SaleQuotCode  { get; set; }
	public string SaleQuotDate  { get; set; }
	public string RefLetterNo   { get; set; }
	public string ValidDays     { get; set; }
	public string CustomerCode  { get; set; }
	public string Subject       { get; set; }
	public string PaymentTerms  { get; set; }
	public string Exemption     { get; set; }
	public string ForceJajeure  { get; set; }
	public string Warranty      { get; set; }
	public string DocGST        { get; set; }
	public string DocNTN        { get; set; }
    public string DocBrocher { get; set; }
    public string CusDesc  { get; set; }
    public string PhoneNo  { get; set; }
    public string Address1 { get; set; }
        public string CusType { get; set; }
    }
}
