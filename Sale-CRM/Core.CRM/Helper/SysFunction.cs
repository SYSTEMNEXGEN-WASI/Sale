using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CConn;
using Microsoft.ApplicationBlocks.Data;
using System.Data;
using System.Web;
using System.Drawing;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;
using System.Globalization;
using System.Web.Mvc;
using Core.CRM.Helper;

/// <summary>
/// Summary description for SysFunction
/// </summary>
public class SysFunction
{
	string StrCCon = "";

	public SysFunction()
	{
		//
		// TODO: Add constructor logic here
		//
		StrCCon = CConnection.GetConnectionString();
	}

    public static double CustomCDBL(object txt)
    {
        if (object.ReferenceEquals(txt, DBNull.Value)) return 0;
        else if (txt == null) return 0;
        else if (string.IsNullOrEmpty(txt.ToString().Trim())) return 0;
        else return Math.Round(Convert.ToDouble(txt), 2);
    }
    public void GetMultiColumnsDDL(DropDownList ddlEmp, string[] Columns, string TableName, string WhereClause, string DataValueField, string Orderby, bool boolAddDistinct, bool boolGroupbyClause, string StrHavingClause = "")
	{
		SqlDataReader Reader = null;
		string strSql = "WITH maxLen AS (SELECT ";
		int CountLen = 0;
		foreach (string s in Columns)
		{
			strSql = strSql + "max(len(rtrim(" + s + "))) Len" + CountLen + ",";

			CountLen++;
		}
		strSql = strSql.Remove(strSql.Length - 1, 1) + " ";
		strSql = strSql + "FROM " + TableName + " with(nolock)  ";
		if (WhereClause != string.Empty)
		{
			strSql = strSql + " Where " + WhereClause + ") ";
		}
		if (boolAddDistinct == true)
			strSql = strSql + "SELECT Distinct " + Columns[0].ToString() + ",";// +DataValueFieldColumns[0].ToString();
		else
			strSql = strSql + "SELECT " + Columns[0].ToString() + ",";// +DataValueFieldColumns[0].ToString();
		CountLen = 0;
		foreach (string s in Columns)
		{
			//if (Columns[0] != s)
			//{
			if (Columns[0] == s) //if (Columns[1] == s)
			{
				strSql = strSql + " + ' | '  + (rtrim(" + s + ") + space((1+maxLen.Len" + CountLen + ")-len(rtrim(" + s + "))) ";
			}
			else
			{
				strSql = strSql + " + ' | '  + rtrim(" + s + ") + space((1+maxLen.Len" + CountLen + ")-len(rtrim(" + s + "))) ";
			}

			//}
			CountLen++;

		}
		strSql = strSql + ") As Name from " + TableName + " with(nolock),maxLen  ";
		if (WhereClause != string.Empty)
		{
			strSql = strSql + " Where " + WhereClause + " ";
		}
		if (boolGroupbyClause == true)
		{
			string strGroupBy = "Group by ";
			CountLen = 0;
			foreach (string s in Columns)
			{
				strGroupBy = strGroupBy + s + "," + "maxLen.Len" + CountLen + ",";
				CountLen++;
			}
			strGroupBy = strGroupBy.Substring(0, strGroupBy.Length - 1);
			strSql = strSql + strGroupBy + " ";
		}
		strSql = strSql + StrHavingClause + " ";
		strSql = strSql + Orderby;
		if (Reader != null && Reader.IsClosed == false) Reader.Close();
		ExecuteQuery(strSql, ref Reader);
		ddlEmp.Items.Clear();
		ListItem item = new ListItem();
		item.Text = "Select";
		item.Value = "0";
		ddlEmp.Items.Add(item);
		while (Reader.Read())
		{
			StringWriter myWriter = new StringWriter();
			HttpUtility.HtmlDecode(Reader["NAME"].ToString().Trim(), myWriter);//ddlEmp.Items.Add(myWriter.ToString());  Replace(" ", "&nbsp;")
			item = new ListItem();
			item.Text = myWriter.ToString();
			item.Value = Reader[DataValueField].ToString();
			ddlEmp.Items.Add(item);
		}
		Reader.Close();
	}

