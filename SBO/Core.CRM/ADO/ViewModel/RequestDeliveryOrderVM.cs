using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CRM.ADO.ViewModel
{
    public class RequestDeliveryOrderVM
    {
           public string DeliveryNo   { get; set; }
           public string DeliveryDate { get; set; }
           public string BookRefNo    { get; set; }
           public string CusCode      { get; set; }
        public string CusDesc { get; set; }
        public string Type         { get; set; }
           public string ReceiverCode { get; set; }
           public string Remarks      { get; set; }
           public string Segment      { get; set; }
           public string Usage        { get; set; }
           public string VehTypeCode  { get; set; }
           public string CusContNo    { get; set; }
           public string BrandCode    { get; set; }
           public string ProdCode     { get; set; }
           public string VersionCode  { get; set; }
           public string EngineNo     { get; set; }
           public string ChasisNo     { get; set; }
           public string ColorCode    { get; set; }
           public string ProdRecNo    { get; set; }
           public string LocCode      { get; set; }
        public string StockType { get; set; }
        public string BookingNo { get; set; }
        public string DocumentNo { get; set; }
        public string EmpCode { get; set; }
    }
}
