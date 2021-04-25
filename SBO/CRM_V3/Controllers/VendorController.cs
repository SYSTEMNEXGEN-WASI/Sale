using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CRM_V3.Models;
using System.Data.Entity.Validation;
using Core.CRM.ADO;
using Core.CRM.ADO.ViewModel;
using System.Data.SqlClient;
using Core.CRM.Helper;
using System.Data;
using DXBMS;
using CRM.Models.Classes;

namespace CRM_V3.Controllers
{
    public class VendorController : Controller
    {
        // GET: Vendor
        static string dealerCode = string.Empty;
        SecurityBll common = new SecurityBll();
        public ActionResult Vendor()
        {

            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("NewLogin", "Home");
            }
            
          
            if (common.UserRight("2501", "001"))
            {
                dealerCode = Session["DealerCode"].ToString();

            List<SelectListItem> ddlCity = new List<SelectListItem>();
            ddlCity = GeneralMethods.GetDataFromSP("Select_City");
            ViewBag.City = ddlCity;

            List<SelectListItem> ddlState = new List<SelectListItem>();
            ddlState = GeneralMethods.GetDataFromSP("Select_State");
            ViewBag.State = ddlState;

            List<SelectListItem> ddlCountry = new List<SelectListItem>();
            ddlCountry = GeneralMethods.GetDataFromSP("Select_Country");
            ViewBag.Country = ddlCountry;

            //List<SelectListItem> ddlVendors = new List<SelectListItem>();
            //ddlVendors = GeneralMethods.GetDataFromSPWithDealerCode("SP_Select_Vendor",dealerCode);
            //ViewBag.Vendors = ddlVendors;

            // List<SelectListItem> ddlCusType = new List<SelectListItem>();
            //ddlCusType = GeneralMethods.GetDataFromSPWithDealerCode("SP_SelectVendorType", dealerCode);
            //ViewBag.CusType = ddlCusType;

            //DataTable dt = new DataTable();
            //List<VendorVM> lst = new List<VendorVM>();

            //SqlParameter[] sqlParam =
            //    {
            //        new SqlParameter("@DealerCode",dealerCode)
            //    };
            //dt = DataAccess.getDataTable("SP_Select_VendorModal", sqlParam, General.GetBMSConString());

            //if (dt.Rows.Count > 0)
            //{
            //    lst = EnumerableExtension.ToList<VendorVM>(dt);
            //}
            List<VendorVM> lstCustommer = VendorMethods.GetVendorModal(dealerCode);
            ViewBag.Vendor = lstCustommer;

            List<AccountVM> lstAccount = AdvSerChargMethods.GetAccountModal(dealerCode);
            ViewBag.Accounts = lstAccount;
            }
            else
            {
                TempData["TestAccessError"] = MessageAlert.MsgAuthorized();
                return RedirectToAction("Error", "Definition");
            }
            return View();
        }


        public JsonResult Insert_Vendor(VendorVM VendorVM)
        {
            bool result = false;

            string msg = "Failed to save record..";

            result = VendorMethods.Insert_Vendor(VendorVM, Session["DealerCode"].ToString());

            if (result)
            {
                msg = "Successfully Added";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public JsonResult Select_Vendor(string EnquiryId)
        {
            string data = "";
            bool result = false;
            data = VendorMethods.Get_VendorData(EnquiryId, Session["DealerCode"].ToString());

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }

        //  [HttpGet]


        //public JsonResult Delete_Vendor(string EnquiryId)
        //{
        //    bool result = false;

        //    string msg = "Vendor is Used , Data can't be deleted";

        //    result = VendorMethods.Delete_Vendor_Record(EnquiryId, Session["DealerCode"].ToString());

        //    if (result)
        //    {
        //        msg = "Successfully Deleted";
        //    }

        //    return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        //}




    }
}