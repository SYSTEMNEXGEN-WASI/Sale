using Core.CRM.ADO;
using Core.CRM.ADO.ViewModel;
using Core.CRM.Helper;
using CRM_V3.assets;
using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRM_V3.Controllers
{
    public class OutGoingPaymentController : Controller
    {
        // GET: OutGoingPayment
        public ActionResult OutgoingPayment()
        {
            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("Login", "Home");
            }
            string dealerCode = Session["DealerCode"].ToString();
            List<SelectListItem> ddlPayMode = new List<SelectListItem>();
            ddlPayMode = GeneralMethods.GetDataFromSPWithDealerCode("Select_PaymentMode", dealerCode);
            ViewBag.PayMode = ddlPayMode;

            List<SelectListItem> ddlVehExpHead = new List<SelectListItem>();
            ddlVehExpHead = GeneralMethods.GetDataFromSP("SP_Select_VehExpHead");
            ViewBag.VehExpHead = ddlVehExpHead;

            List<SelectListItem> ddlBank = new List<SelectListItem>();
            ddlBank = GeneralMethods.GetBank();
            ViewBag.Bank = ddlBank;

            List<SelectListItem> ddlCity = new List<SelectListItem>();
            ddlCity = GeneralMethods.GetCity();
            ViewBag.City = ddlCity;
            return View();
        }
    }
}