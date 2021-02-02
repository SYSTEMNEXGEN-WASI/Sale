using Core.CRM.ADO;
using Core.CRM.ADO.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRM_V3.Controllers
{
    public class BookingNoController : Controller
    {
        static string dealerCode = string.Empty;
        // GET: BookingNo
        public ActionResult BNMain()
        {
            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("Login", "Home");
            }
            dealerCode = Session["DealerCode"].ToString();

            List<SelectListItem> ddlBONo = new List<SelectListItem>();
            ddlBONo = BookingOrderMethods.GetDataFromSPWithDealerCode("SP_SelectBONo", dealerCode);
            ViewBag.BONo = ddlBONo;

            return View();
        }

        [HttpGet]
        public JsonResult Select_BookingOrder(string EnquiryId)
        {
            string data = "";
            bool result = false;
            data = BookingOrderMethods.Get_BookingOrderData(EnquiryId, Session["DealerCode"].ToString());

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Update_BookingNo(BookOrdMasterVM VehStockVM)
        {
            bool result = false;

            string msg = "Failed to save record..";

            result = BookingOrderMethods.Update_BookingNo(VehStockVM, Session["DealerCode"].ToString());

            if (result)
            {
                msg = "Successfully Added";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }
    }
}