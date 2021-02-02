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
    public class MonthlyCommisionController : Controller
    {
        static string dealerCode = string.Empty;

        // GET: MonthlyCommision
        public ActionResult Main()
        {
            DataTable dt = new DataTable();
            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("Login", "Home");
            }
            dealerCode = Session["DealerCode"].ToString();

            List<MonthlyCommisionVM> CommisionList = new List<MonthlyCommisionVM>();
            dt = GeneralMethods.GetDataForModal("SP_Select_MonthlyCommisionMaster", dealerCode);
            if (dt.Rows.Count > 0)
            {
                CommisionList = EnumerableExtension.ToList<MonthlyCommisionVM>(dt);
            }
            ViewBag.MonthlyComm = CommisionList;

            return View();
        }

        [HttpGet]
        public JsonResult Get_MonthlyCommision(string Service,string Month, string DealerCode)
        {
            string data = "";
            bool result = false;

            if(Service == "BO")
            {
                data = MonthlyCommisionMethods.Get_MonthlyCommisionForEmp("SP_Select_MonthlyCommisionForEmp", Month, DealerCode);
            }
            else if(Service == "JC")
            {
                data = MonthlyCommisionMethods.Get_MonthlyCommisionForEmp("SP_MonthlyCommisionForJobCard", Month, DealerCode);
            }
            else
            {
                data = MonthlyCommisionMethods.Get_MonthlyCommisionForEmp("SP_MonthlyCommisionForTechnician", Month, DealerCode);
            }


            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Select_MCDetail(string EnquiryId)
        {
            string data = "";
            bool result = false;
            data = MonthlyCommisionMethods.Get_MCDetailData(EnquiryId, Session["DealerCode"].ToString());

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Insert_MonthlyCommisionMaster(MonthlyCommisionVM objects)
        {
            bool result = false;

            string msg = "Failed to save record..";

            result = MonthlyCommisionMethods.Insert_MonthlyCommisionMaster(objects, ref msg);

            if (result)
            {
                msg = "Successfully Added";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Insert_MonthlyCommisionDetail(List<MonthlyCommisionVM> objects)
        {
            bool result = false;
            string msg = "Failed to save record..";

            result = MonthlyCommisionMethods.Insert_MonthlyCommisionDetail(objects, ref msg);

            if (result)
            {
                msg = "Successfully Added";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Export(string EnquiryId, string DealerCode)
        {
            DSReports data = new DSReports();
            ReportDocument RD = new ReportDocument();

            SqlParameter[] param =
            {
                new SqlParameter("@DealerCode",SqlDbType.Char),
                new SqlParameter("@BONo",SqlDbType.VarChar)

            };

            param[0].Value = DealerCode;
            param[1].Value = EnquiryId;

            SqlDataReader rder = null;

            SysFunction sysFunc = new SysFunction();
            if (sysFunc.ExecuteSP("SP_MonthlyCommisionReport", param, ref rder))
            {
                data.EnforceConstraints = false;
                data.SP_MonthlyCommisionReport.Load(rder);
            }

            RD.Load(Server.MapPath("~/Reports/MonthlyCommisionReport.rpt"));

            RD.DataDefinition.FormulaFields["DealerDesc"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";
            RD.DataDefinition.FormulaFields["ReportTitle"].Text = "'Monthly Commission Report'";
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
                return File(stream, "ProductReceiptReport.pdf");
            }
            catch
            {
                throw;
            }


        }
    }
}