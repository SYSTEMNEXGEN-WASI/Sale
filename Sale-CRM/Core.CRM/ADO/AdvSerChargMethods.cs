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
using CConn;
namespace Core.CRM.ADO
{
    //public class AdvSerChargMethods
    //{
    //    static bool IsSaved = false;
    //    static bool IsDeleted = false;
    //    static SqlParameter[] nullSqlParam = null;
    //    static DataTable dt = new DataTable();
    //    static string strAutoCode = string.Empty;
    //    static SysFunction sysfun = new SysFunction();
    //    //static SysFunctions sysfuns = new SysFunctions();

    //    static string ChasisNo = string.Empty;
    //    static string EngineNo = string.Empty;

    //    static Transaction ObjTrans = new Transaction();
    //    static SqlTransaction Trans;

    //    public static string GetSaleInvoiceModal(string TransType,string dealerCode)
    //    {
    //        string json = "";
    //        List<SelectListItem> item = new List<SelectListItem>();

    //        var Serializer = new JavaScriptSerializer();
    //        List<VehicleSaleMasterVM> lst = new List<VehicleSaleMasterVM>();
    //        try
    //        {
    //            //var Serializer = new JavaScriptSerializer();
    //            SqlParameter[] sqlParam =
    //            {
    //                new SqlParameter("@DealerCode",dealerCode),
    //                new SqlParameter("@TransType",TransType)
    //            };
    //            dt = DataAccess.getDataTable("SP_Get_VehicleSaleTypes", sqlParam, General.GetBMSConString());

    //            if (dt.Rows.Count > 0)
    //            {
    //                lst = EnumerableExtension.ToList<VehicleSaleMasterVM>(dt);
    //            }
    //            json = Serializer.Serialize(lst);

    //        }
    //        catch (Exception ex)
    //        {

    //            //throw;
    //        }
    //        return json;
    //    }

    //    public static string GetSaleInvoiceDetail(string TransNo,string ChasisNo,string DealerCode)
    //    {
    //        string json = "";
    //        List<SelectListItem> item = new List<SelectListItem>();

    //        var Serializer = new JavaScriptSerializer();
    //        List<VehicleSaleMasterVM> lst = new List<VehicleSaleMasterVM>();
    //        try
    //        {
    //            //var Serializer = new JavaScriptSerializer();
    //            SqlParameter[] sqlParam =
    //            {
    //                new SqlParameter("@DealerCode",DealerCode),
    //                new SqlParameter("@TransCode",TransNo),
    //                new SqlParameter("@ChasisNo",ChasisNo)
    //            };
    //            dt = DataAccess.getDataTable("SP_Get_SaleInvoiceDetail", sqlParam, General.GetBMSConString());

    //            if (dt.Rows.Count > 0)
    //            {
    //                lst = EnumerableExtension.ToList<VehicleSaleMasterVM>(dt);
    //            }
    //            json = Serializer.Serialize(lst);

    //        }
    //        catch (Exception ex)
    //        {

    //            //throw;
    //        }
    //        return json;
    //    }

    //    public static List<AccountVM> GetAccountModal(string dealerCode)
    //    {

    //        List<AccountVM> lst = new List<AccountVM>();
    //        try
    //        {
    //            SqlParameter[] sqlParam =
    //            {
    //                new SqlParameter("@CompCode",dealerCode)
    //            };
    //            dt = DataAccess.getDataTable("SP_Select_AccountCode", sqlParam, General.GetFAMConString());

    //            if (dt.Rows.Count > 0)
    //            {
    //                lst = EnumerableExtension.ToList<AccountVM>(dt);
    //            }


    //        }
    //        catch (Exception ex)
    //        {

    //            //throw;
    //        }
    //        return lst;
    //    }

    //    public static List<ReceiptMasterVM> GetReceiptModal(string dealerCode)
    //    {

    //        List<ReceiptMasterVM> lst = new List<ReceiptMasterVM>();
    //        try
    //        {
    //            SqlParameter[] sqlParam =
    //            {
    //                new SqlParameter("@DealerCode",dealerCode)
    //            };
    //            dt = DataAccess.getDataTable("SP_ReceiptModal", sqlParam, General.GetBMSConString());

    //            if (dt.Rows.Count > 0)
    //            {
    //                lst = EnumerableExtension.ToList<ReceiptMasterVM>(dt);
    //            }


    //        }
    //        catch (Exception ex)
    //        {

    //            throw;
    //        }
    //        return lst;
    //    }

    //    public static bool Insert_ReceiptMaster(ReceiptMasterVM model)
    //    {
    //        DateTime recDate;
    //        ChasisNo = model.ChasisNo;
    //        EngineNo = model.EngineNo;

    //        try
    //        {
    //            if (model.ReceiptNo == "" || model.ReceiptNo == null)
    //            {
    //                strAutoCode = sysfun.AutoGen("ReceiptMaster", "ReceiptNo", DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MM/yyyy"), model.DealerCode);

    //            }
    //            else
    //            {

