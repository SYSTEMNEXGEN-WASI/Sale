using Core.CRM.ADO;
using Core.CRM.ADO.ViewModel;
using CRM_V3.assets;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace CRM_V3.Controllers
{
    public class EventController : Controller
    {
        static string dealerCode = string.Empty;
        // GET: Event
        public ActionResult Index()
        {
            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("Login", "Home");
            }
            dealerCode = Session["DealerCode"].ToString();
            List<SelectListItem> ddlAssignTo = new List<SelectListItem>();
            ddlAssignTo = GeneralMethods.GetDataFromSPWithDealerCode("Select_DealerEmplpoyee",dealerCode);
            ViewBag.AssignTo = ddlAssignTo;

            List<SelectListItem> ddlBrandCode = new List<SelectListItem>();
            ddlBrandCode = GeneralMethods.GetDataFromSPWithDealerCode("Select_Brand",dealerCode,"Y");
            ViewBag.BrandCode = ddlBrandCode;

            List<SelectListItem> ddlProdCode = new List<SelectListItem>();
            ddlProdCode = GeneralMethods.GetProduct();
            ViewBag.ProdCode = ddlProdCode;

            List<SelectListItem> ddlColor = new List<SelectListItem>();
            ddlColor = GeneralMethods.GetColor(Session["VehicleCategory"].ToString());
            ViewBag.Color = ddlColor;

            List<SelectListItem> ddlEvent = new List<SelectListItem>();
            ddlEvent = GeneralMethods.GetDataFromSPWithDealerCode("SP_SelectEvent",dealerCode,"Y");
            ViewBag.Event = ddlEvent;

            

            return View();
        }

        [HttpPost]
        public void InvoicePrint(string EnquiryId, string DealerCode)
        {
            DSReports data = new DSReports();
            ReportDocument RD = new ReportDocument();

            SqlParameter[] param =
        {
                new SqlParameter("@DealerCode",SqlDbType.Char),
                new SqlParameter("@EnquiryId",SqlDbType.VarChar)

            };

            param[0].Value = DealerCode;
            param[1].Value = EnquiryId;

            SqlDataReader rder = null;

            SysFunction sysFunc = new SysFunction();
            if (sysFunc.ExecuteSP("SP_EventDetail_Select", param, ref rder))
            {
                data.SP_EventDetail_Select.Load(rder);

            }
            this.HttpContext.Session["rptSource"] = data;

        }

        
        public ActionResult Export(string EnquiryId, string DealerCode)
        {
            DSReports data = new DSReports();
            ReportDocument RD = new ReportDocument();

            SqlParameter[] param =
        {
                new SqlParameter("@DealerCode",SqlDbType.Char),
                new SqlParameter("@EnquiryId",SqlDbType.VarChar)

            };

            param[0].Value = DealerCode;
            param[1].Value = EnquiryId;

            SqlDataReader rder = null;

            SysFunction sysFunc = new SysFunction();
            if (sysFunc.ExecuteSP("SP_EventDetail_Select_Print", param, ref rder))
            {
                data.SP_EventDetail_Select.Load(rder);

            }
            RD.Load(Server.MapPath("~/Reports/EventDetail.rpt"));

            RD.DataDefinition.FormulaFields["DealerDesc"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";
            RD.DataDefinition.FormulaFields["ReportTitle"].Text = "'Event Report'";
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
            try
            {
                Stream stream = RD.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "EventDetail.pdf");
            }
            catch
            {
                throw;
            }


        }
        
        public JsonResult Insert_EventDetail(EventVM objects)
        {
            bool result = false;
            
            string msg = "Failed to save record..";
            
            result = EventMethods.Insert_Event(objects);

            if (result)
            {
                msg = "Successfully Added";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult Select_EventDetail(string EnquiryId, string DealerCode)
        {
            string data = "";
            bool result = false;
            data = EventMethods.Get_EventDetailData(EnquiryId, DealerCode);

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }
        


        [HttpGet]
        public JsonResult Select_BrandProduct(string EnquiryId, string DealerCode)
        {
            List<SelectListItem> data;
            bool result = false;
            data = EventMethods.Get_BrandProductData(EnquiryId, DealerCode,Session["VehicleCategory"].ToString());

            if (data.Count > 0)
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
            
        }
    }
}