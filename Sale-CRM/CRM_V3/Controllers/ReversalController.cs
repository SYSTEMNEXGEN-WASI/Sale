using Core.CRM.ADO;
using Core.CRM.ADO.ViewModel;
using Core.CRM.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRM_V3.Controllers
{
    public class ReversalController : Controller
    {
        // GET: Reversal
        static DataTable dt = new DataTable();
        static string dealerCode = string.Empty;
        public ActionResult Index()
        {
            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("Login", "Home");
            }
            dealerCode = Session["DealerCode"].ToString();

            dt = ReversalMethods.GetDataForModal(dealerCode);
            List<AccountTransactionVM> lstAccount = new List<AccountTransactionVM>();
            if (dt.Rows.Count > 0)
            {
                lstAccount = EnumerableExtension.ToList<AccountTransactionVM>(dt);
            }

            ViewBag.AccountTrans = lstAccount;
            

            return View();
        }

        public JsonResult Insert_Reversal(ReversalVM objects)
        {
            bool result = false;

            string msg = "Failed to save record..";

            result = ReversalMethods.Insert_Reversal(objects,ref msg);

            if (result)
            {
                msg = "Successfully Added";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Insert_ReversalAccountTrans(AccountTransactionVM objects)
        {
            bool result = false;
            string msg = "Failed to save record..";

            result = ReversalMethods.Insert_AccountTransaction(objects, ref msg);

            if (result)
            {
                msg = "Successfully Added";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }
    }
}