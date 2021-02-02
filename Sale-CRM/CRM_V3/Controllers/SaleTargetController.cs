using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.CRM.ADO;
using Core.CRM.ADO.ViewModel;

namespace CRM_V3.Controllers
{
    public class SaleTargetController : Controller
    {
        // GET: SaleTarget
        static string dealerCode = string.Empty;
        public ActionResult SaleTarget()

        {
            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("Login", "Home");
            }
            dealerCode = Session["DealerCode"].ToString();
            List<SelectListItem> ddlBrandCode = new List<SelectListItem>();
            ddlBrandCode = GeneralMethods.GetDataFromSPWithDealerCode("Select_Brand", dealerCode);
            ViewBag.BrandCode = ddlBrandCode;

            List<SelectListItem> ddlProdCode = new List<SelectListItem>();
            //ddlProdCode = GeneralMethods.GetProduct();
            ViewBag.ProdCode = ddlProdCode;
            return View();
        }
        [HttpGet]
        public JsonResult Select_SaleTarget(string dealer)
        {
            string data = "";
            bool result = false;
            string msg = "Failed to Get Data..";
           
            data = SaleTargetMethod.Select_SaleTarget(dealer, ref msg);

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Insert_SaleTarget(SaleTargetVM SaleTargetVM)
        {
            bool result = false;

            string msg = "Failed to save record..";

            result = SaleTargetMethod.Insert_SaleTarget(SaleTargetVM, ref msg);

            if (result)
            {
                msg = "Successfully Added";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }
    }
}