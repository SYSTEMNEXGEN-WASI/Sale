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
   
   public class DetailEvaluationMethods
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

        public static List<UCS_EvaluationVM> Get_PriceOfferNegociationData(string dealerCode)
        {
            DataTable dt = new DataTable();
            string json = "";
            var Serializer = new JavaScriptSerializer();
            // string TransType = "CSI";
            //string SaleType = "Cash";


            List<UCS_EvaluationVM> lst = new List<UCS_EvaluationVM>();
            try
            {
                SqlParameter[] sqlParam = {
                                    new SqlParameter("@DealerCode",dealerCode)//0
                                    // , new SqlParameter("@TransType",TransType)//0
                                    //, new SqlParameter("@SaleType",SaleType)//0
									};

                dt = DataAccess.getDataTable("Select_DetailEvaluationCode", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<UCS_EvaluationVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {
                throw;
            }

            return lst;
        }
        public static List<UCS_EvaluationVM> Get_DealFailYes(string dealerCode)
        {
            DataTable dt = new DataTable();
            string json = "";
            var Serializer = new JavaScriptSerializer();
            // string TransType = "CSI";
            //string SaleType = "Cash";


            List<UCS_EvaluationVM> lst = new List<UCS_EvaluationVM>();
            try
            {
                SqlParameter[] sqlParam = {
                                    new SqlParameter("@DealerCode",dealerCode)//0
                                    // , new SqlParameter("@TransType",TransType)//0
                                    //, new SqlParameter("@SaleType",SaleType)//0
									};

                dt = DataAccess.getDataTable("Select_DealFailYes", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<UCS_EvaluationVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {
                throw;
            }

            return lst;
        }

        public static bool Insert_DealFail(UCS_EvaluationVM model, ref string msg)
        {

            try
            {

                SqlParameter[] param2 = {
                                 new SqlParameter("@DealerCode",model.DealerCode),//0
								 new SqlParameter("@EvaluationCode",model.EvaluationCode),//1
								 new SqlParameter("@IsDealFail",model.IsDealFail),//2								 
								
                            };

                if (sysfun.ExecuteSP_NonQuery("sp_IsDealFail_Evaluation", param2))
                {
                    IsSaved = true;
                }
                else
                {

                    IsSaved = false;
                }

            }

            catch (Exception ex)
            {

                msg = ex.Message;
                IsSaved = false;
            }

            return IsSaved;
        }
    }
}
