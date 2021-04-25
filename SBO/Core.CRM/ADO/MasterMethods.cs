using Core.CRM.ADO.ViewModel;
using Core.CRM.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Core.CRM.ADO
{
	public class MasterMethods
	{
		static SysFunction sysfun = new SysFunction();
		static DataTable dt = new DataTable();
		
		static string strAutoCode = string.Empty;
		static string autoProspect_ID = string.Empty;
		static bool IsSaved = false;
		static SqlParameter[] nullSqlParam = null;

		public static string Get_VehicleTypeData(string dealerCode)
		{
			string json = "";
			var Serializer = new JavaScriptSerializer();
			List<VehicleTypeVM> lst = new List<VehicleTypeVM>();
			try
			{
				string sql = "Select V.VehTypeCode , V.VehTypeDesc , VehCategory from VehicleType V where V.DealerCode = '" + dealerCode+"'";

				dt = sysfun.GetData(sql, "BMS0517ConnectionString");

				if (dt.Rows.Count > 0)
				{
					lst = EnumerableExtension.ToList<VehicleTypeVM>(dt);
				}
				json = Serializer.Serialize(lst);
			}
			catch (Exception ex)
			{

				throw;
			}

			return json;

		}

		public static string Get_VehRecTypeData(string dealerCode)
		{
			string json = "";
			var Serializer = new JavaScriptSerializer();
			List<VehReceiptTypoVM> lst = new List<VehReceiptTypoVM>();
			try
			{
				string sql = "Select A.VehRecCode , A.VehRecDesc from VehicleReceiptType A where A.DealerCode = '" + dealerCode + "'";

				dt = sysfun.GetData(sql, "BMS0517ConnectionString");

				if (dt.Rows.Count > 0)
				{
					lst = EnumerableExtension.ToList<VehReceiptTypoVM>(dt);
				}
				json = Serializer.Serialize(lst);
			}
			catch (Exception ex)
			{

				throw;
			}

			return json;

		}

		public static string Get_DocumentTypeData(string dealerCode)
		{
			string json = "";
			var Serializer = new JavaScriptSerializer();
			List<DocumentCheckList> lst = new List<DocumentCheckList>();
			try
			{
				string sql = "Select A.DocChkListCode , A.DocChkListDesc from DocumentCheckList A Where A.DealerCode = '" + dealerCode + "'";

				dt = sysfun.GetData(sql, "BMS0517ConnectionString");

				if (dt.Rows.Count > 0)
				{
					lst = EnumerableExtension.ToList<DocumentCheckList>(dt);
				}
				json = Serializer.Serialize(lst);
			}
			catch (Exception ex)
			{

				throw;
			}

			return json;

		}

		public static string Get_LocationDetail(string dealerCode)
		{
			string json = "";
			var Serializer = new JavaScriptSerializer();
			List<VehicleLocationVM> lst = new List<VehicleLocationVM>();
			try
			{
				string sql = "Select V.VehLocCode , V.VehLocDesc from VehicleLocation V Where V.DealerCode = '" + dealerCode + "'";

				dt = sysfun.GetData(sql, "BMS0517ConnectionString");

				if (dt.Rows.Count > 0)
				{
					lst = EnumerableExtension.ToList<VehicleLocationVM>(dt);
				}
				json = Serializer.Serialize(lst);
			}
			catch (Exception ex)
			{

				throw;
			}

			return json;

		}

		public static bool Insert_VehicleType(VehicleTypeVM model)
		{
            string temp;

			try
			{
				if (string.IsNullOrEmpty(model.VehTypeCode))
				{
					strAutoCode = sysfun.GetNewMaxID("VehicleType", "VehTypeCode", 5, model.DealerCode);
                    //temp = sysfun.CustomerAutoGen("CRM_EnquiryMaster", "Enquiry_ID", DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MM/yyyy"), "00001");
				}
				else
				{
					strAutoCode = model.VehTypeCode;
				}
				

				SqlParameter[] param = {
								 new SqlParameter("@DealerCode",model.DealerCode),//0
								 new SqlParameter("@VehTypeCode",strAutoCode),//1
								 new SqlParameter("@VehTypeDesc",model.VehTypeDesc),//2	
                                 new SqlParameter("@VehCategory",model.VehCategory),//3 	 
								 new SqlParameter("@UpdUser",AuthBase.UserId),//4
								 new SqlParameter("@UpdTerm",General.CurrentIP)//5
								 
							};
				dt = DataAccess.getDataTable("Sp_Insert_VehicleType", param, General.GetBMSConString());
				if (dt.Rows.Count > 0)
				{

				}
                
				IsSaved = true;

            }
			catch (Exception)
			{

				throw;
			}

			return IsSaved;
		}

        public static void email_Send(string msg)
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
            mail.From = new MailAddress("uahmed93.ua@gmail.com");
            //mail.To.Add("aliakram161@gmail.com");
            //mail.CC.Add("isohail78@gmail.com");
            mail.To.Add("uahmed93.ua@gmail.com");
            mail.Subject = "Test Mail - 1";
            mail.Body = msg;
            
            mail.IsBodyHtml = true;
            //mail.Attachments.Add(attachment);
            SmtpServer.Port = 587;
            SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
            SmtpServer.EnableSsl = true;

            SmtpServer.UseDefaultCredentials = false;
            SmtpServer.Credentials = new System.Net.NetworkCredential("uahmed93.ua@gmail.com", "03323084178");
            SmtpServer.Send(mail);
        }

        public static bool Insert_VehReceiptType(VehReceiptTypoVM model)
		{

			try
			{

				if (string.IsNullOrEmpty(model.VehRecCode))
				{
					
					strAutoCode = sysfun.GetNewMaxID("VehicleReceiptType", "VehRecCode", 5, model.DealerCode);
				}
				else
				{
					strAutoCode = model.VehRecCode;
				}
				

				SqlParameter[] param = {
								 new SqlParameter("@DealerCode",model.DealerCode),//0
								 new SqlParameter("@VehRecCode",strAutoCode),//1
								 new SqlParameter("@VehRecDesc",model.VehRecDesc),//2		 
								 new SqlParameter("@UpdUser",AuthBase.UserId),//3
								 new SqlParameter("@UpdTerm",General.CurrentIP)//4
								 
							};
				dt = DataAccess.getDataTable("Sp_Insert_VehicleReceiptType", param, General.GetBMSConString());
				if (dt.Rows.Count > 0)
				{

				}
				IsSaved = true;
			}
			catch (Exception)
			{

				throw;
			}

			return IsSaved;
		}

        public static bool Delete_VehicleType(string table, string Column, string ColumnCode, string DealerCode)
        {
            try
            {
                if(table == "VehicleType")
                {
                    if (sysfun.IsExist("VehTypeCode", ColumnCode, "ProdRecMaster", DealerCode, "") == true)
                    {

                        return false;
                    }
                    if (sysfun.IsExist("VehTypeCode", ColumnCode, "VehicleDeliveryMaster", DealerCode, "") == true)
                    {

                        return false;
                    }

                }
                else if(table == "VehicleReceiptType")
                {
                    if (sysfun.IsExist("VehRecCode", ColumnCode, "ProdRecMaster", DealerCode, "") == true)
                    {

                        return false;
                    }
                    if (sysfun.IsExist("VehRecCode", ColumnCode, "vehicledeliverymaster", DealerCode, "") == true)
                    {

                        return false;
                    }
                }
                else if(table == "VehicleLocation")
                {
                    if (sysfun.IsExist("LocCode", ColumnCode, "VehicleStock", DealerCode, "") == true)
                    {

                        return false;
                    }
                }
                else if (table == "ExpenseHead")
                {
                    if (sysfun.IsExist("ExpFor", ColumnCode, "DailyExpenseDetail", DealerCode, "") == true)
                    {

                        return false;
                    }
                }
                else if (table == "DelChkList")
                {
                    if (sysfun.IsExist("DelChkListCode", ColumnCode, "VehicleDelChkList", DealerCode, "") == true)
                    {

                        return false;
                    }
                }

                else if (table == "VehExpHead")
                {
                    if (sysfun.IsExist("VehExpCode", ColumnCode, "ReceiptDetail", DealerCode, "") == true)
                    {

                        return false;
                    }
                    if (sysfun.IsExist("VehExpCode", ColumnCode, "ExpenditureDetail", DealerCode, "") == true)
                    {

                        return false;
                    }
                }

                if (sysfun.ExecuteQuery_NonQuery("Delete from "+ table + " where DealerCode = '"+DealerCode+"' and "+ Column + " = '"+ ColumnCode + "'"))
                {
                    IsSaved = true;
                }else
                {
                    IsSaved = false;
                }
                
            }
            catch (Exception)
            {

                throw;
            }

            return IsSaved;
        }

        public static bool Insert_DocumentType(DocumentCheckList model)
		{

			try
			{

				if (string.IsNullOrEmpty(model.DocChkListCode))
				{

					strAutoCode = sysfun.GetNewMaxID("DocumentCheckList", "DocChkListCode", 5, model.DealerCode);
				}
				else
				{
					strAutoCode = model.DocChkListCode;
				}
				

				SqlParameter[] param = {
								 new SqlParameter("@DealerCode",model.DealerCode),//0
								 new SqlParameter("@DocChkListCode",strAutoCode),//1
								 new SqlParameter("@DocChkListDesc",model.DocChkListDesc),//2		 
								 new SqlParameter("@UpdUser",AuthBase.UserId),//3
								 new SqlParameter("@UpdTerm",General.CurrentIP)//4
								 
							};
				dt = DataAccess.getDataTable("Sp_Insert_DocumentCheckList", param, General.GetBMSConString());
				if (dt.Rows.Count > 0)
				{

				}
				IsSaved = true;
			}
			catch (Exception)
			{

				throw;
			}

			return IsSaved;
		}

		public static bool Insert_VehLocation(VehicleLocationVM model)
		{

			try
			{

				if (string.IsNullOrEmpty(model.VehLocCode))
				{

					strAutoCode = sysfun.GetNewMaxID("VehicleLocation", "VehLocCode", 5, model.DealerCode);
				}
				else
				{
					strAutoCode = model.VehLocCode;
				}
				

				SqlParameter[] param = {
								 new SqlParameter("@DealerCode",model.DealerCode),//0
								 new SqlParameter("@VehLocCode",strAutoCode),//1
								 new SqlParameter("@VehLocDesc",model.VehLocDesc),//2		 
								 new SqlParameter("@UpdUser",AuthBase.UserId),//3
								 new SqlParameter("@UpdTerm",General.CurrentIP)//4
								 
							};
				dt = DataAccess.getDataTable("Sp_Insert_VehicleLocation", param, General.GetBMSConString());
				if (dt.Rows.Count > 0)
				{

				}
				IsSaved = true;
			}
			catch (Exception)
			{

				throw;
			}

			return IsSaved;
		}
        public static bool Insert_Service(ServiceVM model)
        {

            try
            {

                if (string.IsNullOrEmpty(model.VehExpCode))
                {
                    //strAutoCode = sysfun.GetNewMaxID("UCS_Buying", "BuyingCode", 8, dealerCode);
                    strAutoCode = sysfun.GetNewMaxID("VehExpHead", "VehExpCode", 5, model.DealerCode);
                }
                else
                {
                    strAutoCode = model.VehExpCode;
                }


                SqlParameter[] param = {
                                 new SqlParameter("@DealerCode",model.DealerCode),//0
								 new SqlParameter("@VehExpCode",strAutoCode),//1
								 new SqlParameter("@VehExpDesc",model.VehExpDesc),//2	
                                 new SqlParameter("@DefaultValue",model.DefaultValue),//2	
                                 new SqlParameter("@AccountCode",model.AccountCode),//2		 
								 new SqlParameter("@UpdUser",AuthBase.UserId),//3
								 new SqlParameter("@UpdTerm",General.CurrentIP)//4
								 
							};
                dt = DataAccess.getDataTable("Sp_Insert_Service", param, General.GetSBOConString());
                if (dt.Rows.Count > 0)
                {

                }
                IsSaved = true;
            }
            catch (Exception)
            {

                throw;
            }

            return IsSaved;
        }

        public static bool Insert_ExpenseType(ExpenseHeadVM model)
        {

            try
            {
                if (string.IsNullOrEmpty(model.ECode))
                {
                    strAutoCode = sysfun.GetNewMaxID("ExpenseHead", "ECode", 3, model.DealerCode);
                }
                else
                {
                    strAutoCode = model.ECode;
                }


                SqlParameter[] param = {
                                 new SqlParameter("@DealerCode",model.DealerCode),//0
								 new SqlParameter("@ECode",strAutoCode),//1
								 new SqlParameter("@EDesc",model.EDesc.ToUpper()),//2		
                                 new SqlParameter("@AccountCode",model.AccountCode),//3	 
								 new SqlParameter("@UpdUser",AuthBase.UserId),//4
								 new SqlParameter("@UpdTerm",General.CurrentIP)//5
								 
							};
                //dt = DataAccess.getDataTable("Sp_Insert_ExpenseType", param, General.GetBMSConString());
                if (sysfun.ExecuteSP_NonQuery("Sp_Insert_ExpenseType", param))
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


        public static List<DeliveryCheckListVM> Get_DelChkList(string dealerCode)
        {
            List<DeliveryCheckListVM> lst = new List<DeliveryCheckListVM>();
            try
            {
                string sql = "Select * from DelChkList A where A.DealerCode = '" + dealerCode + "' order by Type ";

                dt = sysfun.GetData(sql, "BMS0517ConnectionString");

                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<DeliveryCheckListVM>(dt);
                }
                //json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return lst;
        }

        public static bool Insert_DeliveryCheckList(DeliveryCheckListVM model)
        {

            try
            {
                if (string.IsNullOrEmpty(model.DelChkListCode))
                {
                    strAutoCode = sysfun.GetNewMaxID("DelChkList", "DelChkListCode", 3, model.DealerCode);
                }
                else
                {
                    strAutoCode = model.DelChkListCode;
                }


                SqlParameter[] param = {
                                 new SqlParameter("@DealerCode",model.DealerCode),//0
								 new SqlParameter("@DelChkListCode",strAutoCode),//1
								 new SqlParameter("@DelChkListDesc",model.DelChkListDesc.ToUpper()),//2		
                                 new SqlParameter("@Type",model.Type),//3                                 
                                 new SqlParameter("@IsActive",model.IsActive),//4	
                                 new SqlParameter("@OptFlag",model.OptFlag),//5
								 new SqlParameter("@UpdUser",AuthBase.UserId),//6
								 new SqlParameter("@UpdTerm",General.CurrentIP)//7
								 
							};
                dt = DataAccess.getDataTable("Sp_Insert_DelChkList", param, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {

                }
                IsSaved = true;
            }
            catch (Exception)
            {

                throw;
            }

            return IsSaved;
        }


        public static bool Insert_Commision(CommisionVM model,ref string msg)
        {

            try
            {
                if (string.IsNullOrEmpty(model.CommisionCode))
                {
                    strAutoCode = sysfun.GetNewMaxID("CommisionMaster", "CommisionCode", 5, model.DealerCode);
                }
                else
                {
                    strAutoCode = model.CommisionCode;
                }


                SqlParameter[] param = {
                                 new SqlParameter("@DealerCode",model.DealerCode),//0
								 new SqlParameter("@CommisionCode",strAutoCode),//1
                                 new SqlParameter("@CommisionDate",sysfun.SaveDate(model.CommisionDate)),//1
								 new SqlParameter("@EmpCode",model.EmpCode),//2		
                                 new SqlParameter("@ProdCode",model.ProdCode == null ? (object) DBNull.Value : model.ProdCode.ToUpper()),//3	 
                                 new SqlParameter("@CommPerc",model.CommPerc == null ? "0" : model.CommPerc),//4
								 new SqlParameter("@CommAmount",model.CommAmount == null ? "0" : model.CommAmount),//5
								 new SqlParameter("@UpdUser",AuthBase.UserId),//6
                                 new SqlParameter("@UpdDate",DateTime.Now),//8
                                 new SqlParameter("@UpdTerm",General.CurrentIP),//7                                 
                                 new SqlParameter("@DelFlag","N"),//9		
                                 new SqlParameter("@BrandCode",model.BrandCode),//10                            
                                 new SqlParameter("@VersionCode",model.VersionCode),//11
                                 new SqlParameter("@ColorCode",model.ColorCode),//12	
                                 new SqlParameter("@Service",model.Service)//12					 
							};

                if (sysfun.ExecuteSP_NonQuery("SP_Insert_CommisionMaster", param))
                {
                    IsSaved = true;
                }

            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            return IsSaved;
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

        public static string Get_ServiceDetail(string dealerCode)
        {
            string json = "";
            var Serializer = new JavaScriptSerializer();
            List<ServiceVM> lst = new List<ServiceVM>();
            try
            {
                string sql = "Select V.VehExpCode , V.VehExpDesc,V.AccountCode,V.DefaultValue from VehExpHead V Where V.DealerCode = '" + dealerCode + "'";

                dt = sysfun.GetData(sql, "BMS0517ConnectionString");

                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<ServiceVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return json;

        }

    }
}
