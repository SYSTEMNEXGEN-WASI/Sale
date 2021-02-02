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
using System.Web.Mvc;

namespace Core.CRM.ADO
{
   public class SaleTargetMethod
    {
        static bool IsSaved = false;
        static bool IsDeleted = false;
        static SqlParameter[] nullSqlParam = null;
        static DataTable dt = new DataTable();
        static string strAutoCode = string.Empty;
        static SysFunction sysfun = new SysFunction();

        public static string Select_SaleTarget(string dealerCode,ref string msg)
        {
            string json = "";
            List<SelectListItem> item = new List<SelectListItem>();

            var Serializer = new JavaScriptSerializer();
            List<SaleTargetVM> lst = new List<SaleTargetVM>();
            try
            {
                //var Serializer = new JavaScriptSerializer();
                SqlParameter[] sqlParam =
                {
                    new SqlParameter("@DealerCode",dealerCode),
                   
                };
                dt = DataAccess.getDataTable("SP_SelectSaleTarget", sqlParam, General.GetBMSConString());

                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<SaleTargetVM>(dt);
                }
                json = Serializer.Serialize(lst);

            }
            catch (Exception ex)
            {
                msg = ex.Message;
                //throw;
            }
            return json;
        }
        public static bool Insert_SaleTarget(SaleTargetVM model, ref string msg)
        {
            IsSaved = false;
            try
            {
                if (sysfun.IsExist("TargetYear", model.TargetYear, "VehicleSalesTarget",model.DealerCode, " and TargetMonth='"+model.TargetMonth+"' and TargetYear='"+model.TargetYear+"'"))
                {

                    strAutoCode = model.AutoNo;
                }
               else
                {

                    strAutoCode = sysfun.AutoGen("VehicleSalesTarget", "AutoNo", DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MM/yyyy"), model.DealerCode);


                }
                SqlParameter[] param = {
                                 new SqlParameter("@DealerCode",model.DealerCode),//0
								 new SqlParameter("@TargetYear",model.TargetYear),//1
								 new SqlParameter("@TargetMonth",model.TargetMonth),//2								 
								 new SqlParameter("@BrandCode",model.BrandCode),//3								 
								 new SqlParameter("@ProdCode",model.ProdCode),//4
								 new SqlParameter("@VersionCode",model.VersionCode),//5
								 new SqlParameter("@InvoiceTargetQTY",model.InvoiceTargetQty),//6
                                 new SqlParameter("@BookingTargetQTY",model.BookingTargetQty),//7
                                 new SqlParameter("@AllocationTargetQTY",model.AllocationTargetQty),//8
								 new SqlParameter("@UpdUser",AuthBase.UserId),//9
								 new SqlParameter("@UpdTerm",General.CurrentIP),//10
                                  new SqlParameter("@AutoNo",strAutoCode),//10
								
                            };

               
                    if (sysfun.ExecuteSP_NonQuery("SP_Insert_SaleTarget", param))
                    {
                        IsSaved = true;
                    }
                    else
                    {
                        return IsSaved;
                    }
              

          

            }
            catch (Exception ex)
            {
                //ObjTrans.RollBackTransaction(ref Trans);
                msg = ex.Message;
            }

            return IsSaved;
        }

    }
}
