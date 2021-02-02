using Core.CRM.ADO.ViewModel;
using Core.CRM.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Core.CRM.ADO
{
    public class VendorMethods
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

        public static bool Insert_Vendor(VendorVM model, string dealerCode)
        {
            string csgNo;
            try
            {
                if (model.VendorCode == "" || model.VendorCode == null)
                {
                    strAutoCode = sysfun.GetNewMaxIDwithoutDealerCode("Vendor", "VendorCode", 6, "");

                }

                else
                {
                    strAutoCode = model.VendorCode;

                }
                SqlParameter[] param = {
                               new SqlParameter("@DealerCode",dealerCode),//0
							   new SqlParameter("@VendorCode",strAutoCode),//1
							   new SqlParameter("@VendorDesc",model.VendorDesc),//2
							  // new SqlParameter("@FatherHusName",(object)DBNull.Value),//3
							   new SqlParameter("@Add1",model.Add1),//4
							   new SqlParameter("@Add2",(object)DBNull.Value),//5
							   new SqlParameter("@Add3",(object)DBNull.Value),//6
							   new SqlParameter("@ContPerson",model.ContPerson),//7
							   new SqlParameter("@PaymentTerm",model.PaymentTerm ),//8
							   new SqlParameter("@CreditLimit",model.CreditLimit ),//9
							  // new SqlParameter("@NIC",model.NIC),//10
							 //  new SqlParameter("@VendorType",model.VendorType),//11
							  // new SqlParameter("@DOB",(object)DBNull.Value ),//12
							   new SqlParameter("@Phone1",model.Phone1),//13
							   new SqlParameter("@Phone2",model.Phone2),//14
							 //  new SqlParameter("@CellNo",model.CellNo),//15
							   new SqlParameter("@Fax",model.Fax),//16
							   new SqlParameter("@Email",model.Email),//17
							   new SqlParameter("@URL",model.URL),//18
							   new SqlParameter("@NTN",model.NTN),//19
							 //  new SqlParameter("@AdvanceReceipt",(object)DBNull.Value ),//20
							   new SqlParameter("@UpdUser",AuthBase.UserId),//21
							   new SqlParameter("@UpdTerm",General.CurrentIP),//22
							   new SqlParameter("@GSTno",model.GSTno),//23
							  // new SqlParameter("@Behavior",(object)DBNull.Value),//24
							  // new SqlParameter("@Remarks",(object)DBNull.Value),//25
							  // new SqlParameter("@Distance",model.Distance),//26
							  // new SqlParameter("@CountryCode", model.CountryCode),//27
							  // new SqlParameter("@CityCode", model.CityCode),//28
							  // new SqlParameter("@StateCode", model.StateCode),//29
							   new SqlParameter("@Title", model.Title),//30
							 //  new SqlParameter("@CSGNo", csgNo),//31
							//   new SqlParameter("@MCNo", model.MCNo),//32
                               new SqlParameter("@AccountCode", model.AccountCode), // 33
                               new SqlParameter("@VendorRecievable", model.VendorRecievable), // 34
                               new SqlParameter("@VendorPayable", model.VendorPayable) // 35
                               };

                if (sysfun.ExecuteSP_NonQuery("sp_Insert_VendorNew", param))
                {

                    IsSaved = true;
                }

            }
            catch (Exception ex)
            {
                throw;
            }

            return IsSaved;
        }



        public static string Get_VendorData(string enquiryId, string dealerCode)
        {
            string json = "";
            var Serializer = new JavaScriptSerializer();
            List<VendorVM> lst = new List<VendorVM>();
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] sqlParam = {
                                    new SqlParameter("@DealerCode",dealerCode),//1
									new SqlParameter("@VendorCode",enquiryId)//0
									
									};

                //dt = DataAccess.getDataTable("SP_Select_DeliveryOrder", sqlParam, General.GetBMSConString());
                sysfun.CodeExists("Vendor", "VendorCode", enquiryId, dealerCode, ref ds);

                dt = ds.Tables[0];


                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<VendorVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return json;
        }



        //public static bool Delete_Vendor_Record(string enquiryId, string dealerCode)
        //{
        //    DataSet ds = new DataSet();

        //    if (sysfun.IsExist("VendorCode", enquiryId, "Vendor", dealerCode, "") == false)
        //    {

        //        return false;
        //    }
        //    if (sysfun.IsExist("VendorCode", enquiryId, "BookOrdMaster", dealerCode, "") == true)
        //    {

        //        return false;
        //    }

        //    // If Vendor used in Invoiced name in booking order then should not be delete
        //    if (sysfun.IsExist("DisplayCode", enquiryId.Trim(), "BookOrdMaster", dealerCode, "") == true)
        //    {

        //        return false;
        //    }

        //    //If Vendor has vehicle in Vendor vheicle table then Vendor should not be delete
        //    //{
        //    if (sysfun.IsExist("VendorCode", enquiryId.Trim(), "VendorVehicle", dealerCode, "") == true)
        //    {

        //        return false;
        //    }
        //    //}

        //    //If Vendor used in Jobcard then should not be delete
        //    //{
        //    if (sysfun.IsExist("VendorCode", enquiryId.Trim(), "JobCardMaster", dealerCode, "") == true)
        //    {

        //        return false;
        //    }
        //    //}

        //    //If Vendor used in Counter then should not be delete
        //    //{
        //    if (sysfun.IsExist("VendorCode", enquiryId.Trim(), "CountersaleMaster", dealerCode, "") == true)
        //    {

        //        return IsDeleted = false; ;
        //    }

        //    SqlParameter[] param = {
        //        new SqlParameter("@DealerCode",dealerCode),
        //        new SqlParameter("@VendorCode",enquiryId)
        //    };

        //    if (sysfun.ExecuteSP_NonQuery("sp_Sales_Delete_Vendor", param))
        //    {
        //        IsDeleted = true;
        //    }
        //    else
        //    {
        //        IsDeleted = false;
        //    }


        //    return IsDeleted;
        //}

        public static List<VendorVM> GetVendorModal(string dealerCode)
        {
            List<VendorVM> lst = new List<VendorVM>();
            try
            {
                DataTable dt = new DataTable();

                SqlParameter[] sqlParam =
                    {
                    new SqlParameter("@DealerCode",dealerCode)
                };
                dt = DataAccess.getDataTable("SP_Select_VendorModal", sqlParam, General.GetBMSConString());

                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<VendorVM>(dt);
                }

            }
            catch (Exception ex)
            {

                //throw;
            }
            return lst;
        }
    }
}
