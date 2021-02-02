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
   public class BuyingCusVehicleMethods
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
        public static List<BuyingCusVehicleVM> Get_RegNoDetailData(string dealerCode)
        {
            DataTable dt = new DataTable();
            string json = "";
            var Serializer = new JavaScriptSerializer();
            // string TransType = "CSI";
            //string SaleType = "Cash";


            List<BuyingCusVehicleVM> lst = new List<BuyingCusVehicleVM>();
            try
            {
                SqlParameter[] sqlParam = {
                                    new SqlParameter("@DealerCode",dealerCode)//0
                                    // , new SqlParameter("@TransType",TransType)//0
                                    //, new SqlParameter("@SaleType",SaleType)//0
									};

                dt = DataAccess.getDataTable("Select_Buying_RegNo", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<BuyingCusVehicleVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {
                throw;
            }

            return lst;
        }

        public static List<BuyingCusVehicleVM> Get_BuyingData(string dealerCode)
        {
            DataTable dt = new DataTable();
            string json = "";
            var Serializer = new JavaScriptSerializer();
            // string TransType = "CSI";
            //string SaleType = "Cash";


            List<BuyingCusVehicleVM> lst = new List<BuyingCusVehicleVM>();
            try
            {
                SqlParameter[] sqlParam = {
                                    new SqlParameter("@DealerCode",dealerCode)//0
                                    // , new SqlParameter("@TransType",TransType)//0
                                    //, new SqlParameter("@SaleType",SaleType)//0
									};

                dt = DataAccess.getDataTable("Select_BuyingCode", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<BuyingCusVehicleVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {
                throw;
            }

            return lst;
        }

        public static List<BuyingPaymentVM> Get_BankDesc(string dealerCode)
        {
            DataTable dt = new DataTable();
            string json = "";
            var Serializer = new JavaScriptSerializer();
            // string TransType = "CSI";
            //string SaleType = "Cash";


            List<BuyingPaymentVM> lst = new List<BuyingPaymentVM>();
            try
            {
                SqlParameter[] sqlParam = {
                                    new SqlParameter("@DealerCode",dealerCode)//0
                                    // , new SqlParameter("@TransType",TransType)//0
                                    //, new SqlParameter("@SaleType",SaleType)//0
									};

                dt = DataAccess.getDataTable("Select_Buying_Payment_Bank", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<BuyingPaymentVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {
                throw;
            }

            return lst;
        }
    
        public static List<BuyingPaymentVM> Get_CityDesc(string dealerCode)
        {
            DataTable dt = new DataTable();
            string json = "";
            var Serializer = new JavaScriptSerializer();
            // string TransType = "CSI";
            //string SaleType = "Cash";


            List<BuyingPaymentVM> lst = new List<BuyingPaymentVM>();
            try
            {
                SqlParameter[] sqlParam = {
                                    new SqlParameter("@DealerCode",dealerCode)//0
                                    // , new SqlParameter("@TransType",TransType)//0
                                    //, new SqlParameter("@SaleType",SaleType)//0
									};

                dt = DataAccess.getDataTable("Select_Buying_Payment_City", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<BuyingPaymentVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {
                throw;
            }

            return lst;
        }
        public static string Get_DelChkList()
        {
            string json = "";
            var Serializer = new JavaScriptSerializer();
            List<DeliveryCheckListVM> lst = new List<DeliveryCheckListVM>();
            try
            {
                string sql = "Select A.DelChkListCode , A.DelChkListDesc , A.OptFlag from DelChkList A";

                dt = sysfun.GetData(sql);

                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<DeliveryCheckListVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return json;

        }
        public static string Get_ByingPayData(string EnquiryId,string dealerCode)
        {
            string json = "";
            var Serializer = new JavaScriptSerializer();
            List<BuyingCusVehicleVM> lst = new List<BuyingCusVehicleVM>();
            try
            {
                SqlParameter[] sqlParam = {
                      new SqlParameter("@DealerCode",dealerCode),//0
                       new SqlParameter("@BuyingCode",EnquiryId)//0

                                        };

                dt = DataAccess.getDataTable("SP_Get_BuyingPay", sqlParam, General.GetBMSConString());

                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<BuyingCusVehicleVM>(dt);
                }

                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return json;

        }

        public static bool Insert_Buying(BuyingCusVehicleVM model, string dealerCode)
        {

            try
            {

                if (string.IsNullOrEmpty(model.BuyingCode))
                {
                    if (sysfun.IsExist("RegNo", model.RegNo, "UCS_Buying", dealerCode))
                    {
                        return IsSaved;
                    }
                    strAutoCode = sysfun.AutoGen("UCS_Buying", "BuyingCode", DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MM/yyyy"), dealerCode);
                    //strAutoCode = sysfun.GetNewMaxID("UCS_Buying", "BuyingCode", 8, dealerCode);
                }
                else
                {
                    strAutoCode = model.BuyingCode;
                }


                SqlParameter[] param = {
                                 new SqlParameter("@DealerCode         ",dealerCode),//0
								 new SqlParameter("@BuyingCode         ",strAutoCode),//1
                                 new SqlParameter("@BuyingDate         ",sysfun.SaveDate(model.BuyingDate)),//3
                                 new SqlParameter("@RegNo              ",model.RegNo),//5
                                 new SqlParameter("@KM                 ",model.KM),//6
                                 new SqlParameter("@RoadTaxPaid        ",model.RoadTaxPaid ),//7
                                 new SqlParameter("@RoadTaxValidUpTo    ",sysfun.SaveDate(model.RoadTaxValidUpTo)            ),//8
                                 new SqlParameter("@IsInsured          ",model.IsInsured ),//9
                                 new SqlParameter("@InsuranceValidUpTo ",sysfun.SaveDate(model.InsuranceValidUpTo)           ),//10
                                 new SqlParameter("@BuyingPrice        ",Convert.ToDecimal(model.BuyingPrice)      ),//11
                                 new SqlParameter("@EvaluationCode      ",model.EvaluationCode          ),//12
                                 new SqlParameter("@EvaluationDate     ",sysfun.SaveDate(model.EvaluationDate)              ),//13
                                 new SqlParameter("@Pic01              ",model.Pic01  ),//19
                                 new SqlParameter("@Pic02              ",model.Pic02     ),//20
                                 new SqlParameter("@Pic03              ",model.Pic03          ),//21
                                 new SqlParameter("@Pic04              ",model.Pic04      ),//22
                                 new SqlParameter("@Pic05               ",model.Pic05),//23
								 new SqlParameter("@UpdUser             ",AuthBase.UserId),//24
                                 new SqlParameter("@UpdTerm            ",General.CurrentIP),//27
                                 new SqlParameter("@LocationCode               ",model.LocationCode),
                                 new SqlParameter("@BuyingMode               ",model.BuyingMode)                                
                };
                if (ObjTrans.BeginTransaction(ref Trans) == true)
                {
                    sysfun.ExecuteSP_NonQuery("Insert_Buying", param, Trans);


                    IsSaved = true;
                }
               

            }
            catch (Exception)
            {

                throw;
            }

            return IsSaved;
        }
        public static bool Insert_BCVPay_Detail(List<BuyingCusVehicleVM> model2,ref string msg )
        {

            try
            {
                foreach (var item in model2)
                {

                    SqlParameter[] param2 = {
                                 new SqlParameter("@DealerCode",item.DealerCode),//0
								 new SqlParameter("@BuyingCode",strAutoCode),//1
								 new SqlParameter("@InstrumentCode",item.InstrumentCode),//2								 
								 new SqlParameter("@InstrumentNo",item.InstrumentNo),//3								 
								 new SqlParameter("@InstrumentDate",sysfun.SaveDate(item.InstrumentDate)),//4
								 new SqlParameter("@InstrumentAmount",item.InstrumentAmount),//5
								 new SqlParameter("@CityCode",item.CityCode),//6
								 new SqlParameter("@BankCode",item.BankCode),//7
                                 new SqlParameter("@Branch",item.Branch),//8
								new SqlParameter("@UpdUser",AuthBase.UserId),//24
                                 new SqlParameter("@UpdTerm",General.CurrentIP),//27
								  new SqlParameter("@LocationCode",item.LocationCode)
                            };

                    if (sysfun.ExecuteSP_NonQuery("Insert_Buying_PayDetail", param2, Trans) == true)
                    {
                        IsSaved = true;
                    }
                    else
                    {
                        ObjTrans.RollBackTransaction(ref Trans);
                        IsSaved = false;
                    }

                }
                ObjTrans.CommittTransaction(ref Trans);
            }
            catch (Exception ex )
            {
                ObjTrans.RollBackTransaction(ref Trans);
                msg = ex.Message;
                IsSaved = false;
            }
            
            return IsSaved;
        }
        public static bool Insert_VehChkList(string strCheckedValues, string dealerCode)
        {

            try
            {
                SqlParameter[] param2 = {
                                 new SqlParameter("@DealerCode",dealerCode),//0
								 new SqlParameter("@BuyingCode",strAutoCode),//1
								 new SqlParameter("@BuyingDocumentCode",strCheckedValues)//2								 
								 
							};

                if (sysfun.ExecuteSP_NonQuery("SP_BuyingChkList_Insert", param2))
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
        public static bool Delete_BuyingData(string enquiryId, string dealerCode)
        {
            DataSet ds = new DataSet();

            SqlParameter[] param = {
                new SqlParameter("@DealerCode",dealerCode),
                new SqlParameter("@BuyingCode",enquiryId)
            };

            if (sysfun.ExecuteSP_NonQuery("sp_BuyingData_Delete", param))
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
