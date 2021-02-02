using Core.CRM.ADO.ViewModel;
using Core.CRM.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Core.CRM.ADO
{
    public class CreditSaleInvoiceMethods
    {
        static string strAutoCode = string.Empty;
        static string autoProspect_ID = string.Empty;
        static bool IsSaved = false;
        static SqlParameter[] nullSqlParam = null;
        string dealerCode = "MCM01";
        static DataTable dt = new DataTable();
        string LeadDate = "";
        SysFunction sys = new SysFunction();
        public static List<VehicleSaleDataVM> Get_SaleDetailData()
        {
            DataTable dt = new DataTable();
            string json = "";
            var Serializer = new JavaScriptSerializer();
            string DealerCode = "MCM01";
            string SaleType = "Credit";
            string TransType = "SO";

            List<VehicleSaleDataVM> lst = new List<VehicleSaleDataVM>();
            try
            {
                SqlParameter[] sqlParam = {
                                    new SqlParameter("@DealerCode",DealerCode),//0
                                    new SqlParameter("@SaleType",SaleType),
                                    new SqlParameter("@TransType",TransType)


                                    };

                dt = DataAccess.getDataTable("Sp_Get_VehicleSaleData", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<VehicleSaleDataVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {
                throw;
            }

            return lst;
        }
        public static List<CSIVehicleGridDataVM> Get_CrdSIGridData(string TransCode)
        {
            DataTable dt = new DataTable();
            string json = "";
            var Serializer = new JavaScriptSerializer();
            string DealerCode = "MCM01";


            List<CSIVehicleGridDataVM> lst = new List<CSIVehicleGridDataVM>();
            try
            {
                SqlParameter[] sqlParam = {
									new SqlParameter("@DealerCode",DealerCode)//0
                                    ,new SqlParameter("@TransCode",TransCode)//0
									};

                dt = DataAccess.getDataTable("Sp_Get_CSIVehicleStock", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<CSIVehicleGridDataVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {
                throw;
            }

            return lst;
        }
        public static List<CrdSIVM> Get_CrdSIDetailData()
        {
            DataTable dt = new DataTable();
            string json = "";
            var Serializer = new JavaScriptSerializer();
            string DealerCode = "MCM01";
            string TransType = "CrdSI";
            string SaleType = "Credit";


            List<CrdSIVM> lst = new List<CrdSIVM>();
            try
            {
                SqlParameter[] sqlParam = {
                                    new SqlParameter("@DealerCode",DealerCode)//0
                                    , new SqlParameter("@TransType",TransType)//0
                                    , new SqlParameter("@SaleType",SaleType)//0
									};

                dt = DataAccess.getDataTable("Sp_Get_CSIData", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<CrdSIVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {
                throw;
            }

            return lst;
        }
        public static string Insert_CrdSIMasterData(VehicleSaleMasterVM model, AccountTransactionVM AccountModel)
        {
            string leadid = "";
            SysFunction sys = new SysFunction();

            if (model.TransCode == "" || model.TransCode == "0" || model.TransCode == null)
            {


                string getNextTransCode = "declare @lastval varchar(14),@id int " +
                                           "set @id = (select count(*) from VehicleSaleMaster) " +
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
                                           "select @lastval as TransCode";

                dt = DataAccess.getDataTableByQuery(getNextTransCode, nullSqlParam, General.GetBMSConString());

                strAutoCode = dt.Rows[0]["TransCode"].ToString();

            }
            else
            {
                strAutoCode = model.TransCode; ;
            }



            try
            {
                //var Serializer = new JavaScriptSerializer();
                string transType = "CrdSI";
                string delflag = "N";
                string dealerCode = "MCM01";

                DateTime UniDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                DateTime UniTime = Convert.ToDateTime(DateTime.Now.ToShortTimeString());
                string txt = model.TransDate;


                //DateTime leadDate = DateTime.ParseExact(model.TransDate,"MM/dd/yyyy", CultureInfo.InvariantCulture);
                SqlParameter[] sqlParam =
                {
                    new SqlParameter("@DealerCode",dealerCode),//0
					new SqlParameter("@TransCode",strAutoCode),//2
					new SqlParameter("@TransDate",sys.SaveDate(txt)),//3
					new SqlParameter("@TransType",transType),//4
					new SqlParameter("@SaleType",model.SaleType),//5
					new SqlParameter("@CreditTerms",model.CreditTerms),//6
					new SqlParameter("@EmpCode",model.EmpCode),//7
					new SqlParameter("@CusCode",model.CusCode),//8
					new SqlParameter("@CustomerType",model.CustomerType),//9
					new SqlParameter("@CNICNTN",model.CNICNTN),//10
					new SqlParameter("@ContactNo",model.ContactNo),//11
					new SqlParameter("@PriceLevel",model.PriceLevel),//12
					new SqlParameter("@BillTo",model.BillTo),//13
					new SqlParameter("@ShipTo",model.ShipTo),//14
					new SqlParameter("@SameAs",model.SameAs),//15
					new SqlParameter("@TotalQty",model.TotalQty),//16
					new SqlParameter("@ServceQty",model.TotalQty),//17
					new SqlParameter("@TotalAmount",model.TotalAmount),//18
					new SqlParameter("@PaymentReceiptCode",(object)DBNull.Value),//19
					new SqlParameter("@PaidAmoun",(object)DBNull.Value),//20
					new SqlParameter("@DelFlag",delflag),//21
					new SqlParameter("@RefType",model.RefType),//22
					new SqlParameter("@RefDocumentNo",model.RefDocumentNo),//23
					new SqlParameter("@PostFlag",(object)DBNull.Value),//24
					new SqlParameter("@VoucherNo",(object)DBNull.Value),//25
					new SqlParameter("@VoucherDate",(object)DBNull.Value),//26
					new SqlParameter("@UpdUser",AuthBase.UserId),//27
					new SqlParameter("@UpdDate",UniDate),//28
					new SqlParameter("@UpdTime",UniTime),//29
					new SqlParameter("@UpdTerm",General.CurrentIP),//30
					
				};
                dt = DataAccess.getDataTable("SP_Insert_VehicleSaleMaster", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {

                }
                leadid = strAutoCode;
                IsSaved = true;
            }
            catch (Exception ex)
            {

                //throw;
            }



            //-------------------------Account Transaction----------------------------------

            string ActAutoCode;

            if (AccountModel.TransactionCode == "" || AccountModel.TransactionCode == "0" || AccountModel.TransactionCode == null)
            {


                string getNextTransCode = "declare @lastval varchar(14),@id int " +
                                           "set @id = (select count(*) from AccountTransaction) " +
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

                dt = DataAccess.getDataTableByQuery(getNextTransCode, nullSqlParam, General.GetBMSConString());

                ActAutoCode = dt.Rows[0]["TransactionCode"].ToString();

            }
            else
            {
                ActAutoCode = AccountModel.TransactionCode; ;
            }



            try
            {
                //var Serializer = new JavaScriptSerializer();

                string delflag = "N";
                string dealerCode = "MCM01";
                string TrType = "CrdSI";
                int Credit = 0;
                string Narration = " Credit Sale Invoice ";
                //DateTime CreateDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                DateTime CreateTime = Convert.ToDateTime(DateTime.Now.ToShortTimeString());
                DateTime CreateDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                //DateTime leadDate = DateTime.ParseExact(AccountModel.TransactionDate,"MM/dd/yyyy", CultureInfo.InvariantCulture);
                SqlParameter[] sqlParam =
                 {
                     new SqlParameter("@DealerCode",dealerCode),//0
                     //new SqlParameter("@ID",),//2
                     new SqlParameter("@TransactionCode",ActAutoCode),//3
                     new SqlParameter("@TransactionDate",CreateDate),//4
                     new SqlParameter("@CusCode",model.CusCode),//5
                     new SqlParameter("@AccountCode",model.CusCode),//6
                     new SqlParameter("@InvType",TrType),//7
                     new SqlParameter("@TrType",TrType),//8
                     new SqlParameter("@Narration",Narration),//9
                     new SqlParameter("@Reference",model.RefDocumentNo),//10
                     new SqlParameter("@Debit",model.TotalAmount),//11
                     new SqlParameter("@Credit",Credit),//12
                     new SqlParameter("@Balance",model.TotalAmount),//13
                     new SqlParameter("@Remarks",Narration),//14
                     new SqlParameter("@CreateDate",CreateDate),//15
                     new SqlParameter("@CreateTime",CreateTime),//16
                     new SqlParameter("@CreateUser",AuthBase.EmpCode),//17
                     new SqlParameter("@CreateTerm",AuthBase.UserId),//18
                     new SqlParameter("@UpdDate",(object)DBNull.Value),//19
                     new SqlParameter("@UpdTime",(object)DBNull.Value),//20
                     new SqlParameter("@UpdUser",(object)DBNull.Value),//21
                     new SqlParameter("@UpdTerm",(object)DBNull.Value),//22

                 };
                dt = DataAccess.getDataTable("SP_Insert_AccountTransaction", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {

                }
                leadid = strAutoCode;
                IsSaved = true;
            }
            catch (Exception ex)
            {

                //throw;
            }

            return leadid;
        }
        public static string Insert_CrdSIGridData(VehicleSaleDetailVM[] modelDetail, string trancode)
        {
            var code = "MCM01";


            if (trancode == "" || trancode == "0" || trancode == null)
            {

            }
            else
            {
                strAutoCode = trancode;
            }


            try
            {
                foreach (var item in modelDetail)
                {



                    SqlParameter[] sqlParam = {
                                     new SqlParameter("@DealerCode",code),//0
									 new SqlParameter("@TransCode",strAutoCode),//1
                                     new SqlParameter("@BrandCode",item.BrandCode),//2
                                     new SqlParameter("@ProdCode",item.ProdCode),//3
                                     new SqlParameter("@VersionCode",item.VersionCode),//4
                                     new SqlParameter("@ColorCode",item.ColorCode),//5
                                     new SqlParameter("@ColorDesc",item.ColorDesc),//6
                                     new SqlParameter("@ChassisNo",item.ChassisNo),//7
                                     new SqlParameter("@EngineNo",item.EngineNo),//8
                                     new SqlParameter("@Qty",item.Qty),//9
                                     new SqlParameter("@InstallmentPlan",item.InstallmentPlan),//10
                                     new SqlParameter("@FactoryPrice",item.FactoryPrice),//11
                                     new SqlParameter("@SalePrice",item.SalePrice),//12
                                     new SqlParameter("@Discount",item.Discount),//13
                                     new SqlParameter("@FreightCharges",item.FreightCharges),//14
                                     new SqlParameter("@MarketRate",item.MarketRate),//15
                                     new SqlParameter("@Advance",(object)DBNull.Value),//16
                                     new SqlParameter("@TotalAmount",item.TotalAmount),//17
                                     new SqlParameter("@StockType",item.StockType),//18
                                     new SqlParameter("@RecNo",item.RecNo),//19
									 
									};

                    dt = DataAccess.getDataTable("SP_Insert_VehicleSaleDetail", sqlParam, General.GetBMSConString());
                }


                if (dt.Rows.Count > 0)
                {

                }

                string leadId = strAutoCode;
                IsSaved = true;
            }
            catch (Exception)
            {

                throw;
            }
            return strAutoCode;
        }
        public static string Remove_CrdSIData(VehicleSaleMasterVM model)
        {
            string leadid = "";

            if (model.TransCode == "" || model.TransCode == "0" || model.TransCode == null)
            {


                string getNextTransCode = "declare @lastval varchar(14),@id int " +
                                           "set @id = (select count(*) from VehicleSaleMaster) " +
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
                                           "select @lastval as TransCode";

                dt = DataAccess.getDataTableByQuery(getNextTransCode, nullSqlParam, General.GetBMSConString());

                strAutoCode = dt.Rows[0]["TransCode"].ToString();

            }
            else
            {
                strAutoCode = model.TransCode; ;
            }



            try
            {
                //var Serializer = new JavaScriptSerializer();
                string transType = "CrdSI";
                string delflag = "Y";
                string dealerCode = "MCM01";

                DateTime UniDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                DateTime UniTime = Convert.ToDateTime(DateTime.Now.ToShortTimeString());
                //DateTime leadDate = DateTime.ParseExact(model.TransDate,"MM/dd/yyyy", CultureInfo.InvariantCulture);
                SqlParameter[] sqlParam =
                {
                    new SqlParameter("@DealerCode",dealerCode),//0
					new SqlParameter("@TransCode",strAutoCode),//2
					new SqlParameter("@TransDate",UniDate),//3
					new SqlParameter("@TransType",transType),//4
					new SqlParameter("@SaleType",model.SaleType),//5
					new SqlParameter("@CreditTerms",model.CreditTerms),//6
					new SqlParameter("@EmpCode",model.EmpCode),//7
					new SqlParameter("@CusCode",model.CusCode),//8
					new SqlParameter("@CustomerType",model.CustomerType),//9
					new SqlParameter("@CNICNTN",model.CNICNTN),//10
					new SqlParameter("@ContactNo",model.ContactNo),//11
					new SqlParameter("@PriceLevel",model.PriceLevel),//12
					new SqlParameter("@BillTo",model.BillTo),//13
					new SqlParameter("@ShipTo",model.ShipTo),//14
					new SqlParameter("@SameAs",model.SameAs),//15
					new SqlParameter("@TotalQty",model.TotalQty),//16
					new SqlParameter("@ServceQty",model.TotalQty),//17
					new SqlParameter("@TotalAmount",model.TotalAmount),//18
					new SqlParameter("@PaymentReceiptCode",(object)DBNull.Value),//19
					new SqlParameter("@PaidAmoun",(object)DBNull.Value),//20
					new SqlParameter("@DelFlag",delflag),//21
					new SqlParameter("@RefType",model.RefType),//22
					new SqlParameter("@RefDocumentNo",model.RefDocumentNo),//23
					new SqlParameter("@PostFlag",(object)DBNull.Value),//24
					new SqlParameter("@VoucherNo",(object)DBNull.Value),//25
					new SqlParameter("@VoucherDate",(object)DBNull.Value),//26
					new SqlParameter("@UpdUser",AuthBase.UserId),//27
					new SqlParameter("@UpdDate",UniDate),//28
					new SqlParameter("@UpdTime",UniTime),//29
					new SqlParameter("@UpdTerm",General.CurrentIP),//30
					
				};
                dt = DataAccess.getDataTable("SP_Insert_VehicleSaleMaster", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {

                }
                leadid = strAutoCode;
                IsSaved = true;
            }
            catch (Exception ex)
            {

                //throw;
            }
            return leadid;
        }

    }
}
