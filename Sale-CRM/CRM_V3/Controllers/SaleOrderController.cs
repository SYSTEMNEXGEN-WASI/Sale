using Core.CRM.ADO;
using Core.CRM.ADO.ViewModel;
using Core.CRM.Helper;
using CRM_V3.assets;
using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRM_V3.Controllers
{
    public class SaleOrderController : Controller
    {
        // GET: SaleOrder
        #region Start Fahad SNG LHR  --------------------------------------------6/1/2019-----------------------------Sale Order------------------------- 

        static string dealerCode = string.Empty;
        sngonclo_BMSEntities BMS = new sngonclo_BMSEntities();
        SysFunction sysfun = new SysFunction();
        static Transaction ObjTrans = new Transaction();
        static SqlTransaction Trans;

        public ActionResult SaleOrder()
        {
            string dealerCode = Session["DealerCode"].ToString();
            List<SelectListItem> ddlAssignTo = new List<SelectListItem>();
            ddlAssignTo = GeneralMethods.GetDataFromSPWithDealerCode("Select_DealerEmp", dealerCode);
            ViewBag.AssignTo = ddlAssignTo;

            List<GetCustomerSpVM> ddlAssignCus = new List<GetCustomerSpVM>();
            ddlAssignCus = GeneralMethods.GetCustomer(Session["DealerCode"].ToString());
            ViewBag.AssignCus = ddlAssignCus;

            List<SelectListItem> ddlBrandCode = new List<SelectListItem>();
            ddlBrandCode = GeneralMethods.GetBrands(Session["DealerCode"].ToString());
            ViewBag.BrandCode = ddlBrandCode;

            List<GetVehicleDetailVM> SaleDetaillookUp = new List<GetVehicleDetailVM>();
            SaleDetaillookUp = SaleOrderMethods.Get_SaleDetailData();
            ViewBag.DetailLookup = SaleDetaillookUp;

            var OnlyDate = DateTime.Now;
            ViewBag.CurrentDate = OnlyDate.ToString("dd/MM/yyyy");
            return View();
        }


        public ActionResult MTransMethod(string TransCode)
        {
            Session["TransCode"] = TransCode;

            List<GetVehicleGridDataVM> SaleDetailGrid = new List<GetVehicleGridDataVM>();
            SaleDetailGrid = SaleOrderMethods.GridDataa(TransCode);
            var data = SaleDetailGrid;

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BrandMethod(string BrandCode)
        {
            List<GetProductSpVM> ddlAssignPro = new List<GetProductSpVM>();
            ddlAssignPro = GeneralMethods.GetProductDetail(BrandCode, Session["DealerCode"].ToString());
            var data = ddlAssignPro;



            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Insert_SaleOrder(VehicleSaleMasterVM model)
        {
            bool result = false;
            string msg = "Failed to save record..";
            string leadId = "";
            string[] data;
            string dealerCode = Session["DealerCode"].ToString();
            model.DealerCode = dealerCode;
            leadId = SaleOrderMethods.Insert_SaleOrder(model);
            if (!string.IsNullOrEmpty(leadId))
            {
                data = leadId.Split(',');
                leadId = data[0].ToString();

                result = true;
                msg = "Successfully Added";
            }

            return Json(new { Success = result, Message = msg, LeadId = leadId }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Insert_SaleOrderDetail(VehicleSaleDetailVM[] modelDetail, string trancode)
        {
            bool result = false;
            string msg = "Failed to save record..";
            string leadId = "";
            string[] data;
            leadId = SaleOrderMethods.Insert_VehicleSaleDetail(modelDetail, trancode);
            Session["STransCode"] = trancode;
            if (!string.IsNullOrEmpty(leadId))
            {
                data = leadId.Split(',');
                leadId = data[0].ToString();

                result = true;
                msg = "Successfully Added";
            }

            return Json(new { Success = result, Message = msg, LeadId = leadId }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Removed_SaleOrder(VehicleSaleMasterVM model)
        {
            bool result = false;
            string msg = "Failed to Delete record..";
            string leadId = "";
            string[] data;
            leadId = SaleOrderMethods.Remove_SaleOrder(model);
            if (!string.IsNullOrEmpty(leadId))
            {
                data = leadId.Split(',');
                leadId = data[0].ToString();

                result = true;
                msg = "Deleted Successfull";
            }

            return Json(new { Success = result, Message = msg, LeadId = leadId }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Export(string TransCode, string DealerCode)
        {
            //string DealerCode = "MCM01";
            //var TransCode = Session["TransCode"];

            DSReports data = new DSReports();
            ReportDocument RD = new ReportDocument();

            SqlParameter[] param =
            {
                new SqlParameter("@DealerCode",SqlDbType.Char),
                new SqlParameter("@TransCode",SqlDbType.VarChar)
            };

            param[0].Value = DealerCode;
            param[1].Value = TransCode;

            SqlDataReader rder = null;

            SysFunction sysFunc = new SysFunction();
            if (sysFunc.ExecuteSP("SP_Report_VehicleSaleDetail", param, ref rder))
            {
                data.SP_Report_VehicleSaleDetail.Load(rder);

            }
            RD.Load(Server.MapPath("~/Reports/Sale/SaleOrder.rpt"));

            RD.DataDefinition.FormulaFields["DealerDesc"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";
            RD.DataDefinition.FormulaFields["ReportTitle"].Text = "'Sale Order'";
            RD.DataDefinition.FormulaFields["Terminal"].Text = "'" + Request.ServerVariables["REMOTE_ADDR"].ToString() + "'";
            RD.DataDefinition.FormulaFields["UserId"].Text = "'" + Session["UserName"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["NTN"].Text = "'N.T.N # " + Session["DealerNTN"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["SalesTaxNo"].Text = "'Sales Tax No.  " + Session["DealerSaleTaxNo"].ToString() + " '";
            //RD.DataDefinition.FormulaFields["UserCell"].Text = "'" + GetStringValuesAgainstCodes("CusCode", , "CellNo", "Customer") + "'";
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
                return File(stream, "SaleOrderReport.pdf");
            }
            catch
            {
                throw;
            }


        }






        #endregion End    ------------------------------------------------------17/1/2019----------------------------------------------------------



        #region start ----- InstallmentSaleInvoice------------------------------21/1/2019-------------------------------------------------Fahad

        public ActionResult InstallmentSaleInvoice()
        {
            List<VehicleSaleDataVM> SOData = new List<VehicleSaleDataVM>();
            SOData = InstallmentSaleInvoiceMethods.Get_SaleDetailData();
            ViewBag.SOData = SOData;

            List<ISIDetailVM> ISIDetailData = new List<ISIDetailVM>();
            ISIDetailData = InstallmentSaleInvoiceMethods.Get_ISIDetailData();
            //Session["MTransCode"] = ISIDetailData.Select(x => x.TransCode).SingleOrDefault();
            ViewBag.ISIDetail = ISIDetailData;

            //List<ISIVehicleDataVM> VehicleAreaData = new List<ISIVehicleDataVM>();
            //string TransCode = Convert.ToString(Session["MTransCode"]);
            //string TransCodeISI = TransCode;
            //VehicleAreaData = InstallmentSaleInvoiceMethods.Get_ISIVehicleAreaData(TransCodeISI);
            //ViewBag.VehicleAreaData = VehicleAreaData;

            //List<ISIInstallmentVM> ISIInstallmentPlan = new List<ISIInstallmentVM>();
            //ISIInstallmentPlan = InstallmentSaleInvoiceMethods.Get_ISIInstallmentPlan();
            //ViewBag.ISIInstallmentPlan = ISIInstallmentPlan;



            var OnlyDate = DateTime.Now;
            ViewBag.CurrentDate = OnlyDate.ToString("dd/MM/yyyy");

            return View();

        }

        public ActionResult ISICode(string ISITransCode)
        {
            Session["ISITransCode"] = ISITransCode;
            var data = ISITransCode;



            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Insert_ISIMaster(VehicleSaleMasterVM model, AccountTransactionVM AccountModel)
        {

            bool result = false;
            string msg = "Failed to save record..";
            string leadId = "";
            string[] data;
            leadId = InstallmentSaleInvoiceMethods.Insert_ISIMasterData(model, AccountModel);
            if (!string.IsNullOrEmpty(leadId))
            {
                data = leadId.Split(',');
                leadId = data[0].ToString();

                result = true;
                msg = "Successfully Added";
            }

            return Json(new { Success = result, Message = msg, LeadId = leadId }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Insert_ISIVehicleData(VehicleSaleDetailVM VehicleModel, string trancode)
        {
            bool result = false;
            string msg = "Failed to save record..";
            string leadId = "";
            string[] data;
            leadId = InstallmentSaleInvoiceMethods.Insert_ISIVehicleData(VehicleModel, trancode);
            Session["STransCode"] = trancode;
            if (!string.IsNullOrEmpty(leadId))
            {
                data = leadId.Split(',');
                leadId = data[0].ToString();

                result = true;
                msg = "Successfully Added";
            }

            return Json(new { Success = result, Message = msg, LeadId = leadId }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ISIVehicleStock(string TransCode)
        {
            List<ISIVehicleDataVM> VehicleAreaData = new List<ISIVehicleDataVM>();
            VehicleAreaData = InstallmentSaleInvoiceMethods.Get_ISIVehicleAreaData(TransCode);
            var data = VehicleAreaData;

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ISIPlanData(string TransCode)
        {

            List<ISIInstallmentVM> ISIInstallmentPlan = new List<ISIInstallmentVM>();
            ISIInstallmentPlan = InstallmentSaleInvoiceMethods.Get_ISIInstallmentPlan(TransCode);
            var data = ISIInstallmentPlan;

            return Json(data, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult Remove_ISIMaster(VehicleSaleMasterVM model)
        {
            bool result = false;
            string msg = "Failed to delete record..";
            string leadId = "";
            string[] data;
            leadId = InstallmentSaleInvoiceMethods.Remove_ISIMasterData(model);
            if (!string.IsNullOrEmpty(leadId))
            {
                data = leadId.Split(',');
                leadId = data[0].ToString();

                result = true;
                msg = "Successfully Deleted";
            }

            return Json(new { Success = result, Message = msg, LeadId = leadId }, JsonRequestBehavior.AllowGet);
        }



        public ActionResult ISIExport(string TransCode, string DealerCode)
        {
            //string DealerCode = "MCM01";
            //var TransCode = Session["TransCode"];

            DSReports data = new DSReports();
            ReportDocument RD = new ReportDocument();

            SqlParameter[] param =
            {
                new SqlParameter("@DealerCode",SqlDbType.Char),
                new SqlParameter("@TransCode",SqlDbType.VarChar)
            };

            param[0].Value = DealerCode;
            param[1].Value = TransCode;

            SqlDataReader rder = null;

            SysFunction sysFunc = new SysFunction();
            if (sysFunc.ExecuteSP("Sp_Report_ISIData", param, ref rder))
            {
                data.Sp_Report_ISIData.Load(rder);
            }
            RD.Load(Server.MapPath("~/Reports/Sale/InstallmentSaleInvoiceRpt.rpt"));

            RD.DataDefinition.FormulaFields["DealerDesc"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";
            RD.DataDefinition.FormulaFields["ReportTitle"].Text = "'Installment Sale Invoice'";
            RD.DataDefinition.FormulaFields["Terminal"].Text = "'" + Request.ServerVariables["REMOTE_ADDR"].ToString() + "'";
            RD.DataDefinition.FormulaFields["UserId"].Text = "'" + Session["UserName"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["NTN"].Text = "'N.T.N # " + Session["DealerNTN"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["SalesTaxNo"].Text = "'Sales Tax No.  " + Session["DealerSaleTaxNo"].ToString() + " '";
            //rpt.DataDefinition.FormulaFields["UserCell"].Text = "'" + GetStringValuesAgainstCodes("CusCode", , "CellNo", "Customer") + "'";
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
                return File(stream, "InstallmentSaleInvoiceReport.pdf");
            }
            catch
            {
                throw;
            }


        }

        #endregion

        public ActionResult DailyExpenditure()
        {

            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("Login", "Home");
            }
            dealerCode = Session["DealerCode"].ToString();

            var OnlyDate = DateTime.Now;
            ViewBag.CurrentDate = OnlyDate.ToString("dd/MM/yyyy");

            ViewBag.ExpData = BMS.DailyExpenseMasters.Where(x => x.DealerCode == dealerCode & x.DelFlag == "N").ToList();
            ViewBag.ReceiptHead = BMS.ExpenseHeads.Where(x => x.DealerCode == dealerCode).ToList();
            ViewBag.DealerEmp = BMS.DealerEmps.Where(x => x.DealerCode == dealerCode).ToList();
            ViewBag.PaymentMode = BMS.PaymentModes.Where(x => x.DealerCode == "COMON").ToList();
            ViewBag.CashBankSetup = BMS.CashBankSetups.Where(x => x.CompanyCode == dealerCode).ToList();
            ViewBag.Bank = BMS.Banks.Where(x => x.DealerCode == "COMON").ToList();

            return View();
        }

        [HttpPost]
        public JsonResult DExpense(DailyExpenseDetail[] ItemTable,string ExpInvDate,string InsDate)
        {
            Session["Data"] = ItemTable;
            Session["ExpInvDate"] = ExpInvDate;
            Session["InsDate"] = InsDate;
            return Json(ItemTable, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DailyExpenditure(DailyExpenseMaster obj)
        {
            List<DailyExpenseMaster> ListMas = new List<DailyExpenseMaster>();

            
            DailyExpenseMaster ExpMas = new DailyExpenseMaster();

            ViewBag.Data = Session["Data"];
            string ExpInvDate = (string) Session["ExpInvDate"];
            string InsDate = (string) Session["InsDate"];
            dealerCode = Session["DealerCode"].ToString();

            var chk = BMS.DailyExpenseMasters.Where(i => i.ExpInvNo == obj.ExpInvNo & i.DealerCode == dealerCode).FirstOrDefault();
            if (chk == null)
            {
                string date = DateTime.Now.ToString("yy");
                string year = date + "/";
                var sp = BMS.sp_ExpInvNo_DailyExpenseMaster().FirstOrDefault();
                string autogen = (Convert.ToInt32(sp) + 1).ToString();
                while (autogen.Length != 5)
                {
                    autogen = '0' + autogen;
                }
                string ExpInvNo = year + autogen;
                string UpdUser = (string)Session["UserName"];
                string UpdTerm = (string)Session["UserName"];

                ExpMas.ExpInvNo = ExpInvNo;
                ExpMas.ExpInvDate = Convert.ToDateTime(sysfun.SaveDate(ExpInvDate));
                ExpMas.ExpTotalInvAmount = obj.ExpTotalInvAmount;
                ExpMas.InsAmount = obj.InsAmount;

                if(InsDate == "")
                {
                    ExpMas.InsDate = obj.InsDate;
                }
                else
                {
                    ExpMas.InsDate = Convert.ToDateTime(sysfun.SaveDate(InsDate));
                }
                
                ExpMas.InsNo = obj.InsNo;
                ExpMas.PayModeCode = obj.PayModeCode;
                ExpMas.PostFlag = "N";
                ExpMas.Remarks = obj.Remarks;
                ExpMas.UpdDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                ExpMas.UpdTime = Convert.ToDateTime(DateTime.Now.ToShortTimeString());
                ExpMas.UpdUser = Convert.ToString(AuthBase.UserId);
                ExpMas.UpdTerm = General.CurrentIP;
                ExpMas.VoucherFlag = "N";
                ExpMas.VoucherNo = obj.VoucherNo;
                ExpMas.BankCode = obj.BankCode;
                ExpMas.Branch = obj.Branch;
                ExpMas.CBCode = obj.CBCode;
                ExpMas.DealerCode = Session["DealerCode"].ToString();
                ExpMas.CSCode = obj.CSCode;
                ExpMas.DelFlag = "N";

                BMS.DailyExpenseMasters.Add(ExpMas);
                //BMS.SaveChanges();


                if (ViewBag.Data != null)
                {
                    foreach (DailyExpenseDetail a in ViewBag.Data)
                    {
                        DailyExpenseDetail ExpDet = new DailyExpenseDetail();
                        ExpDet.ExpInvNo = ExpInvNo;
                        ExpDet.ExpFor = a.ExpFor;
                        ExpDet.ExpPayTo = a.ExpPayTo;
                        ExpDet.ExpRemarks = a.ExpRemarks;
                        ExpDet.Amount = a.Amount;
                        ExpDet.DealerCode = Session["DealerCode"].ToString();
                        ExpDet.AccountCode = a.AccountCode;
                        BMS.DailyExpenseDetails.Add(ExpDet);
                        BMS.SaveChanges();

                    }
                }
                TempData["Success"] = "Save Succesfully....!";
            }
            else
            {
                // delete code 

                using (sngonclo_BMSEntities bms = new sngonclo_BMSEntities())
                {
                    var query = (from c in BMS.DailyExpenseMasters
                                 where c.ExpInvNo == obj.ExpInvNo
                                 select c).FirstOrDefault();

                    //DailyExpenseDetail found = (from c in BMS.DailyExpenseDetails
                    //             where c.ExpInvNo == obj.ExpInvNo
                    //             select c).Select();

                BMS.DailyExpenseMasters.Remove(query);
                BMS.DailyExpenseDetails.RemoveRange(BMS.DailyExpenseDetails.Where(c => c.ExpInvNo == obj.ExpInvNo & c.DealerCode == dealerCode));
                BMS.SaveChanges();
                }

            string UpdUser = (string)Session["UserName"];
                string UpdTerm = (string)Session["UserName"];
                string ExpInvNo = obj.ExpInvNo;
                ExpMas.ExpInvNo = ExpInvNo;
                ExpMas.ExpInvDate = Convert.ToDateTime(sysfun.SaveDate(ExpInvDate));
                ExpMas.ExpTotalInvAmount = obj.ExpTotalInvAmount;
                ExpMas.InsAmount = obj.InsAmount;
                ExpMas.InsDate = Convert.ToDateTime(sysfun.SaveDate(InsDate));
                ExpMas.InsNo = obj.InsNo;
                ExpMas.PayModeCode = obj.PayModeCode;
                ExpMas.PostFlag = obj.PostFlag;
                ExpMas.Remarks = obj.Remarks;
                ExpMas.UpdDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                ExpMas.UpdTime = Convert.ToDateTime(DateTime.Now.ToShortTimeString());
                ExpMas.UpdUser = Convert.ToString(AuthBase.UserId);
                ExpMas.UpdTerm = General.CurrentIP;
                ExpMas.VoucherFlag = "N";
                ExpMas.VoucherNo = obj.VoucherNo;
                ExpMas.BankCode = obj.BankCode;
                ExpMas.Branch = obj.Branch;
                ExpMas.CBCode = obj.CBCode;
                ExpMas.DealerCode = Session["DealerCode"].ToString();
                ExpMas.CSCode = obj.CSCode;
                ExpMas.DelFlag = "N";

                //ListMas.Add(ExpMas);
                BMS.DailyExpenseMasters.Add(ExpMas);
                BMS.SaveChanges();


                if (ViewBag.Data != null)
                {
                    foreach (DailyExpenseDetail a in ViewBag.Data)
                    {
                        DailyExpenseDetail ExpDet = new DailyExpenseDetail();
                        ExpDet.ExpInvNo = obj.ExpInvNo;
                        ExpDet.ExpFor = a.ExpFor;
                        ExpDet.ExpPayTo = a.ExpPayTo;
                        ExpDet.ExpRemarks = a.ExpRemarks;
                        ExpDet.Amount = a.Amount;
                        ExpDet.DealerCode = Session["DealerCode"].ToString();
                        ExpDet.AccountCode = a.AccountCode;
                        BMS.DailyExpenseDetails.Add(ExpDet);
                        BMS.SaveChanges();
                    }
                    
                }

                TempData["Success"] = "Update Succesfully....!";
            }

            return RedirectToAction("DailyExpenditure", "SaleOrder");
        }

        //public ActionResult DailyExpenseNoLookup(string ExpInvNo) /* UZair*/
        //{
        //    return Json(new { SQMList = SQMList, SQDList = SQDList }, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult DailyExpenseNoLookup(string ExpInvNo)
        {
            string dealerCode = Session["DealerCode"].ToString();

            var SQMaster = BMS.DailyExpenseMasters.Where(d => d.DealerCode == dealerCode && d.ExpInvNo == ExpInvNo).ToList();
            List<DaliyExpenseMasterVM> DEMaster = new List<DaliyExpenseMasterVM>();
            foreach (var d in SQMaster)
            {
                DaliyExpenseMasterVM SQMData = new DaliyExpenseMasterVM();
                SQMData.ExpInvNo = d.ExpInvNo;
                SQMData.ExpInvDate = string.Format("{0:dd/MM/yyyy}", d.ExpInvDate);
                SQMData.Remarks = d.Remarks;
                SQMData.CSCode = d.CSCode;
                SQMData.CSDesc = (from a in BMS.CostCenterSetups where a.CompanyCode == dealerCode && a.CSCode == d.CSCode select a.CSName).FirstOrDefault();
                SQMData.CBCode = d.CBCode;
                SQMData.CBDesc = (from a in BMS.CashBankSetups where a.CompanyCode == dealerCode && a.CBCode == d.CBCode select a.CBDesc).FirstOrDefault();
                SQMData.PayModeCode = d.PayModeCode;
                SQMData.PayModeDesc = (from a in BMS.PaymentModes where a.PayModeCode == d.PayModeCode select a.PayModeDesc).FirstOrDefault();
                SQMData.InsDate = string.Format("{0:dd/MM/yyyy}", d.InsDate);
                SQMData.InsNo = d.InsNo;
                SQMData.InsAmount = Convert.ToDouble(d.InsAmount);
                SQMData.BankCode = d.BankCode;
                SQMData.BankDesc = (from a in BMS.Banks where a.BankCode == d.BankCode select a.BankDesc).FirstOrDefault();
                SQMData.Branch = d.Branch;
                SQMData.ExpTotalInvAmount = Convert.ToDouble(d.ExpTotalInvAmount);
                DEMaster.Add(SQMData);
            }
            var SQDetail = BMS.DailyExpenseDetails.Where(d => d.DealerCode == dealerCode && d.ExpInvNo == ExpInvNo).ToList();
            List<DailyExpenseDetailVM> DEDetail = new List<DailyExpenseDetailVM>();
            foreach (var item in SQDetail)
            {
                DailyExpenseDetailVM SOD = new DailyExpenseDetailVM();
                SOD.ExpFor = item.ExpFor;
                SOD.ExpForDesc = (from a in BMS.ExpenseHeads where a.DealerCode == dealerCode && a.ECode == item.ExpFor select a.EDesc).FirstOrDefault();
                SOD.ExpPayTo = item.ExpPayTo;
                SOD.ExpPayToDesc = (from a in BMS.DealerEmps where a.DealerCode == dealerCode && a.EmpCode == item.ExpPayTo select a.EmpName).FirstOrDefault();
                SOD.ExpRemarks = item.ExpRemarks;
                SOD.Amount = Convert.ToDouble(item.Amount);
                SOD.AccountCode = item.AccountCode;
                DEDetail.Add(SOD);
            }
            return Json(new { SQMList = DEMaster, SQDList = DEDetail }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult DailyExpenseMasterDelate(string ExpInvNo)
        {
            dealerCode = Session["DealerCode"].ToString();

            var foundMas = (from c in BMS.DailyExpenseMasters
                            where c.ExpInvNo == ExpInvNo & c.DealerCode == dealerCode
                            select c).FirstOrDefault();

            BMS.DailyExpenseDetails.RemoveRange(BMS.DailyExpenseDetails.Where(c => c.ExpInvNo == ExpInvNo & c.DealerCode == dealerCode));

            //var foundDet = (from c in BMS.DailyExpenseDetails
            //                where c.ExpInvNo == ExpInvNo & c.DealerCode == dealerCode
            //                select c).FirstOrDefault();

            foundMas.DelFlag = "Y";

            //BMS.DailyExpenseMasters.Remove(foundMas);
            //BMS.DailyExpenseDetails.Remove(foundDet);
            BMS.SaveChanges();

            return Json(ExpInvNo, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DailyExpenseReport(string EnquiryId, string DealerCode)
        {
            DSReports data = new DSReports();
            ReportDocument RD = new ReportDocument();

            SqlParameter[] param =
        {
                new SqlParameter("@ExpInvNo",SqlDbType.Char),
                new SqlParameter("@CompanyCode",SqlDbType.VarChar)

            };

            param[0].Value = EnquiryId;
            param[1].Value = DealerCode;

            SqlDataReader rder = null;

            SysFunction sysFunc = new SysFunction();
            if (sysFunc.ExecuteSP("Sp_DailyExpenditure_Reports_C", param, ref rder))
            {
                data.Sp_DailyExpenditure_Reports_C.Load(rder);

            }

            RD.Load(Server.MapPath("~/Reports/Sale/DailyExpenditure.rpt"));

            RD.DataDefinition.FormulaFields["DealerDesc"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";
            RD.DataDefinition.FormulaFields["ReportTitle"].Text = "'Daily Expenditure'";
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

        //#region start ----Old CashSaleInvoice----------------------------------29/1/2019----------------------------------------------------------------

        //public ActionResult CashSaleInvoice()
        //{

        //    List<VehicleSaleDataVM> SOData = new List<VehicleSaleDataVM>();
        //    SOData = CashSaleInvoiceMethods.Get_SaleDetailData();
        //    ViewBag.SOData = SOData;

        //    List<ISIDetailVM> CSIDetailData = new List<ISIDetailVM>();
        //    CSIDetailData = CashSaleInvoiceMethods.Get_CSIDetailData();
        //    ViewBag.CSIDetail = CSIDetailData;

        //    var OnlyDate = DateTime.Now;
        //    ViewBag.CurrentDate = OnlyDate.ToString("dd/MM/yyyy");

        //    return View();
        //}

        //public ActionResult CSIGetGridData(string TransCode)
        //{
        //    Session["CSITransCode"] = TransCode;

        //    List<CSIVehicleGridDataVM> SaleDetailGrid = new List<CSIVehicleGridDataVM>();
        //    SaleDetailGrid = CashSaleInvoiceMethods.Get_CSIGridData(TransCode);
        //    var data = SaleDetailGrid;



        //    return Json(data, JsonRequestBehavior.AllowGet);
        //}

        //[HttpPost]
        //public JsonResult Insert_CSIMaster(VehicleSaleMasterVM model, AccountTransactionVM AccountModel, ReceiptMasterVM ReceiptModel, string ProdDesc, string EngineNo, string ChassisNo)
        //{

        //    bool result = false;
        //    string msg = "Failed to save record..";
        //    string leadId = "";
        //    string[] data;
        //    leadId = CashSaleInvoiceMethods.Insert_CSIMasterData(model, AccountModel, ReceiptModel, ProdDesc, EngineNo, ChassisNo);
        //    if (!string.IsNullOrEmpty(leadId))
        //    {
        //        data = leadId.Split(',');
        //        leadId = data[0].ToString();

        //        result = true;
        //        msg = "Successfully Added";
        //    }

        //    return Json(new { Success = result, Message = msg, LeadId = leadId }, JsonRequestBehavior.AllowGet);
        //}

        //public JsonResult Insert_CSIGrid(VehicleSaleDetailVM[] modelDetail, string trancode, string CusCode, string EmpCode, string EmpName, string ProdDesc)
        //{
        //    bool result = false;
        //    string msg = "Failed to save record..";
        //    string leadId = "";
        //    string[] data;
        //    leadId = CashSaleInvoiceMethods.Insert_CSIGridData(modelDetail, trancode, CusCode, EmpCode, EmpName, ProdDesc);
        //    Session["STransCode"] = trancode;
        //    if (!string.IsNullOrEmpty(leadId))
        //    {
        //        data = leadId.Split(',');
        //        leadId = data[0].ToString();

        //        result = true;
        //        msg = "Successfully Added";
        //    }

        //    return Json(new { Success = result, Message = msg, LeadId = leadId }, JsonRequestBehavior.AllowGet);
        //}

        //[HttpPost]
        //public JsonResult Remove_CSIRecord(VehicleSaleMasterVM model)
        //{
        //    bool result = false;
        //    string msg = "Failed to delete record..";
        //    string leadId = "";
        //    string[] data;
        //    leadId = CashSaleInvoiceMethods.Remove_CSIData(model);
        //    if (!string.IsNullOrEmpty(leadId))
        //    {
        //        data = leadId.Split(',');
        //        leadId = data[0].ToString();

        //        result = true;
        //        msg = "Successfully Deleted";
        //    }

        //    return Json(new { Success = result, Message = msg, LeadId = leadId }, JsonRequestBehavior.AllowGet);
        //}

        //public ActionResult CSIExport(string TransCode, string DealerCode)
        //{

        //    DSReports data = new DSReports();
        //    ReportDocument RD = new ReportDocument();

        //    SqlParameter[] param =
        //    {
        //        new SqlParameter("@DealerCode",SqlDbType.Char),
        //        new SqlParameter("@TransCode",SqlDbType.VarChar)
        //    };

        //    param[0].Value = DealerCode;
        //    param[1].Value = TransCode;

        //    SqlDataReader rder = null;

        //    SysFunction sysFunc = new SysFunction();
        //    if (sysFunc.ExecuteSP("Sp_Report_CSIData", param, ref rder))
        //    {
        //        data.Sp_Report_CSIData.Load(rder);

        //    }
        //    RD.Load(Server.MapPath("~/Reports/Sale/CashSaleInvoice.rpt"));

        //    RD.DataDefinition.FormulaFields["DealerDesc"].Text = "'" + Session["DealerDesc"].ToString() + "'";
        //    RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
        //    RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
        //    RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";
        //    RD.DataDefinition.FormulaFields["ReportTitle"].Text = "'Cash Sale Invoice'";
        //    RD.DataDefinition.FormulaFields["Terminal"].Text = "'" + Request.ServerVariables["REMOTE_ADDR"].ToString() + "'";
        //    RD.DataDefinition.FormulaFields["UserId"].Text = "'" + Session["UserName"].ToString() + "'";
        //    //RD.DataDefinition.FormulaFields["NTN"].Text = "'N.T.N # " + Session["DealerNTN"].ToString() + "'";
        //    //RD.DataDefinition.FormulaFields["SalesTaxNo"].Text = "'Sales Tax No.  " + Session["DealerSaleTaxNo"].ToString() + " '";
        //    //rpt.DataDefinition.FormulaFields["UserCell"].Text = "'" + GetStringValuesAgainstCodes("CusCode", , "CellNo", "Customer") + "'";
        //    RD.DataDefinition.FormulaFields["CompanyName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
        //    RD.DataDefinition.FormulaFields["Pic"].Text = "'" + Server.MapPath("~") + Session["Logo"] + "'";


        //    RD.Database.Tables[0].SetDataSource(data);

        //    Response.Buffer = false;
        //    Response.ClearContent();
        //    Response.ClearHeaders();

        //    try
        //    {
        //        Stream stream = RD.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
        //        stream.Seek(0, SeekOrigin.Begin);
        //        return File(stream, "CashSaleInvoiceReport.pdf");
        //    }
        //    catch
        //    {
        //        throw;
        //    }


        //}

        //#endregion
        #region start ----CashSaleInvoice----------------------------------29/1/2019----------------------------------------------------------------

        public ActionResult CashSaleInvoice()
        {
            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("NewLogin", "Home");
            }
            String DealerCode = Session["DealerCode"].ToString();

            List<VehicleSaleDataVM> SOData = new List<VehicleSaleDataVM>();
            SOData = CashSaleInvoiceMethods.Get_SaleDetailData(DealerCode);
            ViewBag.SOData = SOData;
            List<SelectListItem> ddlAssignTo = new List<SelectListItem>();
            ddlAssignTo = DeliveryOrderMethods.GetDealerEmployee(dealerCode);
            ViewBag.AssignTo = ddlAssignTo;
            List<CustomerVM> Customer = new List<CustomerVM>();
            Customer = CashSaleInvoiceMethods.Get_CustomerData(DealerCode);
            ViewBag.Customer = Customer;
            List<VehicleSaleMasterVM> CSIDetailData = new List<VehicleSaleMasterVM>();
            CSIDetailData = CashSaleInvoiceMethods.Get_CSIDetailData(DealerCode);
            ViewBag.CSIDetail = CSIDetailData;
            List<SelectListItem> ddlVersion = new List<SelectListItem>();
            ddlVersion = GeneralMethods.GetDataFromSp("Select_Version");
            ViewBag.ddlVersion = ddlVersion;
            List<SelectListItem> ddlColor = new List<SelectListItem>();
            ddlColor = GeneralMethods.GetColor(Session["VehicleCategory"].ToString());
            ViewBag.Color = ddlColor;
            var OnlyDate = DateTime.Now;
            ViewBag.CurrentDate = OnlyDate.ToString("dd/MM/yyyy");



            //voucher
            dealerCode = Session["DealerCode"].ToString();
            List<SelectListItem> ddljournalNo = new List<SelectListItem>();
            ddljournalNo = VoucherMethods.initializeDDLs(Session["DealerCode"].ToString());
            ViewBag.Journal = ddljournalNo;
            ViewBag.VoucherNo = VoucherMethods.GetNewVoucherNo("GVouMaster", "VouchNo", 3, Session["DealerCode"].ToString());



            return View();
        }
        public ActionResult VoucherCSI()
        {



            return View();
        }
        //Voucher

        [HttpGet]
        public JsonResult Select_CSIVoucherNo(string EnquiryId, string DealerCode)
        {
            string data = "";
            bool result = false;
            DealerCode = Session["DealerCode"].ToString();
            data = GeneralMethods.GetVoucherNo(EnquiryId, "SP_Select_CSIVouchNo", DealerCode);
            Session["VoucherNo"] = data;
            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }
        //voucher end
        public ActionResult CSIGetGridData(string TransCode)
        {

            String DealerCode = Session["DealerCode"].ToString();
            Session["CSITransCode"] = TransCode;

            List<CSIVehicleGridDataVM> SaleDetailGrid = new List<CSIVehicleGridDataVM>();
            SaleDetailGrid = CashSaleInvoiceMethods.Get_CSIGridData(TransCode, DealerCode);
            var data = SaleDetailGrid;


            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Insert_CSIMaster(VehicleSaleMasterVM model, AccountTransactionVM AccountModel, ReceiptMasterVM ReceiptModel, string ProdDesc, string EngineNo, string ChassisNo)
        {
            String DealerCode = Session["DealerCode"].ToString();
            bool result = false;
            string msg = "Failed to save record..";
            string leadId = "";
            string[] data;
            result = CashSaleInvoiceMethods.Insert_CSIMasterData(model, AccountModel, ReceiptModel, ProdDesc, EngineNo, ChassisNo, DealerCode, ref msg);
            if (result)
            {

                // leadId = data[0].ToString();

                result = true;
                msg = "Successfully Added";
            }

            return Json(new { Success = result, Message = msg, LeadId = leadId }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Insert_CSIGrid(VehicleSaleDetailVM[] modelDetail, string trancode, string CusCode, string EmpCode, string EmpName, string ProdDesc)
        {
            String DealerCode = Session["DealerCode"].ToString();
            bool result = false;
            string msg = "Failed to save record..";
            string leadId = "";
            string[] data;
            result = CashSaleInvoiceMethods.Insert_CSIGridData(modelDetail, trancode, CusCode, EmpCode, EmpName, ProdDesc, DealerCode, ref msg);
            Session["STransCode"] = trancode;
            //if (!string.IsNullOrEmpty(leadId))
            //{
            //    data = leadId.Split(',');
            //    leadId = data[0].ToString();

            //    result = true;
            //    msg = "Successfully Added";
            //}
            if (result)
            {
                //data = leadId.Split(',');
                //leadId = data[0].ToString();

                //result = true;
                msg = "Successfully Added";
            }

            return Json(new { Success = result, Message = msg, LeadId = leadId }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Remove_CSIRecord(VehicleSaleMasterVM model)
        {
            String DealerCode = Session["DealerCode"].ToString();
            bool result = false;
            string msg = "Failed to delete record..";
            string leadId = "";
            string[] data;
            leadId = CashSaleInvoiceMethods.Remove_CSIData(model, DealerCode, ref msg);
            if (!string.IsNullOrEmpty(leadId))
            {
                data = leadId.Split(',');
                leadId = data[0].ToString();

                result = true;
                msg = "Successfully Deleted";
            }

            return Json(new { Success = result, Message = msg, LeadId = leadId }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Select_DeliveryCheckList(string EnquiryId)
        {
            string data = "";
            bool result = false;
            data = DeliveryOrderMethods.Get_DeliveryChkList(EnquiryId, Session["DealerCode"].ToString(), "CSI");

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
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
        public ActionResult CSIExport(string TransCode, string DealerCode)
        {

            DSReports data = new DSReports();

            ReportDocument RD = new ReportDocument();

            SqlParameter[] param =
            {
                new SqlParameter("@DealerCode",SqlDbType.Char),
                new SqlParameter("@TransCode",SqlDbType.VarChar)
            };

            param[0].Value = DealerCode;
            param[1].Value = TransCode;

            SqlDataReader rder = null;

            SysFunction sysFunc = new SysFunction();
            if (sysFunc.ExecuteSP("Sp_Report_CSIData", param, ref rder))
            {
                data.EnforceConstraints = false;
                data.Sp_Report_CSIData.Load(rder);

            }
            RD.Load(Server.MapPath("~/Reports/Sale/CashSaleInvoice.rpt"));

            RD.DataDefinition.FormulaFields["DealerDesc"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";
            RD.DataDefinition.FormulaFields["ReportTitle"].Text = "'Sale Invoice'";
            RD.DataDefinition.FormulaFields["Terminal"].Text = "'" + Request.ServerVariables["REMOTE_ADDR"].ToString() + "'";
            RD.DataDefinition.FormulaFields["UserId"].Text = "'" + Session["UserName"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["NTN"].Text = "'N.T.N # " + Session["DealerNTN"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["SalesTaxNo"].Text = "'Sales Tax No.  " + Session["DealerSaleTaxNo"].ToString() + " '";
            //rpt.DataDefinition.FormulaFields["UserCell"].Text = "'" + GetStringValuesAgainstCodes("CusCode", , "CellNo", "Customer") + "'";
            RD.DataDefinition.FormulaFields["CompanyName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["Pic"].Text = "'" + Server.MapPath("~") + Session["Logo"] + "'";
            RD.DataDefinition.FormulaFields["Pic"].Text = "'" + Server.MapPath("~") + Session["Logo"] + "'"; ;

            RD.Database.Tables[0].SetDataSource(data);

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            try
            {
                Stream stream = RD.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "CashSaleInvoiceReport.pdf");
            }
            catch
            {
                throw;
            }


        }

        public JsonResult Insert_VehChkList(string strCheckedValues)
        {
            bool result = false;
            string msg = "Failed to save record..";

            result = CashSaleInvoiceMethods.Insert_VehChkList(strCheckedValues, Session["DealerCode"].ToString(), ref msg);

            if (result)
            {
                msg = "Successfully Added";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region -----------Credit Sale Invoice -------------------------------------31/01/2019------------------------------------------------------------

        public ActionResult CreditSaleInvoice()
        {
            List<VehicleSaleDataVM> SOData = new List<VehicleSaleDataVM>();
            SOData = CreditSaleInvoiceMethods.Get_SaleDetailData();
            ViewBag.SOData = SOData;

            List<CrdSIVM> CrdSIDetailData = new List<CrdSIVM>();
            CrdSIDetailData = CreditSaleInvoiceMethods.Get_CrdSIDetailData();
            ViewBag.CrdSIDetail = CrdSIDetailData;

            var OnlyDate = DateTime.Now;
            ViewBag.CurrentDate = OnlyDate.ToString("dd/MM/yyyy");

            return View();
        }

        public ActionResult CrdSIGetGridData(string TransCode)
        {
            Session["CrdSITransCode"] = TransCode;

            List<CSIVehicleGridDataVM> SaleDetailGrid = new List<CSIVehicleGridDataVM>();
            SaleDetailGrid = CreditSaleInvoiceMethods.Get_CrdSIGridData(TransCode);
            var data = SaleDetailGrid;

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CrdSIVehicleStock(string TransCode)
        {
            List<ISIVehicleDataVM> VehicleAreaData = new List<ISIVehicleDataVM>();
            VehicleAreaData = InstallmentSaleInvoiceMethods.Get_ISIVehicleAreaData(TransCode);
            var data = VehicleAreaData;

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CrdSIExport(string TransCode, string DealerCode)
        {

            DSReports data = new DSReports();
            ReportDocument RD = new ReportDocument();

            SqlParameter[] param =
            {
                new SqlParameter("@DealerCode",SqlDbType.Char),
                new SqlParameter("@TransCode",SqlDbType.VarChar)
            };

            param[0].Value = DealerCode;
            param[1].Value = TransCode;

            SqlDataReader rder = null;

            SysFunction sysFunc = new SysFunction();
            if (sysFunc.ExecuteSP("Sp_Report_CrdSIData", param, ref rder))
            {
                data.Sp_Report_CrdSIData.Load(rder);
            }
            RD.Load(Server.MapPath("~/Reports/Sale/CreditSaleInvoice.rpt"));

            RD.DataDefinition.FormulaFields["DealerDesc"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";
            RD.DataDefinition.FormulaFields["ReportTitle"].Text = "'Credit Sale Invoice'";
            RD.DataDefinition.FormulaFields["Terminal"].Text = "'" + Request.ServerVariables["REMOTE_ADDR"].ToString() + "'";
            RD.DataDefinition.FormulaFields["UserId"].Text = "'" + Session["UserName"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["NTN"].Text = "'N.T.N # " + Session["DealerNTN"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["SalesTaxNo"].Text = "'Sales Tax No.  " + Session["DealerSaleTaxNo"].ToString() + " '";
            //rpt.DataDefinition.FormulaFields["UserCell"].Text = "'" + GetStringValuesAgainstCodes("CusCode", , "CellNo", "Customer") + "'";
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
                return File(stream, "CreditSaleInvoiceReport.pdf");
            }
            catch
            {
                throw;
            }


        }

        [HttpPost]
        public JsonResult Insert_CrdSIMaster(VehicleSaleMasterVM model, AccountTransactionVM AccountModel)
        {

            bool result = false;
            string msg = "Failed to save record..";
            string leadId = "";
            string[] data;
            leadId = CreditSaleInvoiceMethods.Insert_CrdSIMasterData(model, AccountModel);
            if (!string.IsNullOrEmpty(leadId))
            {
                data = leadId.Split(',');
                leadId = data[0].ToString();

                result = true;
                msg = "Successfully Added";
            }

            return Json(new { Success = result, Message = msg, LeadId = leadId }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Insert_CrdSIGrid(VehicleSaleDetailVM[] modelDetail, string trancode)
        {
            bool result = false;
            string msg = "Failed to save record..";
            string leadId = "";
            string[] data;
            leadId = CreditSaleInvoiceMethods.Insert_CrdSIGridData(modelDetail, trancode);
            Session["STransCode"] = trancode;
            if (!string.IsNullOrEmpty(leadId))
            {
                data = leadId.Split(',');
                leadId = data[0].ToString();

                result = true;
                msg = "Successfully Added";
            }

            return Json(new { Success = result, Message = msg, LeadId = leadId }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Remove_CrdSIRecord(VehicleSaleMasterVM model)
        {
            bool result = false;
            string msg = "Failed to delete record..";
            string leadId = "";
            string[] data;
            leadId = CreditSaleInvoiceMethods.Remove_CrdSIData(model);
            if (!string.IsNullOrEmpty(leadId))
            {
                data = leadId.Split(',');
                leadId = data[0].ToString();

                result = true;
                msg = "Successfully Deleted";
            }

            return Json(new { Success = result, Message = msg, LeadId = leadId }, JsonRequestBehavior.AllowGet);
        }

        #endregion
        #region
        /// <summary>
        /// //Payment Receipt 
        /// </summary>
        /// <returns></returns>
        public ActionResult PaymentReceipt()
        {
            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("Login", "Home");
            }
            dealerCode = Session["DealerCode"].ToString();
            List<SelectListItem> ddlPayMode = new List<SelectListItem>();
            ddlPayMode = GeneralMethods.GetDataFromSPWithDealerCode("Select_PaymentMode", dealerCode);
            ViewBag.PayMode = ddlPayMode;

            List<SelectListItem> ddlTaxType = new List<SelectListItem>();
            ddlTaxType = GeneralMethods.GetDataFromSP("SP_Select_TaxType");
            ViewBag.TaxType = ddlTaxType;

            List<SelectListItem> ddlBank = new List<SelectListItem>();
            ddlBank = GeneralMethods.GetBank();
            ViewBag.Bank = ddlBank;

            List<SelectListItem> ddlCity = new List<SelectListItem>();
            ddlCity = GeneralMethods.GetCity();
            ViewBag.City = ddlCity;

            return View();
        }
        [HttpGet]
        public JsonResult GetReceiptNo_Model(string EnquiryId)
        {
            string data;
            bool result = false;

            data = PaymentReceiptMethods.GetReceiptNo_Model(Session["DealerCode"].ToString());

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public JsonResult GetReceiptNo_Master(string EnquiryId)
        {
            string data;
            bool result = false;

            data = PaymentReceiptMethods.GetReceiptNo_Master(EnquiryId,Session["DealerCode"].ToString());

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public JsonResult GetReceiptNo_Detail(string EnquiryId)
        {
            string data;
            bool result = false;

            data = PaymentReceiptMethods.GetReceiptNo_Detail(EnquiryId, Session["DealerCode"].ToString());

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public JsonResult GetReceiptNo_TaxDetail(string EnquiryId)
        {
            string data;
            bool result = false;

            data = PaymentReceiptMethods.GetReceiptNo_Tax(EnquiryId, Session["DealerCode"].ToString());

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public JsonResult Select_CSIPendingPay(string EnquiryId)
        {
            string data;
            bool result = false;

            data = PaymentReceiptMethods.GetPendingCSIPay(EnquiryId,Session["DealerCode"].ToString());

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public JsonResult Select_ACSPendingPay(string EnquiryId)
        {
            string data;
            bool result = false;

            data = PaymentReceiptMethods.GetPendingACSPay(EnquiryId, Session["DealerCode"].ToString());

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public JsonResult GetAdvanceReceipt(string EnquiryId)
        {
            string data;
            bool result = false;

            data = PaymentReceiptMethods.GetAdvanceReceipt(EnquiryId, Session["DealerCode"].ToString());

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult Insert_PaymentReceiptMaster(PaymentReceiptVM dto)
        {
            bool result = true;

            string msg = "Failed to save record..";

            result = PaymentReceiptMethods.Insert_PaymentReceiptMaster(dto,ref msg);

            if (result)
            {
                msg = "Successfully Added";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Insert_PaymentReceiptDetail(List<PaymentReceiptDetailVM> objects)
        {
            bool result = false;

            string msg = "Failed to save record..";

            result = PaymentReceiptMethods.Insert_PaymentDetail(objects, Session["DealerCode"].ToString(),ref msg);

            if (result)
            {
                msg = "Successfully Added";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Insert_PaymentReceiptTax(List<PaymentReceiptTaxDetailVM> objectTax)
        {
            bool result = false;

            string msg = "Failed to save record..";

            result = PaymentReceiptMethods.Insert_PaymentTaxDetail(objectTax, Session["DealerCode"].ToString(), ref msg);

            if (result)
            {
                msg = "Successfully Added";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }




        public ActionResult PRExport(string ReceiptNo, string DealerCode)
        {

            DSReports data = new DSReports();
            ReportDocument RD = new ReportDocument();
            DataTable dt;
            string sql;
            SqlParameter[] param =
            {
                new SqlParameter("@DealerCode",SqlDbType.Char),
                new SqlParameter("@ReceiptNo",SqlDbType.Char)
            };

            param[0].Value = DealerCode;
            param[1].Value = ReceiptNo;

            SqlDataReader rder = null;

            SysFunction sysFunc = new SysFunction();
            if (sysFunc.ExecuteSP("sp_PaymentReceipt_Print", param, ref rder))
            {
                data.sp_PaymentReceipt_Print.Load(rder);
                sql = "exec sp_W2_PaymentReceiptTaxDetail_Select '" + Session["DealerCode"].ToString() + "','" + ReceiptNo + "'";
                dt = sysfun.GetData(sql);
                data.sp_W2_PaymentReceiptTaxDetail_Select.Load(dt.CreateDataReader());
            }
            RD.Load(Server.MapPath("~/Reports/rptPaymentRecPrint.rpt"));

            RD.OpenSubreport(Server.MapPath("~/ Reports/ rptPaymentReceiptTaxDetail.rpt"));
           // RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";
        //    RD.DataDefinition.FormulaFields["DealerFax"].Text = "'" + Session["DealerFax"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerDesc"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
            RD.DataDefinition.FormulaFields["UserID"].Text = "'" + Session["UserID"].ToString() + "'";
            RD.DataDefinition.FormulaFields["Terminal"].Text = "'" + Environment.MachineName + "'";
            RD.DataDefinition.FormulaFields["CompanyName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["DealershipName"].Text = "'Authorised " + Session["ParentDesc"].ToString() + " Dealership'";
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
                return File(stream, "InstallmentReceiptReport.pdf");
            }
            catch
            {
                throw;
            }


        }


        [HttpPost]
        public ActionResult DeleteIP(string ReceiptNo,string Voucher)
        {

            if (Session["DealerCode"] == null)
            {
                return RedirectToAction("NewLogin", "Home");
            }

            var DealerCode = Session["DealerCode"].ToString();
            string Msg = "";
            var result = false;
            if (!sysfun.CheckVoucherPostFlag(Session["DealerCode"].ToString(), Voucher))
            {
                sysfun.UpdateJV(Session["DealerCode"].ToString(),Voucher);


                var Check = BMS.PaymentReceiptMaster.Where(g => g.DealerCode == DealerCode && g.ReceiptNo == ReceiptNo && g.DelFlag == "N").FirstOrDefault();
            if (Check != null)
            {
                try
                {
                    var DetailList = BMS.PaymentReceiptDetail.Where(g => g.DealerCode == DealerCode && g.ReceiptNo == ReceiptNo).ToList();
                    if (ObjTrans.BeginTransaction(ref Trans) == true)
                    {
                        foreach (var item in DetailList)
                        {
                            if (item.InvoiceType == "CSI")
                            {
                                result = PaymentReceiptMethods.Delete_Invoice_Record_CSI(item.InvoiceNo, DealerCode, item.AdjAmount, Trans);
                            }
                            else if (item.InvoiceType == "ASC")
                            {
                                result = PaymentReceiptMethods.Delete_Invoice_Record_ASC( item.InvoiceNo,DealerCode, item.AdjAmount, Trans);
                            }

                        }
                        if (result == true)
                        {
                            PaymentReceiptMethods.DelFlagIP( ReceiptNo, DealerCode, Trans);
                                ObjTrans.CommittTransaction(ref Trans);
                            }


                        Msg = "Deleted Successfully...!";
                    }

                }
                catch (Exception ex)
                {

                    ObjTrans.RollBackTransaction(ref Trans);
                    Msg = "Something went wrong with server! /n" + ex;
                }
            } }
            else
            {
                Msg = "You can not delete it; because voucher has already been created against this receipt";
            }
            return Json(Msg, JsonRequestBehavior.AllowGet);
        }
        #endregion
        public ActionResult CustomerServicesReceipt()
        {
            return View();
        }
        

        #region Start  InstallmentReceipt ---------------------------------------------7-Feb-2019-------------------------------------------------------

        public ActionResult InstallmentReceipt()
        {
            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("Login", "Home");
            }
            dealerCode = Session["DealerCode"].ToString();

            List<SelectListItem> ddlPayMode = new List<SelectListItem>();
            ddlPayMode = GeneralMethods.GetDataFromSPWithDealerCode("Select_PaymentMode", dealerCode);
            ViewBag.PayMode = ddlPayMode;

            List<SelectListItem> ddlBank = new List<SelectListItem>();
            ddlBank = GeneralMethods.GetBank();
            ViewBag.Bank = ddlBank;

            List<SelectListItem> ddlCity = new List<SelectListItem>();
            ddlCity = GeneralMethods.GetCity();
            ViewBag.City = ddlCity;

            List<ReceiptMasterVM> IReceiptMasterDetailData = new List<ReceiptMasterVM>();
            IReceiptMasterDetailData = InstallmentReceiptMethods.Get_IReceiptDetailData(dealerCode);
            ViewBag.IRDetail = IReceiptMasterDetailData;

            List<ISIInstallmentReceiptVM> ISIReceiptData = new List<ISIInstallmentReceiptVM>();
            ISIReceiptData = InstallmentReceiptMethods.Get_ISIInstallmentReceiptData(dealerCode);
            ViewBag.ISIIRDetail = ISIReceiptData;

            List<AccountVM> lstAccount = AdvSerChargMethods.GetAccountModal(dealerCode);

            ViewBag.Accounts = lstAccount;

            var OnlyDate = DateTime.Now;
            ViewBag.CurrentDate = OnlyDate.ToString("dd/MM/yyyy");

            return View();
        }

        public ActionResult GetInstallmentReceiptData(string TransCode)
        {
            Session["IRTransCode"] = TransCode;

            List<ISIInstallmentReceiptVM> ISIReceiptData = new List<ISIInstallmentReceiptVM>();
            ISIReceiptData = InstallmentReceiptMethods.Get_ISIInstallmentReceiptData(Session["DealerCode"].ToString());

            var data = ISIReceiptData.Where( a => a.TransCode == TransCode );

            //List<IRGridVM> SaleDetailGrid = new List<IRGridVM>();
            //SaleDetailGrid = InstallmentReceiptMethods.Get_IRGridData(TransCode);
            //var data = SaleDetailGrid;

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult IRExport(string ReceiptNo, string DealerCode)
        {

            DSReports data = new DSReports();
            ReportDocument RD = new ReportDocument();

            SqlParameter[] param =
            {
                new SqlParameter("@DealerCode",SqlDbType.Char),
                new SqlParameter("@ReceiptNo",SqlDbType.Char)
            };

            param[0].Value = DealerCode;
            param[1].Value = ReceiptNo;

            SqlDataReader rder = null;

            SysFunction sysFunc = new SysFunction();
            if (sysFunc.ExecuteSP("SP_Report_IRDetail", param, ref rder))
            {
                data.SP_Report_IRDetail.Load(rder);
            }
            RD.Load(Server.MapPath("~/Reports/Sale/InstallmentReceipt.rpt"));

            RD.DataDefinition.FormulaFields["DealerDesc"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";
            RD.DataDefinition.FormulaFields["ReportTitle"].Text = "'Installment Receipt'";
            RD.DataDefinition.FormulaFields["Terminal"].Text = "'" + Request.ServerVariables["REMOTE_ADDR"].ToString() + "'";
            RD.DataDefinition.FormulaFields["UserId"].Text = "'" + Session["UserName"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["NTN"].Text = "'N.T.N # " + Session["DealerNTN"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["SalesTaxNo"].Text = "'Sales Tax No.  " + Session["DealerSaleTaxNo"].ToString() + " '";
            //rpt.DataDefinition.FormulaFields["UserCell"].Text = "'" + GetStringValuesAgainstCodes("CusCode", , "CellNo", "Customer") + "'";
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
                return File(stream, "InstallmentReceiptReport.pdf");
            }
            catch
            {
                throw;
            }


        }


        [HttpPost]
        public JsonResult Insert_InstallmentReceiptMaster(ReceiptMasterVM DOMasterVM)
        {
            bool result = false;

            string msg = "Failed to save record..";

            result = InstallmentReceiptMethods.Insert_IRMaster(DOMasterVM);

            if (result)
            {
                msg = "Successfully Added";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Insert_InstallmentReceiptDetail(List<ReceiptDetailVM> objects)
        {
            bool result = false;
            string msg = "Failed to save record..";

            result = InstallmentReceiptMethods.Insert_IRDetail(objects);

            if (result)
            {
                msg = "Successfully Added";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Insert_InstallmentAccountTrans(List<AccountTransactionVM> objects)
        {
            bool result = false;
            string msg = "Failed to save record..";

            result = InstallmentReceiptMethods.Insert_AccountTransaction(objects,ref msg);

            if (result)
            {
                msg = "Successfully Added";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }

        #endregion


        public ActionResult CommissionMaster()
        {
            return View();
        }

        public ActionResult Commission()
        {
            return View();
        }

        public ActionResult Refurbished()
        {
            return View();
        }

        #region Full And Final
        public ActionResult FullAndFinal()
        {
            DataTable dt = new DataTable();

            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("Login", "Home");
            }
            dealerCode = Session["DealerCode"].ToString();            
            
            
            List<City> City = new List<City>();
            dt = GeneralMethods.GetDataForModalWithComonDealerCode("sp_2W_Select_City");
            if (dt.Rows.Count > 0)
            {
                City = EnumerableExtension.ToList<City>(dt);
            }
            ViewBag.City = City;

            List<Bank> Bank = new List<Bank>();
            dt = GeneralMethods.GetDataForModalWithComonDealerCode("sp_2W_Select_Bank");
            if (dt.Rows.Count > 0)
            {
                Bank = EnumerableExtension.ToList<Bank>(dt);
            }
            ViewBag.Bank = Bank;

            List<SelectListItem> ddlPayMode = new List<SelectListItem>();
            ddlPayMode = GeneralMethods.GetDataFromSPWithDealerCode("Select_PaymentMode", dealerCode);
            ViewBag.PayMode = ddlPayMode;

            List<ReceiptMasterVM> FNFData = new List<ReceiptMasterVM>();
            FNFData = FullAndFinalMethods.Get_FNFData(dealerCode);
            ViewBag.FNFDetail = FNFData;

            List<ISIInstallmentReceiptVM> IRDetailData = new List<ISIInstallmentReceiptVM>();
            IRDetailData = FullAndFinalMethods.Get_IRDataForFullAndFinal(dealerCode);
            ViewBag.ISIIRDetail = IRDetailData;

            Session["IRListForFullAndFinal"] = IRDetailData;

            return View();
        }
               

        public ActionResult GetFNFData(string TransCode)
        {
            Session["IRTransCode"] = TransCode;

            List<ISIInstallmentReceiptVM> ISIReceiptData = (List<ISIInstallmentReceiptVM>) Session["IRListForFullAndFinal"];
            //ISIReceiptData = InstallmentReceiptMethods.Get_ISIInstallmentReceiptData(Session["DealerCode"].ToString());

            var data = ISIReceiptData.Where(a => a.TransCode == TransCode);

            //List<IRGridVM> SaleDetailGrid = new List<IRGridVM>();
            //SaleDetailGrid = InstallmentReceiptMethods.Get_IRGridData(TransCode);
            //var data = SaleDetailGrid;

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Insert_ReceiptMasterFullAndFinal(ReceiptMasterVM DOMasterVM)
        {
            bool result = false;

            string msg = "Failed to save record..";

            result = FullAndFinalMethods.Insert_IRMaster(DOMasterVM, ref msg);

            if (result)
            {
                msg = "Successfully Added";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Insert_ReceiptDetailFullAndFinal(List<ReceiptDetailVM> objects)
        {
            bool result = false;
            string msg = "Failed to save record..";

            result = FullAndFinalMethods.Insert_IRDetail(objects,ref msg);

            if (result)
            {
                msg = "Successfully Added";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Insert_AccountTransFullAndFinal(List<AccountTransactionVM> objects)
        {
            bool result = false;
            string msg = "Failed to save record..";

            result = FullAndFinalMethods.Insert_AccountTransaction(objects, ref msg);

            if (result)
            {
                msg = "Successfully Added";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult FNFExport(string ReceiptNo, string DealerCode)
        {

            DSReports data = new DSReports();
            ReportDocument RD = new ReportDocument();

            SqlParameter[] param =
            {
                new SqlParameter("@DealerCode",SqlDbType.Char),
                new SqlParameter("@ReceiptNo",SqlDbType.Char)
            };

            param[0].Value = DealerCode;
            param[1].Value = ReceiptNo;

            SqlDataReader rder = null;

            SysFunction sysFunc = new SysFunction();
            if (sysFunc.ExecuteSP("SP_Report_FNF", param, ref rder))
            {
                data.SP_Report_FNF.Load(rder);
            }
            RD.Load(Server.MapPath("~/Reports/Sale/FullAndFinal.rpt"));

            RD.DataDefinition.FormulaFields["DealerDesc"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";
            RD.DataDefinition.FormulaFields["ReportTitle"].Text = "'Full And Final'";
            RD.DataDefinition.FormulaFields["Terminal"].Text = "'" + Request.ServerVariables["REMOTE_ADDR"].ToString() + "'";
            RD.DataDefinition.FormulaFields["UserId"].Text = "'" + Session["UserName"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["NTN"].Text = "'N.T.N # " + Session["DealerNTN"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["SalesTaxNo"].Text = "'Sales Tax No.  " + Session["DealerSaleTaxNo"].ToString() + " '";
            //rpt.DataDefinition.FormulaFields["UserCell"].Text = "'" + GetStringValuesAgainstCodes("CusCode", , "CellNo", "Customer") + "'";
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
                return File(stream, "InstallmentReceiptReport.pdf");
            }
            catch
            {
                throw;
            }


        }

        #endregion //--------------------------------------- Full And Final ----------------------------
        public ActionResult Exchange()
        {

            return View();
        }
        #region New Advance Service Receipt


        #region Advance / Services Charges
        public ActionResult AdvanceServicesCharges()
        {

            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("NewLogin", "Home");
            }
            dealerCode = Session["DealerCode"].ToString();

            List<SelectListItem> ddlPayMode = new List<SelectListItem>();
            ddlPayMode = GeneralMethods.GetDataFromSPWithDealerCode("Select_PaymentMode", dealerCode);
            ViewBag.PayMode = ddlPayMode;

            List<SelectListItem> ddlVehExpHead = new List<SelectListItem>();
            ddlVehExpHead = GeneralMethods.GetDataFromSP("SP_Select_VehExpHead");
            ViewBag.VehExpHead = ddlVehExpHead;

            List<SelectListItem> ddlBank = new List<SelectListItem>();
            ddlBank = GeneralMethods.GetBank();
            ViewBag.Bank = ddlBank;

            List<SelectListItem> ddlCity = new List<SelectListItem>();
            ddlCity = GeneralMethods.GetCity();
            ViewBag.City = ddlCity;

            List<AccountVM> lstAccount = AdvSerChargMethods.GetAccountModal(dealerCode);
            List<ReceiptMasterVM> lstReceipt = AdvSerChargMethods.GetReceiptModal(dealerCode);

            ViewBag.Accounts = lstAccount;
            ViewBag.Receipts = lstReceipt;

            return View();
        }
        [HttpGet]
        public JsonResult Select_BrandProduct(string EnquiryId, string DealerCode, string Segment)
        {
            List<GetProductSpVM> data;
            DealerCode = Session["DealerCode"].ToString();
            bool result = false;
            // List<GetProductSpVM> ddlProduct = new List<GetProductSpVM>();
            data = CashSaleInvoiceMethods.Get_BrandProductData(EnquiryId, DealerCode, Segment);
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
        public JsonResult Select_VehicleReceiptVoucherNo(string EnquiryId, string DealerCode)
        {
            string data = "";
            bool result = false;
            DealerCode = Session["DealerCode"].ToString();
            data = GeneralMethods.GetVoucherNo(EnquiryId, "SP_Select_ReceiptMasterVouchNo", DealerCode);
            Session["VoucherNo"] = data;
            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Get_BrandDetailFromModal(string EnquiryId, string chasisNo)
        {
            //Session["IRTransCode"] = TransCode;

            List<GetProductSpVM> ISIReceiptData = (List<GetProductSpVM>)Session["VehicleRecord"];
            //ISIReceiptData = InstallmentReceiptMethods.Get_ISIInstallmentReceiptData(Session["DealerCode"].ToString());

            var data = ISIReceiptData.Where(a => a.RecNo.Trim() == EnquiryId.Trim() && a.ChasisNo.Trim() == chasisNo);

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Get_SaleInvoiceModal(string EnquiryId, string dealerCode)
        {
            string data;
            bool result = false;

            data = AdvSerChargMethods.GetSaleInvoiceModal(EnquiryId, dealerCode);

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult Get_SaleInvoiceDetail(string TransNo, string ChasisNo, string DealerCode)
        {
            string data;
            bool result = false;

            data = AdvSerChargMethods.GetSaleInvoiceDetail(TransNo, ChasisNo, DealerCode);

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult Insert_ReceiptMaster(ReceiptMasterVM DOMasterVM)
        {
            bool result = false;

            string msg = "Failed to save record..";

            result = AdvSerChargMethods.Insert_ReceiptMaster(DOMasterVM, ref msg);

            if (result)
            {
                msg = "Successfully Added";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Insert_ReceiptDetail(List<ReceiptDetailVM> objects)
        {
            bool result = false;
            int count = 0;
            string msg = "Failed to save record..";

            result = AdvSerChargMethods.Insert_ReceiptDetail(objects, ref msg);

            if (result)
            {
                msg = "Successfully Added";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Insert_AccountTrans(List<AccountTransactionVM> objects)
        {
            bool result = false;
            int count = 0;
            string msg = "Failed to save record..";

            result = AdvSerChargMethods.Insert_AccountTransaction(objects, ref msg);

            if (result)
            {
                msg = "Successfully Added";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Select_ReceiptDetail(string EnquiryId, string Code)
        {
            string data = "";
            bool result = false;
            data = AdvSerChargMethods.Get_ReceiptDetailData(EnquiryId, Session["DealerCode"].ToString());

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Delete_Receipt(string EnquiryId)
        {
            bool result = false;

            string msg = "Record not Delete";

            result = AdvSerChargMethods.Delete_Receipt_Record(EnquiryId, Session["DealerCode"].ToString());

            if (result)
            {
                msg = "Successfully Deleted";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AdvSerCharExport(string EnquiryId, string DealerCode)
        {
            DSReports data = new DSReports();
            ReportDocument RD = new ReportDocument();

            SqlParameter[] param =
            {
                new SqlParameter("@DealerCode",SqlDbType.Char),
                new SqlParameter("@ReceiptNo",SqlDbType.VarChar)

            };

            param[0].Value = DealerCode;
            param[1].Value = EnquiryId;

            SqlDataReader rder = null;

            SysFunction sysFunc = new SysFunction();
            if (sysFunc.ExecuteSP("SP_Report_AdvRecDetail", param, ref rder))
            {
                data.EnforceConstraints = false;
                data.SP_Report_AdvRecDetail.Load(rder);


            }
            RD.Load(Server.MapPath("~/Reports/Sale/AdvanceReceipt.rpt"));

            RD.DataDefinition.FormulaFields["DealerDesc"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";
            RD.DataDefinition.FormulaFields["ReportTitle"].Text = "'Advance / Service Charges'";
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
                return File(stream, "InstalmentReceiptReport.pdf");
            }
            catch
            {
                throw;
            }


        }
        #endregion

        #endregion
        //#region Advance / Services Charges
        //public ActionResult AdvanceServicesCharges()
        //{

        //    if (string.IsNullOrEmpty((string)Session["DealerCode"]))
        //    {
        //        return RedirectToAction("Login", "Home");
        //    }
        //    dealerCode = Session["DealerCode"].ToString();

        //    List<SelectListItem> ddlPayMode = new List<SelectListItem>();
        //    ddlPayMode = GeneralMethods.GetDataFromSPWithDealerCode("Select_PaymentMode", dealerCode);
        //    ViewBag.PayMode = ddlPayMode;

        //    List<SelectListItem> ddlVehExpHead = new List<SelectListItem>();
        //    ddlVehExpHead = GeneralMethods.GetDataFromSP("SP_Select_VehExpHead");
        //    ViewBag.VehExpHead = ddlVehExpHead;

        //    List<SelectListItem> ddlBank = new List<SelectListItem>();
        //    ddlBank = GeneralMethods.GetBank();
        //    ViewBag.Bank = ddlBank;

        //    List<SelectListItem> ddlCity = new List<SelectListItem>();
        //    ddlCity = GeneralMethods.GetCity();
        //    ViewBag.City = ddlCity;

        //    List<AccountVM> lstAccount = AdvSerChargMethods.GetAccountModal(dealerCode);
        //    List<ReceiptMasterVM> lstReceipt = AdvSerChargMethods.GetReceiptModal(dealerCode);

        //    ViewBag.Accounts = lstAccount;
        //    ViewBag.Receipts = lstReceipt;

        //    return View();
        //}

        //[HttpGet]
        //public JsonResult Get_SaleInvoiceModal(string EnquiryId , string dealerCode)
        //{
        //    string data;
        //    bool result = false;

        //    data = AdvSerChargMethods.GetSaleInvoiceModal(EnquiryId, dealerCode);

        //    if (!string.IsNullOrEmpty(data))
        //    {
        //        result = true;
        //    }

        //    return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);

        //}

        //[HttpGet]
        //public JsonResult Get_SaleInvoiceDetail(string TransNo,string ChasisNo , string DealerCode)
        //{
        //    string data;
        //    bool result = false;

        //    data = AdvSerChargMethods.GetSaleInvoiceDetail(TransNo, ChasisNo, DealerCode);

        //    if (!string.IsNullOrEmpty(data))
        //    {
        //        result = true;
        //    }

        //    return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);

        //}
        //[HttpPost]
        //public JsonResult Insert_ReceiptMaster(ReceiptMasterVM DOMasterVM)
        //{
        //    bool result = false;

        //    string msg = "Failed to save record..";

        //    result = AdvSerChargMethods.Insert_ReceiptMaster(DOMasterVM);

        //    if (result)
        //    {
        //        msg = "Successfully Added";
        //    }

        //    return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        //}
        //[HttpPost]
        //public JsonResult Insert_ReceiptDetail(List<ReceiptDetailVM> objects)
        //{
        //    bool result = false;
        //    int count = 0;
        //    string msg = "Failed to save record..";

        //    result = AdvSerChargMethods.Insert_ReceiptDetail(objects);

        //    if (result)
        //    {
        //        msg = "Successfully Added";
        //    }

        //    return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        //}

        //[HttpPost]
        //public JsonResult Insert_AccountTrans(List<AccountTransactionVM> objects)
        //{
        //    bool result = false;
        //    int count = 0;
        //    string msg = "Failed to save record..";

        //    result = AdvSerChargMethods.Insert_AccountTransaction(objects);

        //    if (result)
        //    {
        //        msg = "Successfully Added";
        //    }

        //    return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        //}

        //[HttpGet]
        //public JsonResult Select_ReceiptDetail(string EnquiryId, string Code)
        //{
        //    string data = "";
        //    bool result = false;
        //    data = AdvSerChargMethods.Get_ReceiptDetailData(EnquiryId,Session["DealerCode"].ToString());

        //    if (!string.IsNullOrEmpty(data))
        //    {
        //        result = true;
        //    }

        //    return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        //}

        //public JsonResult Delete_Receipt(string EnquiryId)
        //{
        //    bool result = false;

        //    string msg = "Record not Delete";

        //    result = AdvSerChargMethods.Delete_Receipt_Record(EnquiryId, Session["DealerCode"].ToString());

        //    if (result)
        //    {
        //        msg = "Successfully Deleted";
        //    }

        //    return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        //}

        //public ActionResult AdvSerCharExport(string EnquiryId, string DealerCode)
        //{
        //    DSReports data = new DSReports();
        //    ReportDocument RD = new ReportDocument();

        //    SqlParameter[] param =
        //    {
        //        new SqlParameter("@DealerCode",SqlDbType.Char),
        //        new SqlParameter("@ReceiptNo",SqlDbType.VarChar)

        //    };

        //    param[0].Value = DealerCode;
        //    param[1].Value = EnquiryId;

        //    SqlDataReader rder = null;

        //    SysFunction sysFunc = new SysFunction();
        //    if (sysFunc.ExecuteSP("SP_Report_AdvRecDetail", param, ref rder))
        //    {
        //        data.EnforceConstraints = false;
        //        data.SP_Report_AdvRecDetail.Load(rder);


        //    }
        //    RD.Load(Server.MapPath("~/Reports/Sale/AdvanceReceipt.rpt"));

        //    RD.DataDefinition.FormulaFields["DealerDesc"].Text = "'" + Session["DealerDesc"].ToString() + "'";
        //    RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
        //    RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
        //    RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";
        //    RD.DataDefinition.FormulaFields["ReportTitle"].Text = "'Advance / Service Charges'";
        //    RD.DataDefinition.FormulaFields["Terminal"].Text = "'" + Request.ServerVariables["REMOTE_ADDR"].ToString() + "'";
        //    RD.DataDefinition.FormulaFields["UserId"].Text = "'" + Session["UserName"].ToString() + "'";
        //    //RD.DataDefinition.FormulaFields["NTN"].Text = "'N.T.N # " + Session["DealerNTN"].ToString() + "'";
        //    //RD.DataDefinition.FormulaFields["SalesTaxNo"].Text = "'Sales Tax No.  " + Session["DealerSaleTaxNo"].ToString() + " '";
        //    //rpt.DataDefinition.FormulaFields["UserCell"].Text = "'" + GetStringValuesAgainstCodes("CusCode", , "CellNo", "Customer") + "'";
        //    RD.DataDefinition.FormulaFields["CompanyName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
        //    //RD.DataDefinition.FormulaFields["Pic"].Text = "'C:\\Users\\u_ahm\\OneDrive\\Documents\\Visual Studio 2010\\Projects\\WebApplication1\\WebApplication1\\" + Session["Logo"] + "'";
        //    RD.DataDefinition.FormulaFields["Pic"].Text = "'" + Server.MapPath("~") + Session["Logo"] + "'";

        //    RD.Database.Tables[0].SetDataSource(data);

        //    Response.Buffer = false;
        //    Response.ClearContent();
        //    Response.ClearHeaders();
        //    try
        //    {
        //        Stream stream = RD.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
        //        stream.Seek(0, SeekOrigin.Begin);
        //        return File(stream, "InstalmentReceiptReport.pdf");
        //    }
        //    catch
        //    {
        //        throw;
        //    }


        //}
        //#endregion



        #region Price Offer Negociation

        public ActionResult PriceOfferNegociation()

        {
            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("Login", "Home");
            }
            DataTable dt = new DataTable();
            string dealerCode = Session["DealerCode"].ToString();
            List<SelectListItem> ddlOfferCode = new List<SelectListItem>();
            ddlOfferCode = GeneralMethods.Get_EvaluationCode(dealerCode);
            ViewBag.ddlOfferCode = ddlOfferCode;
            List<SelectListItem> ddlSource = new List<SelectListItem>();
            ddlSource = GeneralMethods.Get_CRM_LeadSource(dealerCode);
            ViewBag.ddlSource = ddlSource;
            List<SelectListItem> ddlEmp = new List<SelectListItem>();
            ddlEmp = GeneralMethods.Get_DealerEmp(dealerCode);
            ViewBag.ddlDealerEmp = ddlEmp;
            List<GetRegNoRecVM> RegNoDetailData = new List<GetRegNoRecVM>();
            RegNoDetailData = SaleOrderMethods.Get_RegNoDetailData(dealerCode);
            ViewBag.RegNo = RegNoDetailData;
            List<UCS_EvaluationVM> Select_PriceOfferNegociation = new List<UCS_EvaluationVM>();
            Select_PriceOfferNegociation = PriceOfferNegotiationMethods.Get_PriceOfferNegociationData(dealerCode);
            ViewBag.Evaluation = Select_PriceOfferNegociation;
            Session["Select_PriceOfferNegociation"] = Select_PriceOfferNegociation;
            return View();
        }

        public JsonResult Insert_PriceOfferNegociation(UCS_EvaluationVM Objects)
        {
            string dealerCode = Session["DealerCode"].ToString();
            bool result = false;

            string msg = "Failed to save record..";


            result = PriceOfferNegotiationMethods.Insert_PriceOfferNegociation(Objects, dealerCode);

            if (result)
            {
                msg = "Successfully Added";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Insert_PriceOfferNegociation_EvaluationDetail(UCS_EvaluationVM Objects)
        {
            string dealerCode = Session["DealerCode"].ToString();
            bool result = false;

            string msg = "Failed to save record..";


            result = PriceOfferNegotiationMethods.Insert_PriceOfferNegociation_EvaluationDetail(Objects, dealerCode);

            if (result)
            {
                msg = "Successfully Added";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]

        public ActionResult Get_DataPONegociation(string EnquiryId)
        {
            //Session["IRTransCode"] = TransCode;

            List<UCS_EvaluationVM> ISIReceiptData = (List<UCS_EvaluationVM>)Session["Select_PriceOfferNegociation"];
            //ISIReceiptData = InstallmentReceiptMethods.Get_ISIInstallmentReceiptData(Session["DealerCode"].ToString());

            var data = ISIReceiptData.Where(a => a.EvaluationCode.Trim() == EnquiryId.Trim());

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Delete_PriceOfferNegociation(string EnquiryId)
        {
            bool result = false;
            string dealerCode = Session["DealerCode"].ToString();
            string msg = " Data can't be deleted";

            result = PriceOfferNegotiationMethods.Delete_PriceOfferNegociation_Record(EnquiryId, dealerCode);

            if (result)
            {
                msg = "Successfully Deleted";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }




        public ActionResult Exports(string EnquiryId)
        {

            DSReports data = new DSReports();
            ReportDocument RD = new ReportDocument();
            string dealerCode = Session["DealerCode"].ToString();
            SqlParameter[] param =
            {
                new SqlParameter("@DealerCode",SqlDbType.Char),
                new SqlParameter("@EvaluationCode",SqlDbType.Char)
            };

            param[0].Value = dealerCode;
            param[1].Value = EnquiryId;

            SqlDataReader rder = null;

            SysFunction sysFunc = new SysFunction();
            if (sysFunc.ExecuteSP("Select_EvaluationCodeReport", param, ref rder))
            {
                data.Select_EvaluationCodeReport.Load(rder);
            }
            RD.Load(Server.MapPath("~/Reports/Sale/PriceOfferNegociation.rpt"));

            //RD.DataDefinition.FormulaFields["DealerDesc"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";
            RD.DataDefinition.FormulaFields["ReportTitle"].Text = "'Price Offer/ Negociation '";
            //RD.DataDefinition.FormulaFields["Terminal"].Text = "'" + Request.ServerVariables["REMOTE_ADDR"].ToString() + "'";
            RD.DataDefinition.FormulaFields["UserId"].Text = "'" + Session["UserName"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["NTN"].Text = "'N.T.N # " + Session["DealerNTN"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["SalesTaxNo"].Text = "'Sales Tax No.  " + Session["DealerSaleTaxNo"].ToString() + " '";
            //rpt.DataDefinition.FormulaFields["UserCell"].Text = "'" + GetStringValuesAgainstCodes("CusCode", , "CellNo", "Customer") + "'";
            RD.DataDefinition.FormulaFields["CompanyName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            // RD.DataDefinition.FormulaFields["Pic"].Text = "'C:\\Users\\u_ahm\\OneDrive\\Documents\\Visual Studio 2010\\Projects\\WebApplication1\\WebApplication1\\" + Session["Logo"] + "'";



            RD.Database.Tables[0].SetDataSource(data);

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();


            try
            {
                Stream stream = RD.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "InstallmentReceiptReport.pdf");
            }
            catch
            {
                throw;
            }


        }

        public ActionResult ExportPriceDetail(string todate, string fromdate)
        {
            SysFunction sysFunc = new SysFunction();
            DSReports data = new DSReports();
            ReportDocument RD = new ReportDocument();

            SqlParameter[] param =
        {
                new SqlParameter("@fromDate",SqlDbType.DateTime),
                new SqlParameter("@ToDate",SqlDbType.DateTime),

            };

            param[0].Value = sysFunc.SaveDate(todate);
            param[1].Value = sysFunc.SaveDate(fromdate);


            SqlDataReader rder = null;

         
            if (sysFunc.ExecuteSP("SP_Report_DetailPriceOffer", param, ref rder))
            {
                data.EnforceConstraints = false;
                data.SP_Report_DetailPriceOffer.Load(rder);

            }
            RD.Load(Server.MapPath("~/Reports/Sale/PriceOfferDetail.rpt"));

            RD.DataDefinition.FormulaFields["DealerDesc"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";
            RD.DataDefinition.FormulaFields["ReportTitle"].Text = "'Price Offer/ Negociation Details '";
            //RD.DataDefinition.FormulaFields["Terminal"].Text = "'" + Request.ServerVariables["REMOTE_ADDR"].ToString() + "'";
            RD.DataDefinition.FormulaFields["UserId"].Text = "'" + Session["UserName"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["NTN"].Text = "'N.T.N # " + Session["DealerNTN"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["SalesTaxNo"].Text = "'Sales Tax No.  " + Session["DealerSaleTaxNo"].ToString() + " '";
            //rpt.DataDefinition.FormulaFields["UserCell"].Text = "'" + GetStringValuesAgainstCodes("CusCode", , "CellNo", "Customer") + "'";
            RD.DataDefinition.FormulaFields["CompanyName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            // RD.DataDefinition.FormulaFields["Pic"].Text = "'C:\\Users\\u_ahm\\OneDrive\\Documents\\Visual Studio 2010\\Projects\\WebApplication1\\WebApplication1\\" + Session["Logo"] + "'";
            RD.DataDefinition.FormulaFields["Pic"].Text = "'" + Server.MapPath("~") + Session["Logo"] + "'";



            RD.Database.Tables[0].SetDataSource(data);

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();


            try
            {
                Stream stream = RD.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "InstallmentReceiptReport.pdf");
            }
            catch
            {
                throw;
            }


        }


        #endregion
    }
}