    //                strAutoCode = model.ReceiptNo;

    //            }
    //            SqlParameter[] param = {
    //                             new SqlParameter("@DealerCode",model.DealerCode),//0
    //				 new SqlParameter("@ReceiptNo",strAutoCode),//1
    //				 new SqlParameter("@ReceiptDate",sysfun.SaveDate(model.ReceiptDate)),//2								 
    //				 new SqlParameter("@CusCode",model.CusCode),//3								 
    //				 new SqlParameter("@Remarks",model.Remarks),//4
    //				 new SqlParameter("@ISFullAndFinal",'N'),//5
    //				 new SqlParameter("@FullAndFinalReceiveable",0.0),//6
    //				 new SqlParameter("@FullAndFinalPenalty",0.0),//7
    //				 new SqlParameter("@FullAndFinalTotalReceiveable",0.0),//8
    //				 new SqlParameter("@FullAndFinalDiscount",0.0),//9
    //				 new SqlParameter("@CreateDate",sysfun.SaveDate(DateTime.Now.ToString("dd-MM-yyyy"))),//10
    //				 new SqlParameter("@CreateTime",sysfun.SaveDate(DateTime.Now.ToString("dd-MM-yyyy"))),//11
    //				 new SqlParameter("@CreatedBy",AuthBase.UserId),//12
    //				 new SqlParameter("@CreateTerm",General.CurrentIP),//13
    //				 new SqlParameter("@DelFlag",'N'),//14
    //				 new SqlParameter("@PostFlag",'Y'),//15								 
    //				 new SqlParameter("@PostDate",sysfun.SaveDate(DateTime.Now.ToString("dd-MM-yyyy"))),//16								 
    //				 new SqlParameter("@TransferStatus",'C'),//17
    //				 new SqlParameter("@TransferDate",sysfun.SaveDate(DateTime.Now.ToString("dd-MM-yyyy"))),//18
    //				 new SqlParameter("@SoftwareVersion",DBNull.Value),//19
    //				 new SqlParameter("@CommunicationVersion",DBNull.Value),//20
    //				 new SqlParameter("@EmpCode",model.EmpCode),//21
    //				 new SqlParameter("@EmpDesc",model.EmpDesc),//22
    //				 new SqlParameter("@ProdDesc",model.ProdDesc),//23
    //				 new SqlParameter("@EngineNo",model.EngineNo), //24
    //				 new SqlParameter("@ChasisNo",model.ChasisNo),//25
    //                             new SqlParameter("@Type",DBNull.Value),//26
    //                             new SqlParameter("@OldReceiptNo",DBNull.Value),//27
    //                             new SqlParameter("@InvoiceDate",sysfun.SaveDate(model.InvoiceDate)),//28
    //                             new SqlParameter("@BalanceWithoutPenalty",0),//28
    //                             new SqlParameter("@BalancePenalty",0),//28
    //                             new SqlParameter("@TotalBalance",0), //24
    //				 new SqlParameter("@LastActualTransDate",sysfun.SaveDate(model.ReceiptDate)),//25
    //                             new SqlParameter("@LastPenaltyTransDate",sysfun.SaveDate(model.ReceiptDate)),//26
    //                             new SqlParameter("@PrintCounter",DBNull.Value),//27
    //                             new SqlParameter("@InvoiceNo",model.InvoiceNo),//28
    //                             new SqlParameter("@PRBNo",model.PRBNo),//28
    //                             new SqlParameter("@SONo",model.SONo),//28
    //                             new SqlParameter("@FormName","Advance/Service charges"),//28
    //                             new SqlParameter("@TransType",model.TransType),//28

    //                        };

    //            if (ObjTrans.BeginTransaction(ref Trans) == true)
    //            {
    //                sysfun.ExecuteSP_NonQuery("SP_Insert_ReceiptMaster", param, Trans);


    //                IsSaved = true;
    //            }
    //        }
    //        catch (Exception)
    //        {
    //            ObjTrans.RollBackTransaction(ref Trans);
    //            throw;
    //        }

    //        return IsSaved;
    //    }

    //    public static bool Insert_ReceiptDetail(List<ReceiptDetailVM> model2)
    //    {
    //        int count = 1;

    //        DataTable dt = new DataTable();

    //        dt = sysfun.GetData("Select BrandCode , ProdCode , VersionCode ,ColorCode, ColorDesc from VehicleSaleDetail where ChassisNo = '" + ChasisNo + "' or EngineNo = '" + EngineNo + "'");

