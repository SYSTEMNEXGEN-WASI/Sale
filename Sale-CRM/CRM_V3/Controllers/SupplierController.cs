using Core.CRM.ADO;
using Core.CRM.ADO.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRM_V3.Controllers
{
    public class SupplierController : Controller
    {
        // GET: Supplier

        static string dealerCode = string.Empty;
        public ActionResult SupplierMain()
        {

            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("Login", "Home");
            }
            dealerCode = Session["DealerCode"].ToString();

            List<SupplierVM> lstSupplier = SupplierMethods.GetSupplierModal(dealerCode);

            ViewBag.Suppliers = lstSupplier;

            return View();
        }

        public JsonResult Insert_SupplierDetail(SupplierVM model)
        {

            string msg = "Oops, Something went wrong.";
            bool result = false;
            result = SupplierMethods.Insert_SupplierMaster(model);

            if (result)
            {
                msg = "Record Successfully Saved.";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Select_Supplier(string EnquiryId)
        {
            string data = "";
            bool result = false;
            data = SupplierMethods.Get_SupplierData(EnquiryId, Session["DealerCode"].ToString());

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }
    }
}