    public string GetStringValuesAgainstCodes(string ItemCodeColomn, string ItemCode, string ValueItemColomn, string ItemTable, string Append, string DealerCode)
    {
        string functionReturnValue = string.Empty;
        SqlDataReader drItemVal = null;
        string strQuerry = null;
        if(DealerCode == "")
        {
            strQuerry = " Select " + ValueItemColomn + " From [" + ItemTable + "] Where " + ItemCodeColomn + " = '" + ItemCode + "' " + Append;
        }else
        {
            strQuerry = " Select " + ValueItemColomn + " From [" + ItemTable + "] Where " + ItemCodeColomn + " = '" + ItemCode + "' AND [DealerCode] = '" + DealerCode + "' " + Append;
        }
        

        if (ExecuteQuery(strQuerry, ref drItemVal))
        {
            try
            {
                if (drItemVal.HasRows)
                {
                    drItemVal.Read();
                    string sTempItem = Convert.ToString(GetNullString(drItemVal[0]));
                    drItemVal.Close();
                    functionReturnValue = sTempItem;
                }
                else
                {
                    return functionReturnValue;
                }

            }
            catch (Exception ex)
            {
                //ShowError();
                drItemVal.Close();
            }
        }
        return functionReturnValue;
    }

    public string CustomerAutoGen(string Table, string Col, string txtDt, string DealerCode)
    {
        string functionReturnValue = null;
        string Sql_Auto = "";
        SqlDataReader drAuto = null;
        string AutoNum = null;
        //string Sql_Auto = "Select Max(" + Col + ") as '" + Col + "' from " + Table + "" + " Where Left(" + Col + ",2) = '" + Strings.Right(txtDt, 2) + "'" + " Order By " + Col;
        Sql_Auto = "Select Max(" + Col + ") as '" + Col + "' from " + Table + " with(nolock) " + " Where DealerCode='" + DealerCode + "' And SUBSTRING(" + Col + ",7,2) = '" + txtDt.Substring(txtDt.Length - 2) + "'" + " Order By " + Col;
        if (drAuto != null) if (drAuto.IsClosed == false) drAuto.Dispose();

        if (ExecuteQuery(Sql_Auto, ref drAuto) == true)
        {
            try
            {
                drAuto.Read();
                AutoNum = drAuto[Col].ToString();
                if (AutoNum.Trim() != "")
                {
                    AutoNum = AutoNum.Substring(AutoNum.Length - 8);
                    AutoNum = (Convert.ToInt32(AutoNum) + 1).ToString();
                    while (!(AutoNum.Length == 8))
                    {
                        AutoNum = "0" + AutoNum;
                    }
                    functionReturnValue = DealerCode +'-'+ txtDt.Substring(txtDt.Length - 2) + "-" + AutoNum;
                }
                else
                {
                    functionReturnValue = DealerCode + '-' + txtDt.Substring(txtDt.Length - 2) + "-" + "00000001";
                }
                drAuto.Close();
            }
            catch (Exception ex)
            {
                functionReturnValue = DealerCode + '-' + txtDt.Substring(txtDt.Length - 2) + "-" + "00000001";
                drAuto.Close();
            }
        }
        else
        {
            functionReturnValue = DealerCode + '-' + txtDt.Substring(txtDt.Length - 2) + "-" + "00000001";
        }
        return functionReturnValue;
    }

