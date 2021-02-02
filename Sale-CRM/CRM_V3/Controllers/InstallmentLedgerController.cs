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
    public class InstallmentLedgerController : Controller
    {
        // GET: InstallmentLedger

        static string dealerCode = string.Empty;
        SysFunction sysfun = new SysFunction();
        DataTable dt = new DataTable();
        public ActionResult Index()
        {

            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("Login", "Home");
            }
            dealerCode = Session["DealerCode"].ToString();

            List<ISIInstallmentReceiptVM> ISIReceiptData = new List<ISIInstallmentReceiptVM>();

            dt = GeneralMethods.GetDataForModal("Sp_GetDataForInstallmentLedger", dealerCode);
            if (dt.Rows.Count > 0)
            {
                ISIReceiptData = EnumerableExtension.ToList<ISIInstallmentReceiptVM>(dt);
            }
            
            ViewBag.ISIIRDetail = ISIReceiptData;

            return View();
        }


        public ActionResult Export(string EnquiryId, string DealerCode)
        {
            DSReports data = new DSReports();
            ReportDocument RD = new ReportDocument();

            SqlParameter[] param =
        {
                new SqlParameter("@DealerCode",SqlDbType.Char),
                new SqlParameter("@CusCode",SqlDbType.VarChar)

            };

            param[0].Value = DealerCode;
            param[1].Value = EnquiryId;

            SqlDataReader rder = null;

            SysFunction sysFunc = new SysFunction();
            if (sysFunc.ExecuteSP("SP_InstallmentLedgerReport", param, ref rder))
            {
                data.SP_InstallmentLedgerReport.Load(rder);

            }


            RD.Load(Server.MapPath("~/Reports/InstallmentLedgerReport.rpt"));

            RD.DataDefinition.FormulaFields["DealerDesc"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";
            RD.DataDefinition.FormulaFields["ReportTitle"].Text = "'CUSTOMER INSTALLMENT LEDGER'";
            RD.DataDefinition.FormulaFields["Terminal"].Text = "'" + Request.ServerVariables["REMOTE_ADDR"].ToString() + "'";
            RD.DataDefinition.FormulaFields["UserId"].Text = "'" + Session["UserName"].ToString() + "'";
            RD.DataDefinition.FormulaFields["CompanyName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
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