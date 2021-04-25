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
    

    public class DealerMethods
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
        public static string Get_DealerData(string dealerCode)
        {
            string json = "";
            var Serializer = new JavaScriptSerializer();
            List<DealerInfoVM> lst = new List<DealerInfoVM>();
            DataSet ds = new DataSet();
            try
            {
                string sql = "Select DealerCode,DealerDesc,SaleTaxNo,NTN,PST,Logo,Address1,Phone1,Email,Fax from Dealer where DealerCode='" + dealerCode + "'";
                dt = sysfun.GetData(sql, "BMS0517ConnectionString");
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<DealerInfoVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return json;
        }
        public static bool Insert_Dealer(DealerVM model, string dealerCode)
        {
            try
            {

                
                    string sql = "UPDATE Dealer SET [DealerDesc] = '" + model.DealerDesc + "' ,[Address1] = '" + model.Address1 + "',[SaleTaxNo] = '" + model.SaleTaxNo + "',[Phone1] = '" + model.Phone1+ "',[Phone2] = '" + model.Phone2 + "',[Email] = '" + model.Email + "',[Fax] = '" + model.Fax + "',[NTN] = '" + model.NTN + "'" +
              ",[UpdUser] = '" + AuthBase.UserId + "',[UpdDate] = '" + sysfun.SaveDate(DateTime.Now.ToString("dd/MM/yyyy")) + "',[UpdTerm] = '" + GlobalVar.mUserIPAddress + "'" +
              ",[Logo] = '" + model.Logo + "'  WHERE DealerCode = '" + model.DealerCode + "'";

                dt = sysfun.GetData(sql,"BMS0517ConnectionString");
                IsSaved = true;
              



                }
            catch (Exception ex)
            {
                //ObjTrans.CommittTransaction(ref Trans);
                IsSaved = false;
            }

            return IsSaved;
        }

      

      
      
    }
}
