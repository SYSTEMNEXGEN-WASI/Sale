using Core.CRM.ADO;
using Core.CRM.ADO.ViewModel;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Linq;

namespace CRM_V3.Controllers
{
   
    public class VoucherController : Controller
    {

        SysFunction sys = new SysFunction();
        static string dealerCode = string.Empty;
        static string Formtype = "";
        static string newVouch;

        // GET: Voucher

        public ActionResult VoucherReceipt(string leadId = "", string type = "", string ChassisNo = "", string VouchNo = "")
        {
            Session["Formtype"] = type;
            VouchNo = Session["VoucherNo"].ToString();
            DataTable dt = new DataTable();

            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("NewLogin", "Home");
            }
            dealerCode = Session["DealerCode"].ToString();
            List<SelectListItem> ddljournalNo = new List<SelectListItem>();
            ddljournalNo = VoucherMethods.initializeDDLs(Session["DealerCode"].ToString());
            ViewBag.Journal = ddljournalNo;

            bool result = VoucherMethods.GetNewVoucherNoReceipt("GVouMaster", "VouchNo", 3, Session["DealerCode"].ToString(), VouchNo, ref newVouch);
            ViewBag.VoucherNo = newVouch;
            List<VoucherVM> lstAccount = new List<VoucherVM>();

            // string voucheNo = "";
            if (type == "VReceipt")
            {
                if (VouchNo != "")
                {
                    ViewBag.Text = "Edit";
                    ViewBag.VoucherNo = VouchNo;
                    lstAccount = VoucherMethods.LoadVehicleReceiptGrid(leadId, Session["DealerCode"].ToString(), ChassisNo);
                    //lstAccount = VoucherMethods.LoadVoucher(VouchNo, Session["DealerCode"].ToString());

                    // ViewBag.InstNo = lstAccount.ElementAt(1).InstrumentNo;
                    // ViewBag.InstDate = lstAccount.ElementAt(1).InstrumentDate;

                }
                else
                {
                    if (type == "VReceipt")
                    {
                        ViewBag.Text = "Add";
                        // voucheNo=
                        List<SelectListItem> AccountCode = new List<SelectListItem>();
                        // AccountCode = GeneralMethods.GetDataFromSPWithDealerCodeRec("SP_Get_VehicleAccountCodeDetail", dealerCode,leadId);
                        // voucheNo = Acc;
                        lstAccount = VoucherMethods.LoadVehicleReceiptGrid(leadId, Session["DealerCode"].ToString(), ChassisNo);
                    }
                }

                ViewBag.lstAccount = lstAccount;

                double debitrec = 0;
                double creditrec = 0;
                foreach (var item in lstAccount)
                {
                    creditrec = creditrec + double.Parse(item.Credit);
                    debitrec = debitrec + double.Parse(item.Debit);

                }

                ViewBag.Debit = debitrec;
                ViewBag.Credit = creditrec;
            }

            return View();
        }

        //csi voucher
        public ActionResult VoucherCSI(string leadId = "", string type = "", string ChassisNo = "", string VouchNo = "")
        {

            Session["Formtype"] = type;
            VouchNo = Session["VoucherNo"].ToString();
            if (string.IsNullOrEmpty((string)Session["VoucherNo"]))
            {
                VouchNo = "";
            }
            DataTable dt = new DataTable();

            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("NewLogin", "Home");
            }
            dealerCode = Session["DealerCode"].ToString();
            List<SelectListItem> ddljournalNo = new List<SelectListItem>();
            ddljournalNo = VoucherMethods.initializeDDLs(Session["DealerCode"].ToString());
            ViewBag.Journal = ddljournalNo;

            bool result = VoucherMethods.GetNewVoucherNoReceipt("GVouMaster", "VouchNo", 3, Session["DealerCode"].ToString(), VouchNo, ref newVouch);
            ViewBag.VoucherNo = newVouch;
            List<VoucherVM> lstAccount = new List<VoucherVM>();

