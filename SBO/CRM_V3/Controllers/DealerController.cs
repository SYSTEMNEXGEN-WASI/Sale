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
using System.IO;
using DXBMS;
using CRM.Models.Classes;

namespace CRM_V3.Controllers
{
    public class DealerController : Controller
    {
        // GET: Dealer
        SecurityBll common = new SecurityBll();
        public ActionResult Dealer()
        {

            Session["CompLogo"] = "";
            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("NewLogin", "Home");
            }
            if (common.UserRight("2504", "001"))
            {

                string DealerCode=Session["DealerCode"].ToString();
            List<SelectListItem> ddlCity = new List<SelectListItem>();
            ddlCity = GeneralMethods.GetDataFromSP("Select_City");
            ViewBag.City = ddlCity;

           

            List<SelectListItem> ddlCountry = new List<SelectListItem>();
            ddlCountry = GeneralMethods.GetDataFromSP("Select_Country");
            ViewBag.Country = ddlCountry;


            }
            else
            {
                TempData["TestAccessError"] = MessageAlert.MsgAuthorized();
                return RedirectToAction("Error", "Definition");
            }
            return View();
        }

        [HttpPost]
        public JsonResult UploadFile(string data, string EnquiryId)
        {
            var path = "";
            var fileExtension = "";
            var fileName = "";
            var j = "";


            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase file = Request.Files[0];

                if (file != null && file.ContentLength > 0)
                {
                    fileName = Path.GetFileName(file.FileName);
                    fileExtension = Path.GetExtension(file.FileName);
                    path = "Images/"+fileName;
                    if (EnquiryId == "Pic1")
                    {
                        //do bits, save to DB etc./..

                        //file.SaveAs(path);
                        Session["CompLogo"] = path;
                        ViewBag.path1 = path;
                    }
                    
                }
            }
            return Json(new { fileName = fileName });
        }


        [HttpGet]
        public JsonResult Select_Dealer(string EnquiryId)
        {
            string DealerCode = Session["DealerCode"].ToString();
            string data = "";
            bool result = false;
            data = DealerMethods.Get_DealerData(DealerCode);

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Insert_Dealer(DealerVM DealerVM)
        {
            bool result = false;

            string msg = "Failed to save record..";
            DealerVM.Logo = Session["CompLogo"].ToString();
            result = DealerMethods.Insert_Dealer(DealerVM, Session["DealerCode"].ToString());

            if (result)
            {
                msg = "Successfully Added";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }
    }
}