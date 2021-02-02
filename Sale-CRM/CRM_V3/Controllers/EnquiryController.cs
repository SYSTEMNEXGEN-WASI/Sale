using Core.CRM.ADO;
using Core.CRM.ADO.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;

namespace CRM_V3.Controllers
{
	public class EnquiryController : Controller
	{
        static string dealerCode = string.Empty;
        static string DealerCode = string.Empty;
        //
        // GET: /Enquiry/

        public ActionResult Alert()
        {
            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("NewLogin", "Home");
            }
            dealerCode = Session["DealerCode"].ToString();
            return View();
        }
        public ActionResult PendingFollowUpList()
        {
            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("NewLogin", "Home");
            }
            dealerCode = Session["DealerCode"].ToString();
            return View();
        }
        public JsonResult DTRRedirectLCV(string EnquiryId = "", string EnDealerCode = "", string EnCLose = "")
        {
            bool result = false;
            DealerCode = Session["DealerCode"].ToString();
            Session["id"] = EnquiryId;
            Session["dealer"] = EnDealerCode;

            Session["close"] = EnCLose;

            return Json(new { Success = result }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult QuickEnquiry()
		{

            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("NewLogin", "Home");
            }
            dealerCode = Session["DealerCode"].ToString();

            List<SelectListItem> ddlAssignTo = new List<SelectListItem>();
			ddlAssignTo = GeneralMethods.GetDealerEmployee(dealerCode);
            ViewBag.AssignTo = ddlAssignTo;

            List<SelectListItem> ddlBrandCode = new List<SelectListItem>();
            ddlBrandCode = GeneralMethods.GetDataFromSPWithDealerCode("Select_Brand", dealerCode,"Y");
            ViewBag.BrandCode = ddlBrandCode;

            List<SelectListItem> ddlProdCode = new List<SelectListItem>();
			//ddlProdCode = GeneralMethods.GetDataFromSPWithDealerCode("Select_Product",dealerCode);
			ViewBag.ProdCode = ddlProdCode;

			List<SelectListItem> ddlColor = new List<SelectListItem>();
			ddlColor = GeneralMethods.GetColor(Session["VehicleCategory"].ToString());
			ViewBag.Color = ddlColor;

			Session["userId"] = "001";

			return View();
		}

		public ActionResult EnquiryDetail(string leadId = "")
		{
            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("Login", "Home");
            }
            dealerCode = Session["DealerCode"].ToString();

            List<SelectListItem> ddleadId = new List<SelectListItem>();
			ddleadId = GeneralMethods.GetDataFromSPWithDealerCode("Select_EnquiryId",dealerCode,"Y");
			ViewBag.LeadId = ddleadId;
            List<SelectListItem> ddSaluation = new List<SelectListItem>();
            ddSaluation = GeneralMethods.GetDataFromSp("Select_Profession");
            ViewBag.ddSaluation = ddSaluation;
            List<SelectListItem> ddlLeadType = new List<SelectListItem>();
			ddlLeadType = GeneralMethods.GetDataFromSPWithDealerCode("Select_ProspectType",dealerCode);
			ViewBag.LeadType = ddlLeadType;

			List<SelectListItem> ddlLeadMode = new List<SelectListItem>();
			ddlLeadMode = GeneralMethods.GetDataFromSPWithDealerCode("Select_EnquiryMode",dealerCode);
			ViewBag.LeadMode = ddlLeadMode;
            List<SelectListItem> ddlVersion = new List<SelectListItem>();
            ddlVersion = GeneralMethods.GetDataFromSp("Select_Version");
            ViewBag.ddlVersion = ddlVersion;
            List<SelectListItem> ddlLeadSrouce = new List<SelectListItem>();
			ddlLeadSrouce = GeneralMethods.GetDataFromSPWithDealerCode("Select_EnquirySource",dealerCode);
			ViewBag.LeadSource = ddlLeadSrouce;

			List<SelectListItem> ddlVehicelSegments = new List<SelectListItem>();
			ddlVehicelSegments = GeneralMethods.GetVehicleSegments();
			ViewBag.VehicleSegments = ddlVehicelSegments;

			List<SelectListItem> ddlAssignTo = new List<SelectListItem>();
			ddlAssignTo = DeliveryOrderMethods.GetDealerEmployee(dealerCode);
			ViewBag.AssignTo = ddlAssignTo;

			List<SelectListItem> ddlBank = new List<SelectListItem>();
			ddlBank = GeneralMethods.GetBank();
			ViewBag.Bank = ddlBank;

			List<SelectListItem> ddlCustomers = new List<SelectListItem>();
            ddlCustomers = GeneralMethods.GetDataFromSPWithDealerCode("Select_Customer", dealerCode,"Y");
			ViewBag.Customers = ddlCustomers;
			
			List<SelectListItem> ddlCountry = new List<SelectListItem>();
			ddlCountry = GeneralMethods.GetCountry();
			ViewBag.Country = ddlCountry;

			List<SelectListItem> ddlCity = new List<SelectListItem>();
			ddlCity = GeneralMethods.GetCity();
			ViewBag.City = ddlCity;

			List<SelectListItem> ddlBrandCode = new List<SelectListItem>();
			ddlBrandCode = GeneralMethods.GetDataFromSPWithDealerCode("Select_Brand",dealerCode ,"Y");

            ViewBag.BrandCode = ddlBrandCode;

			List<SelectListItem> ddlProdCode = new List<SelectListItem>();
			ddlProdCode = GeneralMethods.GetProduct();
			ViewBag.ProdCode = ddlProdCode;

			List<SelectListItem> ddlColor = new List<SelectListItem>();
			ddlColor = GeneralMethods.GetColor(Session["VehicleCategory"].ToString());
			ViewBag.Color = ddlColor;

			List<SelectListItem> ddlPriority = new List<SelectListItem>();
			ddlPriority = GeneralMethods.GetStatus();
			ViewBag.Priority = ddlPriority;

            List<SelectListItem> ddlStatusType = new List<SelectListItem>();
            ddlStatusType = GeneralMethods.GetStatusType();
            ViewBag.StatusType = ddlStatusType;

            List<SelectListItem> ddlFurtherContact = new List<SelectListItem>();
			ddlFurtherContact = GeneralMethods.GetTaskType();
			ViewBag.FurtherContact = ddlFurtherContact;

            List<EnquiryMasterVM> lstEnquiry = EnquiryMethods.Get_EnquiryModal(Session["DealerCode"].ToString());

            ViewBag.UrlLeadId = leadId;

            ViewBag.Enquires = lstEnquiry;

            return View();
		}
        public JsonResult Select_PendingFollow(string value)
        {
            string data = "";
            bool result = false;
            data = EnquiryMethods.Get_PendingModal(Session["DealerCode"].ToString());

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Insert_EnquiryMaster(EnquiryMasterVM requestModel)
		{
			bool result = false;
			string msg = "Failed to save record..";
			string leadId = "", prospectid = "";
			string[] data;
			leadId = EnquiryMethods.Insert_EnquiryMaster(requestModel , Session["DealerCode"].ToString());
			if(!string.IsNullOrEmpty(leadId))
			{
				data = leadId.Split(',');
				leadId = data[0].ToString();
				prospectid = data[1].ToString();
				result = true;
				msg = "Successfully Added";
			}

			return Json(new { Success = result, Message = msg, LeadId = leadId, ProspectId = prospectid }, JsonRequestBehavior.AllowGet);
		}

		public JsonResult Insert_Prospect(ProspectDetailVM requestModel)
		{
			bool result = false;
			string msg = "Failed to save record..";
			result = EnquiryMethods.Insert_ProspectDetail(requestModel , Session["DealerCode"].ToString());
			if (result)
			{
				msg = "Successfully Added";
			}

			return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public JsonResult Insert_EnquiryDetail(List<EnquiryDetailVM> objects)
		{
			bool result = false;
            int count;

            if (objects.Count == 1) {
                count = 1;
            }else {
                count = 0;
            }
			
			string msg = "Failed to save record..";
			//foreach (var item in objects)
			//{
			//	if(count >= 1)
			//	{
			result = EnquiryMethods.Insert_EnquiryDetail(objects , Session["DealerCode"].ToString());
			//	}
			//	count++;
			//}
			
			if (result)
			{
				msg = "Successfully Added";
			}

			return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
		}
        [HttpGet]
        public JsonResult Check_MobNo(string MobNo)
        {
           
            string msg = "";
            bool result = false;
            List<EnquiryMasterVM> EnquiryDetailVM=new List<EnquiryMasterVM>();
            //result = VehReceiptMethods.Check_ChassisNo(chassisNo,engineNo, DealerCode);
            DealerCode = Session["DealerCode"].ToString();
            result = EnquiryMethods.Check_MobNo(MobNo, DealerCode, ref msg,ref EnquiryDetailVM);
            if (result)
            {
                
                
            }

            return Json(new { Success = result, Message = msg,Response= EnquiryDetailVM }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Insert_QuickEnquiry(QuickEnquiryVM requestModel)
		{
            bool result = false;
            string msg = "Failed to save record..";
            msg = EnquiryMethods.Insert_QuickEnquiry(requestModel , Session["DealerCode"].ToString());
			if ( msg == "Done")
			{
				msg = "Successfully Added";
                result = true;
            }

			return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		public JsonResult Select_EnquiryMaster(string EnquiryId)
		{
			string data = "";
			bool result = false;
			data = EnquiryMethods.Get_EnquiryMasterData(EnquiryId, Session["DealerCode"].ToString());

			if (!string.IsNullOrEmpty(data))
			{
				result = true;
			}

			return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
		}

        [HttpGet]
        public JsonResult GetEmailAddress()
        {
            string EnquiryId = Session["EmpCode"].ToString();
            string data = "";
            bool result = false;
            data = EnquiryMethods.Get_EmailAddress(EnquiryId, Session["DealerCode"].ToString());

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
		public JsonResult Select_EnquiryDetail(string EnquiryId)
		{
			string data = "";
			bool result = false;
			data = EnquiryMethods.Get_EnquiryDetailData(EnquiryId, Session["DealerCode"].ToString());

			if (!string.IsNullOrEmpty(data))
			{
				result = true;
			}

			return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
		}

		public ActionResult FollowUp(string EnquiryId = "")
		{

            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("Login", "Home");
            }
            dealerCode = Session["DealerCode"].ToString();
            //if(EnquiryId == "")
            //{

            //}
            DealerCode = Session["DealerCode"].ToString();
            ViewBag.UrlLeadId = Session["id"].ToString();
            ViewBag.UrlDealerCode = Session["dealer"].ToString();
            ViewBag.EnCLose = Session["close"].ToString();
            Session["id"] = "";
            Session["dealer"] = "";

            List<SelectListItem> ddleadId = new List<SelectListItem>();
			ddleadId = GeneralMethods.GetDataFromSPWithDealerCode("Select_EnquiryId",dealerCode,"Y");
			ViewBag.LeadId = ddleadId;

			List<SelectListItem> ddlLeadType = new List<SelectListItem>();
			ddlLeadType = GeneralMethods.GetLeadType();
			ViewBag.LeadType = ddlLeadType;

			List<SelectListItem> ddlFurtherContact = new List<SelectListItem>();
			ddlFurtherContact = GeneralMethods.GetTaskType();
			ViewBag.FurtherContact = ddlFurtherContact;

			List<SelectListItem> ddlStatusType = new List<SelectListItem>();
			ddlStatusType = GeneralMethods.GetStatusType();
			ViewBag.StatusType = ddlStatusType;

			List<SelectListItem> ddlLostReason = new List<SelectListItem>();
			ddlLostReason = GeneralMethods.GetLostReason();
			ViewBag.LostReason = ddlLostReason;

           // ViewBag.UrlLeadId = EnquiryId;


            return View();
		}

		[HttpPost]
		public JsonResult Insert_FollowupDetail(List<FollowUpDetailVM> objects)
		{
			bool result = false;
			int count = 0;
			string msg = "Failed to save record..";
			//foreach (var item in objects)
			//{
   //             if (count >= 1)
   //             {
                    result = EnquiryMethods.Insert_FollowupDetail(objects , Session["DealerCode"].ToString());
            //    }
            //    count++;
            //}

			if (result)
			{
				msg = "Successfully Added";
			}

			return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
		}

        [HttpPost]
        public JsonResult Insert_NextFollowupDetail(List<FollowUpDetailVM> objects)
        {
            bool result = false;
            int count = 0;
            string msg = "Failed to save record..";
            //foreach (var item in objects)
            //{
            //             if (count >= 1)
            //             {
            result = EnquiryMethods.Insert_NextFollowupDetail(objects, Session["DealerCode"].ToString());
            //    }
            //    count++;
            //}

            if (result)
            {
                msg = "Successfully Added";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
		public JsonResult Select_FollowUpDetail(string EnquiryId)
		{
			string data = "";
			bool result = false;
			data = EnquiryMethods.Get_FollowUpDetailData(EnquiryId, Session["DealerCode"].ToString());

			if (!string.IsNullOrEmpty(data))
			{
				result = true;
			}

			return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
		}

        public JsonResult Select_AlertTaskData()
        {
            string data = "";
            bool result = false;
            string EmpCode = Session["EmpCode"].ToString();
            data = EnquiryMethods.Get_AlertTaskDetails(Session["DealerCode"].ToString(), EmpCode);

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Select_AlertFollowUpData()
        {
            string data = "";
            bool result = false;
            data = EnquiryMethods.Get_AlertFollowUpDetails(Session["DealerCode"].ToString());

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Get_LostEnquiriesData()
        {
            string data = "";
            bool result = false;
            data = EnquiryMethods.Get_LostEnquiriesDetails(Session["DealerCode"].ToString());

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Select_PreCustomerID(string EnquiryId)
        {
            string data = "";
            bool result = false;
            data = EnquiryMethods.Get_PreCustomerData(EnquiryId, Session["DealerCode"].ToString());

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }
    }
}