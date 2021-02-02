using Core.CRM.ADO;
using Core.CRM.ADO.ViewModel;
using Core.CRM.Helper;
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
    public class GeneralReportController : Controller
    {
        static string dealerCode = string.Empty;
        SysFunction sysfunc = new SysFunction();
        // GET: GeneralReport
        public ActionResult GRMain(string id)
        {
            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("Login", "Home");
            }
            dealerCode = Session["DealerCode"].ToString();

            if (id == "BO")
            {
                ViewBag.Heading = "Booking Order";
            }
            else if (id == "DO")
            {
                ViewBag.Heading = "Delivery Order";
            }
            else if (id == "PBN")
            {
                ViewBag.Heading = "Pending Booking Number";
            }
            else if (id == "PIN")
            {
                ViewBag.Heading = "Pending Invoice Number";
            }
            else if (id == "PP")
            {
                ViewBag.Heading = "Pending Payments";
            }
            else if (id == "ST")
            {
                ViewBag.Heading = "Sale Target";
            }
            else
            {
                ViewBag.Heading = "Vehicle Receipt";                
            }

            return View();
        }

        public ActionResult EnquiryReport(string id)
        {
            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("Login", "Home");
            }
            dealerCode = Session["DealerCode"].ToString();

            if (id == "ER")
            {
                ViewBag.Heading = "Enquiry Detail";
            }
            else
            {
                ViewBag.Heading = "FollowUp Detail";
            }

            List<SelectListItem> ddlAssignTo = new List<SelectListItem>();
            ddlAssignTo = GeneralMethods.GetDataFromSPWithDealerCode("Select_DealerEmplpoyee", dealerCode,"Y");
            ViewBag.AssignTo = ddlAssignTo;

            return View();
        }

        public ActionResult DateWiseReport(string id)
        {
            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("Login", "Home");
            }
            dealerCode = Session["DealerCode"].ToString();

            if (id == "DE")
            {
                ViewBag.Heading = "Detail Expenditure";
            }
            else if(id == "VR")
            {
                ViewBag.Heading = "Vehicle Return";
            }
            else if (id == "PO")
            {
                ViewBag.Heading = "Price Offer Negotiation";
            }
            else if (id == "ST")
            {
                ViewBag.Heading = "Sales Target";
            }
            else if (id == "MS")
            {
                ViewBag.Heading = "Monthly Sale w.e.f Booking Order";
            }
            else
            {
                ViewBag.Heading = "Conversion Ratio";
            }
            

            return View();
        }


        public ActionResult Export(string EnquiryId, string Type , string FromDate , string ToDate)
        {
            DSReports data = new DSReports();
            ReportDocument RD = new ReportDocument();

            SqlParameter[] param =
            {
            new SqlParameter("@DealerCode",SqlDbType.Char),//
            new SqlParameter("@FromDate",SqlDbType.VarChar), //1
            new SqlParameter("@ToDate",SqlDbType.VarChar), //2
            new SqlParameter("@Type",SqlDbType.VarChar)

            };

            param[0].Value = dealerCode;
            param[1].Value = sysfunc.SaveDate(FromDate);
            param[2].Value = sysfunc.SaveDate(ToDate);
            param[3].Value = Type;

            try
            {
                SqlDataReader rder = null;
            if(EnquiryId == "Vehicle Receipt") {

                    sysfunc.ExecuteSP("SP_ProdRecMaster_Report", param, ref rder);                
                    data.EnforceConstraints = false;
                    data.SP_ProdRecMaster_Report.Load(rder);
                    RD.Load(Server.MapPath("~/Reports/crptProdRecMasterReport.rpt"));
                    RD.DataDefinition.FormulaFields["ReportTitle"].Text = "'Vehicle Receipt Detail'";
                
            }else if (EnquiryId == "Delivery Order")
            {
                    sysfunc.ExecuteSP("SP_VehicleDeliveryMaster_Report", param, ref rder);
                
                    data.EnforceConstraints = false;
                    data.SP_VehicleDeliveryMaster_Report.Load(rder);
                    RD.Load(Server.MapPath("~/Reports/crptVehicleDeliveryReport.rpt"));
                    RD.DataDefinition.FormulaFields["ReportTitle"].Text = "'Delivery Order Detail'";                

            }else if (EnquiryId == "Pending Booking Number")
                {
                    sysfunc.ExecuteSP("SP_Select_PendingBookingNumber", param, ref rder);
                    data.EnforceConstraints = false;
                    data.SP_Select_PendingBookingNumber.Load(rder);
                    RD.Load(Server.MapPath("~/Reports/PendingBookingNo.rpt"));
                    RD.DataDefinition.FormulaFields["ReportTitle"].Text = "'Pending Booking Number'";

                }
                else if (EnquiryId == "Pending Invoice Number")
                {
                    sysfunc.ExecuteSP("SP_Select_PendingVehicleReceipt", param, ref rder);
                    data.EnforceConstraints = false;
                    data.SP_Select_PendingVehicleReceipt.Load(rder);
                    RD.Load(Server.MapPath("~/Reports/PendingInvoice.rpt"));
                    RD.DataDefinition.FormulaFields["ReportTitle"].Text = "'Pending Vehicle Receipt'";

                }
                else if (EnquiryId == "Pending Payments")
                {
                    sysfunc.ExecuteSP("SP_PendingPaymentsReport", param, ref rder);
                    data.EnforceConstraints = false;
                    data.SP_PendingPaymentsReport.Load(rder);
                    RD.Load(Server.MapPath("~/Reports/PendingPaymentsReport.rpt"));
                    RD.DataDefinition.FormulaFields["ReportTitle"].Text = "'Pending Payments'";

                }
               
                else
            {
                    sysfunc.ExecuteSP("SP_BookingOrderDetailReport", param, ref rder);                
                    data.EnforceConstraints = false;
                    data.SP_BookingOrderDetailReport.Load(rder);
                    RD.Load(Server.MapPath("~/Reports/BookingOrderDetailReport.rpt"));
                    RD.DataDefinition.FormulaFields["ReportTitle"].Text = "'Booking Order Detail'";
                
            }


            RD.DataDefinition.FormulaFields["DealerDesc"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";
            RD.DataDefinition.FormulaFields["Terminal"].Text = "'" + Request.ServerVariables["REMOTE_ADDR"].ToString() + "'";
            RD.DataDefinition.FormulaFields["UserId"].Text = "'" + Session["UserName"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["NTN"].Text = "'N.T.N # " + Session["DealerNTN"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["SalesTaxNo"].Text = "'Sales Tax No.  " + Session["DealerSaleTaxNo"].ToString() + " '";
            //rpt.DataDefinition.FormulaFields["UserCell"].Text = "'" + GetStringValuesAgainstCodes("CusCode", , "CellNo", "Customer") + "'";
            RD.DataDefinition.FormulaFields["CompanyName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
                //RD.DataDefinition.FormulaFields["Pic"].Text = "'C:\\Users\\u_ahm\\OneDrive\\Documents\\Visual Studio 2010\\Projects\\WebApplication1\\WebApplication1\\" + Session["Logo"] + "'";
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

        public ActionResult ERExport(string EnquiryId, string EmpCode, string FromDate, string ToDate)
        {
            DSReports data = new DSReports();
            ReportDocument RD = new ReportDocument();

            SqlParameter[] param =
            {
            new SqlParameter("@DealerCode",SqlDbType.Char),//
            new SqlParameter("@FromDate",SqlDbType.VarChar), //1
            new SqlParameter("@ToDate",SqlDbType.VarChar), //2
            new SqlParameter("@Type",SqlDbType.VarChar)

            };

            param[0].Value = dealerCode;
            param[1].Value = sysfunc.SaveDate(FromDate);
            param[2].Value = sysfunc.SaveDate(ToDate);
            param[3].Value = EmpCode == "0" ? "" : EmpCode;

            try
            {
                SqlDataReader rder = null;

                if (EnquiryId == "Enquiry Detail")
                {
                    sysfunc.ExecuteSP("SP_EnquiryDetailReport", param, ref rder);
                    data.EnforceConstraints = false;
                    data.SP_EnquiryDetailReport.Load(rder);
                    RD.Load(Server.MapPath("~/Reports/EnquiryDetailReport.rpt"));
                    RD.DataDefinition.FormulaFields["ReportTitle"].Text = "'Enquiry Detail Report'";
                }
                else
                {
                    sysfunc.ExecuteSP("SP_FollowUpDetailReport", param, ref rder);
                    data.EnforceConstraints = false;
                    data.SP_FollowUpDetailReport.Load(rder);
                    RD.Load(Server.MapPath("~/Reports/FollowUpDetailReport.rpt"));
                    RD.DataDefinition.FormulaFields["ReportTitle"].Text = "'FollowUp Detail Report'";
                }


                
                RD.DataDefinition.FormulaFields["DealerDesc"].Text = "'" + Session["DealerDesc"].ToString() + "'";
                RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
                RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
                RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";
                RD.DataDefinition.FormulaFields["Terminal"].Text = "'" + Request.ServerVariables["REMOTE_ADDR"].ToString() + "'";
                RD.DataDefinition.FormulaFields["UserId"].Text = "'" + Session["UserName"].ToString() + "'";
                //RD.DataDefinition.FormulaFields["NTN"].Text = "'N.T.N # " + Session["DealerNTN"].ToString() + "'";
                //RD.DataDefinition.FormulaFields["SalesTaxNo"].Text = "'Sales Tax No.  " + Session["DealerSaleTaxNo"].ToString() + " '";
                
                RD.DataDefinition.FormulaFields["CompanyName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
                //RD.DataDefinition.FormulaFields["Pic"].Text = "'C:\\Users\\u_ahm\\OneDrive\\Documents\\Visual Studio 2010\\Projects\\WebApplication1\\WebApplication1\\" + Session["Logo"] + "'";
                RD.DataDefinition.FormulaFields["Pic"].Text = "'" + Server.MapPath("~") + Session["Logo"] + "'";

                RD.Database.Tables[0].SetDataSource(data);

                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();

                Stream stream = RD.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "DetailEnquiryReport.pdf");
            }
            catch
            {
                throw;
            }
        }

        public ActionResult DetailReport(string Enquiry,string FromDate, string ToDate)
        {
            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("Login", "Home");
            }

            DSReports data = new DSReports();
            ReportDocument RD = new ReportDocument();

            SqlParameter[] param =
            {
            new SqlParameter("@DealerCode",SqlDbType.Char),//
            new SqlParameter("@FromDate",SqlDbType.VarChar), //1
            new SqlParameter("@ToDate",SqlDbType.VarChar), //2

            };

            param[0].Value = Session["DealerCode"].ToString();
            param[1].Value = sysfunc.SaveDate(FromDate);
            param[2].Value = sysfunc.SaveDate(ToDate);

            try
            {
                SqlDataReader rder = null;

                if(Enquiry == "Detail Expenditure")
                {
                    sysfunc.ExecuteSP("Sp_ExpenditureDetail_DateWise", param, ref rder);
                    data.EnforceConstraints = false;
                    data.Sp_ExpenditureDetail_DateWise.Load(rder);
                    RD.Load(Server.MapPath("~/Reports/Sale/ExpenditureDetail_DateWise.rpt"));
                    RD.DataDefinition.FormulaFields["ReportTitle"].Text = "'Daily Expenditure Detail Report'";
                }
                else if(Enquiry == "Vehicle Return")
                {
                    sysfunc.ExecuteSP("SP_VehicleReturn_DetailReport", param, ref rder);
                    data.EnforceConstraints = false;
                    data.SP_VehicleReturn_DetailReport.Load(rder);
                    RD.Load(Server.MapPath("~/Reports/VehicleReturnDetail.rpt"));
                    RD.DataDefinition.FormulaFields["ReportTitle"].Text = "'Vehicle Return Detail Report'";
                }
                else if (Enquiry == "Price Offer Negotiation")
                {
                    sysfunc.ExecuteSP("SP_Report_DetailPriceOffer", param, ref rder);
                    data.EnforceConstraints = false;
                    data.SP_Report_DetailPriceOffer.Load(rder);
                    RD.Load(Server.MapPath("~/Reports/Sale/PriceOfferDetail.rpt.rpt"));
                    RD.DataDefinition.FormulaFields["ReportTitle"].Text = "'Price Offer/ Negociation Details '";
                }
                else if (Enquiry == "Sales Target")
                {
                    sysfunc.ExecuteSP("SP_SaleTargetReport", param, ref rder);
                    data.EnforceConstraints = false;
                    data.SP_SaleTargetReport.Load(rder);
                    RD.Load(Server.MapPath("~/Reports/SalesTargetReport.rpt"));
                    RD.DataDefinition.FormulaFields["ReportTitle"].Text = "'Sales Target'";

                }
                else if (Enquiry == "Monthly Sale w.e.f Booking Order")
                {
                    sysfunc.ExecuteSP("sp_Sales_CommisionReport", param, ref rder);
                    data.EnforceConstraints = false;
                    data.sp_Sales_CommisionReport.Load(rder);
                    RD.Load(Server.MapPath("~/Reports/MonthlySale.rpt"));
                    RD.DataDefinition.FormulaFields["ReportTitle"].Text = "'Monthly Sale From Booking Order'";

                }
                else
                {
                    sysfunc.ExecuteSP("SP_ConversionRatio_Report", param, ref rder);
                    data.EnforceConstraints = false;
                    data.SP_ConversionRatio_Report.Load(rder);
                    RD.Load(Server.MapPath("~/Reports/ConversionRatio.rpt"));
                }

                RD.DataDefinition.FormulaFields["DealerDesc"].Text = "'" + Session["DealerDesc"].ToString() + "'";
                RD.DataDefinition.FormulaFields["FromDate"].Text = "'" + FromDate + "'";
                RD.DataDefinition.FormulaFields["ToDate"].Text = "'" + ToDate + "'";
                RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
                RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";
                RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
                RD.DataDefinition.FormulaFields["Terminal"].Text = "'" + Request.ServerVariables["REMOTE_ADDR"].ToString() + "'";
                RD.DataDefinition.FormulaFields["UserId"].Text = "'" + Session["UserName"].ToString() + "'";
                //RD.DataDefinition.FormulaFields["NTN"].Text = "'N.T.N # " + Session["DealerNTN"].ToString() + "'";
                //RD.DataDefinition.FormulaFields["SalesTaxNo"].Text = "'Sales Tax No.  " + Session["DealerSaleTaxNo"].ToString() + " '";

                RD.DataDefinition.FormulaFields["CompanyName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
                ////RD.DataDefinition.FormulaFields["Pic"].Text = "'C:\\Users\\u_ahm\\OneDrive\\Documents\\Visual Studio 2010\\Projects\\WebApplication1\\WebApplication1\\" + Session["Logo"] + "'";
                RD.DataDefinition.FormulaFields["Pic"].Text = "'" + Server.MapPath("~") + Session["Logo"] + "'";

                RD.Database.Tables[0].SetDataSource(data);

                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();

                Stream stream = RD.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "DetailEnquiryReport.pdf");
            }
            catch
            {
                throw;
            }
        }

        public ActionResult MonthlyCommisionDateWise()
        {
            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("Login", "Home");
            }
            dealerCode = Session["DealerCode"].ToString();

            DataTable Empdt = new DataTable();
            List<EmployeeVM> item = new List<EmployeeVM>();
            Empdt = GeneralMethods.GetDataForModal("SP_Select_DealerEmpForCommision", dealerCode);
            if (Empdt.Rows.Count > 0)
            {
                item = EnumerableExtension.ToList<EmployeeVM>(Empdt);
            }

            ViewBag.AssignTo = item;

            return View();
        }
        public ActionResult CommisionDetailReport(string Enquiry, string FromDate, string ToDate, string Emp , string Service)
        {
            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("Login", "Home");
            }

            DSReports data = new DSReports();
            ReportDocument RD = new ReportDocument();

            SqlParameter[] param =
            {
            new SqlParameter("@DealerCode",SqlDbType.Char),//
            new SqlParameter("@FromDate",SqlDbType.VarChar), //1
            new SqlParameter("@ToDate",SqlDbType.VarChar), //2
            new SqlParameter("@Emp",SqlDbType.VarChar), //1
            new SqlParameter("@Service",SqlDbType.VarChar), //2
            };

            param[0].Value = Session["DealerCode"].ToString();
            param[1].Value = sysfunc.SaveDate(FromDate);
            param[2].Value = sysfunc.SaveDate(ToDate);
            param[3].Value = Emp;
            param[4].Value = Service == "0" ? "" : Service; 
            try
            {
                SqlDataReader rder = null;
                
                sysfunc.ExecuteSP("SP_MonthlyCommisionDetailReport", param, ref rder);
                data.EnforceConstraints = false;
                data.SP_MonthlyCommisionDetailReport.Load(rder);

                RD.Load(Server.MapPath("~/Reports/MonthyCommisionDetailReport.rpt"));
                RD.DataDefinition.FormulaFields["ReportTitle"].Text = "'Monthly Commission Detail Report'";
               
                RD.DataDefinition.FormulaFields["FromDate"].Text = "'" + FromDate + "'";
                RD.DataDefinition.FormulaFields["ToDate"].Text = "'" + ToDate + "'";
                RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
                RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";
                RD.DataDefinition.FormulaFields["Terminal"].Text = "'" + Request.ServerVariables["REMOTE_ADDR"].ToString() + "'";
                RD.DataDefinition.FormulaFields["UserId"].Text = "'" + Session["UserName"].ToString() + "'";
                RD.DataDefinition.FormulaFields["CompanyName"].Text = "'" + Session["DealerDesc"].ToString() + "'";                
                RD.DataDefinition.FormulaFields["Pic"].Text = "'" + Server.MapPath("~") + Session["Logo"] + "'";

                RD.Database.Tables[0].SetDataSource(data);

                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();

                Stream stream = RD.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "DetailEnquiryReport.pdf");
            }
            catch
            {
                throw;
            }
        }
    }
}