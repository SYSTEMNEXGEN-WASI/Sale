using Core.CRM.ADO;
using Core.CRM.ADO.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DXBMS;
using CRM.Models.Classes;

namespace CRM_V3.Controllers
{
    public class VehicleController : Controller
    {
        static string dealerCode = string.Empty;
        SecurityBll common = new SecurityBll();
     

        // GET: Vehicle
        public ActionResult Vehicle()
        {
            
            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("NewLogin", "Home");

            }
            if (common.UserRight("2503", "001"))
            {

                dealerCode = Session["DealerCode"].ToString();

                //List<SelectListItem> ddlVehicle = new List<SelectListItem>();
                //ddlVehicle = GeneralMethods.GetDataFromSPWithDealerCode("SP_Vehicles", dealerCode,"Y");
                //ViewBag.Vehicle = ddlVehicle;

                List<VehicleVM> lstVehicle = VehicleMethods.GetVehicleModal(dealerCode);
                ViewBag.Vehicles = lstVehicle;
                Session["lstVehicle"] = lstVehicle;

                List<SelectListItem> ddlBrand = new List<SelectListItem>();
                ddlBrand = GeneralMethods.GetDataFromSPWithDealerCode("Select_Brand", dealerCode, "Y");
                ViewBag.Made = ddlBrand;

                List<SelectListItem> ddlVehicleType = new List<SelectListItem>();
                ddlVehicleType = GeneralMethods.GetDataFromSPWithDealerCode("Select_VehicleType", dealerCode);
                ViewBag.VehicleType = ddlVehicleType;

                List<SelectListItem> ddlColor = new List<SelectListItem>();
                ddlColor = GeneralMethods.GetColor();
                ViewBag.Color = ddlColor;
                List<VehicleVM> Get_AccDesc = new List<VehicleVM>();
                Get_AccDesc = VehicleMethods.Get_AccDesc(dealerCode);
                ViewBag.Get_AccDesc = Get_AccDesc;
            }
            else
            {
                TempData["TestAccessError"] = MessageAlert.MsgAuthorized();
                return RedirectToAction("Error", "Definitions");
            }



            return View();
        }
        public ActionResult VehiclePrice()
        {

            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("NewLogin", "Home");
            }
            dealerCode = Session["DealerCode"].ToString();

            //List<SelectListItem> ddlVehicle = new List<SelectListItem>();
            //ddlVehicle = GeneralMethods.GetDataFromSPWithDealerCode("SP_Vehicles", dealerCode,"Y");
            //ViewBag.Vehicle = ddlVehicle;

            List<VehicleVM> lstVehicle = VehicleMethods.GetVehicleModal(dealerCode);
            ViewBag.Vehicles = lstVehicle;
            Session["lstVehicle"] = lstVehicle;

            List<SelectListItem> ddlBrand = new List<SelectListItem>();
            ddlBrand = GeneralMethods.GetDataFromSPWithDealerCode("Select_Brand", dealerCode, "Y");
            ViewBag.Made = ddlBrand;

            List<SelectListItem> ddlVehicleType = new List<SelectListItem>();
            ddlVehicleType = GeneralMethods.GetDataFromSPWithDealerCode("Select_VehicleType", dealerCode);
            ViewBag.VehicleType = ddlVehicleType;

            List<SelectListItem> ddlColor = new List<SelectListItem>();
            ddlColor = GeneralMethods.GetColor();
            ViewBag.Color = ddlColor;
            List<VehicleVM> Get_AccDesc = new List<VehicleVM>();
            Get_AccDesc = VehicleMethods.Get_AccDesc(dealerCode);
            ViewBag.Get_AccDesc = Get_AccDesc;


            return View();
        }
        public JsonResult Insert_VehicleMaster(VehicleVM VehicleVM)
        {
            bool result = false;

            string msg = "Failed to save record..";

            msg = VehicleMethods.Insert_VehicleMaster(VehicleVM, Session["DealerCode"].ToString(),ref msg);

            if (msg == "Completed")
            {
                result = true;
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Get_veh_Data(string EnquiryId)
        {
            //Session["IRTransCode"] = TransCode;

            List<VehicleVM> ISRegNoData = (List<VehicleVM>)Session["lstVehicle"];


            var data = ISRegNoData.Where(a => a.VersionCode.Trim() == EnquiryId.Trim());

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Insert_VehicleTaxDetail(List<VehicleTaxDetailVM> objects)
        {
            bool result = false;
            int count = 0;
            string msg = "Failed to save record..";


            //foreach(var item in objects)
            //{
            //    if(count >= 1 || item.BrandCode != null)
            //    {
            result = VehicleMethods.Insert_VehicleTaxDetail(objects, Session["DealerCode"].ToString(),ref msg);
            //    }
            //    count++;

            //}       
            msg = msg + "  Failed to save record..";
            if (result)
            {
                msg = "Successfully Added";
            }
           

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Select_Vehicle(string EnquiryId)
        {
            string data = "";
            bool result = false;
            data = VehicleMethods.Get_VehicleMasterData(EnquiryId, Session["DealerCode"].ToString());

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Select_VehicleDetail(string EnquiryId, string Code)
        {
            string data = "";
            bool result = false;
            data = VehicleMethods.Get_VehicleDetailData(EnquiryId, Session["DealerCode"].ToString());

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Select_VehicleCategory(string EnquiryId, string DealerCode)
        {
            string data = "";
            bool result = false;
            data = VehicleMethods.Get_VehicleCategory(EnquiryId, DealerCode);

            if (data != "")
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);

        }
    }
}