    public string AutoGen(string Table, string Col, string txtDt,string DealerCode)
    {
        string functionReturnValue = null;
        string Sql_Auto = "";
        SqlDataReader drAuto = null;
        string AutoNum = null;
        //string Sql_Auto = "Select Max(" + Col + ") as '" + Col + "' from " + Table + "" + " Where Left(" + Col + ",2) = '" + Strings.Right(txtDt, 2) + "'" + " Order By " + Col;
        Sql_Auto = "Select Max(" + Col + ") as '" + Col + "' from " + Table + " with(nolock) " + " Where DealerCode='" + DealerCode + "' And Left(" + Col + ",2) = '" + txtDt.Substring(txtDt.Length - 2) + "'" + " Order By " + Col;
        if (drAuto != null) if (drAuto.IsClosed == false) drAuto.Dispose();

        if (ExecuteQuery(Sql_Auto, ref drAuto) == true)
        {
            try
            {
                drAuto.Read();
                AutoNum = drAuto[Col].ToString();
                if (AutoNum.Trim() != "")
                {
                    AutoNum = AutoNum.Substring(AutoNum.Length - 5);
                    AutoNum = (Convert.ToInt32(AutoNum) + 1).ToString();
                    while (!(AutoNum.Length == 5))
                    {
                        AutoNum = "0" + AutoNum;
                    }
                    functionReturnValue = txtDt.Substring(txtDt.Length - 2) + "" + AutoNum;
                }
                else
                {
                    functionReturnValue = txtDt.Substring(txtDt.Length - 2) + "00001";
                }
                drAuto.Close();
            }
            catch (Exception ex)
            {
                functionReturnValue = txtDt.Substring(txtDt.Length - 2) + "00001";
                drAuto.Close();
            }
        }
        else
        {
            functionReturnValue = txtDt.Substring(txtDt.Length - 2) + "00001";
        }
        return functionReturnValue;
    }


    public bool ExecuteQuery(string sQuery, ref SqlDataReader dr)
	{
		bool flag;
		try
		{

			dr = SqlHelper.ExecuteReader(StrCCon, CommandType.Text, sQuery);

			if (dr.HasRows) flag = true; else flag = false;
		}
		catch (Exception exception) { throw exception; }
		return flag;

	}

	/* new methods starts general methods  */

	public DataTable GetData(string SQL)
	{
		try
		{
			////if (StrCCon == String.Empty)
			//{
			//    GetConnection();
			//}

			SqlDataAdapter dta = new SqlDataAdapter(SQL, StrCCon);
			DataTable dt = new DataTable();
			dta.Fill(dt);

			return dt;
		}
		catch (Exception e)
		{
			//GlobalVar.mErrorMessage = " Cretical connection Error cant continue ,please contact with concern person \n" + e.Message;
			return null;
		}


	}

    public DataTable GetDatas(string SQL, string database)
    {
        try
        {
            //StrCCon = CConnection.GetDMISConnectionString();
            //if (database == "DMIS")
            //{
            //    StrCCon = CConnection.GetDMISConnectionString();
            //}
             if (database == "FAMSConnectionString")
            {
                StrCCon = General.GetFAMConString();

            }
            else
            {
                StrCCon = CConnection.GetConnectionString();
           }
            SqlDataAdapter dta = new SqlDataAdapter(SQL, StrCCon);
            DataTable dt = new DataTable();
            dta.Fill(dt);
            StrCCon = CConnection.GetConnectionString();
            return dt;
        }
        catch (Exception e)
        {
            // msg = e.Message;
            //GlobalVar.mErrorMessage = " Cretical connection Error cant continue ,please contact with concern person \n" + e.Message;
            return null;
        }


    }
    public bool ExecuteQuery_NonQuery(string strQuery, SqlTransaction Trans)
    {
        bool flag;
        try
        {
            SqlHelper.ExecuteNonQuery(Trans, CommandType.Text, strQuery);
            flag = true;
        }
        catch (Exception exception)
        {
            flag = false;
            throw exception;
        }
        return flag;
    }

    public bool ExecuteQuery_NonQuery(string strQuery)
	{
		bool flag;

		try
		{
			SqlHelper.ExecuteNonQuery(StrCCon, CommandType.Text, strQuery);
			flag = true;
		}
		catch (Exception exception)
		{
			flag = false;
			throw exception;
		}

		return flag;
	}

	public bool ExecuteSP_NonQuery(string SP_Name, SqlParameter[] param, SqlTransaction Trans)
	{
		bool flag;
		try
		{
			SqlHelper.ExecuteNonQuery(Trans, CommandType.StoredProcedure, SP_Name, param);
			flag = true;
		}
		catch (Exception exception)
		{
			flag = false;
			throw exception;
		}
		return flag;
	}

