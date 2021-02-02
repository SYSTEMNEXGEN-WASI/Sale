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
    public class EvaluationDetailController : Controller
    {
        SysFunction sysfunc = new SysFunction();
        // GET: EvaluationDetail
        public ActionResult DealFailYes()
        {
            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("Login", "Home");
            }
            DataTable dt = new DataTable();
            string dealerCode = Session["DealerCode"].ToString();
            List<UCS_EvaluationVM> Select_PriceOfferNegociation = new List<UCS_EvaluationVM>();
            Select_PriceOfferNegociation = DetailEvaluationMethods.Get_DealFailYes(dealerCode);
            ViewBag.Evaluation = Select_PriceOfferNegociation;
            return View();
        }
        public ActionResult DetailEvaluation()
        {
            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("Login", "Home");
            }
            DataTable dt = new DataTable();
            string dealerCode = Session["DealerCode"].ToString();
            List<UCS_EvaluationVM> Select_PriceOfferNegociation = new List<UCS_EvaluationVM>();
            Select_PriceOfferNegociation = DetailEvaluationMethods.Get_PriceOfferNegociationData(dealerCode);
            ViewBag.Evaluation = Select_PriceOfferNegociation;
            List<SelectListItem> ddlDFReasonDesc = new List<SelectListItem>();
            ddlDFReasonDesc = GeneralMethods.GetDataFromSPWithDealerCode("Select_DetailEvaluation_Deal",dealerCode);
            ViewBag.ddlDFReasonDesc = ddlDFReasonDesc;
            return View();
        }
        public JsonResult Insert_DealFail(UCS_EvaluationVM objects )
        {
            //string dealerCode = Session["DealerCode"].ToString();
            bool result = false;
            int count = 0;
           string msg = "Failed to save record..";


            result = DetailEvaluationMethods.Insert_DealFail(objects, ref msg);
        

            if (result)
            {
                msg = "Successfully Updated";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Exports( )
        {

            DSReports data = new DSReports();
            ReportDocument RD = new ReportDocument();
            string dealerCode = Session["DealerCode"].ToString();
            SqlParameter[] param =
            {
                new SqlParameter("@DealerCode",SqlDbType.Char),
                //new SqlParameter("@BuyingCode",SqlDbType.Char)
            };

            param[0].Value = dealerCode;
          //  param[1].Value = EnquiryId;

            SqlDataReader rder = null;

            SysFunction sysFunc = new SysFunction();
            try
            { 
            if (sysFunc.ExecuteSP("Select_DealFailYes", param, ref rder))
            {
                data.EnforceConstraints = false;
                data.Select_DealFailYes.Load(rder);
            }
            RD.Load(Server.MapPath("~/Reports/Sale/DealFailReport.rpt"));

           // RD.DataDefinition.FormulaFields["DealerDesc"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";
            RD.DataDefinition.FormulaFields["ReportTitle"].Text = "'Deal Fail Evaluation Details'";
            //RD.DataDefinition.FormulaFields["Terminal"].Text = "'" + Request.ServerVariables["REMOTE_ADDR"].ToString() + "'";
            RD.DataDefinition.FormulaFields["UserId"].Text = "'" + Session["UserName"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["NTN"].Text = "'N.T.N # " + Session["DealerNTN"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["SalesTaxNo"].Text = "'Sales Tax No.  " + Session["DealerSaleTaxNo"].ToString() + " '";
            //rpt.DataDefinition.FormulaFields["UserCell"].Text = "'" + GetStringValuesAgainstCodes("CusCode", , "CellNo", "Customer") + "'";
            RD.DataDefinition.FormulaFields["CompanyName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            // RD.DataDefinition.FormulaFields["Pic"].Text = "'C:\\Users\\u_ahm\\OneDrive\\Documents\\Visual Studio 2010\\Projects\\WebApplication1\\WebApplication1\\" + Session["Logo"] + "'";
            RD.DataDefinition.FormulaFields["Pic"].Text = "'" + Server.MapPath("~") + Session["Logo"] + "'";
                //RD.DataDefinition.FormulaFields["FrontImg"].Text = "'" + data.Select_BuyingCodeReport.Rows[0]["Pic01"].ToString() + "'";
                //RD.DataDefinition.FormulaFields["BackImg"].Text = "'" + data.Select_BuyingCodeReport.Rows[0]["Pic02"].ToString() + "'";
                //RD.DataDefinition.FormulaFields["LeftImg"].Text = "'" + data.Select_BuyingCodeReport.Rows[0]["Pic03"].ToString() + "'";
                //RD.DataDefinition.FormulaFields["RightImg"].Text = "'" + data.Select_BuyingCodeReport.Rows[0]["Pic04"].ToString() + "'";
                //RD.DataDefinition.FormulaFields["TopImg"].Text = "'" + data.Select_BuyingCodeReport.Rows[0]["Pic05"].ToString() + "'";
                RD.Database.Tables[0].SetDataSource(data);

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