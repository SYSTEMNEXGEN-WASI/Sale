using Core.CRM.ADO;
using Core.CRM.ADO.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CRM.Models.Classes;

namespace CRM_V3.Controllers
{
    public class VehicleUpdateController : Controller
    {
        static string dealerCode = string.Empty;
        SecurityBll common = new SecurityBll();
        // GET: VehicleUpdate
        public ActionResult VUMain()
        {
            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("NewLogin", "Home");
            }
            if (common.UserRight("2018", "001"))
            {
                dealerCode = Session["DealerCode"].ToString();
                dealerCode = Session["DealerCode"].ToString();

            List<SelectListItem> ddlInsuracne = new List<SelectListItem>();
            ddlInsuracne = VehicleUpdateMethods.GetInsuranceCompanies();
            ViewBag.Insurance = ddlInsuracne;

            //List<SelectListItem> ddlChassisNo = new List<SelectListItem>();
            //ddlChassisNo = GeneralMethods.GetDataFromSPWithDealerCode("SP_SelectChassisNo",dealerCode);
            //ViewBag.ChassisNo = ddlChassisNo; 

            List<VehicleStockVM> ddlEngineNo = new List<VehicleStockVM>();
            ddlEngineNo = VehicleUpdateMethods.GetDataFromSPWithDealerCode("SP_SelectChassisNo&EngineNo", dealerCode);
            ViewBag.EngineNo = ddlEngineNo;

            List<SelectListItem> ddlCustomers = new List<SelectListItem>();
            ddlCustomers = VehReceiptMethods.GetDatafromSP("SP_Select_Customer", dealerCode);
            ViewBag.Customers = ddlCustomers;

            List<SelectListItem> ddlColor = new List<SelectListItem>();
            ddlColor = GeneralMethods.GetColor(Session["VehicleCategory"].ToString());
            ViewBag.Color = ddlColor;

            List<SelectListItem> ddlBrandCode = new List<SelectListItem>();
            ddlBrandCode = GeneralMethods.GetDataFromSPWithDealerCode("Select_Brand", dealerCode);
            ViewBag.BrandCode = ddlBrandCode;
            }
            else
            {
                TempData["TestAccessError"] = MessageAlert.MsgAuthorized();
                return RedirectToAction("Error", "Definition");
            }

            return View();
        }


        [HttpGet]
        public JsonResult Select_VehicleDetail(string EnquiryId)
        {
            string data;
            bool result = false;

            data = VehicleUpdateMethods.GetVehicleDetail(EnquiryId, Session["DealerCode"].ToString());

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult Insert_VSMaster(VehicleStockVM VehStockVM)
        {
            bool result = false;

            string msg = "Failed to save record..";

            result = VehicleUpdateMethods.Insert_VSMaster(VehStockVM, Session["DealerCode"].ToString(),ref msg);

            if (result)
            {
                msg = "Successfully Added";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult Insert_VSMasterInv(VehicleStockVM VehStockVM)
        {
            bool result = false;

            string msg = "Failed to save record..";

            result = VehicleUpdateMethods.Insert_VSMaster(VehStockVM, Session["DealerCode"].ToString(),ref msg);

            if (result)
            {
                msg = "Successfully Added";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Insert_ProdRecDetail(List<VehicleStockVM> objects)
        {
            bool result = false;
            string msg = "Failed to save record..";
          

            result = VehicleUpdateMethods.Insert_InvDetail(objects, ref msg);

            if (result)
            {
                msg = "Successfully Added";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult Select_InvoiceDetail(string EnquiryId)
        {
            string data;
            bool result = false;

            data = VehicleUpdateMethods.GetInvoiceDetail(EnquiryId, Session["DealerCode"].ToString());

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);

        }
    }
}