	public bool ExecuteSP_NonQuery(string SP_Name, SqlParameter[] param)
	{
		bool flag;
		try
		{
			//GetConnection();
			SqlHelper.ExecuteNonQuery(StrCCon, CommandType.StoredProcedure, SP_Name, param);
			flag = true;
		}
		catch (Exception exception)
		{
			flag = false;
			throw exception;
		}
		return flag;
	}

    public static DataTable getDataTable(string spName, SqlParameter[] parameters, string connectionString)
    {
        DataTable table = new DataTable();
        using (SqlConnection sqlcon = new SqlConnection(connectionString))
        {
            SqlCommand command = new SqlCommand(spName, sqlcon);
            command.CommandType = CommandType.StoredProcedure;
            if (parameters != null)
            {
                foreach (SqlParameter param in parameters)
                {
                    command.Parameters.Add(param);
                }
            }
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(table);
            return table;
        }
    }
    public string ActiveUserName(string SessionUserID,string DealerCode)
    {
        string usrname = "";
        DataTable dt = GetData("select * from SecurityUser where ID='" + SessionUserID + "' and DealerCode = '"+DealerCode+"'");
        if (dt.Rows.Count > 0)
        {
            usrname = dt.Rows[0]["UserName"].ToString();
        }
        dt.Dispose();
        return usrname;
    }


    public string GetNewMaxID(string sTableName, string sColumn, int NoOfChar, string DealerCode)
	{
		//#GET MAX ID FROM TABLE
		string sQuery = "SELECT MAX(" + sColumn + ") MAXID FROM " + sTableName + " WHERE DealerCode='" + DealerCode + "'";
		SqlDataReader drItemVal = null;
		string sNewVersion = "0";
		int iNewVersion;
		if (ExecuteQuery(sQuery, ref drItemVal))
		{
			try
			{
				if (drItemVal.HasRows)
				{
					drItemVal.Read();
					sNewVersion = GetNullString(drItemVal["MAXID"]);
					drItemVal.Close();
					if (sNewVersion == "")
					{
						sNewVersion = "0";
						iNewVersion = Convert.ToInt32(sNewVersion) + 1;
						sNewVersion = Convert.ToString(iNewVersion).PadLeft(NoOfChar, '0');
					}
					else
					{
						iNewVersion = Convert.ToInt32(sNewVersion) + 1;
						sNewVersion = Convert.ToString(iNewVersion).PadLeft(NoOfChar, '0');
					}
					return sNewVersion;

				}
				else
				{
					iNewVersion = 0;
					sNewVersion = Convert.ToString(iNewVersion).PadLeft(NoOfChar, '0');
					return sNewVersion;
				}

			}
			catch (Exception ex)
			{
				//ShowError();
				drItemVal.Close();
			}

		}
		return sNewVersion;
	}
    public string GetNewMaxIDwithoutDealerCode(string sTableName, string sColumn, int NoOfChar, string DealerCode)
    {
        //#GET MAX ID FROM TABLE
        string sQuery = "SELECT MAX(" + sColumn + ") MAXID FROM " + sTableName ;
        SqlDataReader drItemVal = null;
        string sNewVersion = "0";
        int iNewVersion;
        if (ExecuteQuery(sQuery, ref drItemVal))
        {
            try
            {
                if (drItemVal.HasRows)
                {
                    drItemVal.Read();
                    sNewVersion = GetNullString(drItemVal["MAXID"]);
                    drItemVal.Close();
                    if (sNewVersion == "")
                    {
                        sNewVersion = "0";
                        iNewVersion = Convert.ToInt32(sNewVersion) + 1;
                        sNewVersion = Convert.ToString(iNewVersion).PadLeft(NoOfChar, '0');
                    }
                    else
                    {
                        iNewVersion = Convert.ToInt32(sNewVersion) + 1;
                        sNewVersion = Convert.ToString(iNewVersion).PadLeft(NoOfChar, '0');
                    }
                    return sNewVersion;

                }
                else
                {
                    iNewVersion = 0;
                    sNewVersion = Convert.ToString(iNewVersion).PadLeft(NoOfChar, '0');
                    return sNewVersion;
                }

            }
            catch (Exception ex)
            {
                //ShowError();
                drItemVal.Close();
            }

        }
        return sNewVersion;
    }

