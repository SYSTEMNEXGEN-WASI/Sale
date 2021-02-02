using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.CRM.Helper
{
    public class AuthBase
    {
       
        #region Admin Area Users

        public static string UserId
        {
            get
            {


                if (HttpContext.Current.Session["UserID"] != null && HttpContext.Current.Session["UserID"].ToString().Length > 0)

                    return HttpContext.Current.Session["UserID"].ToString();
                
                else
                    return "";
            }
            set
            {
                HttpContext.Current.Session["UserID"] = value;
            }
        }

        public static string UserName
        {
            get
            {


                if (HttpContext.Current.Session["UserName"] != null && HttpContext.Current.Session["UserName"].ToString().Length > 0)

                    return HttpContext.Current.Session["UserName"].ToString();

                else
                    return "";
            }
            set
            {
                HttpContext.Current.Session["UserName"] = value;
            }
        }

        public static string EmpCode
        {
            get
            {


                if (HttpContext.Current.Session["EmpCode"] != null && HttpContext.Current.Session["EmpCode"].ToString().Length > 0)

                    return HttpContext.Current.Session["EmpCode"].ToString();

                else
                    return "";
            }
            set
            {
                HttpContext.Current.Session["EmpCode"] = value;
            }
        }
        #endregion

        #region Agent
        public static int AgentId
        {
            get
            {
                if (HttpContext.Current.Session["agentId"] != null && HttpContext.Current.Session["agentId"].ToString().Length > 0)
                    return Convert.ToInt32(HttpContext.Current.Session["agentId"]);
                else
                    return 0;
            }
            set
            {
                HttpContext.Current.Session["agentId"] = value;
            }
        }
        #endregion
    }
}
