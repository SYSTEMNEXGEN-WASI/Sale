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
    public class CustomerController : Controller
    {
        static string dealerCode = string.Empty;
        SecurityBll common = new SecurityBll();
        public ActionResult Customer()
        {
            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("NewLogin", "Home");
            }
            if (common.UserRight("2502", "001"))
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

                //List<SelectListItem> ddlCustomers = new List<SelectListItem>();
                //ddlCustomers = GeneralMethods.GetDataFromSPWithDealerCode("SP_Select_Customer",dealerCode);
                //ViewBag.Customers = ddlCustomers;

                List<SelectListItem> ddlCusType = new List<SelectListItem>();
                ddlCusType = GeneralMethods.GetDataFromSPWithDealerCode("SP_SelectCustomerType", dealerCode);
                ViewBag.CusType = ddlCusType;

                //DataTable dt = new DataTable();
                //List<CustomerVM> lst = new List<CustomerVM>();

                //SqlParameter[] sqlParam =
                //    {
                //        new SqlParameter("@DealerCode",dealerCode)
                //    };
                //dt = DataAccess.getDataTable("SP_Select_CustomerModal", sqlParam, General.GetBMSConString());

                //if (dt.Rows.Count > 0)
                //{
                //    lst = EnumerableExtension.ToList<CustomerVM>(dt);
                //}
                List<CustomerVM> lstCustommer = CustomerMethods.GetCustomerModal(dealerCode);
                ViewBag.Customer = lstCustommer;

                List<AccountVM> lstAccount = CustomerMethods.Get_AccDesc(dealerCode);
                ViewBag.Accounts = lstAccount;
            }
            else
            {
                TempData["TestAccessError"] = MessageAlert.MsgAuthorized();
                return RedirectToAction("Error", "Definition");
            }




            return View();
        }


        public JsonResult Insert_Customer(CustomerVM CustomerVM)
        {
            bool result = false;

            string msg = "Failed to save record..";

            result = CustomerMethods.Insert_Customer(CustomerVM, Session["DealerCode"].ToString(),ref msg);

            if (result)
            {
                msg = "Successfully Added";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Insert_Guarantor(List<GuarantorVM> objects)
        {
            bool result = false;

            string msg = "Failed to save record..";

            result = CustomerMethods.Insert_Guarantor(objects, Session["DealerCode"].ToString());

            if (result)
            {
                msg = "Successfully Added";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Select_Customer(string EnquiryId)
        {
            string data = "";
            bool result = false;
            data = CustomerMethods.Get_CustomerData(EnquiryId, Session["DealerCode"].ToString());

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Select_Guarantor(string EnquiryId)
        {
            string data = "";
            bool result = false;
            data = CustomerMethods.Get_GuarantorData(EnquiryId, Session["DealerCode"].ToString());

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Delete_Customer(string EnquiryId)
        {
            bool result = false;

            string msg = "Customer is Used , Data can't be deleted";

            result = CustomerMethods.Delete_Customer_Record(EnquiryId, Session["DealerCode"].ToString());

            if (result)
            {
                msg = "Successfully Deleted";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }
    }
}