    public string GetNullString(object txt)
	{
		try
		{
			if (object.ReferenceEquals(txt, DBNull.Value)) return string.Empty;
			else return Convert.ToString(txt).Trim();
		}
		catch (Exception) { return string.Empty; }
	}

	public void ClearTextBoxes(Control parent)
	{
		foreach (Control child in parent.Controls)
		{
			TextBox textBox = child as TextBox;
			if (textBox == null)
				ClearTextBoxes(child);
			else
				textBox.Text = string.Empty;
		}
	}

	public void Clearddl(Control parent)
	{
		foreach (Control child in parent.Controls)
		{
			DropDownList ddl = child as DropDownList;

			if (ddl == null)
				Clearddl(child);
			else
				ddl.SelectedIndex = -1;
		}
	}

	public bool ExecuteSP(string SP_Name, SqlParameter[] param, ref SqlDataReader dr)
	{
		bool flag;

		try
		{
		  
			dr = SqlHelper.ExecuteReader(StrCCon, SP_Name, param);
			if (dr.HasRows) flag = true; else flag = false;
		}
		catch (Exception ex) { throw ex; }
		return flag;


	}

	public void FillDropDown(DropDownList ddlFill, string strQuery, string DataTextField, string DataValueField, string StartText)
	{
		//Fill document Type DropDown
		DataSet ds1 = new DataSet();
		ds1 = SqlHelper.ExecuteDataset(StrCCon, CommandType.Text, strQuery);
		ddlFill.DataSource = ds1.Tables[0];
		ddlFill.DataTextField = DataTextField;
		ddlFill.DataValueField = DataValueField;
		ddlFill.DataBind();
		if (StartText != string.Empty)
		{
			ddlFill.Items.Insert(0, new ListItem(StartText, String.Empty));
			ddlFill.SelectedIndex = 0;
		}
		///////////////"--Select--"
	}

	public bool ValidateDate(string sDate)
	{
		if (sDate == "" || sDate == "__/__/____")
		{
			return true;
		}
		DateTime fromDateValue;
		string s = sDate;
		var formats = new[] { "dd/MM/yyyy", "yyyy-MM-dd" };
		if (DateTime.TryParseExact(s, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out fromDateValue))
		{
			// do for valid date
			return true;
		}
		else
		{
			// do for invalid date
			return false;
		}
	}

	public bool IsExist(string strColumnName, string strValue, string strTableName, string DealerCode, string Append = "")
	{
		SqlDataReader dr = null;
		string strSql = null;
		strSql = "Select [" + strColumnName + "] from [" + strTableName + "] Where [" + strColumnName + "] = '" + strValue + "' AND DealerCode IN ('" + DealerCode + "','AAAAA') " + Append;
		try
		{
			//if (!dr.IsClosed)
			//{
			//    dr.Close();
			//}
			bool bFlag = ExecuteQuery(strSql, ref dr);
			if (!bFlag)
			{
				dr.Close();
				return false;
			}
			else
			{
				if ((dr.HasRows))
				{
					dr.Close(); return true;
				}
				else
				{
					dr.Close(); return false;
				}
			}
		}
		catch (SqlException ex)
		{
			//clsLogger.WriteLog(ex.Message, ex.Source);
			dr.Close();
			return false;
		}
	}

