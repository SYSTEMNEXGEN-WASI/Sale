using Core.CRM.ADO;
using Core.CRM.ADO.ViewModel;
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
using CRM.Models.Classes;

namespace CRM_V3.Controllers
{
    public class DeliveryOrderController : Controller
    {
        static string dealerCode = string.Empty;
        SysFunction grl = new SysFunction();
        SecurityBll common = new SecurityBll();
        // GET: DeliveryOrder
        public ActionResult DOMain(string EnquiryId = "")
        {

            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("Login", "Home");
            }
            if (common.UserRight("2017", "001"))
            {
                dealerCode = Session["DealerCode"].ToString();

            List<SelectListItem> ddlRecNo = new List<SelectListItem>();
            ddlRecNo = VehReceiptMethods.GetRecNo(dealerCode);
            ViewBag.RecNo = ddlRecNo;
              List<SelectListItem> ddlVersion = new List<SelectListItem>();
            ddlVersion = GeneralMethods.GetDataFromSp("Select_Version");
            ViewBag.ddlVersion = ddlVersion;
            //List<SelectListItem> ddlDONo = new List<SelectListItem>();
            //ddlDONo = DeliveryOrderMethods.GetDONo(dealerCode);
            //ViewBag.DONo = ddlDONo;

            List<SelectListItem> ddlVehType = new List<SelectListItem>();
            ddlVehType = GeneralMethods.GetDataFromSPWithDealerCode("Select_VehicleType",dealerCode,"Y");
            ViewBag.VehType = ddlVehType;

            List<SelectListItem> ddlRecType = new List<SelectListItem>();
            ddlRecType = GeneralMethods.GetDataFromSPWithDealerCode("Select_ReceiptType",dealerCode);
            ViewBag.RecType = ddlRecType;

            List<SelectListItem> ddlBrandCode = new List<SelectListItem>();
            ddlBrandCode = GeneralMethods.GetDataFromSPWithDealerCode("Select_Brand",dealerCode);
            ViewBag.BrandCode = ddlBrandCode;

            List<SelectListItem> ddlProdCode = new List<SelectListItem>();
            //ddlProdCode = GeneralMethods.GetProduct();
            ViewBag.ProdCode = ddlProdCode;

            List<SelectListItem> ddlColor = new List<SelectListItem>();
            ddlColor = GeneralMethods.GetColor(Session["VehicleCategory"].ToString());
            ViewBag.Color = ddlColor;

            List<SelectListItem> ddlCustomers = new List<SelectListItem>();
            ddlCustomers = VehReceiptMethods.GetDatafromSP("SP_Select_Customer",dealerCode);
            ViewBag.Customers = ddlCustomers;

            List<SelectListItem> ddlVehLoc = new List<SelectListItem>();
            ddlVehLoc = VehReceiptMethods.GetDatafromSP("SP_Select_VehLocation", dealerCode);
            ViewBag.VehLoc = ddlVehLoc;

            List<SelectListItem> ddlAssignTo = new List<SelectListItem>();
            ddlAssignTo = DeliveryOrderMethods.GetDealerEmployee(dealerCode);
            ViewBag.AssignTo = ddlAssignTo;

            List<SelectListItem> ddlVendor = new List<SelectListItem>();
            ddlVendor = GeneralMethods.GetVendor();
            ViewBag.Vendor = ddlVendor;

            ViewBag.UrlLeadId = EnquiryId;
            }
            else
            {
                TempData["TestAccessError"] = MessageAlert.MsgAuthorized();
                return RedirectToAction("Error", "Definition");
            }


            return View();
        }

