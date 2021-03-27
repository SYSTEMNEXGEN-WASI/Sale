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
    public class MonthlyCommisionMethods
    {

        static bool IsSaved = false;
        static bool IsDeleted = false;
        static SqlParameter[] nullSqlParam = null;
        static DataTable dt = new DataTable();
        static string strAutoCode = string.Empty;
        static SysFunction sysfun = new SysFunction();

        static Transaction ObjTrans = new Transaction();
        static SqlTransaction Trans;


        public static string Get_MonthlyCommisionForEmp(string sp , string Month, string dealerCode, string Type)
        {
            string json = "";
            var Serializer = new JavaScriptSerializer();
            List<MonthlyCommisionVM> lst = new List<MonthlyCommisionVM>();
            try
            {
                SqlParameter[] sqlParam = {
                                    new SqlParameter("@DealerCode",dealerCode),//0
									new SqlParameter("@Month",Month),//1
                                    new SqlParameter("@Type",Type)//2

									};

                dt = DataAccess.getDataTable(sp, sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<MonthlyCommisionVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return json;
        }

        public static string Get_MCDetailData(string enquiryId, string dealerCode)
        {
            string json = "";
            var Serializer = new JavaScriptSerializer();
            List<MonthlyCommisionVM> lst = new List<MonthlyCommisionVM>();
            try
            {
                SqlParameter[] sqlParam = {
                                    new SqlParameter("@DealerCode",dealerCode),//1
									new SqlParameter("@TransCode",enquiryId)//0
									
									};

                dt = DataAccess.getDataTable("SP_Select_MonthlyCommisionDetail", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<MonthlyCommisionVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return json;
        }
        public static bool Insert_MonthlyCommisionMaster(MonthlyCommisionVM model, ref string msg)
        {

            try
            {            


                if (string.IsNullOrEmpty(model.TransCode))
                {
                    if (sysfun.IsExist("CommMonth", model.CommMonth, "MonthlyCommision", model.DealerCode, " And Service = '" + model.Service + "'"))
                    {
                        msg = "Monthly Commision for the Month " + model.CommMonth + " for the selected Service is already exists ";

                        //return false;
                        strAutoCode = sysfun.GetNewMaxID("MonthlyCommision", "TransCode", 8, model.DealerCode);
                    }
                    else
                    {
                        strAutoCode = sysfun.GetNewMaxID("MonthlyCommision", "TransCode", 8, model.DealerCode);
                    }                    
                }
                else
                {
                    strAutoCode = model.TransCode;
                }


                SqlParameter[] param = {
                                 new SqlParameter("@DealerCode",model.DealerCode),//0
								 new SqlParameter("@TransCode",strAutoCode),//1
                                 new SqlParameter("@TransDate",sysfun.SaveDate(model.TransDate)),//1
								 new SqlParameter("@CommMonth",model.CommMonth),//2
								 new SqlParameter("@UpdUser",AuthBase.UserId),//6
                                 new SqlParameter("@UpdDate",DateTime.Now),//8
                                 new SqlParameter("@UpdTerm",General.CurrentIP),//7	                          
                                 new SqlParameter("@Remarks",model.Remarks),//11
                                 new SqlParameter("@Service",model.Service)//12					 
							};
                

                if (ObjTrans.BeginTransaction(ref Trans) == true)
                {
                    if (sysfun.ExecuteSP_NonQuery("SP_Insert_MonthlyCommision", param, Trans))
                    {
                        IsSaved = true;
                    }
                    else
                    {
                        return IsSaved;
                    }
                }

            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            return IsSaved;
        }

        public static bool Insert_MonthlyCommisionDetail(List<MonthlyCommisionVM> model2, ref string msg)
        {

            SqlDataReader dr = null;
            try
            {
                foreach (var item in model2)
                {
                    if (item.EmpCode != null)
                    {

       //                 SqlParameter[] param = {
       //                          new SqlParameter("@DealerCode",item.DealerCode),//0
							//	 new SqlParameter("@CommMonth",item.CommMonth),//1
       //                          new SqlParameter("@Service",item.Service),//2	
       //                          new SqlParameter("@EmpCode",item.EmpCode)//3						 
							//};

       //                 if (sysfun.ExecuteSP("SP_EmpExists",param,ref dr))
       //                 {
       //                     ObjTrans.RollBackTransaction(ref Trans);

       //                     msg = "Monthly Commision for the Employee " + dr["EmpName"].ToString() + " for the selected Service and Month is already exists. Data not inserted.";
       //                     dr.Close();
       //                     IsSaved = false;
       //                     return IsSaved;
       //                 }
       //                 else 
       //                 {
                            
       //                 }

                        SqlParameter[] param2 = {
                                 new SqlParameter("@DealerCode",item.DealerCode),//0
								 new SqlParameter("@TransCode",strAutoCode),//1
								 new SqlParameter("@EmpCode",item.EmpCode),//2								 
								 new SqlParameter("@ReferenceNo",item.ReferenceNo),//3								 
								 new SqlParameter("@CommisionAmount",item.CommisionAmount),//4
								 new SqlParameter("@ProdCode",item.ProdCode),//5
								 new SqlParameter("@BrandCode",item.BrandCode),//6
								 new SqlParameter("@VersionCode",item.VersionCode),//7
								 new SqlParameter("@ColorCode",item.ColorCode),//8
								 new SqlParameter("@CommPerc",item.CommPerc),//9
								 new SqlParameter("@CommisionCode",item.CommisionCode),//10
								 new SqlParameter("@TotalQty",item.TotalQty == null ? (object) DBNull.Value : item.TotalQty),//11
                                 new SqlParameter("@TotalAmt",item.TotalAmount),//12								 
							};

                        if (sysfun.ExecuteSP_NonQuery("SP_Insert_MonthlyCommisionDetail", param2, Trans) == true)
                        {
                            IsSaved = true;
                        }
                        else
                        {
                            ObjTrans.RollBackTransaction(ref Trans);
                            IsSaved = false;
                        }
                    }

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
    }
}
