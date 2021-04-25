using Core.CRM.ADO.ViewModel;
using Core.CRM.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Core.CRM.ADO
{
    public class BookingOrderMethods
    {
        static bool IsSaved = false;
        static bool IsDeleted = false;
        static SqlParameter[] nullSqlParam = null;
        static DataTable dt = new DataTable();
        static string strAutoCode = string.Empty;
        static SysFunction sysfun = new SysFunction();
        //static SysFunctions sysfuns = new SysFunctions();


        static Transaction ObjTrans = new Transaction();
        static SqlTransaction Trans;

        public static List<SelectListItem> GetDataFromSP(string sp)
        {
            List<SelectListItem> item = new List<SelectListItem>();

            List<StringNameValueClass> lst = new List<StringNameValueClass>();

            try
            {
                SqlParameter[] sqlParam =
                {

                };
                dt = DataAccess.getDataTable(sp, sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<StringNameValueClass>(dt);
                }

                item = lst.Select(i => new SelectListItem()
                {
                    Value = i.Id.ToString(),
                    Text = i.Title
                }).ToList();

                item.Insert(0, new SelectListItem() { Value = "0", Text = "Select" });

            }
            catch (Exception ex)
            {

                //throw;
            }
            return item;
        }

        public static List<SelectListItem> GetDataFromSPWithDealerCode(string sp, string DealerCode)
        {
            List<SelectListItem> item = new List<SelectListItem>();

            List<StringNameValueClass> lst = new List<StringNameValueClass>();

            try
            {
                SqlParameter[] sqlParam =
                {
                     new SqlParameter("@DealerCode",DealerCode)
                };
                dt = DataAccess.getDataTable(sp, sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<StringNameValueClass>(dt);
                }

                item = lst.Select(i => new SelectListItem()
                {
                    Value = i.Id.ToString(),
                    Text = i.Title
                }).ToList();

                item.Insert(0, new SelectListItem() { Value = "0", Text = "Select" });

            }
            catch (Exception ex)
            {

                //throw;
            }
            return item;
        }

        public static string GetCustomerDetail(string CusCode, string dealerCode)
        {
            string json = "";
            List<SelectListItem> item = new List<SelectListItem>();

            var Serializer = new JavaScriptSerializer();
            List<CustomerVM> lst = new List<CustomerVM>();
            try
            {
                //var Serializer = new JavaScriptSerializer();
                SqlParameter[] sqlParam =
                {
                    new SqlParameter("@DealerCode",dealerCode),
                    new SqlParameter("@CusCode",CusCode)
                };
                dt = DataAccess.getDataTable("SP_SelectCustomer", sqlParam, General.GetBMSConString());

                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<CustomerVM>(dt);
                }
                json = Serializer.Serialize(lst);

            }
            catch (Exception ex)
            {

                //throw;
            }
            return json;
        }

        public static string Get_FactoryPrice(string color, string brand, string product, string version, string dealerCode)
        {
            string json = "";
            List<SelectListItem> item = new List<SelectListItem>();

            var Serializer = new JavaScriptSerializer();
            DataTable dt = new DataTable();
            List<RequestVehicleReceiptVM> lst = new List<RequestVehicleReceiptVM>();
            try
            {
                //var Serializer = new JavaScriptSerializer();
                SqlParameter[] sqlParam =
                {
                    new SqlParameter("@DealerCode",dealerCode),
                    new SqlParameter("@Brand",brand),
                    new SqlParameter("@Product",product),
                    new SqlParameter("@Version",version),
                    new SqlParameter("@Color",color)
                };
                dt = DataAccess.getDataTable("SP_Select_FactoryPrice_VehTaxDetail", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<RequestVehicleReceiptVM>(dt);
                }
                json = Serializer.Serialize(lst);


            }
            catch (Exception ex)
            {

                throw;
            }
            return json;
        }

        public static string GetVehicleDetail(string chassisNo, string dealerCode)
        {
            string json = "";
            List<SelectListItem> item = new List<SelectListItem>();

            var Serializer = new JavaScriptSerializer();
            List<RequestVehicleReceiptVM> lst = new List<RequestVehicleReceiptVM>();
            try
            {
                //var Serializer = new JavaScriptSerializer();
                SqlParameter[] sqlParam =
                {
                    new SqlParameter("@DealerCode",dealerCode),
                    new SqlParameter("@ChassisNo",chassisNo)
                };
                dt = DataAccess.getDataTable("SP_Select_VehicleDetail", sqlParam, General.GetBMSConString());

                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<RequestVehicleReceiptVM>(dt);
                }
                json = Serializer.Serialize(lst);

            }
            catch (Exception ex)
            {

                //throw;
            }
            return json;
        }

        public static string GetCustomerModal(string dealerCode)
        {
            string json = "";
            List<SelectListItem> item = new List<SelectListItem>();

            var Serializer = new JavaScriptSerializer();
            List<CustomerVM> lst = new List<CustomerVM>();
            try
            {
                //var Serializer = new JavaScriptSerializer();
                SqlParameter[] sqlParam =
                {
                    new SqlParameter("@DealerCode",dealerCode)
                };
                dt = DataAccess.getDataTable("SP_Select_CustomerModal", sqlParam, General.GetBMSConString());

                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<CustomerVM>(dt);
                }
                json = Serializer.Serialize(lst);

            }
            catch (Exception ex)
            {

                //throw;
            }
            return json;
        }

        public static string GetBookRefModal(string dealerCode)
        {
            string json = "";
            List<SelectListItem> item = new List<SelectListItem>();

            var Serializer = new JavaScriptSerializer();
            List<BookOrdMasterVM> lst = new List<BookOrdMasterVM>();
            try
            {
                //var Serializer = new JavaScriptSerializer();
                SqlParameter[] sqlParam =
                {
                    new SqlParameter("@DealerCode",dealerCode)
                };
                dt = DataAccess.getDataTable("SP_Get_BookRefModal", sqlParam, General.GetBMSConString());

                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<BookOrdMasterVM>(dt);
                }
                json = Serializer.Serialize(lst);

            }
            catch (Exception ex)
            {

                //throw;
            }
            return json;
        }


        public static string Get_EnquiryModal(string dealerCode)
        {
            string json = "";
            List<SelectListItem> item = new List<SelectListItem>();

            var Serializer = new JavaScriptSerializer();
            List<EnquiryMasterVM> lst = new List<EnquiryMasterVM>();
            try
            {
                //var Serializer = new JavaScriptSerializer();
                SqlParameter[] sqlParam =
                {
                    new SqlParameter("@DealerCode",dealerCode)
                };
                dt = DataAccess.getDataTable("SP_EnquiryID", sqlParam, General.GetBMSConString());

                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<EnquiryMasterVM>(dt);
                }
                json = Serializer.Serialize(lst);

            }
            catch (Exception ex)
            {

                //throw;
            }
            return json;
        }

        public static bool Insert_BOMaster(BookOrdMasterVM model, ref string msg)
        {
            IsSaved = false;
            try
            {
                if (model.BookRefNo == null || model.BookRefNo == "" || model.BookRefNo == "0")
                {
                    strAutoCode = sysfun.AutoGen("BookOrdMaster", "BookRefNo", DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MM/yyyy"), model.DealerCode);

                }
                else
                {

                    strAutoCode = model.BookRefNo;

                }
                SqlParameter[] param = {
                                 new SqlParameter("@DealerCode",model.DealerCode),//0
								 new SqlParameter("@BookRefNo",strAutoCode),//1
								 new SqlParameter("@BookRefDate",sysfun.SaveDate(model.BookRefDate)),//2								 
								 new SqlParameter("@InvTypeCode",model.InvTypeCode),//3								 
								 new SqlParameter("@InvSubTypeCode",model.InvSubTypeCode),//4
								 new SqlParameter("@PriceTypeCode",model.PriceTypeCode),//5
								 new SqlParameter("@CusCode",model.CusCode),//6
								 new SqlParameter("@UpdUser",AuthBase.UserId),//7
								 new SqlParameter("@UpdTerm",General.CurrentIP),//8
								 new SqlParameter("@FHName",model.FHName),//9
								 new SqlParameter("@Address1",model.Address1),//10
								 new SqlParameter("@NIC",model.NIC),//11
								 new SqlParameter("@NTN",model.NTN),//12
								 new SqlParameter("@CellNo",model.CellNo),//13
								 new SqlParameter("@Email",model.Email),//14
								 new SqlParameter("@PhoneNo",model.PhoneNo),//15								 
								 new SqlParameter("@SalesPerson",model.SalesPerson),//16								 
								 new SqlParameter("@BookMode",model.BookMode),//17
								 new SqlParameter("@Segment",model.Segment),//18
								 new SqlParameter("@TaxType",model.TaxType),//19
								 new SqlParameter("@CusTypeCode",model.CusTypeCode),//20
								 new SqlParameter("@StockType",model.StockType),//21
								 new SqlParameter("@DeliveryLocation",model.DeliveryLocation),//22
								 new SqlParameter("@PaymentTerms",model.PaymentTerms),//23
								 new SqlParameter("@AccountOf",model.AccountOf), //24
								 new SqlParameter("@VehicleTotal",model.VehicleTotal),//25
                                 new SqlParameter("@City",model.City),//26
                                 new SqlParameter("@State",model.State),//27
                                 new SqlParameter("@Country",model.Country),//28
                                 new SqlParameter("@VehicleQty",model.VehicleQty),//28
                                 new SqlParameter("@EnquiryNo",model.EnquiryNo),//28
                                 new SqlParameter("@RemainingTotal",model.RemainingTotal),//15
                            };

                if (ObjTrans.BeginTransaction(ref Trans) == true)
                {
                    if (sysfun.ExecuteSP_NonQuery("SP_Insert_BookOrdMaster", param, Trans))
                    {
                        IsSaved = true;
                    }
                    else
                    {
                        return IsSaved;
                    }
                }

                //if (sysfun.ExecuteSP_NonQuery("SP_VehicleDeliveryMaster_Insert", param))
                //{

                //    IsSaved = true;

                //}

            }
            catch (Exception ex)
            {
                //ObjTrans.RollBackTransaction(ref Trans);
                msg = ex.Message;
            }

            return IsSaved;
        }

        public static bool Insert_BODetail(List<BookOrdVehDetailVM> model2, string dealerCode)
        {
            int count = 0;
            try
            {
                foreach (var item in model2)
                {
                    if (item.BrandCode != null || item.ChasisNo != null)
                    {
                        SqlParameter[] param2 = {
                                 new SqlParameter("@DealerCode",dealerCode),//0
								 new SqlParameter("@BookRefNo",strAutoCode),//1
								 new SqlParameter("@BrandCode",item.BrandCode),//2								 
								 new SqlParameter("@ProdCode",item.ProdCode.Trim()),//3								 
								 new SqlParameter("@VersionCode",item.VersionCode),//4                            
								 new SqlParameter("@EngineNo",item.EngineNo == null ? string.Empty : item.EngineNo.Trim()),//5                                 
								 new SqlParameter("@ChasisNo",item.ChasisNo == null ? string.Empty : item.ChasisNo.Trim()),//6
								 new SqlParameter("@ExFactPrice",item.ExFactPrice),//7
								 new SqlParameter("@Qty",item.Qty),//8
								 new SqlParameter("@ColorCode1",item.ColorCode1),//9
								 new SqlParameter("@AdvanceTaxAmt",item.AdvanceTaxAmt),//10
								 new SqlParameter("@SpecialDiscount",item.SpecialDiscount),//11
								 new SqlParameter("@TotalAmt",item.TotalAmt),//12
								 new SqlParameter("@ReceiptNo",item.ReceiptNo == null ? string.Empty : item.ReceiptNo),//13
								 new SqlParameter("@Warranty",item.Warranty),//14
                                 new SqlParameter("@InstalmentPlan",item.InstalmentPlan),//15
                                 new SqlParameter("@EnquiryNo",item.EnquiryNo),//15
                                  
								 
							};


                        if (sysfun.ExecuteSP_NonQuery("SP_Insert_BookOrdVehDetail", param2, Trans) == true)
                        {
                            IsSaved = true;
                        }
                        else
                        {
                            ObjTrans.RollBackTransaction(ref Trans);
                            IsSaved = false;
                        }
                    }

                }
            }
            catch (Exception)
            {
                ObjTrans.RollBackTransaction(ref Trans);
                throw;
            }

            return IsSaved;
        }

        public static bool Insert_PaymentDetail(List<BookingReceiptInstrumentDetailVM> model2, string dealerCode)
        {
            string strReceiptNo;
            int count = 0;
            try
            {
                foreach (var item in model2)
                {
                    //if (count >= 1 || item.InstrumentNo != null)
                    //{
                    if (item.ReceiptNo == null)
                    {
                        strReceiptNo = sysfun.AutoGen("BookingReceiptInstrumentDetail", "ReceiptNo", DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MM/yyyy"), dealerCode);
                    }
                    else
                    {
                        strReceiptNo = item.ReceiptNo;
                    }


                    SqlParameter[] param2 = {
                                 new SqlParameter("@DealerCode",dealerCode),//0
								 new SqlParameter("@BookRefNo",strAutoCode),//1
								 new SqlParameter("@InstrumentNo",item.InstrumentNo),//2								 
								 new SqlParameter("@InstrumentDate",item.InstrumentDate == null ?(object) DBNull.Value: sysfun.SaveDate(item.InstrumentDate)),//3								 
								 new SqlParameter("@InstrumentAmount",item.InstrumentAmount),//4                                 
								 new SqlParameter("@CityCode",item.CityCode),//5                                 
								 new SqlParameter("@BankCode",item.BankCode),//6
								 new SqlParameter("@Branch",item.Branch),//7
								 new SqlParameter("@PaymentMode",item.PaymentMode),//8
								 new SqlParameter("@PaymentType",item.PaymentType),//9		
								 new SqlParameter("@ReceiptNo",strReceiptNo)//10					 
								 
							};

                    if (sysfun.ExecuteSP_NonQuery("SP_Insert_BookingReceiptInstrumentDetail", param2, Trans) == true)
                    {

                        //   string sql = "Update BookOrdMaster set RemainingTotal = VehicleTotal - '" + item.InstrumentAmount + "' where BookRefNo = '" + strAutoCode + "' and DealerCode = '"+dealerCode+"'";

                        //  sysfun.ExecuteQuery_NonQuery(sql , Trans );
                        IsSaved = true;
                    }
                    else
                    {
                        ObjTrans.RollBackTransaction(ref Trans);
                        IsSaved = false;
                    }
                    //}
                }

            }
            catch (Exception)
            {
                ObjTrans.RollBackTransaction(ref Trans);
                throw;
            }

            return IsSaved;
        }

        public static bool Insert_BalancedPaymentDetail(List<BookingReceiptInstrumentDetailVM> model2, string dealerCode, ref string msg)
        {

            int count = 0;
            try
            {
                foreach (var item in model2)
                {
                    if (model2.Count >= 1 || item.InstrumentNo != null)
                    {
                        string strReceiptNo = sysfun.AutoGen("BookingReceiptInstrumentDetail", "ReceiptNo", DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MM/yyyy"), dealerCode);

                        SqlParameter[] param2 = {
                                 new SqlParameter("@DealerCode",dealerCode),//0
								 new SqlParameter("@BookRefNo",item.BookRefNo),//1
								 new SqlParameter("@InstrumentNo",item.InstrumentNo),//2								 
								 new SqlParameter("@InstrumentDate",sysfun.SaveDate(item.InstrumentDate)),//3								 
								 new SqlParameter("@InstrumentAmount",item.InstrumentAmount),//4                                 
								 new SqlParameter("@CityCode",item.CityCode),//5                                 
								 new SqlParameter("@BankCode",item.BankCode),//6
								 new SqlParameter("@Branch",item.Branch),//7
								 new SqlParameter("@PaymentMode",item.PaymentMode),//8
								 new SqlParameter("@PaymentType",item.PaymentType),//9		
                                 new SqlParameter("@ReceiptNo",strReceiptNo)//10						 
								 
							};

                        if (sysfun.ExecuteSP_NonQuery("SP_Insert_BookingReceiptInstrumentDetail", param2))
                        {
                            string sql = "Update BookOrdMaster set RemainingTotal = RemainingTotal - '" + item.InstrumentAmount + "' where BookRefNo = '" + item.BookRefNo + "'and DealerCode = '" + dealerCode + "'";

                            sysfun.ExecuteQuery_NonQuery(sql);

                            IsSaved = true;
                        }
                        else
                        {
                            IsSaved = false;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            return IsSaved;
        }

        public static bool Insert_VehChkList(string strCheckedValues, string dealerCode)
        {

            try
            {
                SqlParameter[] param2 = {
                                 new SqlParameter("@DealerCode",dealerCode),//0
								 new SqlParameter("@Delivery",strAutoCode),//1
								 new SqlParameter("@DelChkListCode",strCheckedValues)//2								 
								 
							};

                if (sysfun.ExecuteSP_NonQuery("SP_BookOrdChkList_Insert", param2))
                {
                    ObjTrans.CommittTransaction(ref Trans);
                    IsSaved = true;
                }
                else
                {
                    ObjTrans.RollBackTransaction(ref Trans);
                    IsSaved = false;
                }

            }
            catch (Exception)
            {
                ObjTrans.RollBackTransaction(ref Trans);
                throw;
            }

            return IsSaved;
        }


        public static string Get_BookingOrderData(string enquiryId, string dealerCode)
        {
            string json = "";
            var Serializer = new JavaScriptSerializer();
            List<BookOrdMasterVM> lst = new List<BookOrdMasterVM>();
            try
            {
                SqlParameter[] sqlParam = {
                                    new SqlParameter("@DealerCode",dealerCode),//1
									new SqlParameter("@BookRefNo",enquiryId)//0
									
									};

                dt = DataAccess.getDataTable("SP_Select_BookOrdMaster", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<BookOrdMasterVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return json;
        }

        public static string Get_VehicleDetailData(string enquiryId, string dealerCode)
        {
            string json = "";
            var Serializer = new JavaScriptSerializer();
            List<BookOrdVehDetailVM> lst = new List<BookOrdVehDetailVM>();
            try
            {
                SqlParameter[] sqlParam = {
                                    new SqlParameter("@DealerCode",dealerCode),//1
									new SqlParameter("@BookRefNo",enquiryId)//0
									
									};

                dt = DataAccess.getDataTable("SP_Select_BookOrdVehDetail", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<BookOrdVehDetailVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return json;
        }

        public static string Get_BookingReceiptInstrumentDetailData(string enquiryId, string dealerCode)
        {
            string json = "";
            var Serializer = new JavaScriptSerializer();
            List<BookingReceiptInstrumentDetailVM> lst = new List<BookingReceiptInstrumentDetailVM>();
            try
            {
                SqlParameter[] sqlParam = {
                                    new SqlParameter("@DealerCode",dealerCode),//1
									new SqlParameter("@BookRefNo",enquiryId)//0
									
									};

                dt = DataAccess.getDataTable("SP_Select_BookingReceiptInstrumentDetail", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<BookingReceiptInstrumentDetailVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return json;
        }

        public static string Get_BookingChkList(string enquiryId)
        {
            string json = "";
            var Serializer = new JavaScriptSerializer();
            List<DeliveryCheckListVM> lst = new List<DeliveryCheckListVM>();
            try
            {
                string sql = "Select BookChkListCode from BookOrdChklist where BookRefNo = '" + enquiryId + "'";

                dt = sysfun.GetData(sql,CConn.CConnection.GetConnectionString());

                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<DeliveryCheckListVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return json;

        }

        public static bool Delete_BookingOrder_Record(string enquiryId, string dealerCode)
        {
            DataSet ds = new DataSet();

            SqlParameter[] param = {
                new SqlParameter("@DealerCode",dealerCode),
                new SqlParameter("@BONo",enquiryId)
            };

            if (sysfun.ExecuteSP_NonQuery("sp_BookingOrder_Delete", param))
            {
                IsDeleted = true;
            }
            else
            {
                IsDeleted = false;
            }


            return IsDeleted;
        }

        public static bool Proceed_BookingOrder_Record(string enquiryId, string dealerCode)
        {
            DataSet ds = new DataSet();

            SqlParameter[] param = {
                new SqlParameter("@DealerCode",dealerCode),
                new SqlParameter("@BONo",enquiryId)
            };

            if (sysfun.ExecuteSP_NonQuery("SP_ProceedBookingOrder", param))
            {
                IsDeleted = true;
            }
            else
            {
                IsDeleted = false;
            }


            return IsDeleted;
        }

        public static bool Update_BookingNo(BookOrdMasterVM model, string dealerCode)
        {

            try
            {

                SqlParameter[] param = {
                                 new SqlParameter("@DealerCode",dealerCode),//0
								 new SqlParameter("@BookRefNo",model.BookRefNo),//1
								 new SqlParameter("@BookingNo",model.BookingNo),//2								 
								 new SqlParameter("@BookingDate",sysfun.SaveDate(model.BookingDate)),//3								 
								 new SqlParameter("@TentativeDate",sysfun.SaveDate(model.TentativeDate)),//4								 

                            };

                if (sysfun.ExecuteSP_NonQuery("SP_Update_BookingNo", param))
                {

                    IsSaved = true;

                }

            }
            catch (Exception)
            {
                throw;
            }

            return IsSaved;
        }

        public static List<GetProductSpVM> Get_BrandProductData(string enquiryId, string dealerCode)
        {

            var Serializer = new JavaScriptSerializer();
            List<GetProductSpVM> lst = new List<GetProductSpVM>();
            try
            {
                SqlParameter[] sqlParam = {
                                    new SqlParameter("@EnquiryId",enquiryId),//0
									new SqlParameter("@DealerCode",dealerCode)//1
									};

                dt = DataAccess.getDataTable("SP_BrandProduct_Select", sqlParam, CConn.CConnection.GetConnectionString().ToString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<GetProductSpVM>(dt);
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return lst;
        }
    }
}
