using Core.CRM.ADO;
using Core.CRM.ADO.ViewModel;
using Core.CRM.Helper;
using CRM_V3.assets;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRM_V3.Controllers
{
    public class MasterController : Controller
    {
        static string dealerCode = string.Empty;

        sngonclo_BMSEntities BMS = new sngonclo_BMSEntities();
        // GET: Master
        public ActionResult MasterSetup()
        {
            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("NewLogin", "Home");
            }
            dealerCode = Session["DealerCode"].ToString();
            List<AccountVM> Get_AccDesc = new List<AccountVM>();
            Get_AccDesc = MasterMethods.Get_AccDesc(dealerCode);
            ViewBag.Get_AccDesc = Get_AccDesc;
            return View();
        }

        [HttpGet]
        public JsonResult Select_VehicleDetail()
        {
            string data = "";
            bool result = false;
            data = MasterMethods.Get_VehicleTypeData(Session["DealerCode"].ToString());

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Select_ReceiptDetail()
        {
            string data = "";
            bool result = false;
            data = MasterMethods.Get_VehRecTypeData(Session["DealerCode"].ToString());

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Select_DocumentDetail()
        {
            string data = "";
            bool result = false;
            data = MasterMethods.Get_DocumentTypeData(Session["DealerCode"].ToString());

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Select_LocationDetail()
        {
            string data = "";
            bool result = false;
            data = MasterMethods.Get_LocationDetail(Session["DealerCode"].ToString());

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }
        
              public JsonResult Insert_AccountMapping(AccountTransactionVM objects)
        {
            bool result = false;

            string msg = "Failed to save record..";

            result = MasterMethods.Insert_AccountCOdeSetup(objects);

            if (result)
            {
                msg = "Successfully Added";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Insert_VehicleType(VehicleTypeVM objects)
        {
            bool result = false;

            string msg = "Failed to save record..";

            result = MasterMethods.Insert_VehicleType(objects);

            if (result)
            {
                msg = "Successfully Added";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Delete_VehicleType(string table, string Column, string ColumnCode, string DealerCode)
        {
            bool result = false;

            string msg = "Failed to Delete record..";

            result = MasterMethods.Delete_VehicleType(table, Column, ColumnCode, DealerCode);

            if (result)
            {
                msg = "Successfully Deleted";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Insert_VehicleReceiptType(VehReceiptTypoVM objects)
        {
            bool result = false;

            string msg = "Failed to save record..";

            result = MasterMethods.Insert_VehReceiptType(objects);

            if (result)
            {
                msg = "Successfully Added";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Insert_DocumentType(DocumentCheckList objects)
        {
            bool result = false;

            string msg = "Failed to save record..";

            result = MasterMethods.Insert_DocumentType(objects);

            if (result)
            {
                msg = "Successfully Added";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Insert_VehLocation(VehicleLocationVM objects)
        {
            bool result = false;

            string msg = "Failed to save record..";

            result = MasterMethods.Insert_VehLocation(objects);

            if (result)
            {
                msg = "Successfully Added";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }
        #region ------------ Service Master ------------------

        public JsonResult Insert_Service(ServiceVM objects)
        {
            bool result = false;

            string msg = "Failed to save record..";

            result = MasterMethods.Insert_Service(objects);

            if (result)
            {
                msg = "Successfully Added";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult Select_AccountDetail()
        {
            string data = "";
            bool result = false;
            data = MasterMethods.Get_AccountDetail(Session["DealerCode"].ToString());

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult Select_ServiceDetail()
        {
            string data = "";
            bool result = false;
            data = MasterMethods.Get_ServiceDetail(Session["DealerCode"].ToString());

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region ------------ Expense Master ------------------

        public ActionResult ExpenseMain()
        {
            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("Login", "Home");
            }
            dealerCode = Session["DealerCode"].ToString();

            ViewBag.ReceiptHead = BMS.ExpenseHeads.Where(x => x.DealerCode == dealerCode).ToList();

            List<AccountVM> lstAccount = AdvSerChargMethods.GetAccountModal(dealerCode);

            ViewBag.Accounts = lstAccount;

            return View();
        }

        public JsonResult Insert_ExpenseType(ExpenseHeadVM objects)
        {
            bool result = false;

            string msg = "Failed to save record..";

            result = MasterMethods.Insert_ExpenseType(objects);

            if (result)
            {
                msg = "Successfully Added";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region -------------------- Delivery Check List -------------------------

        public ActionResult DelChkListMain(int PageNumber = 1, int RowsPerPage = 5)
        {
            int skip = (RowsPerPage * (PageNumber - 1));

            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("Login", "Home");
            }
            dealerCode = Session["DealerCode"].ToString();

            List<DeliveryCheckListVM> lstDelChkList = (List<DeliveryCheckListVM>)Session["DelChkList"];

            if (Session["DelChkList"] == null)
            {
                lstDelChkList = MasterMethods.Get_DelChkList(dealerCode);
            }

            var _dataList = new List<DeliveryCheckListVM>();
            _dataList = lstDelChkList.Skip(skip).Take(RowsPerPage).ToList();

            ViewBag.DelChkList = lstDelChkList;

            ViewBag.Pages = lstDelChkList.Count / RowsPerPage + 1;
            ViewBag.PageNumber = PageNumber;
            ViewBag.Records = RowsPerPage;

            return View(_dataList);
        }

        public JsonResult Insert_DeliveryCheckList(DeliveryCheckListVM objects)
        {
            bool result = false;

            string msg = "Failed to save record..";

            result = MasterMethods.Insert_DeliveryCheckList(objects);

            if (result)
            {
                msg = "Successfully Added";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }

        #endregion


        public ActionResult Commision()
        {
            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("Login", "Home");
            }
            dealerCode = Session["DealerCode"].ToString();

            DataTable Empdt, Productdt, Commisondt;

            List<EmployeeVM> item = new List<EmployeeVM>();

            Empdt = GeneralMethods.GetDataForModal("SP_Select_DealerEmpForCommision", dealerCode);
            if (Empdt.Rows.Count > 0)
            {
                item = EnumerableExtension.ToList<EmployeeVM>(Empdt);
            }

            ViewBag.AssignTo = item;

            List<GetProductSpVM> Productlist = new List<GetProductSpVM>();
            Productdt = GeneralMethods.GetDataForModal("SP_Select_ProductForCommision", dealerCode);
            if (Productdt.Rows.Count > 0)
            {
                Productlist = EnumerableExtension.ToList<GetProductSpVM>(Productdt);
            }
            ViewBag.Product = Productlist;

            List<CommisionVM> CommisionList = new List<CommisionVM>();
            Commisondt = GeneralMethods.GetDataForModal("SP_Select_CommisionMaster", dealerCode);
            if (Commisondt.Rows.Count > 0)
            {
                CommisionList = EnumerableExtension.ToList<CommisionVM>(Commisondt);
            }
            ViewBag.Commision = CommisionList;
            List<SelectListItem> ddlColor = new List<SelectListItem>();
            ddlColor = GeneralMethods.GetColor(Session["VehicleCategory"].ToString());
            ViewBag.Color = ddlColor;

            return View();
        }
        [HttpPost]
        public JsonResult Insert_Commision(CommisionVM objects)
        {
            bool result = false;

            string msg = "Failed to save record..";

            result = MasterMethods.Insert_Commision(objects, ref msg);

            if (result)
            {
                msg = "Successfully Added";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }
    }
}