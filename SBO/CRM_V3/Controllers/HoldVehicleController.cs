using Core.CRM.ADO;
using Core.CRM.ADO.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRM_V3.Controllers
{
    public class HoldVehicleController : Controller
    {

        static string dealerCode = string.Empty;
        // GET: HoldVehicle
        public ActionResult HVMain()
        {
            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("NewLogin", "Home");
            }
            dealerCode = Session["DealerCode"].ToString();

            List<SelectListItem> ddlAssignTo = new List<SelectListItem>();
            ddlAssignTo =
            ViewBag.AssignTo = ddlAssignTo;


            return View();
        }

        [HttpGet]
        public JsonResult Select_Emp(string EnquiryId, string DealerCode)
        {
            List<SelectListItem> data;
            bool result = false;
            data = DeliveryOrderMethods.GetDealerEmployee(dealerCode);

            if (data.Count > 0)
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult Select_HoldVehicle( string DealerCode)
        {
            string data = "";
            bool result = false;
            data = HoldVehicleMethods.Get_VehiclesForHold(DealerCode);

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Update_Vehicle(HoldVehicleVM objects)
        {
            bool result = false;

            string msg = "Failed to save record..";

            result = HoldVehicleMethods.Update_Vehicle(objects);

            if (result)
            {
                msg = "Successfully Added";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }
    }
}