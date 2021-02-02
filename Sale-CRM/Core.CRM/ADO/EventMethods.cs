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
	public class EventMethods
	{

		static EventVM response = new EventVM();
		static DataTable dt = new DataTable();
		static string strAutoCode = string.Empty;
		static string autoProspect_ID = string.Empty;
		static bool IsSaved = false;
		static SqlParameter[] nullSqlParam = null;
        static SysFunction sysfun = new SysFunction();

		public static bool Insert_Event(EventVM model)
		{

			try
			{
				if (model.Event_ID.Trim() == "0" || model.Event_ID.Trim() == "")
				{

					string getNextEventID = "declare @lastval varchar(14),@id int " +
													   "set @id = (select count(*) from Event) " +
													   "set @id=@id+1 " +
													   "if len(@id) = 1 " +
													   "set @lastval= '0000' " +
													   "if len(@id) = 2 " +
													   "set @lastval='000' " +
													   "if len(@id) = 3 " +
													   "set @lastval='00' " +
													   "if len(@id) >= 4 " +
													   "set @lastval='0' " +
													   "declare @i varchar(14) " +
													   "set @i = CAST(@id as varchar(14)) " +
													   "set @lastval = @lastval+@i " +
													   "select @lastval as eventID";

					dt = DataAccess.getDataTableByQuery(getNextEventID, nullSqlParam, General.GetBMSConString());

					strAutoCode = dt.Rows[0]["eventID"].ToString();
				}
				else { 

				strAutoCode = model.Event_ID;
				}
				SqlParameter[] param = {
								 new SqlParameter("@DealerCode",model.DealerCode),//0
								 new SqlParameter("@EventID",strAutoCode),//1
								 new SqlParameter("@EventDate",General.CurrentDate),//2								 
								 new SqlParameter("@EventDesc",model.EventDesc.Trim()),//3
								 new SqlParameter("@AssignID",model.AssignID),//4
								 new SqlParameter("@StartDate", sysfun.SaveDate(model.StartDate)),//5
								 new SqlParameter("@EndDate",sysfun.SaveDate(model.EndDate)),//6
								 new SqlParameter("@UpdUser",AuthBase.UserId),//7
								 new SqlParameter("@UpdTerm",General.CurrentIP),//8
								 new SqlParameter("@ExpBudget",model.ExpBudget),//9
								 new SqlParameter("@ActBudget",model.ActBudget),//10
								 new SqlParameter("@Status",model.Status),//11
								 new SqlParameter("@Feedback",model.Feedback),//12
								 new SqlParameter("@EventType",model.EventType),//13
								 new SqlParameter("@ProdCode",model.ProdCode),//14
								 new SqlParameter("@VersionCode",model.VersionCode),//15
								 new SqlParameter("@ColorCode",model.ColorCode == "0" ? (object)DBNull.Value : model.ColorCode),//16
								 new SqlParameter("@BrandCode",model.BrandCode == "0" ? (object)DBNull.Value : model.BrandCode),//17
							};
				dt = DataAccess.getDataTable("Sp_Insert_Event", param, General.GetBMSConString());
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

		public static string Get_EventDetailData(string enquiryId, string dealerCode)
		{
			string json = "";
			var Serializer = new JavaScriptSerializer();
			List<RequestEventDetailVM> lst = new List<RequestEventDetailVM>();
			try
			{
				SqlParameter[] sqlParam = {
									new SqlParameter("@EnquiryId",enquiryId),//0
									new SqlParameter("@DealerCode",dealerCode)//1
									};

				dt = DataAccess.getDataTable("SP_EventDetail_Select", sqlParam, General.GetBMSConString());
				if (dt.Rows.Count > 0)
				{
					lst = EnumerableExtension.ToList<RequestEventDetailVM>(dt);
				}
				json = Serializer.Serialize(lst);
			}
			catch (Exception ex)
			{

				throw;
			}

			return json;
		}
       

        public static List<SelectListItem> Get_BrandProductData(string enquiryId, string dealerCode,string vehiclecategory)
		{
			string json = "";
			List<SelectListItem> item = new List<SelectListItem>();
			var Serializer = new JavaScriptSerializer();
			List<PrdouctVM> lst = new List<PrdouctVM>();
			try
			{
				SqlParameter[] sqlParam = {
									new SqlParameter("@EnquiryId",enquiryId),//0
									new SqlParameter("@DealerCode",dealerCode),//1
                                    new SqlParameter("@VehicleCategory",vehiclecategory)//1
									};

				dt = DataAccess.getDataTable("SP_BrandProduct_Select", sqlParam, General.GetBMSConString());
				if (dt.Rows.Count > 0)
				{
					lst = EnumerableExtension.ToList<PrdouctVM>(dt);
				}
				//json = Serializer.Serialize(lst);

				item = lst.Select(i => new SelectListItem()
				{
					Value = i.ProdCode.ToString(),
                    Text = i.ProdDesc.ToString() + " | " + i.VersionCode.ToString() + " | " + i.ProdDesc.ToString()

                }).ToList();

				item.Insert(0, new SelectListItem() { Value = "0", Text = "Select" });
			}
			catch (Exception ex)
			{
				
				throw;
			}

			return item;
		}

	}
}