            // string voucheNo = "";
            if (type == "VReceipt")
            {
                if (VouchNo != "")
                {
                    ViewBag.Text = "Edit";
                    ViewBag.VoucherNo = VouchNo;
                    lstAccount = VoucherMethods.LoadVehicleReceiptGrid(leadId, Session["DealerCode"].ToString(), ChassisNo);
                    //lstAccount = VoucherMethods.LoadVoucher(VouchNo, Session["DealerCode"].ToString());

                    // ViewBag.InstNo = lstAccount.ElementAt(1).InstrumentNo;
                    // ViewBag.InstDate = lstAccount.ElementAt(1).InstrumentDate;

                }
                else
                {
                    if (type == "VReceipt")
                    {
                        ViewBag.Text = "Add";
                        // voucheNo=
                        List<SelectListItem> AccountCode = new List<SelectListItem>();
                        // AccountCode = GeneralMethods.GetDataFromSPWithDealerCodeRec("SP_Get_VehicleAccountCodeDetail", dealerCode,leadId);
                        // voucheNo = Acc;
                        lstAccount = VoucherMethods.LoadVehicleReceiptGrid(leadId, Session["DealerCode"].ToString(), ChassisNo);
                    }
                }

                ViewBag.lstAccount = lstAccount;

                double debitrec = 0;
                double creditrec = 0;
                foreach (var item in lstAccount)
                {
                    creditrec = creditrec + double.Parse(item.Credit);
                    debitrec = debitrec + double.Parse(item.Debit);

                }

                ViewBag.Debit = debitrec;
                ViewBag.Credit = creditrec;
            }
            return View();
        }
        //csi end
        public ActionResult VoucherMain(string leadId = "", string type = "", string VouchNo = "", string ChassisNo = "")
        {
            Session["Formtype"] = type;

            DataTable dt = new DataTable();
            string msg = "";
            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("NewLogin", "Home");
            }
            dealerCode = Session["DealerCode"].ToString();
            if (string.IsNullOrEmpty((string)Session["VoucherNo"]))
            {
                Session["VoucherNo"] = "";
            }
            VouchNo = Session["VoucherNo"].ToString();
            List<SelectListItem> ddljournalNo = new List<SelectListItem>();
            ddljournalNo = VoucherMethods.initializeDDLs(Session["DealerCode"].ToString());
            ViewBag.Journal = ddljournalNo;

            ViewBag.VoucherNo = VoucherMethods.GetNewVoucherNo("GVouMaster", "VouchNo", 3, Session["DealerCode"].ToString());

            List<VoucherVM> lstAccount = new List<VoucherVM>();

            string voucheNo = "";



            if (type == "ASC")
            {
                voucheNo = sys.GetStringValuesAgainstCodes("ReceiptNo", leadId, "VoucherNo", "ReceiptMaster", "", Session["DealerCode"].ToString());
            }
            else if (type == "CSI")
            {
                voucheNo = sys.GetStringValuesAgainstCodes("TransCode", leadId, "VoucherNo", "VehicleSaleMaster", "", Session["DealerCode"].ToString());
            }
            else if (type == "VReceipt")
            {
                voucheNo = sys.GetStringValuesAgainstCodes("RecNo", leadId, "VoucherNo", "ProdRecMaster", "", Session["DealerCode"].ToString());
            }
            else if (type == "BO")
            {
                voucheNo = sys.GetStringValuesAgainstCodes("BookRefNo", leadId, "VoucherNo", "BookOrdMaster", "", Session["DealerCode"].ToString());
            }
            else if (type == "PR")
            {
                voucheNo = sys.GetStringValuesAgainstCodes("ReceiptNo", leadId, "VoucherNo", "PaymentReceiptMaster", "", Session["DealerCode"].ToString());
            }
            if (voucheNo != "")
            {
                ViewBag.Text = "Edit";
                // ViewBag.VoucherNo = voucheNo;

                //lstAccount = VoucherMethods.LoadVoucher(voucheNo, Session["DealerCode"].ToString());

                //ViewBag.InstNo = lstAccount.ElementAt(1).InstrumentNo;
                //ViewBag.InstDate = lstAccount.ElementAt(1).InstrumentDate;
                if (type == "CSI")
                {

                    ViewBag.VoucherNo = voucheNo;

                    lstAccount = VoucherMethods.LoadCSI(leadId, Session["DealerCode"].ToString(), ChassisNo);

                    ViewBag.InstNo = lstAccount.ElementAt(1).InstrumentNo;
                    ViewBag.InstDate = lstAccount.ElementAt(1).InstrumentDate;

                }
                else if (type == "ASC")
                {
                    ViewBag.VoucherNo = voucheNo;
                    lstAccount = VoucherMethods.LoadCSGrid(leadId, Session["DealerCode"].ToString());

                    dt = sys.GetDatas("Select isnull(InstrumentNo,'') InstrumentNo, isnull(Convert(varchar(10),InstrumentDate,105),'') InstrumentDate From [ReceiptDetail] Where ReceiptNo = '" + leadId + "' AND [DealerCode] = '" + Session["DealerCode"].ToString() + "'", "BMS0517ConnectionString");
                    if (dt.Rows.Count > 0)
                    {
                        ViewBag.InstNo = dt.Rows[0]["InstrumentNo"].ToString();
                        ViewBag.InstDate = dt.Rows[0]["InstrumentDate"].ToString();
                    }

                }
                else if (type == "VReceipt")
                {
                    ViewBag.VoucherNo = voucheNo;

                    lstAccount = VoucherMethods.LoadVehicleReceiptGrid(leadId, Session["DealerCode"].ToString(), ChassisNo);

                    ViewBag.InstNo = lstAccount.ElementAt(1).InstrumentNo;
                    ViewBag.InstDate = lstAccount.ElementAt(1).InstrumentDate;
                }
                else if (type == "BO")
                {
                    ViewBag.VoucherNo = voucheNo;

                    lstAccount = VoucherMethods.LoadBookingOrderGrid(leadId, Session["DealerCode"].ToString(), ChassisNo);

                    ViewBag.InstNo = lstAccount.ElementAt(1).InstrumentNo;
                    ViewBag.InstDate = lstAccount.ElementAt(1).InstrumentDate;
                }
                else if (type == "PR")
                {
                    ViewBag.VoucherNo = voucheNo;

                    lstAccount = VoucherMethods.LoadBookingOrderGrid(leadId, Session["DealerCode"].ToString(), ChassisNo);

                    ViewBag.InstNo = lstAccount.ElementAt(1).InstrumentNo;
                    ViewBag.InstDate = lstAccount.ElementAt(1).InstrumentDate;
                }
            }
            else
            {
                ViewBag.Text = "Add";

                if (type == "DE")
                {
                    lstAccount = VoucherMethods.LoadDEGrid(leadId, Session["DealerCode"].ToString());
                    dt = sys.GetDatas("Select isnull(InsNo,'') InstrumentNo, isnull(Convert(varchar(10),InsDate,105),'') InstrumentDate From [DailyExpenseMaster] Where ExpInvNo = '" + leadId + "' AND [DealerCode] = '" + Session["DealerCode"].ToString() + "'", "BMS0517ConnectionString");
                    if (dt.Rows.Count > 0)
                    {
                        ViewBag.InstNo = dt.Rows[0]["InstrumentNo"].ToString();
                        ViewBag.InstDate = dt.Rows[0]["InstrumentDate"].ToString();
                    }
                }
                else if (type == "CSI")
                {

                    lstAccount = VoucherMethods.LoadCSI(leadId, Session["DealerCode"].ToString(), ChassisNo);

                    //dt = sys.GetData("Select isnull(InstrumentNo,'') InstrumentNo, isnull(Convert(varchar(10),InstrumentDate,105),'') InstrumentDate From [ReceiptDetail] Where ReceiptNo = '" + leadId + "' AND [DealerCode] = '" + Session["DealerCode"].ToString() + "'", "BMS0517ConnectionString");
                    //if (dt.Rows.Count > 0)
                    //{
                    //    ViewBag.InstNo = dt.Rows[0]["InstrumentNo"].ToString();
                    //    ViewBag.InstDate = dt.Rows[0]["InstrumentDate"].ToString();
                    //}


                }
                else if (type == "VReceipt")
                {
                    

                    lstAccount = VoucherMethods.LoadVehicleReceiptGrid(leadId, Session["DealerCode"].ToString(), ChassisNo);

                    
                }
                else if (type == "BO")
                {


                    lstAccount = VoucherMethods.LoadBookingOrderGrid(leadId, Session["DealerCode"].ToString(), ChassisNo);


                }
                else if (type == "PR")
                {


                    lstAccount = VoucherMethods.LoadPRGrid(leadId, Session["DealerCode"].ToString(), ChassisNo);


                }
                else
                {
                    lstAccount = VoucherMethods.LoadCSGrid(leadId, Session["DealerCode"].ToString());

                    dt = sys.GetDatas("Select isnull(InstrumentNo,'') InstrumentNo, isnull(Convert(varchar(10),InstrumentDate,105),'') InstrumentDate From [ReceiptDetail] Where ReceiptNo = '" + leadId + "' AND [DealerCode] = '" + Session["DealerCode"].ToString() + "'", "BMS0517ConnectionString");
                    if (dt.Rows.Count > 0)
                    {
                        ViewBag.InstNo = dt.Rows[0]["InstrumentNo"].ToString();
                        ViewBag.InstDate = dt.Rows[0]["InstrumentDate"].ToString();
                    }
                }



            }

            ViewBag.lstAccount = lstAccount;

            double debit = 0;
            double credit = 0;
            foreach (var item in lstAccount)
            {
                debit = debit + double.Parse(item.Debit);
                credit = credit + double.Parse(item.Credit);
            }

            ViewBag.Debit = debit;
            ViewBag.Credit = credit;

            return View();
        }


        [HttpPost]
        public JsonResult Insert_GvoucherMaster(List<GVouMasterVM> objects)
        {
            bool result = false;

            string msg = "Failed to save record..";

            result = VoucherMethods.Insert_Gvoucher(objects, Session["Formtype"].ToString(), ref msg);

            if (result)
            {
                msg = "Successfully Added";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Select_VoucherType(string Type)
        {
            List<SelectListItem> data;
            bool result = false;

            //List<SelectListItem> ddljournalNo = new List<SelectListItem>();
            //ddljournalNo = VoucherMethods.initializeDDLs(Session["DealerCode"].ToString());
            //ViewBag.Journal = ddljournalNo;

            data = VoucherMethods.initializeDDLs(Session["DealerCode"].ToString(), Type);

            if (data.Count > 0)
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);

        }
    }
}