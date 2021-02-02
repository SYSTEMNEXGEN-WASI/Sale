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
    public class VehLocTransMethods
    {
        static bool IsSaved = false;
        static bool IsDeleted = false;
        static SqlParameter[] nullSqlParam = null;
        static DataTable dt = new DataTable();
        static string strAutoCode = string.Empty;
        static SysFunction sysfun = new SysFunction();
        //static SysFunctions sysfuns = new SysFunctions();

        static string ChasisNo = string.Empty;
        static string EngineNo = string.Empty;

        static Transaction ObjTrans = new Transaction();
        static SqlTransaction Trans;

        public static List<VehicleStockVM> Get_ChasisNoForVehicleTransLoc(string enquiryId, string dealerCode,ref string msg)
        {
            var Serializer = new JavaScriptSerializer();
            List<VehicleStockVM> lst = new List<VehicleStockVM>();
            try
            {
                SqlParameter[] sqlParam = {
                                    new SqlParameter("@DealerCode",dealerCode),//0
									new SqlParameter("@VehLoc",enquiryId)//1
									};

                dt = DataAccess.getDataTable("SP_Select_ChassisNo_For_VehLocTrans", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<VehicleStockVM>(dt);
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            return lst;
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
                dt = DataAccess.getDataTable("SP_Select_ChassisNoDetail_ForVehLocTrans", sqlParam, General.GetBMSConString());

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

        public static bool Insert_VehLocTransMaster(VehicleLocTransVM model, string dealerCode,ref string msg)
        {

            try
            {
                if (model.TransNo == "" || model.TransNo == null)
                {
                    strAutoCode = sysfun.AutoGen("VehicleLocTransMaster", "TransNo", DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MM/yyyy"), dealerCode);

                }
                else
                {

                    strAutoCode = model.TransNo;


                }
                SqlParameter[] param = {
                                 new SqlParameter("@DealerCode",dealerCode),//0
								 new SqlParameter("@TransNo",strAutoCode),//1
								 new SqlParameter("@TransDate",sysfun.SaveDate(model.TransDate)),//2								 
								 new SqlParameter("@FromLocCode",model.FromLocCode),//3								 
                                 new SqlParameter("@ToLocCode",model.ToLocCode),//4
                                 new SqlParameter("@UpdUser",AuthBase.UserId),//5
                                 new SqlParameter("@UpdDate",DateTime.Now.ToShortDateString()),//6
                                 new SqlParameter("@UpdTime",DateTime.Now.ToShortTimeString()),//7
                                 new SqlParameter("@UpdTerm",General.CurrentIP),//8
								 new SqlParameter("@Remarks",model.Remarks)//9
								 
                            };

                if (ObjTrans.BeginTransaction(ref Trans) == true)
                {
                    sysfun.ExecuteSP_NonQuery("SP_Insert_VehicleLocTransMaster", param, Trans);


                    IsSaved = true;
                }

            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            return IsSaved;
        }

        public static bool Insert_VehLocTransDetail(List<VehicleLocTransVM> model2, string dealerCode,ref string msg)
        {
            int count = 0;
            try
            {
                foreach (var item in model2)
                {
                    if (item.ChasisNo != null || item.ChasisNo != "")
                    {
                        SqlParameter[] param2 = {
                                 new SqlParameter("@DealerCode",dealerCode),//0
								 new SqlParameter("@TransNo",strAutoCode),//1
								 new SqlParameter("@BrandCode",item.BrandCode),//2								 
								 new SqlParameter("@ProdCode",item.ProdCode.Trim()),//3								 
								 new SqlParameter("@VersionCode",item.VersionCode.Trim()),//4
								 new SqlParameter("@EngineNo",item.EngineNo.Trim()),//5
								 new SqlParameter("@ChasisNo",item.ChasisNo.Trim()),//6						 
								 new SqlParameter("@RegNo",item.RegNo)//7
								 
							};

                        if (sysfun.ExecuteSP_NonQuery("SP_Insert_VehicleLocTransDetail", param2, Trans) == true && IsSaved == true)
                        {
                            string sql = "update VehicleStock set LocCode = '"+item.ToLocCode+"'  where" +
                                " BrandCode = '" + item.BrandCode + "' and ProdCode = '" + item.ProdCode + "' and VersionCode = '" + item.VersionCode + "' and EngineNo = '" + item.EngineNo + "'" +
                                " and ChasisNo = '" + item.ChasisNo + "' and DealerCode = '" + dealerCode + "'";

                            if(sysfun.ExecuteQuery_NonQuery(sql, Trans))
                            {
                                IsSaved = true;
                            }else
                            {
                                IsSaved = false;
                            }
                            
                        }
                        else
                        {
                            
                            IsSaved = false;
                        }
                    }
                    count++;
                }
                ObjTrans.CommittTransaction(ref Trans);

            }
            catch (Exception ex)
            {
                ObjTrans.RollBackTransaction(ref Trans);
                msg = ex.Message;
                IsSaved = false;
            }

            return IsSaved;
        }

        public static string Get_VehLocTransData(string enquiryId, string dealerCode, ref string msg)
        {
            string json = "";
            var Serializer = new JavaScriptSerializer();
            List<VehicleLocTransVM> lst = new List<VehicleLocTransVM>();
            try
            {
                SqlParameter[] sqlParam =
                {
                    new SqlParameter("@DealerCode",dealerCode),
                    new SqlParameter("@TransNo",enquiryId)
                };
                dt = DataAccess.getDataTable("SP_Select_VehicleLocTrans", sqlParam, General.GetBMSConString());
                

                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<VehicleLocTransVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            return json;
        }
    }
}
