using Core.CRM.ADO.ViewModel;
using Core.CRM.Helper;
using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CRM_V3.WebForms.Sale
{
    public partial class CreditSaleInvoice : System.Web.UI.Page
    {

        protected void Page_Init(object sender, EventArgs e)
        {
            string DealerCode = Session["DealerCode"].ToString();
            var TransCode = Session["CrdSITransCode"];

            DataTable dt = new DataTable();
            ReportDocument Rd = new ReportDocument();

            string json = "";
            var Serializer = new JavaScriptSerializer();

            List<ISIDetailVM> lst = new List<ISIDetailVM>();
            try
            {
                SqlParameter[] sqlParam = {
                                    new SqlParameter("@DealerCode",DealerCode),//0
									new SqlParameter("@TransCode",TransCode)
                                    };

                dt = DataAccess.getDataTable("Sp_Report_CrdSIData", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<ISIDetailVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            Rd.Load(Path.Combine(Server.MapPath("~/Reports/Sale/CreditSaleInvoice.rpt")));
            Rd.SetDataSource(lst);
            CrystalReportViewerCrdSI.ReportSource = Rd;


        }


        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}