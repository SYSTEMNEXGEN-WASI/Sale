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
    public class HoldVehicleMethods
    {
        static bool IsSaved = false;
        static bool IsDeleted = false;
        static SqlParameter[] nullSqlParam = null;
        static DataTable dt = new DataTable();
        static string strAutoCode = string.Empty;
        static SysFunction sysfun = new SysFunction();
        //static SysFunctions sysfuns = new SysFunctions();


        static Transaction ObjTrans = new Transaction();
        static SqlTransaction Trans;

        public static string Get_VehiclesForHold(string dealerCode)
        {
            string json = "";
            var Serializer = new JavaScriptSerializer();
            List<HoldVehicleVM> lst = new List<HoldVehicleVM>();
            try
            {
                SqlParameter[] sqlParam =
                {
                    new SqlParameter("@DealerCode",dealerCode)
                };
                dt = DataAccess.getDataTable("SP_GetVehiclesForHold", sqlParam, General.GetBMSConString());

                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<HoldVehicleVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return json;

        }


        public static bool Update_Vehicle(HoldVehicleVM model)
        {
            string temp;

            try
            {
                


                SqlParameter[] param = {
                                 new SqlParameter("@DealerCode",model.DealerCode),//0
								 new SqlParameter("@BrandCode",model.BrandCode),//1
								 new SqlParameter("@ProdCode",model.ProdCode),//2		 
								 new SqlParameter("@VersionCode",model.versionCode),//3
								 new SqlParameter("@ColorCode",model.ColorCode),//4
                                 new SqlParameter("@EngineNo",model.EngineNo),//5
								 new SqlParameter("@ChasisNo",model.ChasisNo),//6
								 new SqlParameter("@HoldBy",model.HoldBy),//7
								 new SqlParameter("@HoldTill",model.HoldTill),//8
								 new SqlParameter("@HoldAmount",model.HoldAmount),//9
                                 new SqlParameter("@Hold",model.HoldFlag),//9
                                  new SqlParameter("@HoldAt",model.HoldAt)//10
								 
							};               
                

                if (sysfun.ExecuteSP_NonQuery("SP_UpdateVehicleStockHold", param))
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
    }
}
