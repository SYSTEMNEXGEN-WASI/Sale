using Core.CRM.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Core.CRM.ADO.ViewModel
{
    public class InstallmentPlanMethods
    {
        static DataTable dt = new DataTable();
        public static List<InstallmentPlanVM> Get_InstallmentData()
        {
            string json = "";
            var Serializer = new JavaScriptSerializer();
            string DealerCode = "MCM01";

            List<InstallmentPlanVM> lst = new List<InstallmentPlanVM>();
            try
            {
                SqlParameter[] sqlParam = {
									new SqlParameter("@DealerCode",DealerCode),//0
									
									};

                dt = DataAccess.getDataTable("SP_Get_InstallmentPlan", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<InstallmentPlanVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {
                throw;
            }

            return lst;
        }

        public static List<CustomerInstallmentScheduleVM> Get_InstallmentPlanSchedule(string BrandCode, string ProdCode, string VersionCode, string Color)
        {
            string json = "";
            var Serializer = new JavaScriptSerializer();
            string DealerCode = "MCM01";

            List<CustomerInstallmentScheduleVM> lst = new List<CustomerInstallmentScheduleVM>();
            try
            {
                SqlParameter[] sqlParam = {

                                    new SqlParameter("@DealerCode",DealerCode),//0
                                    new SqlParameter("@ProdCode",ProdCode),
                                    new SqlParameter("@VersionCode",VersionCode),
                                    new SqlParameter("@Color",Color)

                                    };

                dt = DataAccess.getDataTable("Sp_Get_CustomerInstallmentSchedule", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<CustomerInstallmentScheduleVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {
                throw;
            }

            return lst;
        }

        public static string Insert_InstallmentPlan(InstallmentPlanVM[] modelDetail)
        {
            var code = "MCM01";
            bool IsSaved = false;
            string Result = "NotSaved";
            SqlParameter[] nullSqlParam = null;
            string PlanID = "";

            try
            {
                foreach (var item in modelDetail)
                {
                    if (item.PlanID == null)
                    {
                        string getNextTransCode = "declare @lastval varchar(14),@id int " +
                                       "set @id = (select count(*) from InstallmentPlan) " +
                                       "set @id=@id+1 " +
                                       "if len(@id) = 1 " +
                                       "set @lastval='" + "IP" + "' + '-' +'0000' " +
                                       "if len(@id) = 2 " +
                                       "set @lastval='" + "IP" + "'+ '-' +'000' " +
                                       "if len(@id) >= 3 " +
                                       "set @lastval='" + "IP" + "'+ '-' +'00' " +
                                       "if len(@id) >= 4 " +
                                       "set @lastval='" + "IP" + "'+ '-' +'0' " +
                                       "declare @i varchar(14) " +
                                       "set @i = CAST(@id as varchar(14)) " +
                                       "set @lastval = @lastval+@i " +
                                       "select @lastval as PlanID";

                        dt = DataAccess.getDataTableByQuery(getNextTransCode, nullSqlParam, General.GetBMSConString());
                        PlanID = dt.Rows[0]["PlanID"].ToString();

                    }
                    else
                    {
                        PlanID = item.PlanID;
                    }
                    if (item.Active == null || item.Active == 0)
                    {
                        item.Active = 1;
                        item.TransferStatus = "T";
                        item.Remarks = "FormData";
                    }

                    SqlParameter[] sqlParam = {
                                     new SqlParameter("@DealerCode",code)
                                    ,new SqlParameter("@PlanID",PlanID)
                                    ,new SqlParameter("@PlanType",item.PlanType)
                                    ,new SqlParameter("@BrandCode",item.BrandCode)
                                    ,new SqlParameter("@ProdCode",item.ProdCode)
                                    ,new SqlParameter("@VersionCode",item.VersionCode)
                                    ,new SqlParameter("@ColorCode",item.ColorCode)
                                    ,new SqlParameter("@Color",item.Color)
                                    ,new SqlParameter("@MonthlyInstallment",item.MonthlyInstallment)
                                    ,new SqlParameter("@DownPayment",item.DownPayment)
                                    ,new SqlParameter("@NoOfInstallment",item.NoOfInstallment)
                                    ,new SqlParameter("@InstallmentPercentage",item.InstallmentPercentage)
                                    ,new SqlParameter("@StartEffectiveDate",item.StartEffectiveDate)
                                    ,new SqlParameter("@EndEffectiveDate",item.EndEffectiveDate)
                                    ,new SqlParameter("@Active",item.Active)
                                    ,new SqlParameter("@TransferStatus",item.TransferStatus)
                                    ,new SqlParameter("@Remarks",item.Remarks)

                                    };

                    dt = DataAccess.getDataTable("SP_Insert_InstallmentPlan", sqlParam, General.GetBMSConString());
                }


                if (dt.Rows.Count > 0)
                {
                    Result = "Saved";
                }



            }
            catch (Exception)
            {

                throw;
            }
            return Result;
        }
    }
}
