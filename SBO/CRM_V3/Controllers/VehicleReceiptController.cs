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
using DXBMS;
using CRM.Models.Classes;

namespace CRM_V3.Controllers
{
    public class VehicleReceiptController : Controller

    {
        

            static string DealerCode = string.Empty;
        SecurityBll common = new SecurityBll();
        // GET: VehicleReceipt

      
        public ActionResult Main()
        {

            if(string.IsNullOrEmpty((string)Session["DealerCode"])) {
                return RedirectToAction("NewLogin", "Home");
            }
            if (common.UserRight("2505", "001"))
            {
                DealerCode = Session["DealerCode"].ToString();

            //List<SelectListItem> ddlRecNo = new List<SelectListItem>();
            //ddlRecNo = VehReceiptMethods.GetRecNo(DealerCode);
            //ViewBag.RecNo = ddlRecNo;

            List<SelectListItem> ddlVehType = new List<SelectListItem>();
            ddlVehType = GeneralMethods.GetDataFromSPWithDealerCode("Select_VehicleType",DealerCode);
            ViewBag.VehType = ddlVehType;

            List<SelectListItem> ddlRecType = new List<SelectListItem>();
            ddlRecType = GeneralMethods.GetDataFromSPWithDealerCode("Select_ReceiptType",DealerCode);
            ViewBag.RecType = ddlRecType;

           
           // ViewBag.RecType = AccountCode;

            List<SelectListItem> ddlBrandCode = new List<SelectListItem>();
            ddlBrandCode = GeneralMethods.GetDataFromSPWithDealerCode("Select_Brand", DealerCode,"Y");
            ViewBag.BrandCode = ddlBrandCode;

            List<SelectListItem> ddlProdCode = new List<SelectListItem>();
            //ddlProdCode = GeneralMethods.GetProduct();
            ViewBag.ProdCode = ddlProdCode;

            List<SelectListItem> txtcolordesc = new List<SelectListItem>();
            txtcolordesc = GeneralMethods.GetColor();
            ViewBag.Color = txtcolordesc;

            List<SelectListItem> ddlCustomers = new List<SelectListItem>();
            ddlCustomers = VehReceiptMethods.GetDatafromSP("SP_Select_Customer",DealerCode);
            ViewBag.Customers = ddlCustomers;

            List<SelectListItem> ddlVehLoc = new List<SelectListItem>();
            ddlVehLoc = GeneralMethods.GetDataFromSPWithDealerCode("SP_Select_VehLocation", DealerCode);
            ViewBag.ddlVehLoc = ddlVehLoc;

           

            List<SelectListItem> ddlBookingNo = new List<SelectListItem>();
            ddlBookingNo = VehReceiptMethods.GetDatafromSP("Select_BookingNo", DealerCode);
            ViewBag.BookingNo = ddlBookingNo;

            List<SelectListItem> ddlVendor = new List<SelectListItem>();
            ddlVendor = GeneralMethods.GetDataFromSPWithDealerCode("Select_Vendor",DealerCode);
            ViewBag.Vendor = ddlVendor;
            }
            else
            {
                TempData["TestAccessError"] = MessageAlert.MsgAuthorized();
                return RedirectToAction("Error", "Definition");
            }



            return View();
        }
        public JsonResult Select_VehRecModal(string EnquiryId)
        {
            string data = "";
            bool result = false;
            data = VehReceiptMethods.GetRecNo(Session["DealerCode"].ToString());

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
                new SqlParameter("@EnquiryId",SqlDbType.VarChar)

            };

            param[0].Value = DealerCode;
            param[1].Value = EnquiryId;

            SqlDataReader rder = null;

            SysFunction sysFunc = new SysFunction();
            if (sysFunc.ExecuteSP("SP_Select_ProdRec_Print", param, ref rder))
            {
                data.EnforceConstraints = false;
                data.SP_Select_ProdRec_Print.Load(rder);
                
            }

           // SqlParameter[] param2 =
           //{
           //     new SqlParameter("@DealerCode",SqlDbType.Char),
           //     new SqlParameter("@DONo",SqlDbType.VarChar),
           //     new SqlParameter("@Type",SqlDbType.VarChar)
           // };

