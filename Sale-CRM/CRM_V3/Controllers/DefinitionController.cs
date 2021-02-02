using Core.CRM.ADO;
using Core.CRM.ADO.ViewModel;
using CRM_V3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Excel = Microsoft.Office.Interop.Excel;

namespace CRM_V3.Controllers
{
    public class DefinitionController : Controller
    {

        SecurityBll common = new SecurityBll();
        //
        // GET: /Definition/
        public ActionResult CostCenter()
        {
            return View();
        }

        public ActionResult AccountSetup()
        {
            
            return View();
        }
        public ActionResult Error()
        {
            if (Session["UserCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }

        //[HttpPost]
        //public ActionResult AccountSetup()
        //{

        //    return RedirectToAction("AccountSetup");
        //}

        public ActionResult PaymentMode()
        {
            return View();
        }

        public ActionResult BankForm()
        {
            return View();
        }

        public ActionResult EmployeeProfile()
        {
            return View();
        }

        public ActionResult TaxPaymentReceiptHead()
        {
            return View();
        }

        public ActionResult InstallmentPlan()
        {

            List<InstallmentPlanVM> InstallmentPlanData = new List<InstallmentPlanVM>();
            InstallmentPlanData = InstallmentPlanMethods.Get_InstallmentData();
            ViewBag.InstallmentPlanData = InstallmentPlanData;

            List<SelectListItem> ddlBrandCode = new List<SelectListItem>();
            ddlBrandCode = GeneralMethods.GetBrands(Session["DealerCode"].ToString());
            ViewBag.BrandCode = ddlBrandCode;

            var OnlyDate = DateTime.Now;
            ViewBag.CurrentDate = OnlyDate.ToString("dd/MM/yyyy");

            return View();
        }

        public ActionResult BrandMethod(string BrandCode)
        {
            List<GetProductSpVM> ddlAssignPro = new List<GetProductSpVM>();
            ddlAssignPro = GeneralMethods.GetProductDetail(BrandCode,Session["DealerCode"].ToString());
            var data = ddlAssignPro;

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult InstallmentPlanSchedule(string BrandCode, string ProdCode, string VersionCode, string Color)
        {
            List<CustomerInstallmentScheduleVM> IPlanSchedule = new List<CustomerInstallmentScheduleVM>();
            IPlanSchedule = InstallmentPlanMethods.Get_InstallmentPlanSchedule(BrandCode, ProdCode, VersionCode, Color);
            var data = IPlanSchedule;

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Import(HttpPostedFileBase excelfile)
        {
            //List<SelectListItem> ddlSMSTemp = new List<SelectListItem>();
            string data = "";
            bool result = false;
            var Serializer = new JavaScriptSerializer();
            //ddlSMSTemp = GeneralMethods.GetDataFromSpWithDealerCode("SP_SelectSMStemplate", DealerCode);
            //ViewBag.SMSTemp = ddlSMSTemp;

            if (excelfile == null || excelfile.ContentLength == 0)
            {
                ViewBag.Error = "Please Select a excel file<br>";
                return View("InstallmentPlan");
                //return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (excelfile.FileName.EndsWith(".xls") || excelfile.FileName.EndsWith(".xlsx"))
                {
                    string path = Server.MapPath("~/Content/" + excelfile.FileName);
                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);

                    excelfile.SaveAs(path);
                    Excel.Application application = new Excel.Application();
                    Excel.Workbook workbook = application.Workbooks.Open(path);
                    Excel.Worksheet worksheet = (Excel.Worksheet)workbook.ActiveSheet;
                    Excel.Range range = worksheet.UsedRange;
                    List<InstallmentPlanVM> listProduct = new List<InstallmentPlanVM>();
                    for (int row = 2; row <= range.Rows.Count; row++)
                    {

                        InstallmentPlanVM P = new InstallmentPlanVM();

                        P.PlanID = ((Excel.Range)range.Cells[row, 3]).Text;
                        P.PlanType = ((Excel.Range)range.Cells[row, 4]).Text;
                        P.BrandCode = ((Excel.Range)range.Cells[row, 5]).Text;
                        P.ProdCode = ((Excel.Range)range.Cells[row, 6]).Text;
                        P.VersionCode = ((Excel.Range)range.Cells[row, 7]).Text;
                        P.ColorCode = ((Excel.Range)range.Cells[row, 8]).Text;
                        P.Color = ((Excel.Range)range.Cells[row, 9]).Text;
                        P.MonthlyInstallment = ((Excel.Range)range.Cells[row, 10]).Value;
                        P.DownPayment = ((Excel.Range)range.Cells[row, 11]).Value;
                        P.NoOfInstallment = Convert.ToInt32(((Excel.Range)range.Cells[row, 12]).Value);
                        P.InstallmentPercentage = Convert.ToDecimal(((Excel.Range)range.Cells[row, 13]).Value);
                        P.StartEffectiveDate = ((Excel.Range)range.Cells[row, 14]).Value;
                        P.EndEffectiveDate = ((Excel.Range)range.Cells[row, 15]).Value;
                        P.Active = Convert.ToInt32(((Excel.Range)range.Cells[row, 16]).Value);
                        P.TransferStatus = ((Excel.Range)range.Cells[row, 17]).Text;
                        P.Remarks = ((Excel.Range)range.Cells[row, 18]).Text;

                        listProduct.Add(P);
                    }
                    ViewBag.InstallmentPlanData = listProduct;

                    data = Serializer.Serialize(listProduct);

                    workbook.Close(true);
                    application.Quit();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(application);
                    result = true;
                    return View("Installmentplan");
                    //return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    ViewBag.Error = "Installmentplan";
                    return View("Installmentplan");
                    //return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
                }
            }

        }

        [HttpPost]
        public JsonResult Insert_IPData(InstallmentPlanVM[] modeldetail)
        {

            bool result = false;
            string msg = "Failed to save record..";
            string leadId = "";
            string[] data;
            leadId = InstallmentPlanMethods.Insert_InstallmentPlan(modeldetail);
            if (!string.IsNullOrEmpty(leadId))
            {
                data = leadId.Split(',');
                leadId = data[0].ToString();

                result = true;
                msg = "Successfully Added";
            }

            return Json(new { Success = result, Message = msg, LeadId = leadId }, JsonRequestBehavior.AllowGet);
        }


    }
}