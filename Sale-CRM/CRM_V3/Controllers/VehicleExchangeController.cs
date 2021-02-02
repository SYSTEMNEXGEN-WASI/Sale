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
    public class VehicleExchangeController : Controller
    {
        SysFunction sysfunc = new SysFunction();
        // GET: VehicleExchange
        public ActionResult VEDetailReport() {


            return View();
        }
        public ActionResult VEMaster()
        {
            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("Login", "Home");
            }
            DataTable dt = new DataTable();
            string dealerCode = Session["DealerCode"].ToString();
            List<SelectListItem> ddlEmp = new List<SelectListItem>();
            ddlEmp = GeneralMethods.Get_DealerEmp(dealerCode);
            ViewBag.ddlDealerEmp = ddlEmp;
            List<VehicleExchangeVM> RegNoDetailData = new List<VehicleExchangeVM>();
            RegNoDetailData = VehicleExchangeMethods.Get_RegNoDetailData(dealerCode);
            ViewBag.Buying = RegNoDetailData;
            Session["Select_Exchange_Reg"] = RegNoDetailData;
            List<VehicleExchangeVM> VehicleDetailData = new List<VehicleExchangeVM>();
            VehicleDetailData = VehicleExchangeMethods.Get_VehicleDetailData(dealerCode);
            ViewBag.Vehicle = VehicleDetailData;
            Session["Select_Exchange_Vehicle"] = VehicleDetailData;
            List<SelectListItem> ddlLocation = new List<SelectListItem>();
            ddlLocation = GeneralMethods.GetDataFromSPWithDealerCode("Select_Buying_Location", dealerCode);
            ViewBag.ddlLocation = ddlLocation;

            List<VehicleExchangeVM> ExchangeCodelst = new List<VehicleExchangeVM>();
            ExchangeCodelst = VehicleExchangeMethods.Get_ExchangeVehicle(dealerCode);
            ViewBag.Exchange = ExchangeCodelst;
            Session["ExchangeCodeLst"] = ExchangeCodelst;
            return View();
        }
        public ActionResult Get_RegNo_Data(string EnquiryId)
        {
            //Session["IRTransCode"] = TransCode;

            List<VehicleExchangeVM> ISRegNoData = (List<VehicleExchangeVM>)Session["Select_Exchange_Reg"];


            var data = ISRegNoData.Where(a => a.BuyingCode.Trim() == EnquiryId.Trim());

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Get_VehicleDataView(string EnquiryId)
        {
            //Session["IRTransCode"] = TransCode;

            List<VehicleExchangeVM> VehicleData = (List<VehicleExchangeVM>)Session["Select_Exchange_Vehicle"];
            //ISIReceiptData = InstallmentReceiptMethods.Get_ISIInstallmentReceiptData(Session["DealerCode"].ToString());

            var data = VehicleData.Where(b => b.VehicleCode.Trim() == EnquiryId.Trim());

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Get_ExchangeCode(string EnquiryId)
        {
            //List<VehicleExchangeVM> ExchangeCodelst = new List<VehicleExchangeVM>();
            //ExchangeCodelst = VehicleExchangeMethods.Get_ExchangeVehicle(dealerCode);
            //ViewBag.Exchange = ExchangeCodelst;
            //Session["ExchangeCodeLst"] = ExchangeCodelst;

            List<VehicleExchangeVM> ExchangeCodelst = (List<VehicleExchangeVM>)Session["ExchangeCodeLst"];
            //ISIReceiptData = InstallmentReceiptMethods.Get_ISIInstallmentReceiptData(Session["DealerCode"].ToString());

            var data = ExchangeCodelst.Where(b => b.ExchangeCode.Trim() == EnquiryId.Trim());

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Insert_Exchange(VehicleExchangeVM Objects)
        {

            string dealerCode = Session["DealerCode"].ToString();
            bool result = false;

            string msg = "Failed to save record..";


            result = VehicleExchangeMethods.Insert_Exchange(Objects, dealerCode);

            if (result)
            {
                msg = "Successfully Added";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Delete_ExchangeData(string EnquiryId)
        {
            bool result = false;
            string dealerCode = Session["DealerCode"].ToString();
            string msg = " Data can't be deleted";

            result = VehicleExchangeMethods.Delete_ExchangeData(EnquiryId, dealerCode);

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
                new SqlParameter("@ExchangeCode",SqlDbType.Char)
            };

            param[0].Value = dealerCode;
            param[1].Value = EnquiryId;

            SqlDataReader rder = null;

            SysFunction sysFunc = new SysFunction();
            if (sysFunc.ExecuteSP("Select_ExchangeVehicle_ReportData", param, ref rder))
            {
                data.EnforceConstraints = false;
                data.Select_ExchangeVehicle_ReportData.Load(rder);
            }
            RD.Load(Server.MapPath("~/Reports/ExchangeVehicle.rpt"));

            //RD.DataDefinition.FormulaFields["DealerDesc"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";
            RD.DataDefinition.FormulaFields["ReportTitle"].Text = "'Exchange Vehicle'";
            //RD.DataDefinition.FormulaFields["Terminal"].Text = "'" + Request.ServerVariables["REMOTE_ADDR"].ToString() + "'";
            RD.DataDefinition.FormulaFields["UserId"].Text = "'" + Session["UserName"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["NTN"].Text = "'N.T.N # " + Session["DealerNTN"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["SalesTaxNo"].Text = "'Sales Tax No.  " + Session["DealerSaleTaxNo"].ToString() + " '";
            //rpt.DataDefinition.FormulaFields["UserCell"].Text = "'" + GetStringValuesAgainstCodes("CusCode", , "CellNo", "Customer") + "'";
            RD.DataDefinition.FormulaFields["CompanyName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            // RD.DataDefinition.FormulaFields["Pic"].Text = "'C:\\Users\\u_ahm\\OneDrive\\Documents\\Visual Studio 2010\\Projects\\WebApplication1\\WebApplication1\\" + Session["Logo"] + "'";
            RD.DataDefinition.FormulaFields["Pic"].Text = "'" + Server.MapPath("~") + Session["Logo"] + "'";
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
        public ActionResult ExportBuyingDetail(string todate, string fromdate)
        {
            string dealerCode = Session["DealerCode"].ToString();
            DSReports data = new DSReports();
            ReportDocument RD = new ReportDocument();

            SqlParameter[] param =
        {
                 new SqlParameter("@DealerCode",SqlDbType.Char),
                new SqlParameter("@fromDate",SqlDbType.DateTime),
                new SqlParameter("@ToDate",SqlDbType.DateTime),

            };
            param[0].Value = dealerCode;
            param[1].Value = sysfunc.SaveDate(todate);
            param[2].Value = sysfunc.SaveDate(fromdate);


            SqlDataReader rder = null;

            SysFunction sysFunc = new SysFunction();
            if (sysFunc.ExecuteSP("Select_ExchangeVehicle_DetailReportData", param, ref rder))
            {
                data.EnforceConstraints = false;
                data.Select_ExchangeVehicle_DetailReportData.Load(rder);

            }
            RD.Load(Server.MapPath("~/Reports/VehicleExchangeDetail.rpt"));

            RD.DataDefinition.FormulaFields["DealerDesc"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";
            RD.DataDefinition.FormulaFields["ReportTitle"].Text = "'Exchange  Details '";
            //RD.DataDefinition.FormulaFields["Terminal"].Text = "'" + Request.ServerVariables["REMOTE_ADDR"].ToString() + "'";
            RD.DataDefinition.FormulaFields["UserId"].Text = "'" + Session["UserName"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["NTN"].Text = "'N.T.N # " + Session["DealerNTN"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["SalesTaxNo"].Text = "'Sales Tax No.  " + Session["DealerSaleTaxNo"].ToString() + " '";
            //rpt.DataDefinition.FormulaFields["UserCell"].Text = "'" + GetStringValuesAgainstCodes("CusCode", , "CellNo", "Customer") + "'";
            RD.DataDefinition.FormulaFields["CompanyName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            // RD.DataDefinition.FormulaFields["Pic"].Text = "'C:\\Users\\u_ahm\\OneDrive\\Documents\\Visual Studio 2010\\Projects\\WebApplication1\\WebApplication1\\" + Session["Logo"] + "'";
            RD.DataDefinition.FormulaFields["Pic"].Text = "'" + Server.MapPath("~") + Session["Logo"] + "'";
            RD.DataDefinition.FormulaFields["FromDate"].Text = "'" + fromdate + "'";
            RD.DataDefinition.FormulaFields["ToDate"].Text = "'" + todate + "'";


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