using Core.CRM.ADO.ViewModel;
using Core.CRM.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Core.CRM.ADO
{
	public class EnquiryMethods
	{
		static EnquiryMasterVM response = new EnquiryMasterVM();
		static DataTable dt = new DataTable();
		static string strAutoCode = string.Empty;
		static string autoProspect_ID = string.Empty;
		static bool IsSaved = false;
		static  SqlParameter[] nullSqlParam = null;
		static Transaction ObjTrans = new Transaction();
		static SqlTransaction Trans;
		static SysFunction sysfunc = new SysFunction();
		static StringBuilder mailMsg = new StringBuilder();
        static string email;

        public static List<EnquiryMasterVM> Get_EnquiryModal(string dealerCode)
        {

            var Serializer = new JavaScriptSerializer();
            List<EnquiryMasterVM> lst = new List<EnquiryMasterVM>();
            try
            {
                //var Serializer = new JavaScriptSerializer();
                SqlParameter[] sqlParam =
                {
                    new SqlParameter("@DealerCode",dealerCode)
                };
                dt = DataAccess.getDataTable("SP_EnquiryID", sqlParam, General.GetBMSConString());

                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<EnquiryMasterVM>(dt);
                }

            }
            catch (Exception ex)
            {

                //throw;
            }
            return lst;
        }
        public static string Get_PendingModal(string dealerCode)
        {

            string json = "";
            var Serializer = new JavaScriptSerializer();
            List<EnquiryMasterVM> lst = new List<EnquiryMasterVM>();
            try
            {
                //var Serializer = new JavaScriptSerializer();
                SqlParameter[] sqlParam =
                {
                    new SqlParameter("@DealerCode",dealerCode)
                };
                dt = DataAccess.getDataTable("SP_EnquiryID_PendingFollowsUp", sqlParam, General.GetBMSConString());

                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<EnquiryMasterVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                //throw;
            }
            return json;
        }

        public static string Insert_EnquiryMaster(EnquiryMasterVM model , string dealerCode)
		{
			string leadid = "";
			string prospectid = "";

            email = model.Email == null ? "" : model.Email;

			if(model.Enquiry_ID == "" || model.Enquiry_ID == null )
			{
				
				strAutoCode = sysfunc.AutoGen("CRM_EnquiryMaster", "Enquiry_ID", DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MM/yyyy"), dealerCode);

			}
			else
			{
				strAutoCode = model.Enquiry_ID;
			}

			if (model.ProspectID == "" || model.ProspectID == "0" || model.ProspectID == null)
			{
				
				autoProspect_ID = sysfunc.AutoGen("CRM_Prospect", "ProspectID", DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MM/yyyy"), dealerCode);
			}
			else
			{
				autoProspect_ID = model.ProspectID;
			}

			try
			{
				//var Serializer = new JavaScriptSerializer();
				//DateTime leadDate = DateTime.ParseExact(model.EnqDate,"MM/dd/yyyy", CultureInfo.InvariantCulture);
				SqlParameter[] sqlParam = 
				{
					new SqlParameter("@DealerCode",dealerCode),//0
					new SqlParameter("@Enquiry_ID",strAutoCode),//1
					new SqlParameter("@EnqDate",sysfunc.SaveDate(model.EnqDate)),//2
					new SqlParameter("@Interest",model.Interest),//3
					new SqlParameter("@ProspectID",autoProspect_ID),//4
					new SqlParameter("@EmpCode",model.EmpCode),//5
					new SqlParameter("@ProspectTypeID",model.ProspectTypeID),//6
					new SqlParameter("@CompanyDetail",model.CompanyDetail),//7
					new SqlParameter("@EnquiryMode",model.EnquiryModeID),//8
					new SqlParameter("@EnquirySource",model.EnquirySourceID),//9
					new SqlParameter("@Event_Campaign",model.Event),//10
					new SqlParameter("@EnquiryStatus",(object)DBNull.Value),//11
					new SqlParameter("@TestDriveGiven",(object)DBNull.Value),//12
					new SqlParameter("@CashFinanced",model.CashFinanced),//13
					new SqlParameter("@IsFinanced",model.IsFinanced),//14
					new SqlParameter("@FinancedThrough",(object)DBNull.Value),//15
					new SqlParameter("@FinancedDetail",(object)DBNull.Value),//16
					new SqlParameter("@FinancedBank",model.FinancedBank),//17
					new SqlParameter("@InsuranceThrough",(object)DBNull.Value),//18
					new SqlParameter("@InsuranceDetail",(object)DBNull.Value),//19
					new SqlParameter("@ProspectRequist",(object)DBNull.Value),//20
					new SqlParameter("@Remarks",model.Remarks),//21
					new SqlParameter("@NextFollowupDate",(object)DBNull.Value),//22
					new SqlParameter("@NextFollowupTime",(object)DBNull.Value),//23
					new SqlParameter("@ActionPlan",(object)DBNull.Value),//24
					new SqlParameter("@Purpose",(object)DBNull.Value),//25
					new SqlParameter("@LikelyPurchaseDate",(object)DBNull.Value),//26
					new SqlParameter("@CreatedBy",model.CreatedBy),//27
					new SqlParameter("@RefBy",(object)DBNull.Value),//28
					new SqlParameter("@RegMobile",(object)DBNull.Value),//29
					new SqlParameter("@FinanceAppliedDate",(object)DBNull.Value),//30
					new SqlParameter("@FinanceApprovedDate",(object)DBNull.Value),//31
					new SqlParameter("@ProspectLost",(object)DBNull.Value),//32
					new SqlParameter("@LostReason",(object)DBNull.Value),//33
					new SqlParameter("@LostByDealer",(object)DBNull.Value),//34
					new SqlParameter("@LostByModel",(object)DBNull.Value),//35
					new SqlParameter("@IsDeleted",model.IsDeleted),//36
					new SqlParameter("@IsMatured",model.IsMatured),//37
					new SqlParameter("@TransferStatus",(object)DBNull.Value),//38
					new SqlParameter("@EnquiryType",model.EnquiryTypeID),//39
					new SqlParameter("@Gridstate",false),//40
					new SqlParameter("@PMatured",(object)DBNull.Value),//41
					new SqlParameter("@PartiallyLost",false),//42
					new SqlParameter("@UpdUser",AuthBase.UserId),//43
					new SqlParameter("@UpdTerm",General.CurrentIP),//44
					new SqlParameter("@SoftwareVersion",(object)DBNull.Value),//45
					new SqlParameter("@Blocked",false),//46
					new SqlParameter("@Campaign",model.Campaign),//47
					new SqlParameter("@Replacement",(object)DBNull.Value),//48
					new SqlParameter("@Addtional",(object)DBNull.Value),//49
					new SqlParameter("@ExChange",(object)DBNull.Value),//50
					new SqlParameter("@Mobile",model.Mobile),//51
					new SqlParameter("@InvoiceDetail",model.InvoiceDetail),//52
					new SqlParameter("@VehicleSegments",model.SegmentID)//53
				};

				if (ObjTrans.BeginTransaction(ref Trans) == true)
				{
					sysfunc.ExecuteSP_NonQuery("SP_Insert_EnquiryMaster", sqlParam, Trans);

					leadid = strAutoCode + "," + autoProspect_ID;
					IsSaved = true;
                    mailMsg.AppendLine("<html><table>");

					mailMsg.AppendLine("<tr><td>Sale Person : </td><td>" + sysfunc.GetStringValuesAgainstCodes("EmpCode", model.EmpCode, "EmpName", "DealerEmp", "", dealerCode) + "</td></tr>");
					//mailMsg.Append(Environment.NewLine);
					mailMsg.AppendLine("<tr><td>Enquiry ID : </td><td>" + strAutoCode + "</td></tr>");
					//mailMsg.Append(Environment.NewLine);
					mailMsg.AppendLine("<tr><td>Enquiry Date : </td><td>" + model.EnqDate + "</td></tr>");
					//mailMsg.Append(Environment.NewLine);
					mailMsg.AppendLine("<tr><td>Source : </td><td>" + sysfunc.GetStringValuesAgainstCodes("EnquirySourceID", model.EnquirySourceID, "EnquirySource", "CRM_EnquirySource","",dealerCode) + "</td></tr>");
					//mailMsg.Append(Environment.NewLine);
				}
	//                dt = DataAccess.getDataTable("SP_Insert_EnquiryMaster", sqlParam, General.GetBMSConString());
				//if (dt.Rows.Count > 0)
				//{
					
				//}
				
			}
			catch (Exception ex)
			{
                if(ObjTrans.sql.State == ConnectionState.Open)
                {
                    leadid = null;
                    ObjTrans.RollBackTransaction(ref Trans);
                }

                throw;
            }
			return leadid;
		}

		public static bool Insert_ProspectDetail(ProspectDetailVM model , string dealerCode)
		{
            DateTime dob = new DateTime();

            try
			{
                //if(model.DOB != null)
                    //dob = DateTime.ParseExact(model.DOB, "MM/dd/yyyy", CultureInfo.InvariantCulture);               
				

				SqlParameter[] sqlParam = {
									 new SqlParameter("@ProspectID",autoProspect_ID),//0
									 new SqlParameter("@DealerCode",dealerCode),//1
									 new SqlParameter("@ProspectTitle",model.ProspectTitle),//2
									 new SqlParameter("@Name",model.Name),//3
									 new SqlParameter("@FatherName",model.FatherName),//4
									 new SqlParameter("@ProspectType",model.ProspectType),//5
									 new SqlParameter("@NIC",(object)DBNull.Value),//6
									 new SqlParameter("@NTN",model.NTN),//7
									 new SqlParameter("@ResAddress",model.ResAddress),//8
									 new SqlParameter("@ResCityCode",model.ResCityCode),//9
									 new SqlParameter("@ResPhone",model.ResPhone),//10
									 new SqlParameter("@Mobile",model.Mobile),//11
									 new SqlParameter("@OfficeAddress",model.OfficeAddress),//12
									 new SqlParameter("@OffCityCode",model.OffCityCode),//13
									 new SqlParameter("@OffPhone",model.OffPhone),//14
									 new SqlParameter("@Fax",(object)DBNull.Value),//15
									 new SqlParameter("@Gender",(object)DBNull.Value),//16
									 new SqlParameter("@DOB",(model.DOB == null ? (object)DBNull.Value :sysfunc.SaveDate(model.DOB))),//17
									 new SqlParameter("@WeddingAnniversary",(object)DBNull.Value),//18
									 new SqlParameter("@Profession",model.Profession),//19
									 new SqlParameter("@Designation",model.Designation),//20
									 new SqlParameter("@Hobbies",(object)DBNull.Value),//21
									 new SqlParameter("@Remarks",(object)DBNull.Value),//22
									 new SqlParameter("@Education",model.Education),//23
									 new SqlParameter("@Income",(object)DBNull.Value),//24
									 new SqlParameter("@Email",model.Email),//25
									 new SqlParameter("@Createdby",model.Createdby),//26
									 new SqlParameter("@IsDeleted",false),//27
									 new SqlParameter("@CusCode",(object)DBNull.Value),//28
									 new SqlParameter("@CompanyDetail",model.CompanyDetail),//29
									 new SqlParameter("@ContactPerson",model.ContactPerson),//30
									 new SqlParameter("@UpdUser",AuthBase.UserId),//31
									 new SqlParameter("@UpdTerm",General.CurrentIP),//32
									};

				if (sysfunc.ExecuteSP_NonQuery("SP_Insert_Prospect", sqlParam, Trans) == true)
				{
					mailMsg.AppendLine("<tr><td>Customer Name : </td><td>" + model.Name + "</td></tr>");
					//mailMsg.Append(Environment.NewLine);
					IsSaved = true;
				}
				else
				{
					ObjTrans.RollBackTransaction(ref Trans);
					IsSaved = false;
				}

				//dt = DataAccess.getDataTable("SP_Insert_Prospect", sqlParam, General.GetBMSConString());
				//if (dt.Rows.Count > 0)
				//{

				//}

				//IsSaved = true;
			}
			catch (Exception)
			{
				ObjTrans.RollBackTransaction(ref Trans);	
				throw;
			}
			return IsSaved;
		}

		public static bool Insert_EnquiryDetail(List<EnquiryDetailVM> model , string dealerCode)
		{
			int count = 0;
			try
			{
				foreach (var item in model)
				{
					if (item.BrandCode != "" && item.BrandCode != null)
					{

						SqlParameter[] sqlParam = {
									new SqlParameter("@Enquiry_ID",strAutoCode),//0
									new SqlParameter("@BrandCode",item.BrandCode),//1
									new SqlParameter("@ProdCode",item.ProdCode),//2
									new SqlParameter("@VersionCode",item.VersionCode),//3
									new SqlParameter("@ColorCode",item.ColorCode),//4
									new SqlParameter("@Qty",item.Qty),//5
									new SqlParameter("@RequiredDate",(object)DBNull.Value),//6
									new SqlParameter("@Remarks",item.Remarks),//7
									new SqlParameter("@Blocked",false),//8
									new SqlParameter("@BlockedDate",(object)DBNull.Value),//9
									new SqlParameter("@BlockedBy",(object)DBNull.Value),//10
									new SqlParameter("@PrimaryModel",(object)DBNull.Value),//11
									new SqlParameter("@StatusCode",item.StatusCode),//12
									new SqlParameter("@IsDeleted",false),//13
									new SqlParameter("@DealerCode",dealerCode)//14
									//new SqlParameter("@FurtherContact",model.FurtherContact),//15
									//new SqlParameter("@FurtherDate",furhterdate)//16
											
									};

						if (sysfunc.ExecuteSP_NonQuery("SP_Insert_EnquiryDetail", sqlParam, Trans) == true)
						{
							mailMsg.AppendLine("<tr><td>ProdCode : </td><td>" + item.ProdCode + "</td></tr>");
							//mailMsg.Append(Environment.NewLine);
							IsSaved = true;
						}
						else
						{
							ObjTrans.RollBackTransaction(ref Trans);
							IsSaved = false;
						}
					}
					count++;
				}

				//ObjTrans.CommittTransaction(ref Trans);
			}
			catch (Exception)
			{
				ObjTrans.RollBackTransaction(ref Trans);
				throw;
			}
			return IsSaved;
		}

		public static void email_Send()
		{
            mailMsg.AppendLine("</table></html>");
            string msg = mailMsg.ToString();

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

		public static string Insert_QuickEnquiry(QuickEnquiryVM model , string dealerCode)
		{
            string msg = "";
			try
			{

                if(sysfunc.IsExist("Name",model.Name, "CRM_Prospect",dealerCode, "and Mobile = '" + model.Mobile + "'"))
                {
                    msg = "Enquiry for this Customer is already generated";
                    //return Json(new { Success = false, Message = "Enquiry for this Customer is already generated" });
                    return msg;
                }
                else
                {
                    if (model.Enquiry_ID == "" || model.Enquiry_ID == null)
                    {
                        strAutoCode = sysfunc.AutoGen("CRM_EnquiryMaster", "Enquiry_ID", DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MM/yyyy"), dealerCode);
                    }
                    else
                    {
                        strAutoCode = model.Enquiry_ID;
                    }
                    if (model.ProspectID == "" || model.ProspectID == null)
                    {
                        autoProspect_ID = sysfunc.AutoGen("CRM_Prospect", "ProspectID", DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MM/yyyy"), dealerCode);
                    }
                    else
                    {
                        autoProspect_ID = model.ProspectID;
                    }

               


                    SqlParameter[] param = {
                                 new SqlParameter("@DealerCode",dealerCode),//0
								 new SqlParameter("@Enquiry_ID",strAutoCode),//1
								 new SqlParameter("@EnqDate",General.CurrentDate),//2
								 new SqlParameter("@ProspectID",autoProspect_ID),//3
								 new SqlParameter("@EmpCode",model.EmpCode),//4
								 new SqlParameter("@CashFinanced",model.CashFinanced),//5
								 new SqlParameter("@IsFinanced",model.IsFinanced),//6
								 new SqlParameter("@CreatedBy",model.CreatedBy),//7
								 new SqlParameter("@UpdUser",AuthBase.UserId),//8
								 new SqlParameter("@UpdTerm",General.CurrentIP),//9
								 new SqlParameter("@BrandCode",model.BrandCode),//10
								 new SqlParameter("@ProdCode",model.ProdCode),//11
								 new SqlParameter("@ColorCode",model.ColorCode),//12
								 new SqlParameter("@Qty",model.Qty),//13
								 new SqlParameter("@Name",model.Name),//14
								 new SqlParameter("@Mobile",model.Mobile),//15
								 new SqlParameter("@Title",model.ProspectTitle),//16
                                 new SqlParameter("@VersionCode",model.VersionCode)//17
							};
                    dt = DataAccess.getDataTable("Insert_QuickEnquiryMaster", param, General.GetBMSConString());
                    if (dt.Rows.Count > 0)
                    {

                        //mailMsg = sysfunc.GetData("Select EmpName from DealerEmp where DealerCode = '" + dealerCode + "' and EmpCode = '" + model.EmpCode + "'");
                    }
                    msg = "Done";
                }
			   
					
			}
			catch (Exception ex)
			{
                msg = ex.Message;
                return msg;
				//throw;
			}

			return msg;
		}

		public static string Get_EmailAddress(string enquiryId, string dealerCode)
		{
			string json = "";
			
			try
			{
				dt = sysfunc.GetData("Select Email from DealerEmp where DealerCode = '" + dealerCode + "' and EmpCode = '" + enquiryId + "'");
				
				if (dt.Rows.Count > 0)
				{
					json = dt.Rows[0]["Email"].ToString();

				}
				
			}
			catch (Exception ex)
			{

				throw;
			}

			return json;
		}

		public static string Get_EnquiryMasterData(string enquiryId, string dealerCode)
		{
			string json = "";
			var Serializer = new JavaScriptSerializer();
			List<RequestEnquiryMasterVM> lst = new List<RequestEnquiryMasterVM>();
			try
			{
				SqlParameter[] sqlParam = {
									new SqlParameter("@EnquiryId",enquiryId),//0
									new SqlParameter("@DealerCode",dealerCode)//1
									};

				dt = DataAccess.getDataTable("Select_EnquiryMaster", sqlParam, General.GetBMSConString());
				if (dt.Rows.Count > 0)
				{
					lst = EnumerableExtension.ToList<RequestEnquiryMasterVM>(dt);
				}
				json = Serializer.Serialize(lst);
			}
			catch (Exception ex)
			{
				
				throw;
			}

			return json;
		}

		public static string Get_PreCustomerData(string enquiryId, string dealerCode)
		{
			string json = "";
			var Serializer = new JavaScriptSerializer();
			List<ProspectDetailVM> lst = new List<ProspectDetailVM>();
			try
			{
				SqlParameter[] sqlParam = {
									new SqlParameter("@EnquiryId",enquiryId),//0
									new SqlParameter("@DealerCode",dealerCode)//1
									};

				dt = DataAccess.getDataTable("Select_PreCustomer", sqlParam, General.GetBMSConString());
				if (dt.Rows.Count > 0)
				{
					lst = EnumerableExtension.ToList<ProspectDetailVM>(dt);
				}
				json = Serializer.Serialize(lst);
			}
			catch (Exception ex)
			{

				throw;
			}

			return json;
		}

		public static string Get_EnquiryDetailData(string enquiryId, string dealerCode)
		{
			string json = "";
			var Serializer = new JavaScriptSerializer();
			List<RequestEnquiryDetailVM> lst = new List<RequestEnquiryDetailVM>();
			try
			{
				SqlParameter[] sqlParam = {
									new SqlParameter("@EnquiryId",enquiryId),//0
									new SqlParameter("@DealerCode",dealerCode)//1
									};

				dt = DataAccess.getDataTable("Select_EnquiryDetail", sqlParam, General.GetBMSConString());
				if (dt.Rows.Count > 0)
				{
					lst = EnumerableExtension.ToList<RequestEnquiryDetailVM>(dt);
				}
				json = Serializer.Serialize(lst);
			}
			catch (Exception ex)
			{

				throw;
			}

			return json;

		}

		public static bool Insert_FollowupDetail(List<FollowUpDetailVM> model , string dealerCode)
		{
			int count = 0;
			try
			{
				foreach (var item in model)
				{
					if(strAutoCode == "")
					{
						strAutoCode = item.EnquiryID;
					}

					if (item.NextFollowUpActionPlan != "" && item.NextFollowUpActionPlan != null)
					{                       

						//DateTime furhterdate = DateTime.ParseExact(item.NextFollowUpDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
						DateTime furhtertime = Convert.ToDateTime(item.NextFollowUpTime);
						//string query = "declare @lastval varchar(50),@id int " +
						//				"set @id = (select count(*) from CRM_FollowUpDetail) " +
						//				"set @id=@id+1 " +
						//				"if len(@id) = 1 " +
						//				"set @lastval= '000000' " + 
						//				"if len(@id) = 2 " +
						//				"set @lastval= '00000' " +
						//				"if len(@id) = 3 " +
						//				"set @lastval= '0000' " + 
						//				"if len(@id) >= 4 " +
						//				"set @lastval= '000' " + 
						//				"if len(@id) >= 5  " +
						//				"set @lastval= '00' " +
						//				"if len(@id) >= 6  " +
						//				"set @lastval= '0' " +
						//				"declare @i varchar(50) " +
						//				"set @i = Convert(varchar(50),@id) " +
						//				"set @lastval = @lastval+@i " +
						//				"select @lastval as FollowupId";
						//dt = DataAccess.getDataTableByQuery(query, nullSqlParam, General.GetBMSConString());
						item.FollowupId = sysfunc.AutoGen("CRM_FollowUpDetail", "FollowupId", DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MM/yyyy"), dealerCode);
						SqlParameter[] sqlParam = {
									new SqlParameter("@FollowupId",item.FollowupId),//0
									new SqlParameter("@DealerCode",dealerCode),//1
									new SqlParameter("@EnquiryID",strAutoCode),//2
									new SqlParameter("@PlanFollowupDate",(object)DBNull.Value),// 3
									new SqlParameter("@PlanFollowupTime",(object)DBNull.Value),// 4
									new SqlParameter("@PlanFollowupAction",(object)DBNull.Value),// 5
									new SqlParameter("@PlanFollowupPurpose",(object)DBNull.Value),// 6
									new SqlParameter("@PlanFollowupAssignTo",(object)DBNull.Value),// 7
									new SqlParameter("@Schedule",(object)DBNull.Value),// 8
									new SqlParameter("@ActualFollowupDate",(object)DBNull.Value),// 9
									new SqlParameter("@ActualFollowupTime",(object)DBNull.Value),// 10
									new SqlParameter("@ActualFollowupAction", (object)DBNull.Value), //11
									new SqlParameter("@ActualFollowupBy",(object)DBNull.Value), //12
									new SqlParameter("@AfterFollowupEnqStatus",(object)DBNull.Value),// 13
									new SqlParameter("@ActualFollowupRemarks",(object)DBNull.Value),// 14
									new SqlParameter("@IsLostEnq",item.IsLostEnq),//15
									new SqlParameter("@LostByDealer",(object)DBNull.Value), //16
									new SqlParameter("@LostByModel", (object)DBNull.Value), //17
									new SqlParameter("@LostReasonId", item.LostReasonId),//)18
									new SqlParameter("@NextFollowUpDate",sysfunc.SaveDate(item.NextFollowUpDate)),//19
									new SqlParameter("@NextFollowUpTime",furhtertime.ToString("HH:mm")),//20
									new SqlParameter("@NextFollowUpActionPlan",item.NextFollowUpActionPlan),//21
									new SqlParameter("@NextFollowUpPurpose",item.NextFollowUpPurpose),//22
									new SqlParameter("@CreatedBy",General.GetCurrentUsername()),//23
									new SqlParameter("@IsDeleted", false), //24
									new SqlParameter("@TransferStatus", (object)DBNull.Value),//25
									new SqlParameter("@PartiallyLost", false),//26
									new SqlParameter("@LostDate", item.LostDate),//27
									new SqlParameter("@EmpCode", AuthBase.EmpCode),//28     
									new SqlParameter("@StatusTypeId",item.StatusTypeId)//29
									};


						if (sysfunc.ExecuteSP_NonQuery("SP_Insert_CRM_FollowUpDetail", sqlParam, Trans) == true)
						{
							mailMsg.AppendLine("<tr><td>FollowUp ID : </td><td>" + item.FollowupId + "</td></tr>");
							//mailMsg.Append(Environment.NewLine);
                           
							mailMsg.Append("<tr><td>FollowUp Date : </td><td>" + item.NextFollowUpDate + "</td></tr>");
							//mailMsg.Append(Environment.NewLine);
							mailMsg.Append("<tr><td>Next Action : </td><td>" + sysfunc.GetStringValuesAgainstCodes("TaskTypeCode", item.NextFollowUpActionPlan, "TaskTypeDesc", "CRM_TaskType", "", "") + "</td></tr>");
							//mailMsg.Append(Environment.NewLine);
							mailMsg.Append("<tr><td>Status : </td><td>" + sysfunc.GetStringValuesAgainstCodes("Id", item.StatusTypeId, "StatusDesc", "CRM_StatusType", "", "") + "</td></tr>");
							//mailMsg.Append(Environment.NewLine);
							IsSaved = true;
						}
						else
						{
							ObjTrans.RollBackTransaction(ref Trans);
							IsSaved = false;
						}
						
					}
					count++;
				}
				ObjTrans.CommittTransaction(ref Trans);

                //if(email != "")
                //email_Send();
                //mailMsg.Clear();
            }
            catch (Exception)
			{
				throw;
			}
			return IsSaved;
		}


		public static bool Insert_NextFollowupDetail(List<FollowUpDetailVM> model, string dealerCode)
		{
			int count = 0;
			try
			{
				foreach (var item in model)
				{
					

					if (item.FollowupId == "" || item.FollowupId == null)
					{
						item.FollowupId = sysfunc.AutoGen("CRM_FollowUpDetail", "FollowupId", DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MM/yyyy"), dealerCode);
					}
						

					if (item.NextFollowUpActionPlan != "" && item.NextFollowUpActionPlan != null)
					{

						//DateTime furhterdate = DateTime.ParseExact(item.NextFollowUpDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
						DateTime furhtertime = Convert.ToDateTime(item.NextFollowUpTime);

						
						SqlParameter[] sqlParam = {
									new SqlParameter("@FollowupId",item.FollowupId),//0
									new SqlParameter("@DealerCode",dealerCode),//1
									new SqlParameter("@EnquiryID",item.EnquiryID),//2
									new SqlParameter("@PlanFollowupDate",(object)DBNull.Value),// 3
									new SqlParameter("@PlanFollowupTime",(object)DBNull.Value),// 4
									new SqlParameter("@PlanFollowupAction",(object)DBNull.Value),// 5
									new SqlParameter("@PlanFollowupPurpose",(object)DBNull.Value),// 6
									new SqlParameter("@PlanFollowupAssignTo",(object)DBNull.Value),// 7
									new SqlParameter("@Schedule",(object)DBNull.Value),// 8
									new SqlParameter("@ActualFollowupDate",(object)DBNull.Value),// 9
									new SqlParameter("@ActualFollowupTime",(object)DBNull.Value),// 10
									new SqlParameter("@ActualFollowupAction", (object)DBNull.Value), //11
									new SqlParameter("@ActualFollowupBy",(object)DBNull.Value), //12
									new SqlParameter("@AfterFollowupEnqStatus",(object)DBNull.Value),// 13
									new SqlParameter("@ActualFollowupRemarks",(object)DBNull.Value),// 14
									new SqlParameter("@IsLostEnq",item.IsLostEnq),//15
									new SqlParameter("@LostByDealer",(object)DBNull.Value), //16
									new SqlParameter("@LostByModel", (object)DBNull.Value), //17
									new SqlParameter("@LostReasonId", item.LostReasonId),//)18
									new SqlParameter("@NextFollowUpDate",sysfunc.SaveDate( item.NextFollowUpDate)),//19
									new SqlParameter("@NextFollowUpTime",furhtertime.ToString("HH:mm")),//20
									new SqlParameter("@NextFollowUpActionPlan",item.NextFollowUpActionPlan),//21
									new SqlParameter("@NextFollowUpPurpose",item.NextFollowUpPurpose),//22
									new SqlParameter("@CreatedBy",General.GetCurrentUsername()),//23
									new SqlParameter("@IsDeleted", false), //24
									new SqlParameter("@TransferStatus", (object)DBNull.Value),//25
									new SqlParameter("@PartiallyLost", false),//26
									new SqlParameter("@LostDate", item.LostDate),//27
									new SqlParameter("@EmpCode", AuthBase.EmpCode),//28     
									new SqlParameter("@StatusTypeId",item.StatusTypeId)//29
									};


						if (sysfunc.ExecuteSP_NonQuery("SP_Insert_CRM_FollowUpDetail", sqlParam) == true)
						{
							
							IsSaved = true;
						}
						else
						{
							
							IsSaved = false;
						}

					}
					count++;
				}
				
			}
			catch (Exception)
			{
				throw;
			}
			return IsSaved;
		}

		public static string Get_FollowUpDetailData(string enquiryId,string dealerCode)
		{
			string json = "";
			var Serializer = new JavaScriptSerializer();
			List<RequestFollowUpDetailVM> lst = new List<RequestFollowUpDetailVM>();
			try
			{
				SqlParameter[] sqlParam = {
									new SqlParameter("@EnquiryId",enquiryId),//0
									new SqlParameter("@EmpCode",AuthBase.EmpCode),//1
									new SqlParameter("@DealerCode",dealerCode)//1
									};

				dt = DataAccess.getDataTable("Select_FollowUpDetail", sqlParam, General.GetBMSConString());
				if (dt.Rows.Count > 0)
				{
					lst = EnumerableExtension.ToList<RequestFollowUpDetailVM>(dt);
				}
				json = Serializer.Serialize(lst);
			}
			catch (Exception ex)
			{

				throw;
			}

			return json;

		}

		public static string Get_AlertFollowUpDetails(string dealerCode)
		{
			string json = "";
			var Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
			List<EnquiryDetailVM> lst = new List<EnquiryDetailVM>();
			try
			{
				SqlParameter[] sqlParam = {

					new SqlParameter("@DealerCode",dealerCode),//1

										};

				dt = DataAccess.getDataTable("SP_SelectAlertFollowUpDetails", sqlParam, General.GetBMSConString());
				if (dt.Rows.Count > 0)
				{
					lst = EnumerableExtension.ToList<EnquiryDetailVM>(dt);
				}
				json = Serializer.Serialize(lst);
			}
			catch (Exception ex)
			{

				throw;
			}

			return json;
		}
		public static string Get_AlertTaskDetails(string dealerCode, string EmpCode)
		{
			string json = "";
			var Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
			List<TaskVM> lst = new List<TaskVM>();
			try
			{
				SqlParameter[] sqlParam = {

					new SqlParameter("@DealerCode",dealerCode),//0
                    new SqlParameter("@EmpCode",EmpCode),//1
										};

				dt = DataAccess.getDataTable("SP_SelectAlertTaskDetails", sqlParam, General.GetBMSConString());
				if (dt.Rows.Count > 0)
				{
					lst = EnumerableExtension.ToList<TaskVM>(dt);
				}
				json = Serializer.Serialize(lst);
			}
			catch (Exception ex)
			{

				throw;
			}

			return json;
		}

        public static string Get_Enquiries(string sp ,string dealerCode)
        {
            string json = "";
            var Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<EnquiryDetailVM> lst = new List<EnquiryDetailVM>();
            try
            {
                SqlParameter[] sqlParam = {

                    new SqlParameter("@DealerCode",dealerCode),//0
										};

                dt = DataAccess.getDataTable(sp, sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<EnquiryDetailVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return json;
        }
        

        public static string Get_LostEnquiriesDetails(string dealerCode)
		{
			string json = "";
			var Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
			List<EnquiryDetailVM> lst = new List<EnquiryDetailVM>();
			try
			{
				SqlParameter[] sqlParam = {

					new SqlParameter("@DealerCode",dealerCode),//1

										};

				dt = DataAccess.getDataTable("SP_SelectLostFollowUpDetails", sqlParam, General.GetBMSConString());
				if (dt.Rows.Count > 0)
				{
					lst = EnumerableExtension.ToList<EnquiryDetailVM>(dt);
				}
				json = Serializer.Serialize(lst);
			}
			catch (Exception ex)
			{

				throw;
			}

			return json;
		}

        public static bool Check_MobNo(string chassisNo, string dealerCode, ref string msg, ref List<EnquiryMasterVM> EnquiryDetailVM)
        {

            DataSet ds = new DataSet();
            if (sysfunc.CodeExists("CRM_EnquiryMaster", "Mobile", chassisNo, dealerCode, ref ds))
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (ds.Tables[0].Rows[0]["IsMatured"].ToString() == "N")
                    {
                        msg = "This Customer Enquiry No: " + ds.Tables[0].Rows[0]["Enquiry_ID"].ToString() + " Is Already Open Please See Enquiry Detail ";

                    }
                }
                if (sysfunc.CodeExists("CRM_Prospect", "ProspectID", ds.Tables[0].Rows[0]["ProspectID"].ToString(), dealerCode, ref ds))
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        EnquiryDetailVM = EnumerableExtension.ToList<EnquiryMasterVM>(ds.Tables[0]);
                        return false;
                    }
                }
            }
            
             return true;
        }
    }
}
