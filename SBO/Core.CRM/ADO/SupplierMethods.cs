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
    public class SupplierMethods
    {
        static bool IsSaved = false;
        static bool IsDeleted = false;
        static SqlParameter[] nullSqlParam = null;
        static DataTable dt = new DataTable();
        static string strAutoCode = string.Empty;
        static SysFunction sysfun = new SysFunction();
        static Transaction ObjTrans = new Transaction();
        static SqlTransaction Trans;

        public static bool Insert_SupplierMaster(SupplierVM model)
        {

            try
            {
                if (model.VendorCode == "" || model.VendorCode == null)
                {
                    strAutoCode = sysfun.GetNewMaxID("Vendor", "VendorCode", 6, model.DealerCode);

                }
                else
                {
                    strAutoCode = model.VendorCode;

                }

                SqlParameter[] param = {
                                new SqlParameter("@DealerCode",SqlDbType.Char),//0
								new SqlParameter("@VendorCode",SqlDbType.Char),//1
								new SqlParameter("@VendorDesc",SqlDbType.VarChar),//2
								new SqlParameter("@Add1",SqlDbType.VarChar),//3
								new SqlParameter("@Add2",SqlDbType.VarChar),//4
								new SqlParameter("@Add3",SqlDbType.VarChar),//5
								new SqlParameter("@ContPerson ", SqlDbType.VarChar),//6
								new SqlParameter("@Phone1",SqlDbType.VarChar),//7
								new SqlParameter("@Phone2",SqlDbType.VarChar),//8
								new SqlParameter("@Fax ",SqlDbType.VarChar),//9
								new SqlParameter("@Email" ,SqlDbType.VarChar),//10
								new SqlParameter("@URL" ,SqlDbType.VarChar),//11
								new SqlParameter("@GSTno ",SqlDbType.VarChar),//12
								new SqlParameter("@NTN ",SqlDbType.VarChar),//13
								new SqlParameter("@PaymentTerm" ,SqlDbType.Float),//14
								new SqlParameter("@AccountCode",SqlDbType.VarChar),//15
								new SqlParameter("@AdvanceGiven" , SqlDbType.Float),//16
								new SqlParameter("@UpdUser ",SqlDbType.VarChar),//17
								new SqlParameter("@UpdTerm ",SqlDbType.VarChar),//18
								new SqlParameter("@CreditLimit",SqlDbType.Float),//19
                                new SqlParameter("@Title",SqlDbType.VarChar) //20
								};

                param[0].Value = model.DealerCode;
                param[1].Value = strAutoCode;
                param[2].Value = model.VendorDesc;
                param[3].Value = model.Add1;
                param[4].Value = model.Add2;
                param[5].Value = model.Add3;
                param[6].Value = model.ContPerson;
                param[7].Value = model.Phone1;
                param[8].Value = model.Phone2;
                param[9].Value = model.Fax;
                param[10].Value = model.Email;
                param[11].Value = model.URL;
                param[12].Value = model.GSTno;
                param[13].Value = model.NTN;
                param[14].Value = model.PaymentTerm == "" ? "0" : model.PaymentTerm;
                param[15].Value = model.AccountCode;
                param[16].Value = 0;
                param[17].Value = AuthBase.UserName; 
                param[18].Value = General.CurrentIP;
                param[19].Value = (model.CreditLimit == "" ? "0" : model.CreditLimit);
                param[20].Value = "Supplier";

                if (sysfun.ExecuteSP_NonQuery("Sp_Insert_Vendor", param))
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

        public static List<SupplierVM> GetSupplierModal(string dealerCode)
        {

            List<SupplierVM> lst = new List<SupplierVM>();
            try
            {
                SqlParameter[] sqlParam =
                {
                    new SqlParameter("@DealerCode",dealerCode)
                };
                dt = DataAccess.getDataTable("SP_Get_SupplierModal", sqlParam, General.GetBMSConString());

                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<SupplierVM>(dt);
                }


            }
            catch (Exception ex)
            {

                //throw;
            }
            return lst;
        }

        public static string Get_SupplierData(string enquiryId, string dealerCode)
        {
            string json = "";
            var Serializer = new JavaScriptSerializer();
            List<SupplierVM> lst = new List<SupplierVM>();
            try
            {

                string sql = "select * from Vendor where DealerCode='" + dealerCode + "' And  VendorCode = '" + enquiryId + "'";
                dt = sysfun .GetData(sql, "BMS0517ConnectionString");

                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<SupplierVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return json;
        }

    }
}