    //        try
    //        {
    //            foreach (var item in model2)
    //            {
    //                if (item.InstrumentTypeCode != null || item.InstrumentNo != null)
    //                {
    //                    SqlParameter[] param2 = {
    //                                new SqlParameter("@DealerCode",item.DealerCode),
    //                                new SqlParameter("@ReceiptNo",strAutoCode) ,
    //                                new SqlParameter("@ReceiptDate",sysfun.SaveDate(item.ReceiptDate)) ,
    //                                new SqlParameter("@InstrumentTypeCode",item.InstrumentTypeCode) ,
    //                                new SqlParameter("@InstrumentNo",item.InstrumentNo) ,
    //                                new SqlParameter("@InstrumentDate",item.InstrumentDate == null ? DBNull.Value : sysfun.SaveDate(item.InstrumentDate)) ,
    //                                new SqlParameter("@CityCode",item.CityCode),
    //                                new SqlParameter("@BankCode",item.BankCode) ,
    //                                new SqlParameter("@Branch",item.Branch) ,
    //                                new SqlParameter("@DrawnBankCode",DBNull.Value) ,
    //                                new SqlParameter("@DrawnBranch",DBNull.Value) ,
    //                                new SqlParameter("@CusCode",item.CusCode),
    //                                new SqlParameter("@AccountCode",DBNull.Value),
    //                                new SqlParameter("@ReceiptAmount",item.ReceiptAmount),
    //                                new SqlParameter("@AdjustedAmount",DBNull.Value),
    //                                new SqlParameter("@AdjustableAmount",DBNull.Value),
    //                                new SqlParameter("@InvoiceNo",item.InvoiceNo),
    //                                new SqlParameter("@ISFullAndFinal",'N'),
    //                                new SqlParameter("@ISPenalty",'N'),
    //                                new SqlParameter("@CreateDate",DateTime.Now) ,
    //                                new SqlParameter("@CreateTime",DateTime.Now) ,
    //                                new SqlParameter("@CreatedBy",AuthBase.UserId),
    //                                new SqlParameter("@CreatedTerm",General.CurrentIP) ,
    //                                new SqlParameter("@UpdDate",DateTime.Now),
    //                                new SqlParameter("@UpdTime",DateTime.Now),
    //                                new SqlParameter("@UpdUser",AuthBase.UserId),
    //                                new SqlParameter("@UpdTerm",General.CurrentIP) ,
    //                                new SqlParameter("@BookRefNo",DBNull.Value),
    //                                new SqlParameter("@ProdCode",dt.Rows[0]["ProdCode"].ToString()),
    //                                new SqlParameter("@VersionCode",dt.Rows[0]["ProdCode"].ToString()),
    //                                new SqlParameter("@ColorDesc",dt.Rows[0]["ColorDesc"].ToString()),
    //                                new SqlParameter("@ProdDesc",sysfun.GetStringValuesAgainstCodes("ProdCode", dt.Rows[0]["ProdCode"].ToString(),"ProdDesc","Vehicle","",item.DealerCode)),
    //                                new SqlParameter("@EmpCode",DBNull.Value),
    //                                new SqlParameter("@EmpDesc",DBNull.Value),
    //                                new SqlParameter("@TransferStatus",'C'),
    //                                new SqlParameter("@SNO",count++),
    //                                new SqlParameter("@DepositSlipNo",DBNull.Value),
    //                                new SqlParameter("@RealizeDate",DBNull.Value),
    //                                new SqlParameter("@Status",'R'),
    //                                new SqlParameter("@RefundNo",DBNull.Value),
    //                                new SqlParameter("@PlanCode",DBNull.Value),
    //                                new SqlParameter("@SlipRefNo",DBNull.Value),
    //                                new SqlParameter("@ColorCode",dt.Rows[0]["ColorCode"].ToString()),
    //                                new SqlParameter("@VehExpCode",item.VehExpCode),
    //                                new SqlParameter("@ServicesAccountCode",item.ServicesAccountCode),
    //                                new SqlParameter("@BrandCode",dt.Rows[0]["BrandCode"].ToString())

    //                        };


    //                    if (sysfun.ExecuteSP_NonQuery("SP_Insert_ReceiptDetail", param2, Trans) == true)
    //                    {
    //                        IsSaved = true;
    //                    }
    //                    else
    //                    {
    //                        ObjTrans.RollBackTransaction(ref Trans);
    //                        IsSaved = false;
    //                    }

    //                    IsSaved = true;
    //                }

    //            }
    //        }
    //        catch (Exception)
    //        {
    //            ObjTrans.RollBackTransaction(ref Trans);
    //            throw;
    //        }

    //        return IsSaved;
    //    }

    //    public static bool Insert_AccountTransaction(List<AccountTransactionVM> AccountModel)
    //    {
    //        string strReceiptNo;
    //        int count = 0;
    //        try
    //        {
    //            DataTable dt = sysfun.GetData("SP_GET_BlnAmt_AccountTransaction '" + AccountModel.FirstOrDefault().DealerCode + "','" + AccountModel.FirstOrDefault().Reference + "'");

    //            DataTable dt2 = sysfun.GetData("Select ShortForm from VehExpHead where VehExpCode = '"+ AccountModel.FirstOrDefault().TrType+ "' and DealerCode in ('" + AccountModel.FirstOrDefault().DealerCode + "','COMON')");

