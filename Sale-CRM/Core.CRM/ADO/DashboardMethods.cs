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
    public class DashboardMethods
    {
        static SysFunction sysfun = new SysFunction();
        public static List<SalesPersonPerformanceVM> Select_SalesPersonPerformance(string dealerCode)
        {
            List<SalesPersonPerformanceVM> lst = new List<SalesPersonPerformanceVM>();
            DataTable dt = new DataTable();

            try
            {
                SqlParameter[] param =  
                {
                    new SqlParameter("@EmpCode",AuthBase.EmpCode),
                    new SqlParameter("@DealerCode",dealerCode)
                };

                dt = DataAccess.getDataTable("Select_SalesPersonPerformance ", param, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<SalesPersonPerformanceVM>(dt);
                }
            }
            catch (Exception ex)
            {
                
                throw;
            }

            return lst;
        }

        public static string Select_SalesPersonPerformanceWithDatesRange(string dealerCode , string FromDate , string ToDate)
        {
            List<SalesPersonPerformanceVM> lst = new List<SalesPersonPerformanceVM>();
            DataTable dt = new DataTable();
            string FDate = sysfun.SaveDate(FromDate).ToString();
            string TDate = sysfun.SaveDate(ToDate).ToString();
            string json = "";
           

            var Serializer = new JavaScriptSerializer();

            try
            {
                string sql = "exec SP_SalesPersonPerformanceWithDateRange '" + AuthBase.EmpCode + "' ,'" + dealerCode+ "','" + FDate + "','" + TDate + "'";

                dt = sysfun.GetData(sql);                

                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<SalesPersonPerformanceVM>(dt);
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
