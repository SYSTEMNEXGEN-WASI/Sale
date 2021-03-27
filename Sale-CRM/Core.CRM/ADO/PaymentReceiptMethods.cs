using Core.CRM.ADO.ViewModel;
using Core.CRM.Helper;
using System;
using System.Collections.Generic;
using System.Data;

using System.Data.SqlClient;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Core.CRM.ADO
{

    public class PaymentReceiptMethods
    {
        static bool IsSaved = false;
        static bool IsDeleted = false;
        static SqlParameter[] nullSqlParam = null;
        static DataTable dt = new DataTable();
        static string strAutoCode = string.Empty;
        static SysFunction sysfun = new SysFunction();
        static double strAdvBalAmt = 0;
        //static SysFunctions sysfuns = new SysFunctions();


        static Transaction ObjTrans = new Transaction();
        static SqlTransaction Trans;
        ///GET Pending CSI 
        ///
        public static string GetPendingCSIPay(string CusCode, string dealerCode)
        {
            string json = "";
            List<SelectListItem> item = new List<SelectListItem>();

            var Serializer = new JavaScriptSerializer();
            List<PaymentReceiptDetailVM> lst = new List<PaymentReceiptDetailVM>();
            try
            {
                //var Serializer = new JavaScriptSerializer();
                SqlParameter[] sqlParam =
                {
                    new SqlParameter("@DealerCode",dealerCode),
                     new SqlParameter("@CusCode",CusCode)
                };
                dt = DataAccess.getDataTable("SP_PendingPayment_CSI", sqlParam, General.GetBMSConString());

                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<PaymentReceiptDetailVM>(dt);
                }
                json = Serializer.Serialize(lst);

            }
            catch (Exception ex)
            {

                //throw;
            }
            return json;
        }
        public static string GetPendingACSPay(string CusCode, string dealerCode)
        {
            string json = "";
            List<SelectListItem> item = new List<SelectListItem>();

            var Serializer = new JavaScriptSerializer();
            List<PaymentReceiptDetailVM> lst = new List<PaymentReceiptDetailVM>();
            try
            {
                //var Serializer = new JavaScriptSerializer();
                SqlParameter[] sqlParam =
                {
                    new SqlParameter("@DealerCode",dealerCode),
                     new SqlParameter("@CusCode",CusCode)
                };
                dt = DataAccess.getDataTable("SP_PendingPayment_ACS", sqlParam, General.GetBMSConString());

                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<PaymentReceiptDetailVM>(dt);
                }
                json = Serializer.Serialize(lst);

            }
            catch (Exception ex)
            {

                //throw;
            }
            return json;
        }
        public static string GetReceiptNo_Model(string dealerCode)
        {
            string json = "";
            List<SelectListItem> item = new List<SelectListItem>();

            var Serializer = new JavaScriptSerializer();
            List<PaymentReceiptVM> lst = new List<PaymentReceiptVM>();
            try
            {
                //var Serializer = new JavaScriptSerializer();
                SqlParameter[] sqlParam =
                {
                    new SqlParameter("@DealerCode",dealerCode),
                   
                };
                dt = DataAccess.getDataTable("SP_Select_PaymentReceiptModal", sqlParam, General.GetBMSConString());

                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<PaymentReceiptVM>(dt);
                }
                json = Serializer.Serialize(lst);

            }
            catch (Exception ex)
            {

                //throw;
            }
            return json;
        }

        public static string GetAdvanceReceipt(string CusCode, string dealerCode)
        {
            string json = "";
            List<SelectListItem> item = new List<SelectListItem>();

            var Serializer = new JavaScriptSerializer();
            List<PaymentReceiptVM> lst = new List<PaymentReceiptVM>();
            try
            {
                //var Serializer = new JavaScriptSerializer();
                SqlParameter[] sqlParam =
                {
                    new SqlParameter("@DealerCode",dealerCode),
                     new SqlParameter("@CusCode",CusCode)
                };
                dt = DataAccess.getDataTable("SP_Select_AdvanceCall", sqlParam, General.GetBMSConString());

                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<PaymentReceiptVM>(dt);
                }
                json = Serializer.Serialize(lst);

            }
            catch (Exception ex)
            {

                //throw;
            }
            return json;
        }
        public static string GetReceiptNo_Master(string recNo,string dealerCode)
        {
            string json = "";
            List<SelectListItem> item = new List<SelectListItem>();

            var Serializer = new JavaScriptSerializer();
            List<PaymentReceiptVM> lst = new List<PaymentReceiptVM>();
            try
            {
                //var Serializer = new JavaScriptSerializer();
                SqlParameter[] dsMasterPram = { new SqlParameter("@DealerCode",dealerCode),
                                        new SqlParameter("@ReceiptNo",recNo), };
                dt = DataAccess.getDataTable("sp_W2_PaymentReceiptMaster_Select", dsMasterPram, General.GetBMSConString());

                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<PaymentReceiptVM>(dt);
                }
                json = Serializer.Serialize(lst);

            }
            catch (Exception ex)
            {

                //throw;
            }
            return json;
        }

        public static string GetReceiptNo_Detail(string recNo, string dealerCode)
        {
            string json = "";
            List<SelectListItem> item = new List<SelectListItem>();

            var Serializer = new JavaScriptSerializer();
            List<PaymentReceiptDetailVM> lst = new List<PaymentReceiptDetailVM>();
            try
            {
                //var Serializer = new JavaScriptSerializer();
                SqlParameter[] dsMasterPram = { new SqlParameter("@DealerCode",dealerCode),
                                        new SqlParameter("@ReceiptNo",recNo), };
                dt = DataAccess.getDataTable("sp_W2_PaymentReceiptDetail_Select", dsMasterPram, General.GetBMSConString());

                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<PaymentReceiptDetailVM>(dt);
                }
                json = Serializer.Serialize(lst);

            }
            catch (Exception ex)
            {

                //throw;
            }
            return json;
        }
        public static string GetReceiptNo_Tax(string recNo, string dealerCode)
        {
            string json = "";
            List<SelectListItem> item = new List<SelectListItem>();

            var Serializer = new JavaScriptSerializer();
            List<PaymentReceiptTaxDetailVM> lst = new List<PaymentReceiptTaxDetailVM>();
            try
            {
                //var Serializer = new JavaScriptSerializer();
                SqlParameter[] dsMasterPram = { new SqlParameter("@DealerCode",dealerCode),
                                        new SqlParameter("@ReceiptNo",recNo), };
                dt = DataAccess.getDataTable("sp_W2_PaymentReceiptTaxDetail_Select", dsMasterPram, General.GetBMSConString());

                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<PaymentReceiptTaxDetailVM>(dt);
                }
                json = Serializer.Serialize(lst);

            }
            catch (Exception ex)
            {

                //throw;
            }
            return json;
        }


        public static bool Insert_PaymentReceiptMaster(PaymentReceiptVM model,ref string msg)
        {
           
            

            try
            {
                if (model.ReceiptNo == "" || model.ReceiptNo == null)
                {
                    strAutoCode = sysfun.AutoGen("PaymentReceiptMaster", "ReceiptNo", DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MM/yyyy"), model.DealerCode);

                }
                else
                {

                    strAutoCode = model.ReceiptNo;

                }
                SqlParameter[] PmtRecMaster_param = {                                            
           /*0*/ new SqlParameter("@DealerCode",model.DealerCode),          /*1*/ new SqlParameter("@ReceiptNo",strAutoCode),
           /*2*/ new SqlParameter("@ReceiptDate",sysfun.SaveDate(model.ReceiptDate)),/*3*/ new SqlParameter("@InvoiceType",model.InvoiceType),
           /*4*/ new SqlParameter("@CusCode",model.CusCode),          /*5*/ new SqlParameter("@InsCompCode",model.InsCompCode),
           /*6*/ new SqlParameter("@BranchCode",model.Branch),          /*7*/ new SqlParameter("@InsCusFlag","N"),
           /*8*/ new SqlParameter("@Remarks",model.Remarks),           /*9*/ new SqlParameter("@PayModeCode",model.PayModeCode),
           /*10*/ new SqlParameter("@InsNo",model.InsNo),          /*11*/ new SqlParameter("@InsDate",sysfun.SaveDate(model.InsDate)),
           /*12*/ new SqlParameter("@AmountPaid",SysFunction.CustomCDBL(model.AmountPaid)),          /*13*/ new SqlParameter("@BankCode",model.BankCode),
           /*14*/ new SqlParameter("@Branch",model.Branch),         /*15*/ new SqlParameter("@AdvanceAmount",SqlDbType.Float),
           /*16*/ new SqlParameter("@InvTotal",SysFunction.CustomCDBL(model.InvTotal)),            /*17*/ new SqlParameter("@OutSTTotal",SysFunction.CustomCDBL(model.OutSTTotal)),
           /*18*/ new SqlParameter("@InvAdjTotal",SysFunction.CustomCDBL(model.InvAdjTotal)),         /*19*/ new SqlParameter("@DelFlag","N"),
           /*20*/ new SqlParameter("@UpdUser",AuthBase.UserId),           /*21*/ new SqlParameter("@UpdTerm",General.CurrentIP),          
           /*22*/ new SqlParameter("@VoucherNo",""),      /*23*/ new SqlParameter("@VoucherFlag","N"),
           /*24*/ new SqlParameter("@AdvancePaid",SqlDbType.Decimal),       /*25*/ new SqlParameter("@TransType",model.TransType),
           /*26*/ new SqlParameter("@IsAdjustAdvance",model.IsAdjustAdvance),
           /*27*/ new SqlParameter("@AdvanceReceiptNo",SqlDbType.Char,8),
           /*28*/ new SqlParameter("@AdvanceAdjustedAmount",SqlDbType.Float),
           /*29*/ new SqlParameter("@AdvanceBalanceAmount",SqlDbType.Float),
           /*30*/ new SqlParameter("@DocumentNo","")
            };
                if (model.TransType == "Advance")
                {
                    PmtRecMaster_param[15].Value = SysFunction.CustomCDBL(model.AmountPaid);
                    PmtRecMaster_param[16].Value = "0";
                    PmtRecMaster_param[17].Value = "0";
                    PmtRecMaster_param[29].Value = SysFunction.CustomCDBL(model.AmountPaid);
                }
                strAdvBalAmt = SysFunction.CustomCDBL(sysfun.GetStringValuesAgainstCodes("ReceiptNo", model.AdvanceReceiptNo, "AdvanceBalanceAmount", "PaymentReceiptMaster", "", model.DealerCode));
                if (SysFunction.CustomCDBL(model.AdvanceAmount) > strAdvBalAmt)
                {
                    msg= "Advance Amount can not be greater then Balance amount";
                    return false;
                }
                if (model.TransType == "Advance")
                {
                    PmtRecMaster_param[26].Value = "Y";
                    PmtRecMaster_param[27].Value = (object)DBNull.Value;
                    PmtRecMaster_param[28].Value =SysFunction.CustomCDBL("0");
                    PmtRecMaster_param[29].Value = SysFunction.CustomCDBL(model.AmountPaid);
                }
                else if (model.IsAdjustAdvance=="Y" && model.TransType != "Advance")
                {
                    PmtRecMaster_param[26].Value = "Y";
                    PmtRecMaster_param[27].Value = model.AdvanceReceiptNo;
                    PmtRecMaster_param[28].Value = SysFunction.CustomCDBL(model.AdvanceAmount);
                    PmtRecMaster_param[29].Value = strAdvBalAmt - SysFunction.CustomCDBL(model.AdvanceAmount);
                }
                else
                {
                    PmtRecMaster_param[26].Value = "N";
                    PmtRecMaster_param[27].Value = (object)DBNull.Value;
                    PmtRecMaster_param[28].Value = (object)DBNull.Value;
                    PmtRecMaster_param[29].Value = (object)DBNull.Value;
                }
                if (ObjTrans.BeginTransaction(ref Trans) == true)
                {
                    if (sysfun.ExecuteSP_NonQuery("sp_W2_PaymentReceipt_Master_Insert", PmtRecMaster_param, Trans))
                    {
                        if (model.IsAdjustAdvance == "Y")
                        {
                            SqlParameter[] UpdateAdvance_param = {                                            
                                                            /*0*/ new SqlParameter("@DealerCode",model.DealerCode),
                                                            /*1*/ new SqlParameter("@ReceiptNo",model.AdvanceReceiptNo),
                                                            /*2*/ new SqlParameter("@AdvanceReceiptNo",strAutoCode),
                                                            /*3*/ new SqlParameter("@AdvanceAdjustedAmount",SysFunction.CustomCDBL(model.AdvanceAmount)),
                                                            /*3*/ new SqlParameter("@AdvanceBalAmount",SqlDbType.Float)
                                                                    };


                            UpdateAdvance_param[4].Value = SysFunction.CustomCDBL(strAdvBalAmt) - SysFunction.CustomCDBL(model.AdvanceAmount);
                            sysfun.ExecuteSP_NonQuery("[sp_PaymentReceiptMaster_UpdateOnAdvance]", UpdateAdvance_param, Trans);
                            
                        }
                        IsSaved = true;


                    }
                    
                }
            }
            catch (Exception ex)
            {
                ObjTrans.RollBackTransaction(ref Trans);
                msg=ex.Message;
            }

            return IsSaved;
        }
        public static bool Insert_PaymentDetail(List<PaymentReceiptDetailVM> modelDetail,string DealerCode, ref string msg)
        {
            // var code = "MCM01";
            //SysFunction Sys = new SysFunction();
            //SysFunction sys = new SysFunction();

           


            try
            {
                if (modelDetail != null)
                {
                    foreach (var item in modelDetail)
                    {

                        SqlParameter[] PmtRecDetail_param = {                                            
                                                            /*0*/ new SqlParameter("@DealerCode",DealerCode),
                                                            /*1*/ new SqlParameter("@ReceiptNo",strAutoCode),
                                                            /*2*/ new SqlParameter("@InvoiceType",item.InvoiceType),
                                                            /*3*/ new SqlParameter("@InvoiceNo",item.InvoiceNo),
                                                            /*4*/ new SqlParameter("@InvoiceDate",sysfun.SaveDate(item.InvoiceDate)),
                                                            /*5*/ new SqlParameter("@InvAmount",SysFunction.CustomCDBL(item.InvAmount)),
                                                            /*6*/ new SqlParameter("@OutStAmount",SysFunction.CustomCDBL(item.OutStAmount)),
                                                            /*7*/ new SqlParameter("@AdjAmount",SysFunction.CustomCDBL(item.AdjAmount))
                                                        };


                        if (sysfun.ExecuteSP_NonQuery("sp_W2_PaymentReceipt_Detail_Insert", PmtRecDetail_param, Trans) == true)
                        {
                            if (item.InvoiceType == "CSI")
                            {
                                string IQuery = "Update VehicleSaleMaster set PaidAmount= isnull(PaidAmount,0) +'" + SysFunction.CustomCDBL(item.AdjAmount) + "' " +
                                                                   "Where DealerCode='" + DealerCode + "' and TransCode='" + item.InvoiceNo + "'";
                                sysfun.ExecuteQuery(IQuery, Trans);
                            }
                            else if (item.InvoiceType == "ASC")
                            {
                                string IQuery = "Update ReceiptMaster set PaidC= PaidC +'" + SysFunction.CustomCDBL(item.AdjAmount) + "' " +
                                                                   "Where DealerCode='" + DealerCode + "' and ReceiptNo='" + item.InvoiceNo + "'";
                                sysfun.ExecuteQuery(IQuery, Trans);
                            }

                            IsSaved = true;
                        }



                    }
                }
                else
                {
                    ObjTrans.CommittTransaction(ref Trans);
                    IsSaved = true;
                }
               
            }
            catch (Exception ex)
            {
                ObjTrans.RollBackTransaction(ref Trans);
                msg = ex.Message;
            }



            //  ObjTrans.CommittTransaction(ref Trans);

            return IsSaved;
        }
        public static bool Insert_PaymentTaxDetail(List<PaymentReceiptTaxDetailVM> modelDetail, string DealerCode, ref string msg)
        {
            // var code = "MCM01";
            //SysFunction Sys = new SysFunction();
            //SysFunction sys = new SysFunction();




            try
            {
                if (modelDetail != null)
                {
                    foreach (var item in modelDetail)
                    {
                        SqlParameter[] PmtRecTaxDetail_param = {                                            
                                                            /*0*/ new SqlParameter("@DealerCode",DealerCode),
                                                            /*1*/ new SqlParameter("@ReceiptNo",strAutoCode),
                                                            /*2*/ new SqlParameter("@ReceiptHead",item.ReceiptHead),
                                                            /*3*/ new SqlParameter("@AccountCode",item.AccountCode),
                                                            /*4*/ new SqlParameter("@Amount",SysFunction.CustomCDBL(item.Amount)),
                                                            /*5*/ new SqlParameter("@TaxID",item.TaxID),
                                                            /*6*/ new SqlParameter("@TaxPerc",SysFunction.CustomCDBL(item.TaxPerc)),
                                                           };


                        if (sysfun.ExecuteSP_NonQuery("sp_W2_PaymentReceipt_TaxDetail_Insert", PmtRecTaxDetail_param, Trans) == true)
                        {
                            IsSaved = true;
                        }



                    }
                    ObjTrans.CommittTransaction(ref Trans);
                    IsSaved = true;
                }
                else
                {
                    if (Trans.Connection != null)
                    {
                        ObjTrans.CommittTransaction(ref Trans);
                    }
                    
                    IsSaved = true;
                }
            }
            catch (Exception ex)
            {
                ObjTrans.RollBackTransaction(ref Trans);
                msg = ex.Message;
            }



            //  ObjTrans.CommittTransaction(ref Trans);

            return IsSaved;
        }

        public static bool Delete_Invoice_Record_CSI(string enquiryId, string dealerCode,double Amount,SqlTransaction Trans)
        {
            DataSet ds = new DataSet();

            SqlParameter[] param = {
                new SqlParameter("@CompanyCode",dealerCode),
                new SqlParameter("@ReceiptNo",enquiryId),
                 new SqlParameter("@Amount",Amount)
            };

            if (sysfun.ExecuteSP_NonQuery("sp_VehicleSaleMaster_Update_DeleteIP", param,Trans))
            {
                IsDeleted = true;
            }
            else
            {
                IsDeleted = false;
            }


            return IsDeleted;
        }
        public static bool Delete_Invoice_Record_ASC(string enquiryId, string dealerCode, double Amount, SqlTransaction Trans)
        {
            DataSet ds = new DataSet();

            SqlParameter[] param = {
                new SqlParameter("@CompanyCode",dealerCode),
                new SqlParameter("@ReceiptNo",enquiryId)
            };

            if (sysfun.ExecuteSP_NonQuery("sp_ReceiptMaster_Update_DeleteIP", param, Trans))
            {
                IsDeleted = true;
            }
            else
            {
                IsDeleted = false;
            }


            return IsDeleted;
        }
        public static bool DelFlagIP(string enquiryId, string dealerCode, SqlTransaction Trans)
        {
            DataSet ds = new DataSet();

            SqlParameter[] param = {
                new SqlParameter("@CompanyCode",dealerCode),
                new SqlParameter("@ReceiptNo",enquiryId)
            };

            if (sysfun.ExecuteSP_NonQuery("sp_IncommingPaymentMaster_DeleteIP", param, Trans))
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