        [HttpGet]
        public JsonResult Select_DelChkList(string Type , string DealerCode)
        {
            string data = "";
            bool result = false;
            data = DeliveryOrderMethods.Get_DelChkList(Type,DealerCode);

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Select_DeliveryCheckList(string EnquiryId)
        {
            string data = "";
            bool result = false;
            data = DeliveryOrderMethods.Get_DeliveryChkList(EnquiryId,Session["DealerCode"].ToString(),"DO");

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Export(string EnquiryId, string DealerCode)
        {
            DSReports data = new DSReports();
            ReportDocument RD = new ReportDocument();

            SqlParameter[] param =
            {
                new SqlParameter("@DealerCode",SqlDbType.Char),
                new SqlParameter("@DONo",SqlDbType.VarChar)

            };

            param[0].Value = DealerCode;
            param[1].Value = EnquiryId;

            SqlDataReader rder = null;

            SysFunction sysFunc = new SysFunction();
            if (sysFunc.ExecuteSP("SP_DeliveryOrderReport", param, ref rder))
            {
                data.SP_DeliveryOrderReport.Load(rder);

            }

            SqlParameter[] param2 =
            {
                new SqlParameter("@DealerCode",SqlDbType.Char),
                new SqlParameter("@DONo",SqlDbType.VarChar),
                new SqlParameter("@Type",SqlDbType.VarChar)
            };

            param2[0].Value = DealerCode;
            param2[1].Value = EnquiryId;
            param2[2].Value = "DO";

            if (sysFunc.ExecuteSP("SP_GetDeliveryCheckList", param2, ref rder))
            {
                data.SP_GetDeliveryCheckList.Load(rder);

            }

            RD.Load(Server.MapPath("~/Reports/DeliveryReportNew.rpt"));


            //RD.DataDefinition.FormulaFields["DealerDesc"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["ReportTitle"].Text = "'Delivery Order Report'";
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

        [HttpGet]
        public JsonResult Select_ChassisNo(string EnquiryId, string DealerCode)
        {
            List<SelectListItem> data;
            bool result = false;
            
            data = DeliveryOrderMethods.GetChassisNo(EnquiryId, Session["DealerCode"].ToString());

            if (data.Count > 0)
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult Select_ProRecDetail(string EnquiryId)
        {
            string data;
            bool result = false;

            data = DeliveryOrderMethods.GetProRecDetail(EnquiryId, Session["DealerCode"].ToString());

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult Select_DeliveryOrder(string EnquiryId)
        {
            string data = "";
            bool result = false;
            data = DeliveryOrderMethods.Get_DeliveryOrderData(EnquiryId, Session["DealerCode"].ToString());

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Insert_DOMaster(VehicleDeliveryMasterVM DOMasterVM)
        {
            bool result = false;

            string msg = "Failed to save record..";
            if(DOMasterVM.DeliveryNo!="" && DOMasterVM.DeliveryNo != "0")
            {
                if (Core.CRM.ADO.SecurityBll.UserRights("2017", "003"))
                {

                    result = DeliveryOrderMethods.Insert_DOMaster(DOMasterVM, Session["DealerCode"].ToString());

                    if (result)
                    {
                        msg = "Successfully Added";
                    }

                }
                else
                {
                    msg = "You dn't have a right! Please Contact to Administrator";
                    result = false;
                }

                }
            else
            {
                result = DeliveryOrderMethods.Insert_DOMaster(DOMasterVM, Session["DealerCode"].ToString());

                if (result)
                {
                    msg = "Successfully Added";
                }
            }

            

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult Insert_DODetail(List<VehicleDeliveryDetailVM> objects)
        {
            bool result = false;
            int count = 0;
            string msg = "Failed to save record..";


            //foreach (var item in objects)
            //{
            //    if (count >= 1 || item.BrandCode != null)
            //    {
                    result = DeliveryOrderMethods.Insert_DODetail(objects, Session["DealerCode"].ToString());
            //    }
            //    count++;
            //}

            if (result)
            {
                msg = "Successfully Added";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Insert_VehChkList(string strCheckedValues)
        {
            bool result = false;
            int count = 0;
            string msg = "Failed to save record..";

            result = DeliveryOrderMethods.Insert_VehChkList(strCheckedValues, Session["DealerCode"].ToString());

            if (result)
            {
                msg = "Successfully Added";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Delete_DeliveryOrder(string EnquiryId)
        {
            bool result = false;

            string msg = "Vehicle is Delivered , Data can't be deleted";

            result = DeliveryOrderMethods.Delete_DeliveryOrder_Record(EnquiryId, Session["DealerCode"].ToString());

            if (result)
            {
                msg = "Successfully Deleted";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Select_DOModal(string EnquiryId)
        {
            string data;
            bool result = false;

            data = DeliveryOrderMethods.GetDOModal(Session["DealerCode"].ToString());

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult Insert_GatePass(GatePassVM DOMasterVM)
        {
            bool result = false;

            string msg = "Failed to save record..";

            Session["TransCode"] = DOMasterVM.TransCode;
            result = DeliveryOrderMethods.Insert_GatePass(DOMasterVM, Session["DealerCode"].ToString(),ref msg);

            if (result)
            {
                msg = "Successfully Added";
               
            }
            

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GatePass()
        {
           
            DSReports data = new DSReports();
            ReportDocument RD = new ReportDocument();
          string  DealerCode = Session["DealerCode"].ToString();
           string old= grl.GetStringValuesAgainstCodes("TransCode", Session["TransCode"].ToString(), "GatePassCode", "GatePassTemp", " and Module='Sales'", Session["DealerCode"].ToString());
           
            SqlParameter[] param =
            {
                new SqlParameter("@DealerCode",SqlDbType.Char),
                new SqlParameter("@DONo",SqlDbType.VarChar)

            };

            param[0].Value = DealerCode;
            param[1].Value = old;

            SqlDataReader rder = null;

            SysFunction sysFunc = new SysFunction();
            if (sysFunc.ExecuteSP("SP_DeliveryOrderGatePass", param, ref rder))
            {
                data.SP_DeliveryOrderGatePass.Load(rder);

            }

           

            RD.Load(Server.MapPath("~/Reports/rptGatePass.rpt"));


            //RD.DataDefinition.FormulaFields["DealerDesc"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";
            RD.DataDefinition.FormulaFields["ReportTitle"].Text = "'Delivery Order Gate Pass'";
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