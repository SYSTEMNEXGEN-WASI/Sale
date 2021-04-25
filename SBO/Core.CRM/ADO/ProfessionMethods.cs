using Core.CRM.ADO.ViewModel;
using Core.CRM.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Core.CRM.ADO
{
    public class ProfessionMethods
    {
        static SysFunction sysfun = new SysFunction();
        static DataTable dt = new DataTable();

        static string strAutoCode = string.Empty;
        static string autoProspect_ID = string.Empty;

        public static bool Insert_ProfessionType(ProfessionTypeVM objects, string s)
        {
            throw new NotImplementedException();
        }
        static bool IsSaved = false;
        static SqlParameter[] nullSqlParam = null;

        public static string Get_ProfessionTypeData()
        {
            string json = "";
            var Serializer = new JavaScriptSerializer();
            List<ProfessionTypeVM> lst = new List<ProfessionTypeVM>();
            try
            {
                string sql = "Select P.ProfCode , P.ProfDesc from Profession P";

                dt = sysfun.GetData(sql, "BMS0517ConnectionString");

                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<ProfessionTypeVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return json;

        }

        public static bool Insert_ProfessionType(ProfessionTypeVM model)
        {

            try
            {
                if (string.IsNullOrEmpty(model.ProfCode))
                {
                    strAutoCode = sysfun.GetNewMaxID("Profession", "ProfCode", 5, "COMON");
                }
                else
                {
                    strAutoCode = model.ProfCode;
                }


                SqlParameter[] param = {
                                 new SqlParameter("@DealerCode",model.DealerCode),//0
								 new SqlParameter("@ProfTypeCode",strAutoCode),//1
								 new SqlParameter("@ProfTypeDesc",model.ProfDesc),//2		 
								 new SqlParameter("@UpdUser",AuthBase.UserId),//3
								 new SqlParameter("@UpdTerm",General.CurrentIP)//4
								 
							};
                dt = DataAccess.getDataTable("Sp_Insert_ProfessionType", param, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {

                }
                IsSaved = true;
            }
            catch (Exception)
            {

                throw;
            }

            return IsSaved;
        }
    }
}
