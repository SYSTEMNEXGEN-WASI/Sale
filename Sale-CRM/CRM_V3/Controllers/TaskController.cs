using Core.CRM.ADO;
using Core.CRM.ADO.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRM_V3.Controllers
{
    public class TaskController : Controller
    {
        static string dealerCode = string.Empty;
        // GET: /Task/
        public ActionResult TaskDetail(string TaskId = "")
        {
            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("Login", "Home");
            }
            dealerCode = Session["DealerCode"].ToString();

            List<SelectListItem> ddleadId = new List<SelectListItem>();
            ddleadId = GeneralMethods.GetDataFromSPWithDealerCode("Select_EnquiryId",dealerCode,"Y");
            ViewBag.LeadId = ddleadId;

            List<SelectListItem> ddlAssignTo = new List<SelectListItem>();
            ddlAssignTo = GeneralMethods.GetDataFromSPWithDealerCode("Select_DealerEmplpoyee",dealerCode);
            ViewBag.AssignTo = ddlAssignTo;

            List<SelectListItem> ddlTaskId = new List<SelectListItem>();
            ddlTaskId = GeneralMethods.GetDataFromSPWithDealerCode("Select_TaskId",dealerCode,"Y");
            ViewBag.Tasks = ddlTaskId;

            List<SelectListItem> ddlStatusType = new List<SelectListItem>();
            ddlStatusType = GeneralMethods.GetStatusType();
            ViewBag.StatusType = ddlStatusType;

            List<SelectListItem> ddlSubjects = new List<SelectListItem>();
            ddlSubjects = GeneralMethods.GetDataFromSPWithDealerCode("Select_Subjects",dealerCode);
            ViewBag.Subjects = ddlSubjects;

            List<SelectListItem> ddlCustomers = new List<SelectListItem>();
            ddlCustomers = GeneralMethods.GetDataFromSPWithDealerCode("SP_Select_Prospect", dealerCode,"Y");
            ViewBag.Customers = ddlCustomers;

            List<SelectListItem> ddlRelatedTo = new List<SelectListItem>();
            ddlRelatedTo = GeneralMethods.GetDataFromSPWithDealerCode("Select_LeadSource",dealerCode);
            ViewBag.RelatedTo = ddlRelatedTo;

            List<SelectListItem> ddlTaskType = new List<SelectListItem>();
            ddlTaskType = GeneralMethods.GetTaskType();
            ViewBag.TaskType = ddlTaskType;

            List<SelectListItem> ddlPriority = new List<SelectListItem>();
            ddlPriority = GeneralMethods.GetStatusType();
            ViewBag.Priority = ddlPriority;

            ViewBag.UrlLeadId = TaskId;

            return View();
        }

        public JsonResult Get_TaskDetail(string TaskID)
        {
            string DealerCode = Session["DealerCode"].ToString();

            string data = "";
            bool result = false;
            data = TaskMethods.Get_TaskMasterDetail(TaskID, DealerCode);

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult Insert_TaskDetail(TaskVM model)
        {

            string msg = "Oops, Something went wrong.";
            bool result = false;
            result = TaskMethods.Insert_TaskMaster(model);

            if (result)
            {
                msg = "Record Successfully Saved.";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }
    }
}