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
    public class VehicleUpdateMethods
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

        public static List<SelectListItem> GetInsuranceCompanies()
        {
            List<SelectListItem> item = new List<SelectListItem>();
            DataTable dt = new DataTable();
            List<StringNameValueClass> lst = new List<StringNameValueClass>();
            try
            {
                //var Serializer = new JavaScriptSerializer();
                SqlParameter[] sqlParam =
                {

                };
                dt = DataAccess.getDataTable("sp_2W_InsuranceCompanies_select", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<StringNameValueClass>(dt);
                }
                //json = Serializer.Serialize(lst);

                item = lst.Select(i => new SelectListItem()
                {
                    Value = i.Id.ToString(),
                    Text = i.Title
                }).ToList();

                item.Insert(0, new SelectListItem() { Value = "0", Text = "Select" });

            }
            catch (Exception ex)
            {

                //throw;
            }
            return item;
        }

        public static string GetVehicleDetail(string chassisNo, string dealerCode)
        {
            string json = "";
            List<SelectListItem> item = new List<SelectListItem>();

            var Serializer = new JavaScriptSerializer();
            List<VehicleStockVM> lst = new List<VehicleStockVM>();
            try
            {
                //var Serializer = new JavaScriptSerializer();
                SqlParameter[] sqlParam =
                {
                    new SqlParameter("@DealerCode",dealerCode),
                    new SqlParameter("@ChassisNo",chassisNo)
                };
                dt = DataAccess.getDataTable("SP_Select_VehicleStockByChassisNo", sqlParam, General.GetBMSConString());

                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<VehicleStockVM>(dt);
                }
                json = Serializer.Serialize(lst);

            }
            catch (Exception ex)
            {

                //throw;
            }
            return json;
        }

        public static bool Insert_VSMaster(VehicleStockVM model, string dealerCode)
        {

            try
            {
               
                SqlParameter[] param = {
                                 new SqlParameter("@DealerCode",dealerCode),//0
								 new SqlParameter("@ChasisNo",model.ChasisNo),//1
								 new SqlParameter("@EngineNo",model.EngineNo),//2								 
								 new SqlParameter("@CusCode",model.CusCode),//3								 
								 new SqlParameter("@BrandCode",model.BrandCode),//4
								 new SqlParameter("@ProdCode",model.ProdCode),//5
								 new SqlParameter("@ColorCode",model.ColorCode),//6
								 new SqlParameter("@RegNo",model.RegNo),//9
								 new SqlParameter("@RegDate",model.RegDate == null ? (object) DBNull.Value : model.RegDate),//10
								 new SqlParameter("@InvoiceNo",model.InvoiceNo),//11
								 new SqlParameter("@InvoiceDate",model.InvoiceDate == null ? (object) DBNull.Value : model.InvoiceDate),//12
								 new SqlParameter("@OrderNo",model.OrderNo),//13
								 new SqlParameter("@OrderDate",model.OrderDate == null ? (object) DBNull.Value : model.OrderDate),//14
								 new SqlParameter("@BookingRefNo",model.BookingRefNo),//15								 
								 new SqlParameter("@BookingRefDate",model.BookingRefDate == null ? (object) DBNull.Value : model.BookingRefDate),//16								 
								 new SqlParameter("@BookRecDate",model.BookRecDate == null ? (object) DBNull.Value : model.BookRecDate),//17
                                 new SqlParameter("@BookIssueDate",model.BookIssueDate == null ? (object) DBNull.Value : model.BookIssueDate),//18
								 new SqlParameter("@InsCompCode",model.InsCompCode),//19
								 new SqlParameter("@InsPolicyNo",model.InsPolicyNo),//20
								 new SqlParameter("@InsDate",model.InsDate == null ?(object) DBNull.Value : model.InsDate ),//21
                                 new SqlParameter("@TransportDelivery",model.TransportDelivery == null ?(object) DBNull.Value : model.TransportDelivery )//22

                            };

                //if (ObjTrans.BeginTransaction(ref Trans) == true)
                //{
                //    sysfun.ExecuteSP_NonQuery("SP_Update_VehicleStock", param, Trans);


                //    IsSaved = true;
                //}

                if (sysfun.ExecuteSP_NonQuery("SP_Update_VehicleStock", param))
                {

                    IsSaved = true;

                }

            }
            catch (Exception)
            {
                throw;
            }

            return IsSaved;
        }

        public static List<VehicleStockVM> GetDataFromSPWithDealerCode(string sp, string DealerCode)
        {
            List<SelectListItem> item = new List<SelectListItem>();

            List<VehicleStockVM> lst = new List<VehicleStockVM>();

            try
            {
                SqlParameter[] sqlParam =
                {
                     new SqlParameter("@DealerCode",DealerCode)
                };
                dt = DataAccess.getDataTable(sp, sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<VehicleStockVM>(dt);
                }

            }
            catch (Exception ex)
            {

                //throw;
            }
            return lst;
        }
    }
}
