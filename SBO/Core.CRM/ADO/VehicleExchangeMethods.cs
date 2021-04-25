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
   public class VehicleExchangeMethods
    {
        static DataTable dt = new DataTable();
        static SysFunction sysfun = new SysFunction();
        static string strAutoCode = string.Empty;
        static string autoProspect_ID = string.Empty;
        static bool IsSaved = false;
        static bool IsDeleted = false;
        static SqlParameter[] nullSqlParam = null;
        static Transaction ObjTrans = new Transaction();
        static SqlTransaction Trans;
        public static List<VehicleExchangeVM> Get_RegNoDetailData(string dealerCode)
        {
            DataTable dt = new DataTable();
            string json = "";
            var Serializer = new JavaScriptSerializer();
            // string TransType = "CSI";
            //string SaleType = "Cash";


            List<VehicleExchangeVM> lst = new List<VehicleExchangeVM>();
            try
            {
                SqlParameter[] sqlParam = {
                                    new SqlParameter("@DealerCode",dealerCode)//0
                                    // , new SqlParameter("@TransType",TransType)//0
                                    //, new SqlParameter("@SaleType",SaleType)//0
									};

                dt = DataAccess.getDataTable("Select_Exchangedata", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<VehicleExchangeVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {
                throw;
            }

            return lst;
        }

        public static List<VehicleExchangeVM> Get_ExchangeVehicle(string dealerCode)
        {
            DataTable dt = new DataTable();
            string json = "";
            var Serializer = new JavaScriptSerializer();
            // string TransType = "CSI";
            //string SaleType = "Cash";


            List<VehicleExchangeVM> lst = new List<VehicleExchangeVM>();
            try
            {
                SqlParameter[] sqlParam = {
                                    new SqlParameter("@DealerCode",dealerCode)//0
                                   //, new SqlParameter("@ExchangeCode",ExchangeCode)//0
                                    //, new SqlParameter("@SaleType",SaleType)//0
									};

                dt = DataAccess.getDataTable("Select_ExchangeVehicle_Data", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<VehicleExchangeVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {
                throw;
            }

            return lst;
        }

        public static List<VehicleExchangeVM> Get_VehicleDetailData(string dealerCode)
        {
            DataTable dt = new DataTable();
            string json = "";
            var Serializer = new JavaScriptSerializer();
            // string TransType = "CSI";
            //string SaleType = "Cash";


            List<VehicleExchangeVM> lst = new List<VehicleExchangeVM>();
            try
            {
                SqlParameter[] sqlParam = {
                                    new SqlParameter("@DealerCode",dealerCode)//0
                                    // , new SqlParameter("@TransType",TransType)//0
                                    //, new SqlParameter("@SaleType",SaleType)//0
									};

                dt = DataAccess.getDataTable("Select_New_Vehicle", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<VehicleExchangeVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {
                throw;
            }

            return lst;
        }

        public static bool Insert_Exchange(VehicleExchangeVM model, string dealerCode)
        {

            try
            {

                if (string.IsNullOrEmpty(model.ExchangeCode))
                {

                    strAutoCode = sysfun.AutoGen("UCS_ExchangeVehicle", "ExchangeCode", DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MM/yyyy"), dealerCode);
                }
                else
                {
                    strAutoCode = model.ExchangeCode;
                }


                SqlParameter[] param = {
                                 new SqlParameter("@DealerCode         ",dealerCode),//0
								 new SqlParameter("@ExchangeCode         ",strAutoCode),//1
                                 new SqlParameter("@ExchangeDate         ",sysfun.SaveDate(model.ExchangeDate)),//3
                                 new SqlParameter("@LocationCode               ",model.LocationCode),
                                 new SqlParameter("@TypeOfBuying               ",model.BuyingMode),
                                 new SqlParameter("@BuyingCode              ",model.BuyingCode),//5
                                 new SqlParameter("@BrandCode                 ",model.BrandCode),//6
                                 new SqlParameter("@ColorCode        ",model.ColorCode ),//7
                                 new SqlParameter("@ProdCode          ",model.ProdCode ),//9
                                 new SqlParameter("@VersionCode      ",model.VersionCode          ),//10
                                 new SqlParameter("@ChassisNo      ",model.ChassisNo          ),//12
                                 new SqlParameter("@EngineNo     ",model.EngineNo             ),//13
                                 new SqlParameter("@SalesPerson              ",model.SalesPerson  ),//19
                                 new SqlParameter("@UpdUser             ",AuthBase.UserId),//24
                                 new SqlParameter("@UpdTerm            ",General.CurrentIP),//27
                                 new SqlParameter("@NewVehicleType          ",model.NewVehicleType ),//9
                                  new SqlParameter("@ExchangeRemarks          ",model.ExchangeRemarks )
                };
              
                
                    sysfun.ExecuteSP_NonQuery("Insert_ExchangeVehicle", param);


                    IsSaved = true;
                }


            
            catch (Exception)
            {

               throw;
            }

            return IsSaved;
        }

        public static bool Delete_ExchangeData(string enquiryId, string dealerCode)
        {
            DataSet ds = new DataSet();

            SqlParameter[] param = {
                new SqlParameter("@DealerCode",dealerCode),
                new SqlParameter("@ExchangeCode",enquiryId)
            };

            if (sysfun.ExecuteSP_NonQuery("sp_ExchangeData_Delete", param))
            {
                IsDeleted = true;
            }
            else
            {
                IsDeleted = false;
            }


            return IsDeleted;
        }


    }
}
