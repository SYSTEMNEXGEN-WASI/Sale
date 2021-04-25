using Core.CRM.ADO.ViewModel;
using Core.CRM.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Core.CRM.ADO
{
  public  class PendingVoucherMethods
    {
        static bool IsSaved = false;
        static bool IsDeleted = false;
        static SqlParameter[] nullSqlParam = null;
        static DataTable dt = new DataTable();
        static string strAutoCode = string.Empty;
        static SysFunction sysfun = new SysFunction();
        static DateTime recDate = new DateTime();
        static Transaction ObjTrans = new Transaction();
        static SqlTransaction Trans;
        static int i = 1;

        public static List<PendingVoucherVM> GetVoucherCSIFlag(string dealerCode)
        {
            string json = "";
            var Serializer = new JavaScriptSerializer();
            List<PendingVoucherVM> lst = new List<PendingVoucherVM>();
            DataSet ds = new DataSet();
            try
            {
                string sql = "Select M.TransCode TransctionCode,V.ChassisNo ChassisNo,C.CusDesc Customer,convert(varchar(10),M.TransDate,105) TransctionDate,M.VoucherFlag from VehicleSaleMaster M inner join Customer C on C.CusCode=M.CusCode inner join VehicleSaleDetail V on V.TransCode=M.TransCode  where  M.DelFlag='N' and  VoucherFlag in('','N') and M.DealerCode = '" + dealerCode + "'";
                dt = sysfun.GetData(sql, "BMS0517ConnectionString");

                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<PendingVoucherVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return lst;
        }

        public static List<PendingVoucherVM> GetVoucherVehReceiptFlag(string dealerCode)
        {
            string json = "";
            var Serializer = new JavaScriptSerializer();
            List<PendingVoucherVM> lst = new List<PendingVoucherVM>();
            DataSet ds = new DataSet();
            try
            {
                string sql = "Select M.RecNo TransctionCode, V.ChasisNo ChassisNo , C.VendorDesc Customer , convert(varchar(10), M.RecDate, 105) TransctionDate,M.VoucherFlag from ProdRecMaster M inner join Vendor C on C.VendorCode = M.VendorCode inner join ProdRecDetail V on V.RecNo = M.RecNo where VoucherFlag in ('N','') and M.DelFlag in('N','') and M.DealerCode = '" + dealerCode + "' and M.Segment='2 WHEEL'";
                dt = sysfun.GetData(sql, "BMS0517ConnectionString");

                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<PendingVoucherVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return lst;
        }
        public static List<PendingVoucherVM> GetVoucherASRFlag(string dealerCode)
        {
            string json = "";
            var Serializer = new JavaScriptSerializer();
            List<PendingVoucherVM> lst = new List<PendingVoucherVM>();
            DataSet ds = new DataSet();
            try
            {
                string sql = "Select M.ReceiptNo TransctionCode,M.ChasisNo ChassisNo,convert(varchar(10),M.ReceiptDate,105) TransctionDate,C.CusDesc Customer,M.VoucherFlag from ReceiptMaster M inner join Customer C on C.CusCode = M.CusCode where M.DelFlag in('N','') and M.VoucherFlag = 'N' and M.DealerCode = '" + dealerCode+"'";
                dt = sysfun.GetData(sql, "BMS0517ConnectionString");

                if (dt.Rows.Count > 0)
             
      
                {
                    lst = EnumerableExtension.ToList<PendingVoucherVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return lst;
        }

    }
}
