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
using System.Web.Script.Serialization;
using CRM.Models.Classes;

namespace CRM_V3.Controllers
{
    public class BookingOrderController : Controller
    {

        static string dealerCode = string.Empty;
        SecurityBll common = new SecurityBll();
        // GET: BookingOrder
        public ActionResult BOMain(string EnquiryId = "")
        {
            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("Login", "Home");
            }
            if (common.UserRight("2014", "001"))
            {
                dealerCode = Session["DealerCode"].ToString();

                List<SelectListItem> ddlBONo = new List<SelectListItem>();
                ddlBONo = BookingOrderMethods.GetDataFromSPWithDealerCode("SP_SelectBONo", dealerCode);
                ViewBag.BONo = ddlBONo;

                List<SelectListItem> ddlEnquiry = new List<SelectListItem>();
                ddlEnquiry = BookingOrderMethods.GetDataFromSPWithDealerCode("SP_EnquiryID", dealerCode);
                ViewBag.Enquiry = ddlEnquiry;

                List<SelectListItem> ddlCusType = new List<SelectListItem>();
                ddlCusType = BookingOrderMethods.GetDataFromSPWithDealerCode("SP_SelectCustomerType", dealerCode);
                ViewBag.CusType = ddlCusType;

                List<SelectListItem> ddlInvType = new List<SelectListItem>();
                ddlInvType = BookingOrderMethods.GetDataFromSP("SP_SelectInvoiceType");
                ViewBag.InvType = ddlInvType;

                List<SelectListItem> ddlInvSubType = new List<SelectListItem>();
                ddlInvSubType = BookingOrderMethods.GetDataFromSP("SP_SelectInvoiceSubType");
                ViewBag.InvSubType = ddlInvSubType;

                List<SelectListItem> ddlPriceType = new List<SelectListItem>();
                ddlPriceType = BookingOrderMethods.GetDataFromSP("SP_SelectriceType");
                ViewBag.PriceType = ddlPriceType;

                List<SelectListItem> ddlChassisNo = new List<SelectListItem>();
                ddlChassisNo = BookingOrderMethods.GetDataFromSPWithDealerCode("SP_SelectChassisNo", dealerCode);
                ViewBag.ChassisNo = ddlChassisNo;

                List<SelectListItem> ddlLocation = new List<SelectListItem>();
                ddlLocation = BookingOrderMethods.GetDataFromSPWithDealerCode("SP_Select_BOLocation_Frieght", dealerCode);
                ViewBag.Location = ddlLocation;

                List<SelectListItem> ddlAssignTo = new List<SelectListItem>();
                ddlAssignTo = DeliveryOrderMethods.GetDealerEmployee(dealerCode);
                ViewBag.AssignTo = ddlAssignTo;

                List<SelectListItem> ddlCustomers = new List<SelectListItem>();
                ddlCustomers = VehReceiptMethods.GetDatafromSP("SP_Select_Customer", dealerCode);
                ViewBag.Customers = ddlCustomers;
                ViewBag.AccountOf = ddlCustomers;

                string ddlCustomer;
                ddlCustomer = BookingOrderMethods.GetCustomerModal(dealerCode);
                ViewBag.Customer = ddlCustomer;

                List<SelectListItem> ddlBrandCode = new List<SelectListItem>();
                ddlBrandCode = GeneralMethods.GetDataFromSPWithDealerCode("Select_Brand", dealerCode, "Y");
                ViewBag.BrandCode = ddlBrandCode;

                List<SelectListItem> ddlProdCode = new List<SelectListItem>();
                //ddlProdCode = GeneralMethods.GetProduct();
                ViewBag.ProdCode = ddlProdCode;

                List<SelectListItem> ddlColor = new List<SelectListItem>();
                ddlColor = GeneralMethods.GetColor(Session["VehicleCategory"].ToString());
                ViewBag.Color = ddlColor;

                List<SelectListItem> ddlRecNo = new List<SelectListItem>();
                ddlRecNo = VehReceiptMethods.GetRecNo(dealerCode);
                ViewBag.RecNo = ddlRecNo;

                List<SelectListItem> ddlBank = new List<SelectListItem>();
                ddlBank = GeneralMethods.GetBank();
                ViewBag.Bank = ddlBank;
                ViewBag.DrwanBank = ddlBank;

                List<SelectListItem> ddlCity = new List<SelectListItem>();
                ddlCity = GeneralMethods.GetCity();
                ViewBag.City = ddlCity;

                List<SelectListItem> ddlPaymentMode = new List<SelectListItem>();
                ddlPaymentMode = BookingOrderMethods.GetDataFromSP("sp_PaymentMode_select");
                ViewBag.PaymentMode = ddlPaymentMode;

                List<SelectListItem> BookingList = new List<SelectListItem>();
                BookingList = BookingOrderMethods.GetDataFromSP("sp_DocCheckList");

                ViewBag.BookingList = BookingList;

                @Session["ProdCode"] = string.Empty;

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
        public JsonResult Select_CustomerDetail(string EnquiryId)
        {
            string data;
            bool result = false;

            data = BookingOrderMethods.GetCustomerDetail(EnquiryId, Session["DealerCode"].ToString());

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult Select_CustomerDetailModal(string EnquiryId)
        {
            string data;
            bool result = false;

            data = BookingOrderMethods.GetCustomerModal(Session["DealerCode"].ToString());
          
            ViewBag.Customer = data;
            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult Select_BookRefModal()
        {
            string data;
            bool result = false;

            data = BookingOrderMethods.GetBookRefModal(Session["DealerCode"].ToString());

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult Get_EnquiryModal()
        {
            string data;
            bool result = false;

            data = BookingOrderMethods.Get_EnquiryModal(Session["DealerCode"].ToString());

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult Select_VehicleDetail(string EnquiryId)
        {
            string data;
            bool result = false;

            data = BookingOrderMethods.GetVehicleDetail(EnquiryId, Session["DealerCode"].ToString());

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult Insert_BOMaster(BookOrdMasterVM DOMasterVM)
        {
            bool result = false;

            string msg = "Failed to save record..";
            if (DOMasterVM.BookRefNo != "" && DOMasterVM.BookRefNo != "0")
            {
                if (Core.CRM.ADO.SecurityBll.UserRights("2014", "003"))
                {
                    result = BookingOrderMethods.Insert_BOMaster(DOMasterVM, ref msg);
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
                result = BookingOrderMethods.Insert_BOMaster(DOMasterVM, ref msg);
                if (result)
                {
                    msg = "Successfully Added";
                }
            }
           

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Insert_BODetail(List<BookOrdVehDetailVM> objects)
        {
            bool result = false;
            int count = 0;
            string msg = "Failed to save record..";


            //foreach (var item in objects)
            //{
            //    if (count >= 1 || item.BrandCode != null)
            //    {
                    result = BookingOrderMethods.Insert_BODetail(objects, Session["DealerCode"].ToString(), ref msg);
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
        public JsonResult Insert_PaymentDetail(List<BookingReceiptInstrumentDetailVM> objects)
        {
            bool result = false;
            int count = 0;
            string msg = "Failed to save record..";


            //foreach (var item in objects)
            //{
            //    if (count >= 1 || item.InstrumentNo != null)
            //    {
                    result = BookingOrderMethods.Insert_PaymentDetail(objects, Session["DealerCode"].ToString(),ref msg);
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
        public JsonResult Insert_BalancedPaymentDetail(List<BookingReceiptInstrumentDetailVM> objects)
        {
            bool result = false;
            int count = 0;
            string msg = "Failed to save record..";

            
            result = BookingOrderMethods.Insert_BalancedPaymentDetail(objects, Session["DealerCode"].ToString(),ref msg);
            

            if (result)
            {
                msg = "Successfully Added";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Insert_VehChkList(List<DocumentCheckList> objects)
        {
            //string strCheckedValues
            bool result = false;
            int count = 0;
            string msg = "Failed to save record..";
           
            // result = BookingOrderMethods.Insert_VehChkList(strCheckedValues, Session["DealerCode"].ToString());
        result = BookingOrderMethods.Insert_VehChkLists(objects, Session["DealerCode"].ToString(),ref msg);

            if (result)
            {
                msg = "Successfully Added";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Select_BookingOrder(string EnquiryId)
        {
            string data = "";
            bool result = false;
            data = BookingOrderMethods.Get_BookingOrderData(EnquiryId, Session["DealerCode"].ToString());

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Select_BookOrdVehDetail(string EnquiryId, string Code)
        {
            string data = "";
            bool result = false;
            data = BookingOrderMethods.Get_VehicleDetailData(EnquiryId, Session["DealerCode"].ToString());

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Select_BookingReceiptInstrumentDetail(string EnquiryId, string Code)
        {
            string data = "";
            bool result = false;
            data = BookingOrderMethods.Get_BookingReceiptInstrumentDetailData(EnquiryId, Session["DealerCode"].ToString());

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Select_BookingCheckList(string EnquiryId)
        {
            string data = "";
            bool result = false;
            data = BookingOrderMethods.Get_BookingChkList(EnquiryId,Session["DealerCode"].ToString());

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Proceed_BookingOrder(string EnquiryId)
        {
            bool result = false;

            string msg = "Vehicle is Proceed , Data can't be Proceed";

            result = BookingOrderMethods.Proceed_BookingOrder_Record(EnquiryId, Session["DealerCode"].ToString());

            if (result)
            {
                msg = "Successfully Proceed";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Delete_BookingOrder(string EnquiryId)
        {
            bool result = false;

            string msg = "Vehicle is Delivered , Data can't be deleted";

            result = BookingOrderMethods.Delete_BookingOrder_Record(EnquiryId, Session["DealerCode"].ToString());

            if (result)
            {
                msg = "Successfully Deleted";
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
            if (sysFunc.ExecuteSP("SP_BookingOrderReport", param, ref rder))
            {
                data.EnforceConstraints = false;
                data.SP_BookingOrderReport.Load(rder);

            }
            if (sysFunc.ExecuteSP("SP_PAymentVehicleDetailReport", param, ref rder))
            {
                data.SP_PAymentVehicleDetailReport.Load(rder);

            }

            SqlParameter[] param2 =
          {
                new SqlParameter("@DealerCode",SqlDbType.Char),
                new SqlParameter("@DONo",SqlDbType.VarChar),
                new SqlParameter("@Type",SqlDbType.VarChar)
            };

            param2[0].Value = DealerCode;
            param2[1].Value = EnquiryId;
            param2[2].Value = "BO";

            if (sysFunc.ExecuteSP("SP_GetBookOrdCheckList", param2, ref rder))
            {
                data.SP_GetBookOrdCheckList.Load(rder);

            }


            RD.Load(Server.MapPath("~/Reports/BookingOrderReport.rpt"));
            RD.OpenSubreport(Server.MapPath("~/Reports/PaymentDetails.rpt"));

            RD.DataDefinition.FormulaFields["DealerDesc"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";
            RD.DataDefinition.FormulaFields["ReportTitle"].Text = "'Booking Order to OEM'";
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
        public JsonResult Get_FactoryPrice(string Color, string Brand, string Product, string Version , string DealerCode)
        {
            String data;
            bool result = false;

            data = BookingOrderMethods.Get_FactoryPrice(Color, Brand , Product , Version , DealerCode);

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);

        }


        [HttpGet]
        public JsonResult Get_FreightCharges(string Loc, string Product ,string DealerCode)
        {
            String data;
            bool result = false;

            data = BookingOrderMethods.Get_FreightCharges(Loc, Product, DealerCode);

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult Select_OptionalFeatures(string EnquiryId, string DealerCode)
        {
            string json = "";
            var Serializer = new JavaScriptSerializer();
            List<OptionalFeatureVM> data;
            bool result = false;
            data = BookingOrderMethods.Get_OptionalFeatureData(EnquiryId, DealerCode);

            if (data.Count > 0)
            {
                result = true;
                json = Serializer.Serialize(data);
            }

            return Json(new { Success = result, Response = json }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult Select_OptionalFeatures_VS(string EnquiryId, string DealerCode,string Chassis)
        {
            string json = "";
            var Serializer = new JavaScriptSerializer();
            List<OptionalFeatureVM> data;
            bool result = false;
            data = BookingOrderMethods.Get_OptionalFeatureData_VS(EnquiryId, DealerCode,Chassis);

            if (data.Count > 0)
            {
                result = true;
                json = Serializer.Serialize(data);
            }

            return Json(new { Success = result, Response = json }, JsonRequestBehavior.AllowGet);

        }


        [HttpGet]
        public JsonResult Check_Post(string Bo)
        {
            string msg = "";
            bool result = false;
            //result = VehReceiptMethods.Check_ChassisNo(chassisNo,engineNo, DealerCode);
         string   DealerCode = Session["DealerCode"].ToString();
            if (BookingOrderMethods.Check_Post(Bo, DealerCode))
            {
                msg = "";
                result = true;
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }
        //// Add Image into model lsit  and Convert to Binary
        [HttpPost]
        public JsonResult AddImage_List(string data, string EnquiryId)
        {
            DocumentCheckList delchklist=new DocumentCheckList();
            string json = "";
            var Serializer = new JavaScriptSerializer();
            byte[] imageBytes = null;
            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase file = Request.Files[0];

                if (file != null && file.ContentLength > 0)
                {
                  

                    BinaryReader reader = new BinaryReader(file.InputStream);
                    imageBytes = reader.ReadBytes((int)file.ContentLength);
                    json = "data:image/png;base64," + Convert.ToBase64String(imageBytes, 0, imageBytes.Length);
                    // json= "data:image/png;base64," + Convert.ToBase64String(file.Data, 0, file.Data.Length);


                }



            }

                    return Json(new { Success = true, Message = "" ,Response= json }, JsonRequestBehavior.AllowGet);
        }



        //// Check hold Vehicle
        [HttpGet]
        public JsonResult Check_Hold(string EnquiryId)
        {
            string msg = "";
            bool result = false;
            //result = VehReceiptMethods.Check_ChassisNo(chassisNo,engineNo, DealerCode);
            string DealerCode = Session["DealerCode"].ToString();
            if (BookingOrderMethods.Check_Hold(EnquiryId, DealerCode))
            {
                msg = "This Vehicle is Hold For Booking You Can Not Create its Booking Order";
                result = true;
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }



    }
}