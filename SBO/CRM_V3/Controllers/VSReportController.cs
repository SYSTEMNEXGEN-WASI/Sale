﻿using Core.CRM.ADO;
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
using DXBMS;
using CRM.Models.Classes;

namespace CRM_V3.Controllers
{
    public class VSReportController : Controller
    {

        static string dealerCode = string.Empty;
        SysFunction sysfunc = new SysFunction();
        SecurityBll common = new SecurityBll();
        // GET: VSReport
        public ActionResult VSRMain()
        {

            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("NewLogin", "Home");
            }
            if (common.UserRight("2501", "001"))
            {

                dealerCode = Session["DealerCode"].ToString();

            List<SelectListItem> ddlVehLoc = new List<SelectListItem>();
            ddlVehLoc = GeneralMethods.GetDataFromSPWithDealerCode("SP_Select_VehLocation", dealerCode);
            ViewBag.ddlVehLoc = ddlVehLoc;
            }
            else
            {
                TempData["TestAccessError"] = MessageAlert.MsgAuthorized();
                return RedirectToAction("Error", "Definition");
            }


            return View();
        }

        public ActionResult Export(string Type,string rptType, string FromDate, string ToDate,string Segment,string Location)
        {
            DSReports data = new DSReports();
            ReportDocument RD = new ReportDocument();
            SqlDataReader rder = null;

            SqlParameter[] param =
            {
            new SqlParameter("@DealerCode",SqlDbType.Char),//
            new SqlParameter("@FromDate",SqlDbType.VarChar), //1
            new SqlParameter("@ToDate",SqlDbType.VarChar), //2
            new SqlParameter("@Type",SqlDbType.VarChar),//3
            new SqlParameter("@RptType",SqlDbType.VarChar),//4
            new SqlParameter("@Segment",SqlDbType.VarChar),//5
            new SqlParameter("@LocCode",SqlDbType.VarChar)//6
            };

            if(rptType == "VS") {
                if (Type == "All" && Segment == "All" && Location=="0")
                {

                    param[0].Value = dealerCode;
                    param[1].Value = sysfunc.SaveDate(FromDate);
                    param[2].Value = sysfunc.SaveDate(ToDate);
                    param[3].Value = Type;
                    param[4].Value = "VS";
                    param[5].Value = "";
                    param[6].Value = "";

                    sysfunc.ExecuteSP("SP_VehicleStock_Report_All", param, ref rder);
                    data.EnforceConstraints = false;
                    data.SP_VehicleStock_Report.Load(rder);
                    RD.Load(Server.MapPath("~/Reports/VehicleStockReport.rpt"));
                    RD.DataDefinition.FormulaFields["ReportTitle"].Text = "'Vehicle Stock'";

                }
                else {

                    param[0].Value = dealerCode;
                    param[1].Value = sysfunc.SaveDate(FromDate);
                    param[2].Value = sysfunc.SaveDate(ToDate);
                    param[3].Value = Type;
                    param[4].Value = "VS";
                    param[5].Value = Segment;
                    param[6].Value = Location;

                    sysfunc.ExecuteSP("SP_VehicleStock_Report_New", param, ref rder);
                    data.EnforceConstraints = false;
                    data.SP_VehicleStock_Report.Load(rder);
                    RD.Load(Server.MapPath("~/Reports/VehicleStockReport.rpt"));
                    RD.DataDefinition.FormulaFields["ReportTitle"].Text = "'Vehicle Stock'";


                }
               

            }
            else if(rptType == "ID")
            {
                param[0].Value = dealerCode;
                param[1].Value = sysfunc.SaveDate(FromDate);
                param[2].Value = sysfunc.SaveDate(ToDate);
                param[3].Value = Type;
                param[4].Value = "ID";
                sysfunc.ExecuteSP("SP_VehicleStock_Report_New", param, ref rder);
                data.EnforceConstraints = false;
                data.SP_VehicleStock_Report.Load(rder);
                RD.Load(Server.MapPath("~/Reports/VehicleStockReport.rpt"));
                RD.DataDefinition.FormulaFields["ReportTitle"].Text = "'Invoice Detail'";
            }
            else
            {
                param[0].Value = dealerCode;
                param[1].Value = sysfunc.SaveDate(FromDate);
                param[2].Value = sysfunc.SaveDate(ToDate);
                param[3].Value = Type;
                param[4].Value = "PI";
                sysfunc.ExecuteSP("SP_VehicleStock_Report_New", param, ref rder);
                data.EnforceConstraints = false;
                data.SP_VehicleStock_Report.Load(rder);
                RD.Load(Server.MapPath("~/Reports/VehicleStockReport.rpt"));
                RD.DataDefinition.FormulaFields["ReportTitle"].Text = "'Pending Invoice Detail'";
            }

            try
            {
                                
                RD.DataDefinition.FormulaFields["DealerDesc"].Text = "'" + Session["DealerDesc"].ToString() + "'";
                RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
                RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
                RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";
                RD.DataDefinition.FormulaFields["Terminal"].Text = "'" + Request.ServerVariables["REMOTE_ADDR"].ToString() + "'";
                RD.DataDefinition.FormulaFields["UserId"].Text = "'" + Session["UserName"].ToString() + "'";
                //RD.DataDefinition.FormulaFields["NTN"].Text = "'N.T.N # " + Session["DealerNTN"].ToString() + "'";
                //RD.DataDefinition.FormulaFields["SalesTaxNo"].Text = "'Sales Tax No.  " + Session["DealerSaleTaxNo"].ToString() + " '";
                //rpt.DataDefinition.FormulaFields["UserCell"].Text = "'" + sysfunc.GetStringValuesAgainstCodes("CusCode", , "CellNo", "Customer") + "'";
                RD.DataDefinition.FormulaFields["CompanyName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
                //RD.DataDefinition.FormulaFields["Pic"].Text = "'F:\CRM\CRM_V3\CRM_V3\assets\images" + Session["Logo"] + "'";
                RD.DataDefinition.FormulaFields["Pic"].Text = "'" + Server.MapPath("~") + Session["Logo"] + "'";

                RD.Database.Tables[0].SetDataSource(data);

                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();

                Stream stream = RD.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "ProductReceiptReport.pdf");
            }
            catch
            {
                throw;
            }


        }
    }
}