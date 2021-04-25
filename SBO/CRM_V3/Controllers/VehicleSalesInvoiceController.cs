using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRM_V3.Controllers
{
    public class VehicleSalesInvoiceController : Controller
    {
        static string dealerCode = string.Empty;
        // GET: VehicleSalesInvoice
        public ActionResult VehicleSalesInvoice()
        {
            
            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("NewLogin", "Home");
            }
            dealerCode = Session["DealerCode"].ToString();
            return View();
        }
    }
}