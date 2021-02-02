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
	public class VehReceiptMethods
	{
		static bool IsSaved = false;
		static bool IsDeleted = false;
		static SqlParameter[] nullSqlParam = null;
		static DataTable dt = new DataTable();
		static string strAutoCode = string.Empty;
		static SysFunction sysfun = new SysFunction();
        static DateTime recDate = new DateTime();
        static Transaction ObjTrans = new Transaction();
        static SqlTransaction Trans;
        static int i = 1;



        public static List<SelectListItem> GetRecNo(string dealerCode)
		{
			List<SelectListItem> item = new List<SelectListItem>();
			
			List<StringNameValueClass> lst = new List<StringNameValueClass>();

			//string dealerCode = "00166";
			try
			{
				//var Serializer = new JavaScriptSerializer();
				SqlParameter[] sqlParam =
				{
					new SqlParameter("@DealerCode",dealerCode)
				};
				dt = DataAccess.getDataTable("SP_SelectRecNo", sqlParam, General.GetBMSConString());
				if (dt.Rows.Count > 0)
				{
					lst = EnumerableExtension.ToList<StringNameValueClass>(dt);
				}
				//json = Serializer.Serialize(lst);

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

		

        public static List<SelectListItem> GetDatafromSP( string SP,string dealerCode)
        {
            List<SelectListItem> item = new List<SelectListItem>();
            DataTable dt = new DataTable();
            
            List<StringNameValueClass> lst = new List<StringNameValueClass>();
            try
            {
                //var Serializer = new JavaScriptSerializer();
                SqlParameter[] sqlParam =
                {
                    new SqlParameter("@DealerCode",dealerCode)
                };
                dt = DataAccess.getDataTable(SP, sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<StringNameValueClass>(dt);
                }
                //json = Serializer.Serialize(lst);

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



        public static bool Insert_ProdMaster(ProdReceiptVM model , string dealerCode,ref string msg)
		{
			
			try
			{
				if (model.RecNo == "0" || model.RecNo == "" || model.RecNo == null)
				{
                    strAutoCode = sysfun.AutoGen("ProdRecMaster", "RecNo", DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MM/yyyy"), model.DealerCode);
                    //strAutoCode = sysfun.GetNewMaxID("ProdRecMaster", "RecNo", 8, "00166");
					//recDate = DateTime.Parse( model.RecDate);
				}
				else
				{

					strAutoCode = model.RecNo;
					//string rec = model.RecDate;
					//recDate = DateTime.Parse(rec);

				}
				SqlParameter[] param = {
								 new SqlParameter("@DealerCode",model.DealerCode),//0
								 new SqlParameter("@RecNo",strAutoCode),//1
								 new SqlParameter("@RecDate",sysfun.SaveDate(model.RecDate)),//2								 
								 new SqlParameter("@DocumentNo",model.DocumentNo),//3								 
								 new SqlParameter("@UpdUser",AuthBase.UserId),//4
								 new SqlParameter("@UpdTerm",General.CurrentIP),//5
								 new SqlParameter("@VehTypeCode",model.VehTypeCode),//6
								 new SqlParameter("@VehRecCode",model.VehRecCode),//7
								 new SqlParameter("@Segment",model.Segment.Trim()),//8
								 new SqlParameter("@Category",model.Category.Trim()),//9
								 new SqlParameter("@Usage",model.Usage.Trim()),//10
                                 new SqlParameter("@VendorCode",model.VendorCode.Trim()),//11
                                 new SqlParameter("@Remarks",model.Remarks),//12
                                  new SqlParameter("@OEMAnnounceDate",sysfun.SaveDate(model.OEMAnnounceDate)),//12
                                   new SqlParameter("@TransportID",model.TransportID),//12
                                      new SqlParameter("@TotalVehicleAmount",model.TotalVehicleAmount)//12
                };

                if (ObjTrans.BeginTransaction(ref Trans) == true)
                {
                    sysfun.ExecuteSP_NonQuery("SP_ProdRecMaster_Insert", param, Trans);


                    IsSaved = true;
                }
             
			}
			catch (Exception ex)
			{
                msg = ex.Message;
                return false;
			}

			return IsSaved;
		}

		public static string Get_ProdRecMasterData(string enquiryId, string dealerCode)
		{
			string json = "";
			var Serializer = new JavaScriptSerializer();
			List<ProdReceiptVM> lst = new List<ProdReceiptVM>();
			try
			{
				SqlParameter[] sqlParam = {
									new SqlParameter("@EnquiryId",enquiryId),//0
									new SqlParameter("@DealerCode",dealerCode)//1
									};

				dt = DataAccess.getDataTable("SP_Select_ProdRecMaster", sqlParam, General.GetBMSConString());
				if (dt.Rows.Count > 0)
				{
					lst = EnumerableExtension.ToList<ProdReceiptVM>(dt);
				}
				json = Serializer.Serialize(lst);
			}
			catch (Exception ex)
			{

				throw;
			}

			return json;
		}

		public static bool Insert_ProdDetail(List<ProdReceiptDetailVM> model2 , string dealerCode, ref string msg)
		{
            int count = 0;
            string[] a;
            try
			{
                foreach (var item in model2)
                {
                    if (count >= 1 || item.BrandCode != null)
                    {

                        SqlParameter[] param2 = {
                                 new SqlParameter("@DealerCode",item.DealerCode),//0
								 new SqlParameter("@RecNo",strAutoCode),//1
								 new SqlParameter("@BrandCode",item.BrandCode),//2								 
								 new SqlParameter("@ProdCode",item.ProdCode.Trim()),//3								 
								 new SqlParameter("@VersionCode",item.VersionCode.Trim()),//4
								 new SqlParameter("@ColorCode",item.ColorCode.Trim()),//5
								 new SqlParameter("@EngineNo",item.EngineNo.Trim()),//6
								 new SqlParameter("@ChasisNo",item.ChasisNo.Trim()),//7
								 new SqlParameter("@StockType",item.StockType.Trim()),//8								 
								 new SqlParameter("@Milage",item.Milage),//9
								 new SqlParameter("@CusCode",item.CusCode),//10
                                 new SqlParameter("@BookingNo", SqlDbType.Char,15),//11                
								 new SqlParameter("@LocCode",item.LocCode),//12
                                 new SqlParameter("@BookRefNo",item.BookRefNo),//13
                                 new SqlParameter("@Amount",float.Parse(item.Amount)),//14
                                  new SqlParameter("@ModelYear",item.ModelYear),//14
                                 new SqlParameter("@FreightLoc",item.FreightLoc),//15
                                  new SqlParameter("@FreightCharge",float.Parse(item.FreightCharge)),//16
                                  new SqlParameter("@FCode",item.FCode),//17
                                  new SqlParameter("@StockValue",float.Parse(item.StockValue)),//18
                                 
                        };

                        if (item.StockType != "Open")
                        {
                            param2[11].Value = item.BookingNo;
                        }else
                        {
                            param2[11].Value = DBNull.Value;
                        }

                        if (sysfun.ExecuteSP_NonQuery("SP_ProdRecDetail_Insert", param2, Trans) == true)
                        {
                            if (item.FCode != "" && item.FCode != null)
                            {
                                a = item.FCode.Split('|');
                                if (a.Length>=1)
                                {
                                    foreach (var i in a)
                                    {
                                        SqlParameter[] param3 = {
                                 new SqlParameter("@DealerCode",dealerCode),//0
								 new SqlParameter("@BookRefNo",strAutoCode),//1
								 new SqlParameter("@BrandCode",item.BrandCode),//2								 
								 new SqlParameter("@ProdCode",item.ProdCode.Trim()),//3								 
								 new SqlParameter("@VersionCode",item.VersionCode),//4                            
                                 new SqlParameter("@ColorCode1",item.ColorCode),//9
                                 new SqlParameter("@FID",i),//9


                                    };
                                        if (sysfun.ExecuteSP_NonQuery("SP_Insert_ProdRecFeature", param3, Trans) == true)
                                        {
                                            IsSaved = true;

                                        }
                                        else
                                        {
                                            IsSaved = false;
                                        }
                                    }

                                }
                                

                            }
                            //
                            // IsSaved = true;
                        }
                        else
                        {
                            ObjTrans.RollBackTransaction(ref Trans);
                            return false;
                        }
                    }
                    count++;
                }              

                //ObjTrans.CommittTransaction(ref Trans);

            }
			catch (Exception ex)
			{

                ObjTrans.RollBackTransaction(ref Trans);
                msg = ex.Message;
                return false;
			}

			return IsSaved;
		}

        public static bool Insert_VehChkList(string strCheckedValues, string dealerCode , ref string msg)
        {

            try
            {
                SqlParameter[] param2 = {
                                 new SqlParameter("@DealerCode",dealerCode),//0
								 new SqlParameter("@Delivery",strAutoCode),//1
								 new SqlParameter("@DelChkListCode",strCheckedValues),//2								 
								 new SqlParameter("@Type","VR")//3
							};

                if (sysfun.ExecuteSP_NonQuery("SP_VehicleChkList_Insert", param2))
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
            catch (Exception ex)
            {
                ObjTrans.RollBackTransaction(ref Trans);
                msg = ex.Message;
                return false;
                //throw;
            }

            return IsSaved;
        }

        public static string Get_VehicleReceiptData(string enquiryId, string dealerCode)
		{
			string json = "";
			var Serializer = new JavaScriptSerializer();
			List<RequestVehicleReceiptVM> lst = new List<RequestVehicleReceiptVM>();
			try
			{
				SqlParameter[] sqlParam = {
									new SqlParameter("@DealerCode",dealerCode),//1
									new SqlParameter("@RecNo",enquiryId)//0
									
									};

				dt = DataAccess.getDataTable("SP_VehicleReceipt_Select", sqlParam, General.GetBMSConString());
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

		public static string Get_VehicleReceiptDetailData(string enquiryId, string dealerCode)
		{
			string json = "";
			var Serializer = new JavaScriptSerializer();
			List<RequestVehicleReceiptVM> lst = new List<RequestVehicleReceiptVM>();
			try
			{
				SqlParameter[] sqlParam = {
									new SqlParameter("@DealerCode",dealerCode),//1
									new SqlParameter("@RecNo",enquiryId)//0
									
									};

				dt = DataAccess.getDataTable("SP_Select_ProdRecDetail", sqlParam, General.GetBMSConString());
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

        public static bool Check_ChassisNo(string chassisNo, string dealerCode)
        {
            DataSet ds = new DataSet();
            
            if (sysfun.CodeExists("VehicleStock", "ChasisNo", chassisNo, dealerCode, ref ds))
            {
                return true;

            }          

            return false;
        }

        public static bool Check_EngineNo(string engineNo, string dealerCode)
        {
            DataSet ds = new DataSet();

            if (sysfun.CodeExists("VehicleStock", "EngineNo", engineNo, dealerCode, ref ds))
            {
                return true;

            }

            return false;
        }

        public static bool Delete_VehicleReceipt_Record(string enquiryId, string dealerCode)
		{
            DataSet ds = new DataSet();

            SqlParameter[] param = {
                new SqlParameter("@DealerCode",dealerCode),
                new SqlParameter("@RecNo",enquiryId)
            };
            if (sysfun.CodeExists("VehicleStock", "DeliveryFlag", "Y", dealerCode, ref ds))
            {
                return IsDeleted;
                
            }else
            {
                if (sysfun.ExecuteSP_NonQuery("sp_VehicleReceiptMaster_Delete", param))
                {
                    IsDeleted = true;
                }
                else
                {
                    IsDeleted = false;
                }
            }

			return IsDeleted;
		}

        public static bool Delete_VehicleReceiptDetail_Record(string enquiryId, string dealerCode, string ChasisNo, string EngineNo)
        {
            DataSet ds = new DataSet();

            SqlParameter[] param = {
                new SqlParameter("@DealerCode",dealerCode),
                new SqlParameter("@RecNo",enquiryId),
                new SqlParameter("@ChasisNo",ChasisNo),
                new SqlParameter("@EngineNo",EngineNo)
            };
            if (sysfun.IsExist("RecNo", enquiryId, "VehicleStock", dealerCode, "and  DeliveryFlag = 'Y'  and EngineNo = '"+EngineNo+"' and ChasisNo = '"+ChasisNo+"'"))
            {
                return IsDeleted;

            }
            else
            {
                if (sysfun.ExecuteSP_NonQuery("sp_VehicleReceiptDetail_Delete", param))
                {
                    IsDeleted = true;
                }
                else
                {
                    IsDeleted = false;
                }
            }

            return IsDeleted;
        }

        public static string GetDataFromBookingNo(string EnquiryId, string dealerCode)
        {
            string json = "";
            List<SelectListItem> item = new List<SelectListItem>();

            var Serializer = new JavaScriptSerializer();
            List<BookOrdVehDetailVM> lst = new List<BookOrdVehDetailVM>();
            try
            {
                //var Serializer = new JavaScriptSerializer();
                SqlParameter[] sqlParam =
                {
                    new SqlParameter("@DealerCode",dealerCode),
                    new SqlParameter("@BookingNo",EnquiryId)
                };
                dt = DataAccess.getDataTable("SP_Select_Data_From_BookingNo", sqlParam, General.GetBMSConString());

                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<BookOrdVehDetailVM>(dt);
                }
                json = Serializer.Serialize(lst);

            }
            catch (Exception ex)
            {

                //throw;
            }
            return json;
        }


        public static string GetVehRecModal(string dealerCode)
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
                    new SqlParameter("@DealerCode",dealerCode)
                };
                dt = DataAccess.getDataTable("SP_Get_VehRecModal", sqlParam, General.GetBMSConString());

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


        public static List<PrdouctVM> Get_BrandProductData(string enquiryId, string dealerCode,string VehicleCategory)
        {
            var Serializer = new JavaScriptSerializer();
            List<PrdouctVM> lst = new List<PrdouctVM>();
            try
            {
                SqlParameter[] sqlParam = {
                                    new SqlParameter("@EnquiryId",enquiryId),//0
									new SqlParameter("@DealerCode",dealerCode),//1
                                    new SqlParameter("@VehicleCategory",VehicleCategory)//1
									};

                dt = DataAccess.getDataTable("SP_BrandProduct_Select", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<PrdouctVM>(dt);
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return lst;
        }
        public static string Get_DelChkList(string dealerCode)
        {
            string json = "";
            var Serializer = new JavaScriptSerializer();
            List<DeliveryCheckListVM> lst = new List<DeliveryCheckListVM>();
            try
            {
                string sql = "Select A.DelChkListCode , A.DelChkListDesc , A.OptFlag from DelChkList A where A.DealerCode = '" + dealerCode + "'";

                dt = sysfun.GetDatas(sql, "DMIS");

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


        public static string Get_VehicleReceiptVouchNo(string enquiryId, string dealerCode)
        {
            string json = "";
            var Serializer = new JavaScriptSerializer();
            List<RequestVehicleReceiptVM> lst = new List<RequestVehicleReceiptVM>();
            try
            {
                SqlParameter[] sqlParam = {
                                    new SqlParameter("@DealerCode",dealerCode),//1
									new SqlParameter("@RecNo",enquiryId)//0
									
									};

                dt = DataAccess.getDataTable("SP_Select_ProdRecVouchNo", sqlParam, CConn.CConnection.GetConnectionString());
                if (dt.Rows.Count > 0)
                {
                    // lst = EnumerableExtension.ToList<RequestVehicleReceiptVM>(dt);
                    json = dt.Rows[0]["VoucherNo"].ToString();
                }
                // json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return json;
        }
    }
}