    //            DataTable dt3 = sysfun.GetData("Select AccountCode from Customer where CusCode = '"+ AccountModel.FirstOrDefault().CusCode + "' and DealerCode = '"+ AccountModel.FirstOrDefault().DealerCode + "'");

    //            foreach (var item in AccountModel)
    //            {
    //                if (item.Reference != null)
    //                {

    //                    string getNextTransCode = "declare @lastval varchar(14),@id int " +
    //                                       "set @id = (select count(*) from AccountTransaction) " +
    //                                       "set @id=@id+1 " +
    //                                       "if len(@id) = 1 " +
    //                                       "set @lastval='" + "'+cast((YEAR(getDate()) ) %100  as varchar(10)) +'00000' " +
    //                                       "if len(@id) = 2 " +
    //                                       "set @lastval='" + "'+cast((YEAR(getDate())  ) %100  as varchar(10)) +'0000' " +
    //                                       "if len(@id) = 3 " +
    //                                       "set @lastval='" + "'+cast((YEAR(getDate())  ) %100  as varchar(10)) +'000' " +
    //                                       "if len(@id) >= 4 " +
    //                                       "set @lastval='" + "'+cast((YEAR(getDate())  ) %100  as varchar(10)) +'00' " +
    //                                       "if len(@id) >= 5 " +
    //                                       "set @lastval='" + "'+cast((YEAR(getDate())  ) %100  as varchar(10)) +'0' " +
    //                                       "declare @i varchar(14) " +
    //                                       "set @i = CAST(@id as varchar(14)) " +
    //                                       "set @lastval = @lastval+@i " +
    //                                       "select @lastval as TransactionCode";

    //                    dt = DataAccess.getDataTableByQuery(getNextTransCode, nullSqlParam, General.GetBMSConString());

    //                    strReceiptNo = dt.Rows[0]["TransactionCode"].ToString();

    //                    SqlParameter[] sqlParam =
    //                                         {
    //                                             new SqlParameter("@DealerCode",item.DealerCode),//0
    //                                             new SqlParameter("@TransactionCode",strReceiptNo),//3
    //                                             new SqlParameter("@TransactionDate",sysfun.SaveDate(item.TransactionDate)),//4
    //                                             new SqlParameter("@CusCode",item.CusCode),//5
    //                                             new SqlParameter("@AccountCode",dt3.Rows[0]["AccountCode"].ToString()),//6
    //                                             new SqlParameter("@InvType",item.InvType),//7
    //                                             new SqlParameter("@TrType",dt2.Rows[0]["ShortForm"].ToString()),//8
    //                                             new SqlParameter("@Narration",item.Narration),//9
    //                                             new SqlParameter("@Reference",item.Reference),//10
    //                                             new SqlParameter("@Debit",item.Debit),//11
    //                                             new SqlParameter("@Credit",item.Credit),//12
    //                                             new SqlParameter("@Balance",dt.Rows[0]["Balance"].ToString() == "" ? "0" :dt.Rows[0]["Balance"].ToString() ),//13
    //                                             new SqlParameter("@Remarks",(object)DBNull.Value),//14
    //                                             new SqlParameter("@CreateDate",DateTime.Now) ,
    //                                             new SqlParameter("@CreateTime",DateTime.Now) ,
    //                                             new SqlParameter("@CreateUser",AuthBase.EmpCode),//17
    //                                             new SqlParameter("@CreateTerm",AuthBase.UserId),//18
    //                                             new SqlParameter("@UpdDate",(object)DBNull.Value),//19
    //                                             new SqlParameter("@UpdTime",(object)DBNull.Value),//20
    //                                             new SqlParameter("@UpdUser",(object)DBNull.Value),//21
    //                                             new SqlParameter("@UpdTerm",(object)DBNull.Value),//22
    //                                             new SqlParameter("@ReceiptNo",strAutoCode),//22

    //                                         };

    //                    if (sysfun.ExecuteSP_NonQuery("SP_Insert_AccountTransaction", sqlParam, Trans) == true)
    //                    {


    //                        IsSaved = true;
    //                    }
    //                    else
    //                    {
    //                        ObjTrans.RollBackTransaction(ref Trans);
    //                        return false;
    //                    }
    //                }
    //            }
    //            ObjTrans.CommittTransaction(ref Trans);
    //        }
    //        catch (Exception)
    //        {
    //            ObjTrans.RollBackTransaction(ref Trans);
    //            throw;
    //        }

    //        return IsSaved;
    //    }

    //    public static string Get_ReceiptDetailData(string enquiryId, string dealerCode)
    //    {
    //        string json = "";
    //        var Serializer = new JavaScriptSerializer();
    //        List<ReceiptDetailVM> lst = new List<ReceiptDetailVM>();
    //        try
    //        {
    //            SqlParameter[] sqlParam = {
    //                                new SqlParameter("@DealerCode",dealerCode),//1
    //					new SqlParameter("@RceiptNo",enquiryId)//0

    //					};

