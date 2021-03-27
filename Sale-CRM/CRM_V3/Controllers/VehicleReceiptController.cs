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

    public class VehicleReceiptController : Controller
    {
        static string DealerCode = string.Empty;
        static SysFunction sysfun = new SysFunction();
        SecurityBll common = new SecurityBll();
        sngonclo_BMSEntities BMS = new sngonclo_BMSEntities();
        // GET: VehicleReceipt
        public ActionResult Main()
        {

            if(string.IsNullOrEmpty((string)Session["DealerCode"])) {
                return RedirectToAction("Login", "Home");
            }
            if (common.UserRight("2015", "001"))
            {
                DealerCode = Session["DealerCode"].ToString();

            List<SelectListItem> ddlRecNo = new List<SelectListItem>();
            ddlRecNo = VehReceiptMethods.GetRecNo(DealerCode);
            ViewBag.RecNo = ddlRecNo;
            List<SelectListItem> ddlVersion = new List<SelectListItem>();
            ddlVersion = GeneralMethods.GetDataFromSp("Select_Version");
            ViewBag.ddlVersion = ddlVersion;

            List<SelectListItem> ddlVehType = new List<SelectListItem>();
            ddlVehType = GeneralMethods.GetDataFromSPWithDealerCode("Select_VehicleType",DealerCode);
            ViewBag.VehType = ddlVehType;

            List<SelectListItem> ddlRecType = new List<SelectListItem>();
            ddlRecType = GeneralMethods.GetDataFromSPWithDealerCode("Select_ReceiptType",DealerCode);
            ViewBag.RecType = ddlRecType;
            List<SelectListItem> ddlTransPort = new List<SelectListItem>();
            ddlTransPort = GeneralMethods.GetDataFromSPWithDealerCode("Select_VehicleTransPort", DealerCode);
            ViewBag.ddlTransPort = ddlTransPort;
            
            List<SelectListItem> ddlBrandCode = new List<SelectListItem>();
            ddlBrandCode = GeneralMethods.GetDataFromSPWithDealerCode("Select_Brand", DealerCode,"Y");
            ViewBag.BrandCode = ddlBrandCode;

            List<SelectListItem> ddlProdCode = new List<SelectListItem>();
            //ddlProdCode = GeneralMethods.GetProduct();
            ViewBag.ProdCode = ddlProdCode;

            List<SelectListItem> ddlColor = new List<SelectListItem>();
            ddlColor = GeneralMethods.GetColor(Session["VehicleCategory"].ToString());
            ViewBag.Color = ddlColor;

            List<SelectListItem> ddlCustomers = new List<SelectListItem>();
            ddlCustomers = VehReceiptMethods.GetDatafromSP("SP_Select_Customer",DealerCode);
            ViewBag.Customers = ddlCustomers;

            List<SelectListItem> ddlVehLoc = new List<SelectListItem>();
            ddlVehLoc = VehReceiptMethods.GetDatafromSP("SP_Select_VehLocation",DealerCode);
            ViewBag.VehLoc = ddlVehLoc;

            List<SelectListItem> ddlBookingNo = new List<SelectListItem>();
            ddlBookingNo = VehReceiptMethods.GetDatafromSP("Select_BookingNo", DealerCode);
            ViewBag.BookingNo = ddlBookingNo;

            List<SelectListItem> ddlVendor = new List<SelectListItem>();
            ddlVendor = GeneralMethods.GetDataFromSPWithDealerCode("Select_Vendor",DealerCode);
            ViewBag.Vendor = ddlVendor;
            List<SelectListItem> ddlLocation = new List<SelectListItem>();
            ddlLocation = BookingOrderMethods.GetDataFromSPWithDealerCode("SP_Select_BOLocation_Frieght", DealerCode);
            ViewBag.Location = ddlLocation;
            }
            else
            {
                TempData["TestAccessError"] = MessageAlert.MsgAuthorized();
                return RedirectToAction("Error", "Definition");
            }
            return View();
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

            SqlParameter[] param2 =
           {
                new SqlParameter("@DealerCode",SqlDbType.Char),
                new SqlParameter("@DONo",SqlDbType.VarChar),
                new SqlParameter("@Type",SqlDbType.VarChar)
            };

            param2[0].Value = DealerCode;
            param2[1].Value = EnquiryId;
            param2[2].Value = "VR";

            if (sysFunc.ExecuteSP("SP_GetDeliveryCheckList", param2, ref rder))
            {
                data.SP_GetDeliveryCheckList.Load(rder);

            }


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
        public JsonResult Select_BrandProduct(string EnquiryId, string DealerCode)
        {
            List<PrdouctVM> data;
            DealerCode = Session["DealerCode"].ToString();
            bool result = false;
            data = VehReceiptMethods.Get_BrandProductData(EnquiryId, DealerCode,Session["VehicleCategory"].ToString());

            if (data.Count > 0)
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);

        }


        public JsonResult Insert_ProdRecMaster(ProdReceiptVM ProdReceipt)
        {
            bool result = false;
            DealerCode = Session["DealerCode"].ToString();
            string msg = "Failed to save record..";
            if (ProdReceipt.RecNo != null && ProdReceipt.RecNo != "0")
            {
                if (Core.CRM.ADO.SecurityBll.UserRights("2015", "003"))
                {
                    
                    result = VehReceiptMethods.Insert_ProdMaster(ProdReceipt, DealerCode, ref msg);
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
                result = VehReceiptMethods.Insert_ProdMaster(ProdReceipt, DealerCode, ref msg);
                if (result)
                {
                    msg = "Successfully Added";
                }
            }
           

         

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Delete_ProdRecMaster(string EnquiryId, string DealerCode)
        {
            bool result = false;
            DealerCode = Session["DealerCode"].ToString();
            string msg = "Vehicle is Delivered , Data can't be deleted";

            result = VehReceiptMethods.Delete_VehicleReceipt_Record(EnquiryId, DealerCode);

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
            result = VehReceiptMethods.Delete_VehicleReceiptDetail_Record(EnquiryId, DealerCode, ChasisNo, EngineNo);

            if (result)
            {
                msg = "Successfully Deleted";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Insert_ProdRecDetail( List<ProdReceiptDetailVM> objects,List<ProdReceiptDetailVM> objectFeatures)
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
        public JsonResult Insert_ProdRecFeatures(List<ProdReceiptDetailVM> objectsFeatures)
        {
            bool result = false;
            string msg = "Failed to save record..";
            DealerCode = Session["DealerCode"].ToString();

            result = VehReceiptMethods.Insert_ProdFeature(objectsFeatures, DealerCode, ref msg);

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
        public JsonResult Check_DeliveryFlag(string ChassisNo)
        {
            string msg = "";
            bool result = false;
            string Append = "and ChassisNo="+ChassisNo;
            DealerCode = Session["DealerCode"].ToString();
            if (sysfun.IsExist("DeliveryFlag","Y","VehicleStock", DealerCode, "and ChasisNo='"+ ChassisNo + "'"))
            {
                msg = "Delivery Order is already exist against this Vehicle: "+ChassisNo;
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
        public JsonResult Select_VehicleReceiptFeatureDetail(string EnquiryId, string DealerCode)
        {
            string data = "";
            bool result = false;
            DealerCode = Session["DealerCode"].ToString();
            data = VehReceiptMethods.Get_VehicleReceiptFeatureDetailData(EnquiryId, DealerCode);

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
        public JsonResult Select_VehRecModal(string EnquiryId)
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
        [HttpGet]
        public JsonResult Select_DefaultVendor(string EnquiryId)
        {
            string data;
            bool result = false;
            DealerCode = Session["DealerCode"].ToString();
            data=BMS.Vendor.Where(i =>i.DealerCode == DealerCode && i.DeafultVendor=="Y").Select(k=>k.VendorCode).FirstOrDefault().ToString();
        

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);

        }
    }
}