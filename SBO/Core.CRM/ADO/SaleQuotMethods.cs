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
    public class SaleQuotMethods
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


        public static string GetSaleQuotModal(string dealerCode)
        {
            string json = "";
            List<SelectListItem> item = new List<SelectListItem>();

            var Serializer = new JavaScriptSerializer();
            List<SaleQuotMasterVM> lst = new List<SaleQuotMasterVM>();
            try
            {
                //var Serializer = new JavaScriptSerializer();
                SqlParameter[] sqlParam =
                {
                    new SqlParameter("@DealerCode",dealerCode)
                };
                dt = DataAccess.getDataTable("SP_Get_SaleQuotModal", sqlParam, General.GetBMSConString());

                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<SaleQuotMasterVM>(dt);
                }
                json = Serializer.Serialize(lst);

            }
            catch (Exception ex)
            {

                //throw;
            }
            return json;
        }

        public static string Get_SaleQuotData(string enquiryId, string dealerCode)
        {
            string json = "";
            var Serializer = new JavaScriptSerializer();
            List<SaleQuotMasterVM> lst = new List<SaleQuotMasterVM>();
            try
            {
                SqlParameter[] sqlParam = {
                                    new SqlParameter("@DealerCode",dealerCode),//1
									new SqlParameter("@SaleQuotCode",enquiryId)//0
									
									};

                dt = DataAccess.getDataTable("SP_Select_SaleQuotMaster", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<SaleQuotMasterVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return json;
        }

        public static string Get_ChassisDetail(string color, string brand, string product, string version, string dealerCode)
        {
            string json = "";
            List<SelectListItem> item = new List<SelectListItem>();

            var Serializer = new JavaScriptSerializer();
            DataTable dt = new DataTable();
            List<RequestVehicleReceiptVM> lst = new List<RequestVehicleReceiptVM>();
            try
            {
                //var Serializer = new JavaScriptSerializer();
                SqlParameter[] sqlParam =
                {
                    new SqlParameter("@DealerCode",dealerCode),
                    //new SqlParameter("@Brand",brand),
                    //new SqlParameter("@Product",product),
                    //new SqlParameter("@Version",version),
                    new SqlParameter("@ChassisNo",color),
                };
                dt = DataAccess.getDataTable("SP_SelectChassisQoutation", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<RequestVehicleReceiptVM>(dt);
                }
                json = Serializer.Serialize(lst);


            }
            catch (Exception ex)
            {

                throw;
            }
            return json;
        }


        public static string Get_SaleQuotDetailData(string enquiryId, string dealerCode)
        {
            string json = "";
            var Serializer = new JavaScriptSerializer();
            List<SaleQuotDetailVM> lst = new List<SaleQuotDetailVM>();
            try
            {
                SqlParameter[] sqlParam = {
                                    new SqlParameter("@DealerCode",dealerCode),//1
									new SqlParameter("@SaleQuotCode",enquiryId)//0
									
									};

                dt = DataAccess.getDataTable("SP_Select_SaleQuotDetail", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<SaleQuotDetailVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return json;
        }

        public static bool Insert_SQMaster(SaleQuotMasterVM model)
        {
            DateTime recDate;
            try
            {
                if (model.SaleQuotCode == "" || model.SaleQuotCode == null)
                {
                    strAutoCode = sysfun.AutoGen("SaleQuotMaster", "SaleQuotCode", DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MM/yyyy"), model.DealerCode);
                    //strAutoCode = sysfun.GetNewMaxID("ProdRecMaster", "RecNo", 8, "00166");
                    //recDate = DateTime.Parse( model.RecDate);
                }
                else
                {

                    strAutoCode = model.SaleQuotCode;
                    //string rec = sysfun.SaveDate( ).ToString();
                    //recDate = sysfun.SaveDate(model.BookRefDate);
                    //recDate = DateTime.Parse(rec);

                }
                SqlParameter[] param = {
                                 new SqlParameter("@DealerCode",model.DealerCode),//0
								 new SqlParameter("@SaleQuotCode",strAutoCode),//1
								 new SqlParameter("@SaleQuotDate",sysfun.SaveDate(model.SaleQuotDate)),//2								 
								 new SqlParameter("@RefLetterNo",model.RefLetterNo),//3								 
								 new SqlParameter("@ValidDays",model.ValidDays),//4
								 new SqlParameter("@CustomerCode",model.CustomerCode),//5
								 new SqlParameter("@Subject",model.Subject),//6
								 new SqlParameter("@UpdUser",AuthBase.UserId),//7
								 new SqlParameter("@UpdTerm",General.CurrentIP),//8
								 new SqlParameter("@PaymentTerms",model.PaymentTerms),//9
								 new SqlParameter("@Exemption",model.Exemption),//10
								 new SqlParameter("@ForceJajeure",model.ForceJajeure),//11
								 new SqlParameter("@Warranty",model.Warranty),//12
								 new SqlParameter("@DocGST",model.DocGST),//13
								 new SqlParameter("@DocNTN",model.DocNTN),//14
								 new SqlParameter("@DocBrocher",model.DocBrocher),//15
                            };

                if (ObjTrans.BeginTransaction(ref Trans) == true)
                {
                    sysfun.ExecuteSP_NonQuery("SP_Insert_SaleQuotMaster", param, Trans);


                    IsSaved = true;
                }

            }
            catch (Exception)
            {

                if (ObjTrans.BeginTransaction(ref Trans) == true)
                {
                    ObjTrans.RollBackTransaction(ref Trans);
                }
                
                throw;
            }

            return IsSaved;
        }

        public static bool Insert_SQDetail(List<SaleQuotDetailVM> model2, string dealerCode, ref string msg)
        {
            int count = 0;
            try
            {
                foreach (var item in model2)
                {
                    if (item.BrandCode != null || item.ChasisNo != null)
                    {
                        SqlParameter[] param2 = {
                                 new SqlParameter("@DealerCode",dealerCode),//0
								 new SqlParameter("@SaleQuotCode",strAutoCode),//1
								 new SqlParameter("@BrandCode",item.BrandCode),//2								 
								 new SqlParameter("@ProdCode",item.ProdCode.Trim()),//3								 
								 new SqlParameter("@VersionCode",item.VersionCode.Trim()),//4                            
								 new SqlParameter("@EngineNo",item.EngineNo == null ? string.Empty : item.EngineNo.Trim()),//5                                 
								 new SqlParameter("@ChasisNo",item.ChasisNo == null ? string.Empty : item.ChasisNo.Trim()),//6
								 new SqlParameter("@ExFactPrice",item.ExFactPrice.Trim()),//7
								 new SqlParameter("@ReqQty",item.ReqQty.Trim()),//8
								 new SqlParameter("@ColorCode1",item.ColorCode1),//9
								 new SqlParameter("@SpecialDiscount",item.SpecialDiscount.Trim()),//10
								 new SqlParameter("@TotalAmt",item.TotalAmt.Trim()),//11 
								 new SqlParameter("@Warranty",item.Warranty.Trim()),//12
                                 new SqlParameter("@FreightCharges",item.FreightCharges.Trim()),//12
                                 

                            };


                        if (sysfun.ExecuteSP_NonQuery("SP_Insert_SaleQuotDetail", param2, Trans) == true)
                        {
                            
                            IsSaved = true;
                        }
                        else
                        {
                            ObjTrans.RollBackTransaction(ref Trans);
                            return false;
                        }
                    }
                }


                ObjTrans.CommittTransaction(ref Trans);
            }
            catch (Exception ex)
            {
                
                ObjTrans.RollBackTransaction(ref Trans);

                msg = ex.Message;
            }

            return IsSaved;
        }
    }
}
