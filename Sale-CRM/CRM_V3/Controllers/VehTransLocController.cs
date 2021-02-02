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
    public class VehTransLocController : Controller
    {
        static string DealerCode = string.Empty;
        DataTable dt = new DataTable();
        // GET: VehTransLoc
        public ActionResult Main()
        {
            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("Login", "Home");
            }
            DealerCode = Session["DealerCode"].ToString();

            List<SelectListItem> ddlVehLoc = new List<SelectListItem>();
            ddlVehLoc = GeneralMethods.GetDataFromSPWithDealerCode("SP_Select_VehLocation", DealerCode,"Y");
            ViewBag.VehLoc = ddlVehLoc;

            List<VehicleLocTransVM> VehLocTransList = new List<VehicleLocTransVM>();

            dt = GeneralMethods.GetDataForModal("SP_Modal_VehicleLocTrans", DealerCode);
            if (dt.Rows.Count > 0)
            {
                VehLocTransList = EnumerableExtension.ToList<VehicleLocTransVM>(dt);
            }

            ViewBag.VehLocTrans = VehLocTransList;

            return View();
        }

        [HttpGet]
        public JsonResult Select_ChassisNoForVehicleLocation(string EnquiryId, string DealerCode)
        {
            List<VehicleStockVM> data;
            bool result = false;
            string msg = "";
            data = VehLocTransMethods.Get_ChasisNoForVehicleTransLoc(EnquiryId, DealerCode, ref msg);

            if (data.Count > 0)
            {
                result = true;
            }

            return Json(new { Success = result, Response = data, Message = msg }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult Select_VehicleDetailForVehicleTransferLocation(string EnquiryId)
        {
            string data;
            bool result = false;

            data = VehLocTransMethods.GetVehicleDetail(EnquiryId, Session["DealerCode"].ToString());

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult Insert_VehLocTransMaster(VehicleLocTransVM VehLocTransVM)
        {
            bool result = false;

            string msg = "Failed to save record..";

            result = VehLocTransMethods.Insert_VehLocTransMaster(VehLocTransVM, Session["DealerCode"].ToString(),ref msg);

            if (result)
            {
                msg = "Successfully Added";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Insert_VehLocTransDetail(List<VehicleLocTransVM> objects)
        {
            bool result = false;
            string msg = "Failed to save record..";
            result = VehLocTransMethods.Insert_VehLocTransDetail(objects, Session["DealerCode"].ToString(),ref msg);

            if (result)
            {
                msg = "Successfully Added";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Select_VehLocTrans(string EnquiryId)
        {
            string data = "";
            bool result = false;
            string msg = "";
            data = VehLocTransMethods.Get_VehLocTransData(EnquiryId, Session["DealerCode"].ToString(),ref msg);

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Export(string EnquiryId)
        {
            DSReports data = new DSReports();
            ReportDocument RD = new ReportDocument();

            SqlParameter[] param =
        {
                new SqlParameter("@DealerCode",SqlDbType.Char),
                new SqlParameter("@EnquiryId",SqlDbType.VarChar)

            };

            param[0].Value = Session["DealerCode"].ToString();
            param[1].Value = EnquiryId;

            SqlDataReader rder = null;

            SysFunction sysFunc = new SysFunction();
            if (sysFunc.ExecuteSP("SP_Report_VehicleLocTrans", param, ref rder))
            {
                data.EnforceConstraints = false;
                data.SP_Report_VehicleLocTrans.Load(rder);

            }

            RD.Load(Server.MapPath("~/Reports/VehicleLocTransReport.rpt"));

            RD.DataDefinition.FormulaFields["DealerDesc"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";
            RD.DataDefinition.FormulaFields["ReportTitle"].Text = "'Vehicle Location Transfer'";
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