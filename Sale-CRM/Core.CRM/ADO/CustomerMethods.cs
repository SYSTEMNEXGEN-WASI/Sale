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
	public class CustomerMethods
	{
		static bool IsSaved = false;
		static bool IsDeleted = false;
		static SqlParameter[] nullSqlParam = null;
		static DataTable dt = new DataTable();
		static string strAutoCode = string.Empty;
		static SysFunction sysfun = new SysFunction();
		static DateTime recDate = new DateTime();

		static Transaction ObjTrans = new Transaction();
		static SqlTransaction Trans;

		public static bool Insert_Customer(CustomerVM model, string dealerCode)
		{
            string csgNo;
			try
			{
				if (model.CusCode == "" || model.CusCode == null)
				{
					strAutoCode = sysfun.GetNewMaxID("Customer", "CusCode", 8, dealerCode);
					
				}

				else
				{
					strAutoCode = model.CusCode;

				}
                if(model.CSGNo == null) {
                    csgNo = sysfun.CustomerAutoGen("Customer", "CSGNo", DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MM/yyyy"), dealerCode);
                }
                else
                {
                    csgNo = model.CSGNo;
                }
                SqlParameter[] param = {
							   new SqlParameter("@DealerCode",dealerCode),//0
							   new SqlParameter("@CusCode",strAutoCode),//1
							   new SqlParameter("@CusDesc",model.CusDesc),//2
							   new SqlParameter("@FatherHusName",""),//3
							   new SqlParameter("@Address1",model.Address1),//4
							   new SqlParameter("@Address2",(object)DBNull.Value),//5
							   new SqlParameter("@Address3",(object)DBNull.Value),//6
							   new SqlParameter("@ContPerson",model.ContPerson),//7
							   new SqlParameter("@CreditDays",model.CreditDays ),//8
							   new SqlParameter("@CreditLimit",model.CreditLimit ),//9
							   new SqlParameter("@NIC",model.NIC),//10
							   new SqlParameter("@CusTypeCode",model.CusTypeCode),//11
							   new SqlParameter("@DOB",(object)DBNull.Value ),//12
							   new SqlParameter("@Phone1",model.Phone1),//13
							   new SqlParameter("@Phone2",model.Phone2),//14
							   new SqlParameter("@CellNo",model.CellNo),//15
							   new SqlParameter("@FaxNo",model.FaxNo),//16
							   new SqlParameter("@Email",model.Email),//17
							   new SqlParameter("@URL",model.URL),//18
							   new SqlParameter("@NTNno",model.NTNno),//19
							   new SqlParameter("@AdvanceReceipt",(object)DBNull.Value ),//20
							   new SqlParameter("@UpdUser",AuthBase.UserId),//21
							   new SqlParameter("@UpdTerm",General.CurrentIP),//22
							   new SqlParameter("@SalesTaxRegNo",model.SalesTaxRegNo),//23
							   new SqlParameter("@Behavior",(object)DBNull.Value),//24
							   new SqlParameter("@Remarks",(object)DBNull.Value),//25
							   new SqlParameter("@Distance",model.Distance),//26
							   new SqlParameter("@CountryCode", model.CountryCode),//27
							   new SqlParameter("@CityCode", model.CityCode),//28
							   new SqlParameter("@StateCode", model.StateCode),//29
							   new SqlParameter("@Title", model.Title),//30
							   new SqlParameter("@CSGNo", csgNo),//31
							   new SqlParameter("@MCNo", model.MCNo),//32
                               new SqlParameter("@AccountCode", model.AccountCode) // 33
                               };

				if (sysfun.ExecuteSP_NonQuery("sp_Insert_Customer", param))
				{				

                    IsSaved = true;
				}

			}
			catch (Exception ex)
			{
                throw;
			}

			return IsSaved;
		}

		public static bool Insert_Guarantor(List<GuarantorVM> model, string dealerCode)
		{
			string GRCode="";
			try
			{
                foreach (var item in model)
                {
                    if (item.GRCode == "" || item.GRCode == null)
                    {
                        GRCode = sysfun.AutoGen("Guarantor", "GRCode", DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MM/yyyy"), dealerCode);

                    }
                    else
                    {
                        GRCode = item.GRCode;

                    }
                    SqlParameter[] param2 = {
                               new SqlParameter("@DealerCode",dealerCode),//0
							   new SqlParameter("@CusCode",strAutoCode),//1
							   new SqlParameter("@GRCode",GRCode),//2
							   new SqlParameter("@GRDesc",item.GRDesc),//3
							   new SqlParameter("@Address1",item.Address1),//4
							   new SqlParameter("@NIC",item.NIC),//5
							   new SqlParameter("@Phone1",item.Phone1),//6
							   new SqlParameter("@CellNo",item.CellNo),//7
							   new SqlParameter("@UpdUser",AuthBase.UserId),//8
							   new SqlParameter("@UpdTerm",General.CurrentIP ),//9
							   new SqlParameter("@Remarks",item.Remarks ),//10
							   new SqlParameter("@CountryCode",item.CountryCode),//11
							   new SqlParameter("@StateCode",item.StateCode),//12
							   new SqlParameter("@CityCode",item.CityCode ),//13                              
							   new SqlParameter("@Active",item.Active ),//14                           
				    };


                    if (sysfun.ExecuteSP_NonQuery("SP_Insert_Guarantor", param2))
                    {
                        IsSaved = true;
                    }
                    else
                    {
                        IsSaved = false;
                    }
                }
			}
			catch (Exception)
			{
				throw;
			}

			return IsSaved;
		}

		public static string Get_CustomerData(string enquiryId, string dealerCode)
		{
			string json = "";
			var Serializer = new JavaScriptSerializer();
			List<CustomerVM> lst = new List<CustomerVM>();
            DataSet ds = new DataSet();
            try
			{
				SqlParameter[] sqlParam = {
									new SqlParameter("@DealerCode",dealerCode),//1
									new SqlParameter("@CusCode",enquiryId)//0
									
									};

                //dt = DataAccess.getDataTable("SP_Select_DeliveryOrder", sqlParam, General.GetBMSConString());
                sysfun.CodeExists("Customer", "CusCode", enquiryId, dealerCode , ref ds);

                dt = ds.Tables[0];


                if (dt.Rows.Count > 0)
				{
					lst = EnumerableExtension.ToList<CustomerVM>(dt);
				}
				json = Serializer.Serialize(lst);
			}
			catch (Exception ex)
			{

				throw;
			}

			return json;
		}


        public static string Get_GuarantorData(string enquiryId, string dealerCode)
        {
            string json = "";
            var Serializer = new JavaScriptSerializer();
            List<GuarantorVM> lst = new List<GuarantorVM>();
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] sqlParam = {
                                    new SqlParameter("@DealerCode",dealerCode),//1
									new SqlParameter("@CusCode",enquiryId)//0
									
									};

                dt = DataAccess.getDataTable("SP_Get_CustomerGuarantor", sqlParam, General.GetBMSConString());
                //if(sysfun.("Guarantor", "CusCode", enquiryId, dealerCode, ref ds))
                //{
                //    dt = ds.Tables[0];
                //}else
                //{
                //    return json;
                //}           
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<GuarantorVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return json;
        }

        public static bool Delete_Customer_Record(string enquiryId, string dealerCode)
        {
            DataSet ds = new DataSet();

            if (sysfun.IsExist("CusCode", enquiryId, "Customer", dealerCode, "") == false)
            {
                
                return false;
            }
            if (sysfun.IsExist("CusCode", enquiryId, "BookOrdMaster", dealerCode, "") == true)
            {
                
                return false;
            }

            // If Customer used in Invoiced name in booking order then should not be delete
            if (sysfun.IsExist("DisplayCode", enquiryId.Trim(), "BookOrdMaster", dealerCode, "") == true)
            {
                
                return false;
            }

            //If Customer has vehicle in Customer vheicle table then customer should not be delete
            //{
            if (sysfun.IsExist("CusCode", enquiryId.Trim(), "CustomerVehicle", dealerCode, "") == true)
            {
                
                return false;
            }
            //}

            //If Customer used in Jobcard then should not be delete
            //{
            if (sysfun.IsExist("CusCode", enquiryId.Trim(), "JobCardMaster", dealerCode, "") == true)
            {
                
                return false;
            }
            //}

            //If Customer used in Counter then should not be delete
            //{
            if (sysfun.IsExist("CusCode", enquiryId.Trim(), "CountersaleMaster", dealerCode, "") == true)
            {
                
                return IsDeleted = false; ;
            }

            SqlParameter[] param = {
                new SqlParameter("@DealerCode",dealerCode),
                new SqlParameter("@CusCode",enquiryId)
            };

            if (sysfun.ExecuteSP_NonQuery("sp_Sales_Delete_Customer", param))
            {
                IsDeleted = true;
            }
            else
            {
                IsDeleted = false;
            }


            return IsDeleted;
        }

        public static List<CustomerVM> GetCustomerModal(string dealerCode)
        {
            List<CustomerVM> lst = new List<CustomerVM>();
            try
            {
                DataTable dt = new DataTable();               

                SqlParameter[] sqlParam =
                    {
                    new SqlParameter("@DealerCode",dealerCode)
                };
                dt = DataAccess.getDataTable("SP_Select_CustomerModal", sqlParam, General.GetBMSConString());

                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<CustomerVM>(dt);
                }

            }
            catch (Exception ex)
            {

                //throw;
            }
            return lst;
        }
        public static List<ProspectDetailVM> GetPreCustomerModal(string dealerCode)
        {
            List<ProspectDetailVM> lst = new List<ProspectDetailVM>();
            try
            {
                DataTable dt = new DataTable();

                SqlParameter[] sqlParam =
                    {
                    new SqlParameter("@DealerCode",dealerCode)
                };
                dt = DataAccess.getDataTable("SP_Select_PreCustomerModal", sqlParam, General.GetBMSConString());

                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<ProspectDetailVM>(dt);
                }

            }
            catch (Exception ex)
            {

                //throw;
            }
            return lst;
        }
        public static List<AccountVM> Get_AccDesc(string dealerCode)
        {
            DataTable dt = new DataTable();
            string json = "";
            var Serializer = new JavaScriptSerializer();
            // string TransType = "CSI";
            //string SaleType = "Cash";


            List<AccountVM> lst = new List<AccountVM>();
            try
            {
                SqlParameter[] sqlParam = {
                                    new SqlParameter("@CompCode",dealerCode)//0
                                    // , new SqlParameter("@TransType",TransType)//0
                                    //, new SqlParameter("@SaleType",SaleType)//0
									};

                dt = DataAccess.getDataTable("SP_Select_AccountCode", sqlParam, General.GetFAMConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<AccountVM>(dt);
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
