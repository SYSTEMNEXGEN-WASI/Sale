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
	public class DeliveryOrderMethods
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

		public static List<SelectListItem> GetDONo(string dealerCode)
		{
			List<SelectListItem> item = new List<SelectListItem>();

			List<StringNameValueClass> lst = new List<StringNameValueClass>();

			try
			{
				//var Serializer = new JavaScriptSerializer();
				SqlParameter[] sqlParam =
				{
					new SqlParameter("@DealerCode",dealerCode)
				};
				dt = DataAccess.getDataTable("SP_SelectDONo", sqlParam, General.GetBMSConString());
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

		public static string Get_DelChkList(string type,string dealerCode)
		{
			string json = "";
			var Serializer = new JavaScriptSerializer();
			List<DeliveryCheckListVM> lst = new List<DeliveryCheckListVM>();
			try
			{
				string sql = "Select A.DelChkListCode , A.DelChkListDesc , A.OptFlag from DelChkList A where A.DealerCode = '"+ dealerCode + "' and A.Type='"+type+"'";

				dt = sysfun.GetData(sql);

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


		public static List<SelectListItem> GetDealerEmployee(string DealerCode)
		{
			List<SelectListItem> item = new List<SelectListItem>();
			DataTable dt = new DataTable();
			
			List<StringNameValueClass> lst = new List<StringNameValueClass>();
			try
			{
				//var Serializer = new JavaScriptSerializer();
				SqlParameter[] sqlParam =
				{
					new SqlParameter("@DealerCode",DealerCode)
				};
				dt = DataAccess.getDataTable("Select_DealerEmplpoyee", sqlParam, General.GetBMSConString());
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

		public static string Get_DeliveryChkList(string enquiryId,string dealerCode,string type)
		{
			string json = "";
			var Serializer = new JavaScriptSerializer();
            SqlDataReader rder = null; ;
			List<DeliveryCheckListVM> lst = new List<DeliveryCheckListVM>();
            try
            {

                SqlParameter[] sqlParam = {
                    new SqlParameter("@DealerCode",dealerCode),//1
                    new SqlParameter("@JobCardCode",enquiryId)//1
                                        };
                if (sysfun.ExecuteSP("sp_DeliveryDocList", sqlParam, ref rder))
                {

                    while (rder.Read())
                    {
                          byte[] myimg = (byte[])rder["DocImage"];
                        lst.Add(new DeliveryCheckListVM
                        {
                            BookChkListCode = rder["DelChkListCode"].ToString(),
                            DelChkListDesc = rder["DelChkListDesc"].ToString(),
                            //ContentType = sdr["ContentType"].ToString(),
                            Image = "data:image/png;base64," + Convert.ToBase64String(myimg, 0, myimg.Length),
                        });
                        // Session["Image"] = "data:image/png;base64," + Convert.ToBase64String(myimg, 0, myimg.Length);


                    }



                }
                  json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }
            //try
            //{
            //	string sql = "Select DelChkListCode from VehicleDelChkList where DeliveryNo = '" + enquiryId + "' and DealerCode ='"+ dealerCode + "' and Type = '"+ type + "'";

            //	dt = sysfun.GetData(sql);

            //             if (dt != null && dt.Rows.Count > 0)
            //	{
            //		lst = EnumerableExtension.ToList<DeliveryCheckListVM>(dt);
            //	}
            //	json = Serializer.Serialize(lst);
            //}
            //catch (Exception ex)
            //{

            //	throw;
            //}

            return json;

		}


		public static List<SelectListItem> GetChassisNo(string vehType , string dealerCode)
		{
			List<SelectListItem> item = new List<SelectListItem>();
			DataTable dt = new DataTable();
			List<StringNameValueClass> lst = new List<StringNameValueClass>();
			try
			{
				//var Serializer = new JavaScriptSerializer();
				SqlParameter[] sqlParam =
				{
					new SqlParameter("@DealerCode",dealerCode),
					new SqlParameter("@VehType",vehType)
				};
				dt = DataAccess.getDataTable("SP_SelectChassisNo_FromVehType", sqlParam, General.GetBMSConString());
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

		public static bool Insert_DOMaster(VehicleDeliveryMasterVM model, string dealerCode)
		{

			try
			{
				if (model.DeliveryNo == "" || model.DeliveryNo == null)
				{
					strAutoCode = sysfun.AutoGen("VehicleDeliveryMaster", "DeliveryNo", DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MM/yyyy"), dealerCode);
					
				}
				else
				{

					strAutoCode = model.DeliveryNo;
					

				}
				SqlParameter[] param = {
								 new SqlParameter("@DealerCode",dealerCode),//0
								 new SqlParameter("@DeliveryNo",strAutoCode),//1
								 new SqlParameter("@DeliveryDate",sysfun.SaveDate(model.DeliveryDate)),//2								 
								 new SqlParameter("@BookRefNo",model.BookRefNo),//3								 
								 new SqlParameter("@UpdUser",AuthBase.UserId),//4
								 new SqlParameter("@UpdTerm",General.CurrentIP),//5
								 new SqlParameter("@Type",model.Type),//6
								 new SqlParameter("@ReceiverCode",model.ReceiverCode),//7
								 new SqlParameter("@Remarks",model.Remarks),//8
								 new SqlParameter("@Segment",model.Segment),//9
								 new SqlParameter("@Usage",model.Usage),//10
								 new SqlParameter("@CusCode",model.CusCode),//11
								 new SqlParameter("@VehTypeCode",model.VehTypeCode),//12
								 new SqlParameter("@CusContNo",model.CusContNo),//13
								 new SqlParameter("@EmpCode",model.EmpCode)
							};

				if (ObjTrans.BeginTransaction(ref Trans) == true)
				{
					sysfun.ExecuteSP_NonQuery("SP_VehicleDeliveryMaster_Insert", param, Trans);

					
					IsSaved = true;
				}

				//if (sysfun.ExecuteSP_NonQuery("SP_VehicleDeliveryMaster_Insert", param))
				//{

				//    IsSaved = true;

				//}

			}
			catch (Exception)
			{
				//ObjTrans.RollBackTransaction(ref Trans);
				throw;
			}

			return IsSaved;
		}

		public static bool Insert_DODetail(List<VehicleDeliveryDetailVM> model2, string dealerCode)
		{
            int count = 0;
			try
			{
                foreach (var item in model2)
                {
                    if (count >= 1 || item.BrandCode != null)
                    {
                        SqlParameter[] param2 = {
                                 new SqlParameter("@DealerCode",dealerCode),//0
								 new SqlParameter("@DeliveryNo",strAutoCode),//1
								 new SqlParameter("@BrandCode",item.BrandCode),//2								 
								 new SqlParameter("@ProdCode",item.ProdCode.Trim()),//3								 
								 new SqlParameter("@VersionCode",item.VersionCode.Trim()),//4
								 new SqlParameter("@ColorCode",item.ColorCode.Trim()),//5
								 new SqlParameter("@EngineNo",item.EngineNo.Trim()),//6
								 new SqlParameter("@ChasisNo",item.ChasisNo.Trim()),//7
								 new SqlParameter("@ProdRecNo",item.ProdRecNo.Trim()),//8
								 new SqlParameter("@LocCode",item.LocCode),//9
								 new SqlParameter("@SaleType",item.StockType),//10
								 new SqlParameter("@BookingNo",item.BookingNo),//11
								 new SqlParameter("@DocNo",item.DocumentNo)//12
								 
							};

                        if (sysfun.ExecuteSP_NonQuery("SP_VehicleDeliveryDetail_Insert", param2, Trans) == true)
                        {
                            string sql = "update VehicleStock set DeliveryFlag = 'Y' , InStockFlag = 'N' , OutDate = '"+General.CurrentDate+"' where" +
                                " BrandCode = '" + item.BrandCode + "' and ProdCode = '" + item.ProdCode + "' and VersionCode = '" + item.VersionCode + "' and EngineNo = '" + item.EngineNo + "'" +
                                " and ChasisNo = '" + item.ChasisNo + "' and RecNo = '" + item.ProdRecNo + "' and DealerCode = '" + dealerCode + "'";

                            sysfun.ExecuteQuery_NonQuery(sql, Trans);
                            IsSaved = true;
                        }
                        else
                        {
                            ObjTrans.RollBackTransaction(ref Trans);
                            IsSaved = false;
                        }
                    }
                    count++;
                }

			}
			catch (Exception)
			{
				ObjTrans.RollBackTransaction(ref Trans);
				throw;
			}

			return IsSaved;
		}


		public static string Get_DeliveryOrderData(string enquiryId, string dealerCode)
		{
			string json = "";
			var Serializer = new JavaScriptSerializer();
			List<RequestDeliveryOrderVM> lst = new List<RequestDeliveryOrderVM>();
			try
			{
				SqlParameter[] sqlParam = {
									new SqlParameter("@DealerCode",dealerCode),//1
									new SqlParameter("@DONo",enquiryId)//0
									
									};

				dt = DataAccess.getDataTable("SP_Select_DeliveryOrder", sqlParam, General.GetBMSConString());
				if (dt.Rows.Count > 0)
				{
					lst = EnumerableExtension.ToList<RequestDeliveryOrderVM>(dt);
				}
				json = Serializer.Serialize(lst);
			}
			catch (Exception ex)
			{

				throw;
			}

			return json;
		}

		//public static bool Insert_VehChkList(string strCheckedValues, string dealerCode)
		//{

		//	try
		//	{
		//		SqlParameter[] param2 = {
		//						 new SqlParameter("@DealerCode",dealerCode),//0
		//						 new SqlParameter("@Delivery",strAutoCode),//1
		//						 new SqlParameter("@DelChkListCode",strCheckedValues),//2								 
		//						 new SqlParameter("@Type","DO")//3
		//					};

		//		if (sysfun.ExecuteSP_NonQuery("SP_VehicleChkList_Insert", param2))
		//		{
		//			ObjTrans.CommittTransaction(ref Trans);
		//			IsSaved = true;
		//		}
		//		else
		//		{
		//			ObjTrans.RollBackTransaction(ref Trans);
		//			IsSaved = false;
		//		}

		//	}
		//	catch (Exception)
		//	{
		//		ObjTrans.RollBackTransaction(ref Trans);
		//		throw;
		//	}

		//	return IsSaved;
		//}

		public static bool Delete_DeliveryOrder_Record(string enquiryId, string dealerCode,string Chassis,SqlTransaction Trans)
		{
			DataSet ds = new DataSet();

			SqlParameter[] param = {
				new SqlParameter("@DealerCode",dealerCode),
				new SqlParameter("@DONo",enquiryId),
                new SqlParameter("@ChassisNo",Chassis)
            };
		   
				if (sysfun.ExecuteSP_NonQuery("sp_DeliveryOrder_Delete", param,Trans))
				{
					IsDeleted = true;
				}
				else
				{
					IsDeleted = false;
				}
			

			return IsDeleted;
		}

		public static string GetProRecDetail(string chassisNo, string dealerCode)
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
				dt = DataAccess.getDataTable("SP_Select_ChassisNoDetail", sqlParam, General.GetBMSConString());

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

        public static string GetDOModal(string dealerCode)
        {
            string json = "";
            List<SelectListItem> item = new List<SelectListItem>();

            var Serializer = new JavaScriptSerializer();
            List<RequestDeliveryOrderVM> lst = new List<RequestDeliveryOrderVM>();
            try
            {
                //var Serializer = new JavaScriptSerializer();
                SqlParameter[] sqlParam =
                {
                    new SqlParameter("@DealerCode",dealerCode)
                };
                dt = DataAccess.getDataTable("SP_Get_DOModal", sqlParam, General.GetBMSConString());

                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<RequestDeliveryOrderVM>(dt);
                }
                json = Serializer.Serialize(lst);

            }
            catch (Exception ex)
            {

                //throw;
            }
            return json;
        }

        public static List<RequestDeliveryOrderVM> GetDOModalList(string dealerCode)
        {
            
            List<RequestDeliveryOrderVM> lst = new List<RequestDeliveryOrderVM>();
            try
            {
                SqlParameter[] sqlParam =
                {
                    new SqlParameter("@DealerCode",dealerCode)
                };
                dt = DataAccess.getDataTable("SP_Get_DOModal", sqlParam, General.GetBMSConString());

                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<RequestDeliveryOrderVM>(dt);
                }
                

            }
            catch (Exception ex)
            {

                //throw;
            }
            return lst;
        }

        public static bool Insert_GatePass(GatePassVM model, string dealerCode,ref string msg)
        {

            try
            {
                if (model.GatePassCode == "" || model.GatePassCode == null)
                {
                    strAutoCode = sysfun.AutoGen("GatePassTemp", "GatePassCode", DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MM/yyyy"), dealerCode);

                }
                else
                {

                    strAutoCode = model.GatePassCode;
                    IsSaved = true;
                    return IsSaved;
                }

                SqlParameter[] GatePass_param = {                                            
           /*0*/ new SqlParameter("@DealerCode",SqlDbType.Char,5), 
           
           /*1*/ new SqlParameter("@GatePassCode",SqlDbType.Char,8),
          // /*2*/ new SqlParameter("@GatePassDate",SqlDbType.DateTime),     
           /*4*/ new SqlParameter("@TransCode",SqlDbType.VarChar,8),       
           /*3*/ new SqlParameter("@GatePassType",SqlDbType.Char,1),         
           /*4*/ new SqlParameter("@Remarks",SqlDbType.Char,100),          
            /*6*/ new SqlParameter("@Module",SqlDbType.VarChar,50),            
           /*7*/ new SqlParameter("@UpdUser",SqlDbType.Char,50),
            /*9*/ new SqlParameter("@UpdTerm",SqlDbType.Char,50),
           /*8*/ new SqlParameter("@DelFlag",SqlDbType.Char,1),
           /*4*/ new SqlParameter("@InvoiceNo",SqlDbType.VarChar,8),

            };
                GatePass_param[0].Value = dealerCode;
                GatePass_param[1].Value =strAutoCode;
                GatePass_param[2].Value = model.TransCode;
                GatePass_param[3].Value = "N";
                GatePass_param[4].Value = "";
                GatePass_param[5].Value = "Sales";
                GatePass_param[6].Value = AuthBase.UserId;
                GatePass_param[7].Value = GlobalVar.mUserIPAddress;
                GatePass_param[8].Value = "N";
                GatePass_param[9].Value = model.TransCode;
                if (sysfun.ExecuteSP_NonQuery("[Sp_Insert_GatePassTemp]", GatePass_param))
                {
                    IsSaved = true;
                }
            
               
            




        }
            catch (Exception ex)
            {
                msg = ex.Message;
                //ObjTrans.RollBackTransaction(ref Trans);
                //throw;
            }

            return IsSaved;
        }



        public static bool Insert_VehChkLists(List<DocumentCheckList> model, string dealerCode, ref string msg)
        {
            string[] a;
            byte[] imageBytes=null;

            try
            {
                if (model != null)
                {


                    foreach (var item in model)
                    {
                        if (item.DocChkListCode != "" || item.DocChkListCode != null)

                        {
                            if(item.Image!="" && item.Image != null)
                            {
                                a = item.Image.Split(',');
                                imageBytes = Convert.FromBase64String(a[1]);
                            }
                           
                            SqlParameter[] param2 = {
                                 new SqlParameter("@DealerCode",dealerCode),//0
								 new SqlParameter("@Delivery",strAutoCode),//1
								 new SqlParameter("@DelChkListCode",item.DocChkListCode),//2								 
                                 new SqlParameter("@DocImage",imageBytes)//2
							};
                            // SP_DeliveryChkList_Insert
                            if (sysfun.ExecuteSP_NonQuery("SP_DeliveryChkList_Insert", param2))
                            {
                                IsSaved = true;
                            }
                            else
                            {
                                ObjTrans.RollBackTransaction(ref Trans);
                                IsSaved = false;
                            }
                        }
                        else
                        {
                            ObjTrans.CommittTransaction(ref Trans);
                            IsSaved = true;
                        }


                    }
                    if (IsSaved == true)
                    {
                        ObjTrans.CommittTransaction(ref Trans);
                    }
                    

                }
                else
                {
                    if (Trans.Connection != null)
                    {
                        ObjTrans.CommittTransaction(ref Trans);
                    }
                    
                    IsSaved = true;
                }

            }
            catch (Exception ex)
            {
                ObjTrans.RollBackTransaction(ref Trans);
                // throw;
                msg = ex.Message;
            }

            return IsSaved;
        }


        /// hold Vehicle
        /// 
        public static bool Check_Hold(string chassisNo, string dealerCode)
        {
            DataSet ds = new DataSet();

            if (sysfun.IsExist("ChasisNo", chassisNo, "VehicleStock", dealerCode, "and HoldFlag='Y' and HoldAt='DeliveryOr' "))
            {
                return true;

            }

            return false;
        }

    }
}
