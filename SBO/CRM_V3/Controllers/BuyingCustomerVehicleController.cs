using Core.CRM.ADO;
using Core.CRM.ADO.ViewModel;
using CRM_V3.assets;
using CRM_V3.assets;
using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRM_V3.Controllers
{
    public class BuyingCustomerVehicleController : Controller
    {
        SysFunction sysfunc = new SysFunction();

        // GET: BuyingCustomerVehicle
        public ActionResult BCVDetailReport()
        {
            return View();
        }
        public ActionResult BCV()
        {
            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("NewLogin", "Home");
            }
            Session["FrontImg"]= "";
            Session["BackImg"] = "";
            Session["LeftImg"] = "";
            Session["RightImg"] = "";
            Session["TopImg"]= "";
            
            DataTable dt = new DataTable();
            string dealerCode = Session["DealerCode"].ToString();
            List<BuyingCusVehicleVM> RegNoDetailData = new List<BuyingCusVehicleVM>();
            RegNoDetailData = BuyingCusVehicleMethods.Get_RegNoDetailData(dealerCode);
            ViewBag.RegNo = RegNoDetailData;
            Session["Select_Buying_RegNo"] = RegNoDetailData;
            List<SelectListItem> ddlInsCOde = new List<SelectListItem>();
            ddlInsCOde = GeneralMethods.Get_PaymentMode(dealerCode);
            ViewBag.ddlInsCOde = ddlInsCOde;
            List<BuyingPaymentVM> Get_BankDesc = new List<BuyingPaymentVM>();
            Get_BankDesc = BuyingCusVehicleMethods.Get_BankDesc(dealerCode);
            ViewBag.Get_BankDesc = Get_BankDesc;
            List<BuyingPaymentVM> Get_CityDesc = new List<BuyingPaymentVM>();
            Get_CityDesc = BuyingCusVehicleMethods.Get_CityDesc(dealerCode);
            ViewBag.Get_CityDesc = Get_CityDesc;
            List<BuyingCusVehicleVM> Select_Buying = new List<BuyingCusVehicleVM>();
            Select_Buying = BuyingCusVehicleMethods.Get_BuyingData(dealerCode);
            ViewBag.Buying = Select_Buying;
            Session["Select_Buying"] = Select_Buying;
            List<SelectListItem> ddlLocation = new List<SelectListItem>();
            ddlLocation = GeneralMethods.GetDataFromSPWithDealerCode("Select_Buying_Location",dealerCode);
            ViewBag.ddlLocation = ddlLocation;





            return View();
            }
        

        public ActionResult Get_BuyingDataView(string EnquiryId)
        {
            //Session["IRTransCode"] = TransCode;

            List<BuyingCusVehicleVM> ISIReceiptData = (List<BuyingCusVehicleVM>)Session["Select_Buying"];
            //ISIReceiptData = InstallmentReceiptMethods.Get_ISIInstallmentReceiptData(Session["DealerCode"].ToString());

            var data = ISIReceiptData.Where(a => a.BuyingCode.Trim() == EnquiryId.Trim());

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Select_BuyingPayData(string EnquiryId)
        {
            string dealerCode = Session["DealerCode"].ToString();
            string data = "";
            bool result = false;
            data = BuyingCusVehicleMethods.Get_ByingPayData(EnquiryId,dealerCode);

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Get_RegNo_Data(string EnquiryId)
        {
            //Session["IRTransCode"] = TransCode;

            List<BuyingCusVehicleVM> ISRegNoData = (List<BuyingCusVehicleVM>)Session["Select_Buying_RegNo"];
           

            var data = ISRegNoData.Where(a => a.RegNo.Trim() == EnquiryId.Trim());

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Select_DelChkList()
        {
            string data = "";
            bool result = false;
            data = BuyingCusVehicleMethods.Get_DelChkList();

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UploadFile(string data,string EnquiryId)
        {
            var path = "";
            var fileExtension = "";
            var fileName = "";
            var j = "" ;


            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase file = Request.Files[0];

                if (file != null && file.ContentLength > 0)
                {
                    fileName = Path.GetFileName(file.FileName);
                    fileExtension = Path.GetExtension(file.FileName);
                    path = Server.MapPath("~/Images/") + fileName;
                    if (EnquiryId=="Pic1")
                    {
                        //do bits, save to DB etc./..

                          file.SaveAs(path);
                        Session["FrontImg"] = path;
                       ViewBag.path1=  path;
                    }
                   else if (EnquiryId == "Pic2")
                    {
                        //do bits, save to DB etc./..

                          file.SaveAs(path);
                        Session["BackImg"] = path;
                        ViewBag.path2 = path;
                    }
                    else if (EnquiryId == "Pic3")
                    {
                        //do bits, save to DB etc./..

                         file.SaveAs(path);
                        Session["LeftImg"] = path;
                        ViewBag.path3 = path;
                    }
                    else if (EnquiryId == "Pic4")
                    {
                        //do bits, save to DB etc./..

                         file.SaveAs(path);
                        Session["RightImg"] = path;
                        ViewBag.path4 = path;
                    }
                    else if (EnquiryId == "Pic5")
                    {
                        //do bits, save to DB etc./..

                         file.SaveAs(path);
                        Session["TopImg"] = path;
                        ViewBag.path5 = path;
                    }
                }
            }
            return Json(new { fileName = fileName });
        }
        public JsonResult Insert_Buying(BuyingCusVehicleVM Objects)
        {
            Objects.Pic01 = Session["FrontImg"].ToString();
            Objects.Pic02 = Session["BackImg"].ToString();
            Objects.Pic03 = Session["LeftImg"].ToString();
            Objects.Pic04 = Session["RightImg"].ToString();
            Objects.Pic05 = Session["TopImg"].ToString();
            string dealerCode = Session["DealerCode"].ToString();
            bool result = false;

            string msg = "Failed to save record..";


            result = BuyingCusVehicleMethods.Insert_Buying(Objects, dealerCode);

            if (result)
            {
                msg = "Successfully Added";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Insert_BCVPay_Detail(List<BuyingCusVehicleVM> objects)
        {
            string dealerCode = Session["DealerCode"].ToString();
            bool result = false;
            int count = 0;
            string msg = "Failed to save record..";


            //foreach (var item in objects)
            //{
            //    if (count >= 1 || item.InstrumentNo != null)
            //    {
                    result = BuyingCusVehicleMethods.Insert_BCVPay_Detail(objects, ref msg);
            //    }
            //    count++;
            //}

            if (result)
            {
                msg = "Successfully Added";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Insert_BuyingChkList(string strCheckedValues)
        {
            string dealerCode = Session["DealerCode"].ToString();
            bool result = false;
            int count = 0;
            string msg = "Failed to save record..";

            result = BuyingCusVehicleMethods.Insert_VehChkList(strCheckedValues, dealerCode);

            if (result)
            {
                msg = "Successfully Added";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete_BuyingData(string EnquiryId)
        {
            bool result = false;
            string dealerCode = Session["DealerCode"].ToString();
            string msg = " Data can't be deleted";

            result = BuyingCusVehicleMethods.Delete_BuyingData(EnquiryId, dealerCode);

            if (result)
            {
                msg = "Successfully Deleted";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Exports(string EnquiryId)
        {

            DSReports data = new DSReports();
            ReportDocument RD = new ReportDocument();
            string dealerCode = Session["DealerCode"].ToString();
            SqlParameter[] param =
            {
                new SqlParameter("@DealerCode",SqlDbType.Char),
                new SqlParameter("@BuyingCode",SqlDbType.Char)
            };

            param[0].Value = dealerCode;
            param[1].Value = EnquiryId;

            SqlDataReader rder = null;

            SysFunction sysFunc = new SysFunction();
            if (sysFunc.ExecuteSP("Select_BuyingCodeReport", param, ref rder))
            {
                data.EnforceConstraints = false;
                data.Select_BuyingCodeReport.Load(rder);
            }
            RD.Load(Server.MapPath("~/Reports/Buying.rpt"));

            RD.DataDefinition.FormulaFields["DealerDesc"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";
            RD.DataDefinition.FormulaFields["ReportTitle"].Text = "'Buying Customer Vehicle'";
            //RD.DataDefinition.FormulaFields["Terminal"].Text = "'" + Request.ServerVariables["REMOTE_ADDR"].ToString() + "'";
            RD.DataDefinition.FormulaFields["UserId"].Text = "'" + Session["UserName"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["NTN"].Text = "'N.T.N # " + Session["DealerNTN"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["SalesTaxNo"].Text = "'Sales Tax No.  " + Session["DealerSaleTaxNo"].ToString() + " '";
            //rpt.DataDefinition.FormulaFields["UserCell"].Text = "'" + GetStringValuesAgainstCodes("CusCode", , "CellNo", "Customer") + "'";
            RD.DataDefinition.FormulaFields["CompanyName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            // RD.DataDefinition.FormulaFields["Pic"].Text = "'C:\\Users\\u_ahm\\OneDrive\\Documents\\Visual Studio 2010\\Projects\\WebApplication1\\WebApplication1\\" + Session["Logo"] + "'";
            RD.DataDefinition.FormulaFields["Pic"].Text = "'" + Server.MapPath("~") + Session["Logo"] + "'";
            RD.DataDefinition.FormulaFields["FrontImg"].Text ="'"+data.Select_BuyingCodeReport.Rows[0]["Pic01"].ToString()+"'";
            RD.DataDefinition.FormulaFields["BackImg"].Text = "'" + data.Select_BuyingCodeReport.Rows[0]["Pic02"].ToString() + "'";
            RD.DataDefinition.FormulaFields["LeftImg"].Text = "'" + data.Select_BuyingCodeReport.Rows[0]["Pic03"].ToString() + "'";
            RD.DataDefinition.FormulaFields["RightImg"].Text = "'" + data.Select_BuyingCodeReport.Rows[0]["Pic04"].ToString() + "'";
            RD.DataDefinition.FormulaFields["TopImg"].Text = "'" + data.Select_BuyingCodeReport.Rows[0]["Pic05"].ToString() + "'";
            RD.Database.Tables[0].SetDataSource(data);

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();


            try
            {
                Stream stream = RD.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "InstallmentReceiptReport.pdf");
            }
            catch
            {
                throw;
            }


        }
        public ActionResult ExportBuyingDetail(string todate, string fromdate, string Buyingmode)
        {
            string dealerCode = Session["DealerCode"].ToString();
            DSReports data = new DSReports();
            ReportDocument RD = new ReportDocument();

            SqlParameter[] param =
        {
                new SqlParameter("@DealerCode",SqlDbType.Char),
                new SqlParameter("@fromDate",SqlDbType.DateTime),
                new SqlParameter("@ToDate",SqlDbType.DateTime),
                new SqlParameter("@BuyingMode",SqlDbType.VarChar),

            };
            param[0].Value = dealerCode;
            param[1].Value = sysfunc.SaveDate(todate);
            param[2].Value = sysfunc.SaveDate(fromdate);
            param[3].Value = Buyingmode;

            SqlDataReader rder = null;

            SysFunction sysFunc = new SysFunction();
            if (sysFunc.ExecuteSP("Select_BuyingCodeDetailReport", param, ref rder))
            {
                data.EnforceConstraints = false;
                data.Select_BuyingCodeDetailReport.Load(rder);

            }
            RD.Load(Server.MapPath("~/Reports/BuyingDetail.rpt"));

            RD.DataDefinition.FormulaFields["DealerDesc"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";
            RD.DataDefinition.FormulaFields["ReportTitle"].Text = "'Buying  Details '";
            //RD.DataDefinition.FormulaFields["Terminal"].Text = "'" + Request.ServerVariables["REMOTE_ADDR"].ToString() + "'";
            RD.DataDefinition.FormulaFields["UserId"].Text = "'" + Session["UserName"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["NTN"].Text = "'N.T.N # " + Session["DealerNTN"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["SalesTaxNo"].Text = "'Sales Tax No.  " + Session["DealerSaleTaxNo"].ToString() + " '";
            //rpt.DataDefinition.FormulaFields["UserCell"].Text = "'" + GetStringValuesAgainstCodes("CusCode", , "CellNo", "Customer") + "'";
            RD.DataDefinition.FormulaFields["CompanyName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            // RD.DataDefinition.FormulaFields["Pic"].Text = "'C:\\Users\\u_ahm\\OneDrive\\Documents\\Visual Studio 2010\\Projects\\WebApplication1\\WebApplication1\\" + Session["Logo"] + "'";
            RD.DataDefinition.FormulaFields["Pic"].Text = "'" + Server.MapPath("~") + Session["Logo"] + "'";
            RD.DataDefinition.FormulaFields["FromDate"].Text = "'"+fromdate+"'";
            RD.DataDefinition.FormulaFields["ToDate"].Text = "'"+todate+"'";
            RD.DataDefinition.FormulaFields["BuyingMode"].Text = "'" + Buyingmode + "'";


            RD.Database.Tables[0].SetDataSource(data);

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();


            try
            {
                Stream stream = RD.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "InstallmentReceiptReport.pdf");
            }
            catch
            {
                throw;
            }


        }

    }
}