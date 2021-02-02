using CConn;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Core.CRM.ADO.ViewModel;

namespace Core.CRM.ADO
{
    public class SecurityBll
    {
        SysFunction myfunc = new SysFunction();
        static DataTable SecurityDT = new DataTable();
        static DataTable SecurityDT_Add = new DataTable();
        static DataTable SecurityDT_Edit = new DataTable();
        static DataTable SecurityDT_Delete = new DataTable();
        static DataTable SecurityDT_Post = new DataTable();
        static DataTable SecurityDT_Print = new DataTable();

        static DataTable dt = new DataTable();
        static string LoginName;
        static string CompanyCode;
        static string ApplicationCode;
        public bool LoginVerification(string username,string password,ref DataTable dt)
        {
            try
            {
                //var Serializer = new JavaScriptSerializer();
                SqlParameter[] sqlParam =
                {
                    new SqlParameter("@LoginName",username),
                    new SqlParameter("@Password",password)
                };
                dt = SysFunction.getDataTable("SP_LoginVerification_Sale", sqlParam,CConnection.GetSecurityConnectionString());

                if (dt != null && dt.Rows.Count > 0)
                {
                    
                    return true;
                }               

            }
            catch (Exception ex)
            {
                return false;
            }

            return false;
        }

       
        public void Check_Action_ImageButton(string Formname, ImageButton btn, string action)
        {
            if (SecurityDT != null)
            {
                DataRow[] drr = SecurityDT.Select();
                for (int i = 0; i < drr.Length; i++)
                {

                    if (SecurityDT_Add.Rows[i]["FormName"].ToString().ToUpper() == Formname.ToUpper() && SecurityDT.Rows[i]["FormActionName"].ToString().ToUpper() == action.ToUpper())
                    {
                        btn.Enabled = true;
                    }
                }
            }
        }
        public void Check_Action(string Formname,Button btn, string action)
        {
            if (action == "Add")
            {
                if (SecurityDT_Add != null)
                {
                    DataRow[] drr = SecurityDT_Add.Select();
                    for (int i = 0; i < drr.Length; i++)
                    {

                        if (SecurityDT_Add.Rows[i]["FormName"].ToString().ToUpper() == Formname.ToUpper() && SecurityDT_Add.Rows[i]["FormActionName"].ToString().ToUpper() == action.ToUpper())
                        {
                            btn.Enabled = true;
                        }
                    }
                }
            }
            else if (action == "Edit")
            {
                if (SecurityDT_Edit != null)
                {
                    DataRow[] drr = SecurityDT_Edit.Select();
                    for (int i = 0; i < drr.Length; i++)
                    {

                        if (SecurityDT_Edit.Rows[i]["FormName"].ToString().ToUpper() == Formname.ToUpper() && SecurityDT_Edit.Rows[i]["FormActionName"].ToString().ToUpper() == action.ToUpper())
                        {
                            btn.Enabled = true;
                        }
                    }
                }

            }
            else if (action == "Delete")
            {
                if (SecurityDT_Delete != null)
                {
                    DataRow[] drr = SecurityDT_Delete.Select();
                    for (int i = 0; i < drr.Length; i++)
                    {

                        if (SecurityDT_Add.Rows[i]["FormName"].ToString().ToUpper() == Formname.ToUpper() && SecurityDT_Delete.Rows[i]["FormActionName"].ToString().ToUpper() == action.ToUpper())
                        {
                            btn.Enabled = true;
                        }
                    }
                }

            }
            else if (action == "Print")
            {
                if (SecurityDT_Print != null)
                {
                    DataRow[] drr = SecurityDT_Print.Select();
                    for (int i = 0; i < drr.Length; i++)
                    {

                        if (SecurityDT_Add.Rows[i]["FormName"].ToString().ToUpper() == Formname.ToUpper() && SecurityDT_Print.Rows[i]["FormActionName"].ToString().ToUpper() == action.ToUpper())
                        {
                            btn.Enabled = true;
                        }
                    }
                }

            }
            else if (action == "Post")
            {
                if (SecurityDT_Post != null)
                {
                    DataRow[] drr = SecurityDT_Post.Select();
                    for (int i = 0; i < drr.Length; i++)
                    {

                        if (SecurityDT_Add.Rows[i]["FormName"].ToString().ToUpper() == Formname.ToUpper() && SecurityDT_Post.Rows[i]["FormActionName"].ToString().ToUpper() == action.ToUpper())
                        {
                            btn.Enabled = true;
                        }
                    }
                }
            }





            }


        public bool UserRight(string FormCode, string ActionCode)
        {
            bool Access = false;
            if (HttpContext.Current.Session["UserRights"] != null)
            {
                var Rights = HttpContext.Current.Session["UserRights"] as List<SecurityUserAccess>;
                if (Rights.Count > 0)
                {
                    foreach (var i in Rights)
                    {
                        if (i.FormCode.Trim() == FormCode.Trim() && i.FormActionCode.Trim() == ActionCode.Trim() && i.ActionValue == "Y")
                        {
                            Access = true;
                            return Access;
                        }
                    }
                }
            }
            return Access;
        }
        public static bool UserRights(string FormCode, string ActionCode)
        {
            bool Access = false;
            if (System.Web.HttpContext.Current.Session["UserRights"] != null)
            {
                var Rights = System.Web.HttpContext.Current.Session["UserRights"] as List<SecurityUserAccess>;
                if (Rights.Count > 0)
                {
                    foreach (var i in Rights)
                    {
                        if (i.FormCode.Trim() == FormCode.Trim() && i.FormActionCode.Trim() == ActionCode.Trim() && i.ActionValue == "Y")
                        {
                            Access = true;
                            return Access;
                        }
                    }
                }
            }
            return Access;
        }

    }
    }