    //            dt = DataAccess.getDataTable("SP_Select_ReceiptDetail", sqlParam, General.GetBMSConString());
    //            if (dt.Rows.Count > 0)
    //            {
    //                lst = EnumerableExtension.ToList<ReceiptDetailVM>(dt);
    //            }
    //            json = Serializer.Serialize(lst);
    //        }
    //        catch (Exception ex)
    //        {

    //            throw;
    //        }

    //        return json;
    //    }

    //    public static bool Delete_Receipt_Record(string enquiryId, string dealerCode)
    //    {
    //        DataSet ds = new DataSet();

    //        SqlParameter[] param = {
    //            new SqlParameter("@DealerCode",dealerCode),
    //            new SqlParameter("@ReceiptNo",enquiryId)
    //        };

    //        if (sysfun.ExecuteSP_NonQuery("sp_Receipt_Delete", param))
    //        {
    //            IsDeleted = true;
    //        }
    //        else
    //        {
    //            IsDeleted = false;
    //        }


    //        return IsDeleted;
    //    }
    //}


    public class AdvSerChargMethods
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
        static string msg;
        static Transaction ObjTrans = new Transaction();
        static SqlTransaction Trans;

        public static string GetSaleInvoiceModal(string TransType, string dealerCode)
        {
            string json = "";
            List<SelectListItem> item = new List<SelectListItem>();

            var Serializer = new JavaScriptSerializer();
            List<VehicleSaleMasterVM> lst = new List<VehicleSaleMasterVM>();
            try
            {
                //var Serializer = new JavaScriptSerializer();
                SqlParameter[] sqlParam =
                {
                    new SqlParameter("@DealerCode",dealerCode),
                    //new SqlParameter("@TransType",TransType)
                };
                dt = DataAccess.getDataTable("SP_Get_VehicleSaleTypes", sqlParam, CConnection.GetConnectionString());

                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<VehicleSaleMasterVM>(dt);
                }
                json = Serializer.Serialize(lst);

            }
            catch (Exception ex)
            {

                //throw;
            }
            return json;
        }

        public static string GetSaleInvoiceDetail(string TransNo, string ChasisNo, string DealerCode)
        {
            string json = "";
            List<SelectListItem> item = new List<SelectListItem>();

            var Serializer = new JavaScriptSerializer();
            List<VehicleSaleMasterVM> lst = new List<VehicleSaleMasterVM>();
            try
            {
                //var Serializer = new JavaScriptSerializer();
                SqlParameter[] sqlParam =
                {
                    new SqlParameter("@DealerCode",DealerCode),
                    new SqlParameter("@TransCode",TransNo),
                    new SqlParameter("@ChasisNo",ChasisNo)
                };
                dt = DataAccess.getDataTable("SP_Get_SaleInvoiceDetail", sqlParam, CConnection.GetConnectionString());

                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<VehicleSaleMasterVM>(dt);
                }
                json = Serializer.Serialize(lst);

            }
            catch (Exception ex)
            {

                //throw;
            }
            return json;
        }

        public static List<AccountVM> GetAccountModal(string dealerCode)
        {

            List<AccountVM> lst = new List<AccountVM>();
            try
            {
                SqlParameter[] sqlParam =
                {
                    new SqlParameter("@CompCode",dealerCode)
                };
                dt = DataAccess.getDataTable("SP_Select_AccountCode", sqlParam, General.GetFAMConString());

                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<AccountVM>(dt);
                }


            }
            catch (Exception ex)
            {

                //throw;
            }
            return lst;
        }

        public static List<ReceiptMasterVM> GetReceiptModal(string dealerCode)
        {

            List<ReceiptMasterVM> lst = new List<ReceiptMasterVM>();
            try
            {
                SqlParameter[] sqlParam =
                {
                    new SqlParameter("@DealerCode",dealerCode)
                };
                dt = DataAccess.getDataTable("SP_ReceiptModal", sqlParam, General.GetBMSConString());

                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<ReceiptMasterVM>(dt);
                }


            }
            catch (Exception ex)
            {

                throw;
            }
            return lst;
        }

        public static bool Insert_ReceiptMaster(ReceiptMasterVM model, ref string msg)
        {
            DateTime recDate;
            ChasisNo = model.ChasisNo;
            EngineNo = model.EngineNo;

            try
            {
                if (model.ReceiptNo == "" || model.ReceiptNo == null)
                {
                    strAutoCode = sysfun.AutoGen("ReceiptMaster", "ReceiptNo", DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MM/yyyy"), model.DealerCode);

                }
                else
                {

                    strAutoCode = model.ReceiptNo;

                }
                SqlParameter[] param = {
                                 new SqlParameter("@DealerCode",model.DealerCode),//0
								 new SqlParameter("@ReceiptNo",strAutoCode),//1
								 new SqlParameter("@ReceiptDate",sysfun.SaveDate(model.ReceiptDate)),//2								 
								 new SqlParameter("@CusCode",model.CusCode),//3								 
								 new SqlParameter("@Remarks",model.Remarks),//4
								 new SqlParameter("@ISFullAndFinal",'N'),//5
								 new SqlParameter("@FullAndFinalReceiveable",0.0),//6
								 new SqlParameter("@FullAndFinalPenalty",0.0),//7
								 new SqlParameter("@FullAndFinalTotalReceiveable",0.0),//8
								 new SqlParameter("@FullAndFinalDiscount",0.0),//9
								 new SqlParameter("@CreateDate",sysfun.SaveDate(DateTime.Now.ToString("dd-MM-yyyy"))),//10
								 new SqlParameter("@CreateTime",sysfun.SaveDate(DateTime.Now.ToString("dd-MM-yyyy"))),//11
								 new SqlParameter("@CreatedBy",AuthBase.UserId),//12
								 new SqlParameter("@CreateTerm",General.CurrentIP),//13
								 new SqlParameter("@DelFlag",'N'),//14
								 new SqlParameter("@PostFlag",'Y'),//15								 
								 new SqlParameter("@PostDate",sysfun.SaveDate(DateTime.Now.ToString("dd-MM-yyyy"))),//16								 
								 new SqlParameter("@TransferStatus",'C'),//17
								 new SqlParameter("@TransferDate",sysfun.SaveDate(DateTime.Now.ToString("dd-MM-yyyy"))),//18
								 new SqlParameter("@SoftwareVersion",DBNull.Value),//19
								 new SqlParameter("@CommunicationVersion",DBNull.Value),//20
								 new SqlParameter("@EmpCode",model.EmpCode),//21
								 new SqlParameter("@EmpDesc",model.EmpDesc),//22
								 new SqlParameter("@ProdDesc",model.ProdDesc),//23
								 new SqlParameter("@EngineNo",model.EngineNo), //24
								 new SqlParameter("@ChasisNo",model.ChasisNo),//25
                                 new SqlParameter("@Type",DBNull.Value),//26
                                 new SqlParameter("@OldReceiptNo",DBNull.Value),//27
                                 new SqlParameter("@InvoiceDate",sysfun.SaveDate(model.InvoiceDate)),//28
                                 new SqlParameter("@BalanceWithoutPenalty",0),//28
                                 new SqlParameter("@BalancePenalty",0),//28
                                 new SqlParameter("@TotalBalance",0), //24
								 new SqlParameter("@LastActualTransDate",sysfun.SaveDate(model.ReceiptDate)),//25
                                 new SqlParameter("@LastPenaltyTransDate",sysfun.SaveDate(model.ReceiptDate)),//26
                                 new SqlParameter("@PrintCounter",DBNull.Value),//27
                                 new SqlParameter("@InvoiceNo",model.InvoiceNo),//28
                                 new SqlParameter("@PRBNo",model.PRBNo),//28
                                 new SqlParameter("@SONo",model.SONo),//28
                                 new SqlParameter("@FormName","Advance/Service charges"),//28
                                 new SqlParameter("@TransType",model.TransType),//28
                                  new SqlParameter("@TotalAmount",model.TotalAmount),//28

                            };

                if (ObjTrans.BeginTransaction(ref Trans) == true)
                {
                    sysfun.ExecuteSP_NonQuery("SP_Insert_ReceiptMaster", param, Trans);


                    IsSaved = true;
                }
            }
            catch (Exception ex)
            {
                ObjTrans.RollBackTransaction(ref Trans);
                msg = ex.Message;
                // return false;
                //throw;
            }

            return IsSaved;
        }

        public static bool Insert_ReceiptDetail(List<ReceiptDetailVM> model2, ref string msg)
        {
            int count = 1;

            DataTable dt = new DataTable();

            dt = sysfun.GetDatas("Select BrandCode , ProdCode , VersionCode ,ColorCode, ColorDesc from VehicleSaleDetail where ChassisNo = '" + ChasisNo + "' or EngineNo = '" + EngineNo + "'", "BMS0517ConnectionString");

            try
            {
                foreach (var item in model2)
                {
                    if (item.InstrumentTypeCode != null || item.InstrumentNo != null)
                    {
                        SqlParameter[] param2 = {
                                    new SqlParameter("@DealerCode",item.DealerCode),
                                    new SqlParameter("@ReceiptNo",strAutoCode) ,
                                    new SqlParameter("@ReceiptDate",sysfun.SaveDate(item.ReceiptDate)) ,
                                    new SqlParameter("@InstrumentTypeCode",item.InstrumentTypeCode) ,
                                    new SqlParameter("@InstrumentNo",item.InstrumentNo) ,
                                    new SqlParameter("@InstrumentDate",item.InstrumentDate == null ? DBNull.Value : sysfun.SaveDate(item.InstrumentDate)) ,
                                    new SqlParameter("@CityCode",item.CityCode),
                                    new SqlParameter("@BankCode",item.BankCode) ,
                                    new SqlParameter("@Branch",item.Branch) ,
                                    new SqlParameter("@DrawnBankCode",DBNull.Value) ,
                                    new SqlParameter("@DrawnBranch",DBNull.Value) ,
                                    new SqlParameter("@CusCode",item.CusCode),
                                    new SqlParameter("@AccountCode",DBNull.Value),
                                    new SqlParameter("@ReceiptAmount",item.ReceiptAmount),
                                    new SqlParameter("@AdjustedAmount",DBNull.Value),
                                    new SqlParameter("@AdjustableAmount",DBNull.Value),
                                    new SqlParameter("@InvoiceNo",item.InvoiceNo),
                                    new SqlParameter("@ISFullAndFinal",'N'),
                                    new SqlParameter("@ISPenalty",'N'),
                                    new SqlParameter("@CreateDate",DateTime.Now) ,
                                    new SqlParameter("@CreateTime",DateTime.Now) ,
                                    new SqlParameter("@CreatedBy",AuthBase.UserId),
                                    new SqlParameter("@CreatedTerm",General.CurrentIP) ,
                                    new SqlParameter("@UpdDate",DateTime.Now),
                                    new SqlParameter("@UpdTime",DateTime.Now),
                                    new SqlParameter("@UpdUser",AuthBase.UserId),
                                    new SqlParameter("@UpdTerm",General.CurrentIP) ,
                                    new SqlParameter("@BookRefNo",DBNull.Value),
                                    new SqlParameter("@ProdCode",dt.Rows[0]["ProdCode"].ToString()),
                                    new SqlParameter("@VersionCode",dt.Rows[0]["ProdCode"].ToString()),
                                    new SqlParameter("@ColorDesc",dt.Rows[0]["ColorDesc"].ToString()),
                                    new SqlParameter("@ProdDesc",sysfun.GetStringValuesAgainstCodes("ProdCode", dt.Rows[0]["ProdCode"].ToString(),"ProdDesc","Vehicle","",item.DealerCode)),
                                    new SqlParameter("@EmpCode",DBNull.Value),
                                    new SqlParameter("@EmpDesc",DBNull.Value),
                                    new SqlParameter("@TransferStatus",'C'),
                                    new SqlParameter("@SNO",count++),
                                    new SqlParameter("@DepositSlipNo",DBNull.Value),
                                    new SqlParameter("@RealizeDate",DBNull.Value),
                                    new SqlParameter("@Status",'R'),
                                    new SqlParameter("@RefundNo",DBNull.Value),
                                    new SqlParameter("@PlanCode",DBNull.Value),
                                    new SqlParameter("@SlipRefNo",DBNull.Value),
                                    new SqlParameter("@ColorCode",dt.Rows[0]["ColorCode"].ToString()),
                                    new SqlParameter("@VehExpCode",item.VehExpCode),
                                    new SqlParameter("@ServicesAccountCode",item.ServicesAccountCode),
                                    new SqlParameter("@BrandCode",dt.Rows[0]["BrandCode"].ToString())

                            };


                        if (sysfun.ExecuteSP_NonQuery("SP_Insert_ReceiptDetail", param2, Trans) == true)
                        {
                            IsSaved = true;
                        }
                        else
                        {
                            ObjTrans.RollBackTransaction(ref Trans);
                            IsSaved = false;
                        }

                        IsSaved = true;
                    }

                }
            }
            catch (Exception ex)
            {
                ObjTrans.RollBackTransaction(ref Trans);
                //throw;
                msg = ex.Message;
            }
            ObjTrans.CommittTransaction(ref Trans);
            return IsSaved;
        }

        public static bool Insert_AccountTransaction(List<AccountTransactionVM> AccountModel, ref string msg)
        {
            string strReceiptNo = "";
            double strecNo;
            int count = 0;
            try
            {
                DataTable dt4 = sysfun.GetDatas("SP_GET_BlnAmt_AccountTransaction '" + AccountModel.FirstOrDefault().DealerCode + "','" + AccountModel.FirstOrDefault().Reference + "'", "BMS0517ConnectionString");

                DataTable dt2 = sysfun.GetDatas("Select ShortForm from VehExpHead where VehExpCode = '" + AccountModel.FirstOrDefault().TrType + "' and DealerCode in ('" + AccountModel.FirstOrDefault().DealerCode + "','AAAAA')", "BMS0517ConnectionString");

                DataTable dt3 = sysfun.GetDatas("Select AccountCode from Customer where CusCode = '" + AccountModel.FirstOrDefault().CusCode + "' and DealerCode = '" + AccountModel.FirstOrDefault().DealerCode + "'", "BMS0517ConnectionString");

                foreach (var item in AccountModel)
                {
                    if (item.Reference != null)

                    {

                        // string getNextTransCode = sysfun.GetNewMaxID("AccountTrasnaction", "TransactionCode", 8, AccountModel.FirstOrDefault().DealerCode);

                        string getNextTransCode = "declare @lastval varchar(14),@id int " +
                                           "set @id = (select count(*) from AccountTrasnaction) " +
                                           "set @id=@id+1 " +
                                           "if len(@id) = 1 " +
                                           "set @lastval='" + "'+cast((YEAR(getDate()) ) %100  as varchar(10)) +'00000' " +
                                           "if len(@id) = 2 " +
                                           "set @lastval='" + "'+cast((YEAR(getDate())  ) %100  as varchar(10)) +'0000' " +
                                           "if len(@id) = 3 " +
                                           "set @lastval='" + "'+cast((YEAR(getDate())  ) %100  as varchar(10)) +'000' " +
                                           "if len(@id) >= 4 " +
                                           "set @lastval='" + "'+cast((YEAR(getDate())  ) %100  as varchar(10)) +'00' " +
                                           "if len(@id) >= 5 " +
                                           "set @lastval='" + "'+cast((YEAR(getDate())  ) %100  as varchar(10)) +'0' " +
                                           "declare @i varchar(14) " +
                                           "set @i = CAST(@id as varchar(14)) " +
                                           "set @lastval = @lastval+@i " +
                                           "select @lastval as TransactionCode";
                        //if (strReceiptNo != null && strReceiptNo !="")
                        //{
                        //    count++;
                        //    strecNo = Convert.ToDouble(strReceiptNo);
                        //    strecNo = strecNo + count;
                        //    strReceiptNo = strecNo.ToString();

                        //}
                        //else {
                        dt = DataAccess.getDataTableByQuery(getNextTransCode, nullSqlParam, General.GetConnectionString());

                        strReceiptNo = dt.Rows[0]["TransactionCode"].ToString();

                        // }


                        decimal balance = Convert.ToDecimal(dt4.Rows[0]["Balance"].ToString());

                        SqlParameter[] sqlParam =
                                             {
                                                 new SqlParameter("@DealerCode",item.DealerCode),//0
                                                 new SqlParameter("@TransactionCode",strReceiptNo),//3
                                                 new SqlParameter("@TransactionDate",sysfun.SaveDate(item.TransactionDate)),//4
                                                 new SqlParameter("@CusCode",item.CusCode),//5
                                                 new SqlParameter("@AccountCode",dt3.Rows[0]["AccountCode"].ToString()),//6
                                                 new SqlParameter("@InvType",item.InvType),//7
                                                 new SqlParameter("@TrType",AccountModel.FirstOrDefault().TrType),//8
                                                 new SqlParameter("@Narration",item.Narration),//9
                                                 new SqlParameter("@Reference",item.Reference),//10
                                                 new SqlParameter("@Debit",item.Debit),//11
                                                 new SqlParameter("@Credit",item.Credit),//12
                                                 new SqlParameter("@Balance",balance),//13
                                                 new SqlParameter("@Remarks",(object)DBNull.Value),//14
                                                 new SqlParameter("@CreateDate",DateTime.Now) ,
                                                 new SqlParameter("@CreateTime",DateTime.Now) ,
                                                 new SqlParameter("@CreateUser",AuthBase.EmpCode),//17
                                                 new SqlParameter("@CreateTerm",AuthBase.UserId),//18
                                                 new SqlParameter("@UpdDate",(object)DBNull.Value),//19
                                                 new SqlParameter("@UpdTime",(object)DBNull.Value),//20
                                                 new SqlParameter("@UpdUser",(object)DBNull.Value),//21
                                                 new SqlParameter("@UpdTerm",(object)DBNull.Value),//22
                                                 new SqlParameter("@ReceiptNo",strAutoCode),//22

                                             };
                        if (sysfun.ExecuteSP_NonQuery("SP_Insert_AccountTransaction", sqlParam))
                        {

                            IsSaved = true;
                        }
                        //if (sysfun.ExecuteSP_NonQuery("SP_Insert_AccountTransaction", sqlParam, Trans) == true)
                        //{


                        //    IsSaved = true;
                        //}
                        //else
                        //{
                        //    ObjTrans.RollBackTransaction(ref Trans);
                        //    return false;
                        //}
                    }
                }

            }
            catch (Exception ex)
            {
                // ObjTrans.RollBackTransaction(ref Trans);
                //throw;
                msg = ex.Message;
            }

            return IsSaved;
        }

        public static string Get_ReceiptDetailData(string enquiryId, string dealerCode)
        {
            string json = "";
            var Serializer = new JavaScriptSerializer();
            List<ReceiptDetailVM> lst = new List<ReceiptDetailVM>();
            try
            {
                SqlParameter[] sqlParam = {
                                    new SqlParameter("@DealerCode",dealerCode),//1
									new SqlParameter("@RceiptNo",enquiryId)//0
									
									};

                dt = DataAccess.getDataTable("SP_Select_ReceiptDetail", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<ReceiptDetailVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return json;
        }

        public static bool Delete_Receipt_Record(string enquiryId, string dealerCode)
        {
            DataSet ds = new DataSet();

            SqlParameter[] param = {
                new SqlParameter("@DealerCode",dealerCode),
                new SqlParameter("@ReceiptNo",enquiryId)
            };

            if (sysfun.ExecuteSP_NonQuery("sp_Receipt_Delete", param))
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