	public bool CodeExists(string strTableNames, string strColumnCode, string strColumeValue, string strDealerCode , ref DataSet ds)
	{
		try
		{
			//Create query to check in all tables one by one
			//foreach ( sTableName in strTableNames) {
			string sQuery = "SELECT * FROM " + strTableNames + " WHERE " + strColumnCode + " = '" + strColumeValue + "' AND DealerCode = '" + strDealerCode + "'";

			ds = new DataSet();
			ds = SqlHelper.ExecuteDataset(this.StrCCon, CommandType.Text, sQuery);
			if (ds.Tables[0].Rows.Count > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		catch (Exception ex)
		{
			//clsLogger.WriteLog(e.Message, e.Source);
			throw ex;
		}
	}

	public string GetDates(string txt)
	{
		string functionReturnValue = null;
		DateTime DateTimeFor = Convert.ToDateTime(txt, new System.Globalization.CultureInfo("en-GB"));
		try
		{
			if (object.ReferenceEquals(txt, DBNull.Value) || string.IsNullOrEmpty(txt.ToString()) || txt.ToString() == "__/__/____")
			{
				functionReturnValue = "__/__/____";
			}
			else if (DateTime.Parse(txt.ToString()).Year == 1900)
			{
				functionReturnValue = "__/__/____";
			}
			else
			{
				//DateTime DateTimeFor = Convert.ToDateTime(txt, new System.Globalization.CultureInfo("en-GB"));
				functionReturnValue = DateTimeFor.ToString("dd/MM/yyyy");
			}
		}
		catch
		{
			return string.Empty;
		}
		return functionReturnValue;
	}

	public string GetDate(object txt)
	{
		string functionReturnValue = null;
		DateTime DateTimeFor = Convert.ToDateTime(txt, new System.Globalization.CultureInfo("en-GB"));
		try
		{
			if (object.ReferenceEquals(txt, DBNull.Value) || string.IsNullOrEmpty(txt.ToString()) || txt.ToString() == "__/__/____")
			{
				functionReturnValue = "__/__/____";
			}
			else if (DateTime.Parse(txt.ToString()).Year == 1900)
			{
				functionReturnValue = "__/__/____";
			}
			else
			{
				//DateTime DateTimeFor = Convert.ToDateTime(txt, new System.Globalization.CultureInfo("en-GB"));
				functionReturnValue = DateTimeFor.ToString("dd/MM/yyyy");
			}
		}
		catch
		{
			return string.Empty;
		}
		return functionReturnValue;
	}

	//public DataSet GetData(string SQL, string Table)
	//{
	//    try
	//    {
	//        if (StrCCon == null)
	//        {
	//            CConnection.GetConnectionString();
	//        }
	//        SqlDataAdapter dta = new SqlDataAdapter(SQL, StrCCon);
	//        DataTable dt = new DataTable();
	//        DataSet ds = new DataSet();
	//        dta.Fill(ds, Table);

	//        return ds;
	//    }
	//    catch (Exception e)
	//    {
	//        return null;
	//    }


	//}

	public bool isDateTime(string txtDate)
	{
		DateTime tempDate;

		return DateTime.TryParse(txtDate, out tempDate) ? true : false;
	}

	public object SaveDate(string txt)
	{
		object functionReturnValue = null;
		try
		{
			if (txt == "__/__/____" || string.IsNullOrEmpty(txt))
			{
				functionReturnValue = DBNull.Value;
			}
			else
			{
				DateTime DateTimeFor = Convert.ToDateTime(txt.Substring(6, 4) + "-" + txt.Substring(3, 2) + "-" + txt.Substring(0, 2)); //Convert.ToDateTime(txt, new System.Globalization.CultureInfo("ur-PK"));
				functionReturnValue = DateTimeFor.ToString("yyyy-MM-dd");
			}
		}
		catch (Exception ex)
		{
			functionReturnValue = DBNull.Value; throw ex;
		}
		return functionReturnValue;
	}

    
    public object SaveDatewithSlash(string txt)
    {
        object functionReturnValue = null;
        try
        {
            if (txt == "__/__/____" || string.IsNullOrEmpty(txt))
            {
                functionReturnValue = DBNull.Value;
            }
            else
            {
                DateTime DateTimeFor = Convert.ToDateTime(txt.Substring(6, 4) + "/" + txt.Substring(3, 2) + "/" + txt.Substring(0, 2)); //Convert.ToDateTime(txt, new System.Globalization.CultureInfo("ur-PK"));
                functionReturnValue = DateTimeFor.ToString("yyyy-MM-dd");
            }
        }
        catch (Exception ex)
        {
            functionReturnValue = DBNull.Value; throw ex;
        }
        return functionReturnValue;
    }

    public object SaveTime(string txt)
	{
		object functionReturnValue = null;
		try
		{
			if (txt == "__:__" || string.IsNullOrEmpty(txt))
			{
				functionReturnValue = DBNull.Value;
			}
			else
			{
				DateTime DateTimeFor = Convert.ToDateTime(txt);
				functionReturnValue = DateTimeFor.ToString("HH:mm");
			}
		}
		catch (Exception ex)
		{
			return "00:00";
		}
		return functionReturnValue;
	}

	public void UserMsg(System.Web.UI.WebControls.Label lblMsg, Color txtColor, string strMsg, TextBox txtfocus = null)
	{
		//var page = HttpContext.Current.CurrentHandler as Page;
		lblMsg.Text = strMsg;
		lblMsg.ForeColor = txtColor;
		//page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "ShowFailurMSG();", true);
		if (txtfocus != null)
		{
			txtfocus.Focus();
		}

	}

    //public bool ExecuteSP(string SP_Name, SqlParameter[] param, ref SqlDataReader dr)
    //{
    //    bool flag;

    //    try
    //    {
    //        GetConnection();
    //        dr = SqlHelper.ExecuteReader(GlobalVar.mCon, SP_Name, param);
    //        if (dr.HasRows) flag = true; else flag = false;
    //    }
    //    catch (Exception ex) { throw ex; }
    //    return flag;


    //}

    public DataSet FillDataSet(string SP_Name, SqlParameter[] param)
    {
        DataSet ds = new DataSet();
        ds = SqlHelper.ExecuteDataset(CConnection.GetConnectionString(), CommandType.StoredProcedure, SP_Name, param);
        return ds;
    }

    //public bool ExecuteQuery(string sQuery, ref SqlDataReader dr)
    //{
    //    bool flag;
    //    try
    //    {
    //        GetConnection();

    //        dr = SqlHelper.ExecuteReader(GlobalVar.mCon, CommandType.Text, sQuery);

    //        if (dr.HasRows) flag = true; else flag = false;
    //    }
    //    catch (Exception exception) { throw exception; }
    //    return flag;

    //}


    //public bool ExecuteQuery(string sQuery, ref DataSet ds)
    //{
    //    // tell us later how to use 

    //    bool flag;
    //    ds = new DataSet();
    //    try
    //    {
    //        ds = SqlHelper.ExecuteDataset(GlobalVar.mDataConnection, CommandType.Text, sQuery);
    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            flag = true;
    //        }
    //        else
    //        {
    //            flag = false;
    //        }
    //    }
    //    catch (Exception exception)
    //    {
    //        throw exception;
    //    }
    //    return flag;
    //}

    //public bool ExecuteQuery(string sQuery, SqlTransaction Trans)
    //{
    //    bool flag;
    //    try
    //    {
    //        int i = SqlHelper.ExecuteNonQuery(Trans, CommandType.Text, sQuery);
    //        if (i > 0)
    //        {
    //            flag = true;
    //        }
    //        else
    //        {
    //            flag = false;
    //        }
    //    }
    //    catch (Exception exception)
    //    {
    //        flag = false;
    //        throw exception;
    //    }
    //    return flag;
    //}


    //public bool ExecuteQuery(string sQuery, ref SqlDataReader dr, SqlTransaction trans)
    //{
    //    bool flag;

    //    try
    //    {
    //        dr = SqlHelper.ExecuteReader(trans, CommandType.Text, sQuery);
    //        if (dr.HasRows)
    //        {
    //            flag = true;
    //        }
    //        else
    //        {
    //            flag = false;
    //        }

    //    }
    //    catch (Exception exception)
    //    {
    //        throw exception;

    //    }
    //    return flag;
    //}




    /* end here */
}