           // param2[0].Value = DealerCode;
           // param2[1].Value = EnquiryId;
           // param2[2].Value = "VR";

           // if (sysFunc.ExecuteSP("SP_GetDeliveryCheckList", param2, ref rder))
           // {
           //     data.SP_GetDeliveryCheckList.Load(rder);

           // }


            RD.Load(Server.MapPath("~/Reports/VehicleReceipt.rpt"));

            RD.DataDefinition.FormulaFields["DealerDesc"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";
            RD.DataDefinition.FormulaFields["ReportTitle"].Text = "'Vehicle Receipt Report'";
            RD.DataDefinition.FormulaFields["Terminal"].Text = "'" + Request.ServerVariables["REMOTE_ADDR"].ToString() + "'";
            RD.DataDefinition.FormulaFields["UserId"].Text = "'" + Session["UserName"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["NTN"].Text = "'N.T.N # " + Session["DealerNTN"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["SalesTaxNo"].Text = "'Sales Tax No.  " + Session["DealerSaleTaxNo"].ToString() + " '";
            //rpt.DataDefinition.FormulaFields["UserCell"].Text = "'" + GetStringValuesAgainstCodes("CusCode", , "CellNo", "Customer") + "'";
            RD.DataDefinition.FormulaFields["CompanyName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["Pic"].Text = "'C:\\Users\\u_ahm\\OneDrive\\Documents\\Visual Studio 2010\\Projects\\WebApplication1\\WebApplication1\\" + Session["Logo"] + "'";
            //RD.DataDefinition.FormulaFields["Pic"].Text = "'" + Server.MapPath("~") + Session["Logo"] + "'";
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
        public JsonResult Select_BrandProduct2wheel(string DealerCode,string VehicleType)
        {
            List<GetProductSpVM> data;
            DealerCode = Session["DealerCode"].ToString();
            bool result = false;
            // List<GetProductSpVM> ddlProduct = new List<GetProductSpVM>();
            data = VehReceiptMethods.Get_BrandProductData2wheel(VehicleType, DealerCode);
            Session["VehicleRecord"] = data;
            // ViewBag.ddlProduct = ddlProduct;
            //data = VehReceiptMethods.Get_BrandProductData(EnquiryId,DealerCode);

            if (data.Count > 0)
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult Select_BrandProduct(string EnquiryId, string DealerCode,string vehType)
        { 
            List<GetProductSpVM> data;
            DealerCode = Session["DealerCode"].ToString();
            bool result = false;
           // List<GetProductSpVM> ddlProduct = new List<GetProductSpVM>();
            data = VehReceiptMethods.Get_BrandProductData(EnquiryId, DealerCode);
            Session["VehicleRecord"]= data;
            // ViewBag.ddlProduct = ddlProduct;
            //data = VehReceiptMethods.Get_BrandProductData(EnquiryId,DealerCode);

            if (data.Count > 0)
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult Get_BuyingDataView(string EnquiryId,string chasisNo)
        {
            //Session["IRTransCode"] = TransCode;

            List<GetProductSpVM> ISIReceiptData = (List<GetProductSpVM>)Session["VehicleRecord"];
            //ISIReceiptData = InstallmentReceiptMethods.Get_ISIInstallmentReceiptData(Session["DealerCode"].ToString());

            var data = ISIReceiptData.Where(a => a.ProdCode.Trim() == EnquiryId.Trim() && a.ChasisNo.Trim()== chasisNo);

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Select_DelChkList(string DealerCode)
        {
            string data = "";
            bool result = false;
            data = VehReceiptMethods.Get_DelChkList(DealerCode);

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult Insert_ProdRecMaster(ProdReceiptVM ProdReceiptVM)
        {
            bool result = false;
            DealerCode = Session["DealerCode"].ToString();
            string msg = "Failed to save record..";

            result = VehReceiptMethods.Insert_ProdMaster(ProdReceiptVM , DealerCode,ref msg);

            if (result)
            {
                msg = "Successfully Added";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Delete_ProdRecMaster(string EnquiryId, string DealerCode)
        {
            bool result = false;
            DealerCode = Session["DealerCode"].ToString();
            string msg = "Vehicle is Delivered , Data can't be deleted";

            result = VehReceiptMethods.Delete_VehicleReceipt_Record(EnquiryId, DealerCode,ref msg);

            if (result)
            {
                msg = "Successfully Deleted";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Select_DataFromBookingNo(string EnquiryId)
        {
            string data;
            bool result = false;
            DealerCode = Session["DealerCode"].ToString();
            data = VehReceiptMethods.GetDataFromBookingNo(EnquiryId, DealerCode);

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult Delete_ProdRecDetail(string EnquiryId, string DealerCode, string ChasisNo , string EngineNo)
        {
            bool result = false;

            string msg = "Vehicle is Delivered , Data can't be deleted";
            DealerCode = Session["DealerCode"].ToString();
            result = VehReceiptMethods.Delete_VehicleReceiptDetail_Record(Session["VehicleRecord"].ToString(),EnquiryId, DealerCode, ChasisNo, EngineNo, ref msg);

            if (result)
            {
                msg = "Successfully Deleted";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Insert_ProdRecDetail( List<ProdReceiptDetailVM> objects)
        {
            bool result = false;
            string msg = "Failed to save record..";
            DealerCode = Session["DealerCode"].ToString();
            
            result = VehReceiptMethods.Insert_ProdDetail(objects, DealerCode,ref msg);

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
            string msg = "Failed to save record..";

            result = VehReceiptMethods.Insert_VehChkList(strCheckedValues, Session["DealerCode"].ToString(),ref msg);

            if (result)
            {
                msg = "Successfully Added";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Select_VehicleReceipt(string EnquiryId)
        {
            string data = "";
            bool result = false;
            DealerCode = Session["DealerCode"].ToString();
            data = VehReceiptMethods.Get_VehicleReceiptData(EnquiryId, DealerCode);

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Check_ChassisNo(string chassisNo)
        {
            string msg = "";
            bool result = false;
            //result = VehReceiptMethods.Check_ChassisNo(chassisNo,engineNo, DealerCode);
            DealerCode = Session["DealerCode"].ToString();
            if (VehReceiptMethods.Check_ChassisNo(chassisNo, DealerCode))
            {
                msg = "This Chassis No car is already exist";
                result = true;
            }           

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Check_EngineNo(string engineNo)
        {
            string msg = "";
            bool result = false;
            DealerCode = Session["DealerCode"].ToString();
            if (VehReceiptMethods.Check_EngineNo(engineNo, DealerCode) && result != true)
            {
                msg = "This Engine No car is already exist";
                result = true;
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Select_VehicleReceiptDetail(string EnquiryId, string DealerCode)
        {
            string data = "";
            bool result = false;
            DealerCode = Session["DealerCode"].ToString();
            data = VehReceiptMethods.Get_VehicleReceiptDetailData(EnquiryId, DealerCode);

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult Select_VehicleReceiptVoucherNo(string EnquiryId, string DealerCode)
        {
            string data = "";
            bool result = false;
            DealerCode = Session["DealerCode"].ToString();
            data = VehReceiptMethods.Get_VehicleReceiptVouchNo(EnquiryId, DealerCode);
            Session["VoucherNo"] = data;
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
            data = DeliveryOrderMethods.Get_DeliveryChkList(EnquiryId, Session["DealerCode"].ToString(),"VR");

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Select_RecModal(string EnquiryId)
        {
            string data;
            bool result = false;
            DealerCode = Session["DealerCode"].ToString();
            data = VehReceiptMethods.GetVehRecModal(DealerCode);

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);

        }


        //[HttpGet]
        //public JsonResult Select_BrandProduct(string EnquiryId, string DealerCode)
        //{
        //    List<SelectListItem> data;
        //    bool result = false;
        //    data = EventMethods.Get_BrandProductData(EnquiryId, DealerCode);

        //    if (data.Count > 0)
        //    {
        //        result = true;
        //    }

        //    return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);

        //}
    }
}