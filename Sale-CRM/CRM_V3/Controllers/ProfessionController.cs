using Core.CRM.ADO;
using Core.CRM.ADO.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRM_V3.Controllers
{
    public class ProfessionController : Controller
    {
        static string dealerCode = string.Empty;
        // GET: Profession
        public ActionResult Profession()
        {
            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("Login", "Home");
            }
            dealerCode = Session["DealerCode"].ToString();
            return View();
        }
        [HttpGet]
        public JsonResult Select_ProfessionDetail()
        {
            string data = "";
            bool result = false;
            data = ProfessionMethods.Get_ProfessionTypeData();

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Insert_ProfessionType(ProfessionTypeVM objects)
        {
            bool result = false;

            string msg = "Failed to save record..";

            result = ProfessionMethods.Insert_ProfessionType(objects);

            if (result)
            {
                msg = "Successfully Added";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }
    }
}