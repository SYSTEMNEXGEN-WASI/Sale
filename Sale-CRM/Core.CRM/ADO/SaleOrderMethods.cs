using Core.CRM.ADO.ViewModel;
using Core.CRM.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Core.CRM.ADO
{
	public class SaleOrderMethods
	{
		static DataTable dt = new DataTable();
		static string strAutoCode = string.Empty;
		static string autoProspect_ID = string.Empty;
		static bool IsSaved = false;
		static  SqlParameter[] nullSqlParam = null;
        string dealerCode = "MCM01";

		public static string Insert_SaleOrder(VehicleSaleMasterVM model)
		{
			string leadid = "";
			
			if(model.TransCode == "" || model.TransCode == "0" || model.TransCode==null)
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
                string transType="SO";
                string delflag="N";
               // string dealerCode = "MCM01";
                string EmpCode = "001";
                string CudCode="001";
                DateTime UniDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                DateTime UniTime = Convert.ToDateTime(DateTime.Now.ToShortTimeString());
                //DateTime leadDate = DateTime.ParseExact(model.TransDate,"MM/dd/yyyy", CultureInfo.InvariantCulture);
				SqlParameter[] sqlParam = 
				{
					new SqlParameter("@DealerCode",model.DealerCode),//0
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
					new SqlParameter("@ServceQty",(object)DBNull.Value),//17
					new SqlParameter("@TotalAmount",(object)DBNull.Value),//18
					new SqlParameter("@PaymentReceiptCode",(object)DBNull.Value),//19
					new SqlParameter("@PaidAmoun",(object)DBNull.Value),//20
					new SqlParameter("@DelFlag",delflag),//21
					new SqlParameter("@RefType",(object)DBNull.Value),//22
					new SqlParameter("@RefDocumentNo",(object)DBNull.Value),//23
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

				throw;
			}
			return leadid;
		}

        public static string Insert_VehicleSaleDetail(VehicleSaleDetailVM[] modelDetail,string trancode)
		{
            var code = "MCM01";


            if (trancode == "" || trancode == "0" || trancode == null)
            {

            }
            else
            {
                strAutoCode = trancode ;
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
                                     new SqlParameter("@ColorDesc",(object)DBNull.Value),//6
                                     new SqlParameter("@ChassisNo",(object)DBNull.Value),//7
                                     new SqlParameter("@EngineNo",(object)DBNull.Value),//8
                                     new SqlParameter("@Qty",item.Qty),//9
                                     new SqlParameter("@InstallmentPlan",item.InstallmentPlan),//10
                                     new SqlParameter("@FactoryPrice",(object)DBNull.Value),//11
                                     new SqlParameter("@SalePrice",(object)DBNull.Value),//12
                                     new SqlParameter("@Discount",(object)DBNull.Value),//13
                                     new SqlParameter("@FreightCharges",(object)DBNull.Value),//14
                                     new SqlParameter("@MarketRate",(object)DBNull.Value),//15
                                     new SqlParameter("@Advance",(object)DBNull.Value),//16
                                     new SqlParameter("@TotalAmount",(object)DBNull.Value),//17
                                     new SqlParameter("@StockType",(object)DBNull.Value),//18
                                     new SqlParameter("@RecNo",(object)DBNull.Value),//19
									 
									};

                    dt = DataAccess.getDataTable("SP_Insert_VehicleSaleDetail", sqlParam, General.GetBMSConString());
                }


				if (dt.Rows.Count > 0)
				{

				}

                string leadId =strAutoCode;
				IsSaved = true;
			}
			catch (Exception)
			{
				
				throw;
			}
			return strAutoCode;
		}

        public static string Remove_SaleOrder(VehicleSaleMasterVM model)
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
                string transType = "SO";
                string delflag = "Y";
                string dealerCode = "MCM01";
                string EmpCode = "001";
                string CudCode = "001";
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
					new SqlParameter("@TotalQty",(object)DBNull.Value),//16
					new SqlParameter("@ServceQty",(object)DBNull.Value),//17
					new SqlParameter("@TotalAmount",(object)DBNull.Value),//18
					new SqlParameter("@PaymentReceiptCode",(object)DBNull.Value),//19
					new SqlParameter("@PaidAmoun",(object)DBNull.Value),//20
					new SqlParameter("@DelFlag",delflag),//21
					new SqlParameter("@RefType",(object)DBNull.Value),//22
					new SqlParameter("@RefDocumentNo",(object)DBNull.Value),//23
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

        public static List<GetVehicleDetailVM> Get_SaleDetailData()
        {
            string json = "";
            var Serializer = new JavaScriptSerializer();
            string DealerCode = "MCM01";
            List<GetVehicleDetailVM> lst = new List<GetVehicleDetailVM>();
            try
            {
                SqlParameter[] sqlParam = {
									new SqlParameter("@DealerCode",DealerCode),//0
									
									};

                dt = DataAccess.getDataTable("SP_Get_VehicleSaleDetail", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<GetVehicleDetailVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {
                throw;
            }

            return lst;
        }

        public static List<GetVehicleGridDataVM> GridDataa(string TransCode)
        {
            string json = "";
            var Serializer = new JavaScriptSerializer();
            string DealerCode = "MCM01";
            List<GetVehicleGridDataVM> lst = new List<GetVehicleGridDataVM>();
            try
            {
                SqlParameter[] sqlParam = {
									new SqlParameter("@DealerCode",DealerCode),//0
                                    new SqlParameter("@TransCode",TransCode),//1
									};

                dt = DataAccess.getDataTable("SP_Get_VehicleSaleGridData", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<GetVehicleGridDataVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return lst;
        }

        public static List<VehicleSaleDetailVM> DeleteGridDataa()
        {
            string json = "";
            var Serializer = new JavaScriptSerializer();
            string DealerCode = "MCM01";
            List<VehicleSaleDetailVM> lst = new List<VehicleSaleDetailVM>();
            try
            {
                SqlParameter[] sqlParam = {
									new SqlParameter("@DealerCode",DealerCode),//0
									
									};

                dt = DataAccess.getDataTable("SP_Delete_VehicleSaleGridData", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<VehicleSaleDetailVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return lst;
        }



        public static List<GetRegNoRecVM> Get_RegNoDetailData(string dealerCode)
        {
            DataTable dt = new DataTable();
            string json = "";
            var Serializer = new JavaScriptSerializer();
            // string TransType = "CSI";
            //string SaleType = "Cash";


            List<GetRegNoRecVM> lst = new List<GetRegNoRecVM>();
            try
            {
                SqlParameter[] sqlParam = {
                                    new SqlParameter("@DealerCode",dealerCode)//0
                                    // , new SqlParameter("@TransType",TransType)//0
                                    //, new SqlParameter("@SaleType",SaleType)//0
									};

                dt = DataAccess.getDataTable("Select_PriceOffer_RegNo", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<GetRegNoRecVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {
                throw;
            }

            return lst;
        }


    }
}
