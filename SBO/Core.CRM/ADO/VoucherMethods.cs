using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.Mvc;
using Core.CRM.ADO.ViewModel;
using Core.CRM.Helper;
using Microsoft.ApplicationBlocks.Data;

namespace Core.CRM.ADO
{
    public class VoucherMethods
    {
        
        static SysFunction sysfun = new SysFunction();
        static DataSet ds;
        static double totDebit = 0, totCredit = 0;
        static string DealerCode ;
        //static string ReceiptNo;
        static bool IsSaved = false;
        static string ReceiptNo;
        static string Type;
        static string Postflag="";
        static string Delflag = "";
        static DataTable dt = new DataTable();
        public static List<SelectListItem> initializeDDLs(string dealerCode, string type = "")
        {
            Type = type;

            List<SelectListItem> item = new List<SelectListItem>();
            DataTable dt = new DataTable();
            List<StringNameValueClass> lst = new List<StringNameValueClass>();
            try
            {
                string sql = "Select JournalNo as Id,JournalDesc as Title from Gjournal ";
                //string sql = "Select A.contacccode +'-'+  A.SubCode +'-'+  A.subsubcode +'-'+  A.loccode +'-'+  A.DetailCode as Id , A.contacccode +'-'+  A.SubCode +'-'+  A.subsubcode +'-'+  A.loccode +'-'+  A.DetailCode + ' | ' + rtrim(B.DetailDesc) + ' | ' + A.Booktype + ' | ' + A.BookNo Title  from GbookSetup  A" +
                //    " inner join GDetail B " +
                //    " on B.contacccode + '-' + B.SubCode + '-' + B.subsubcode + '-' + B.loccode + '-' + B.DetailCode = " +
                //    " A.contacccode + '-' + A.SubCode + '-' + A.subsubcode + '-' + A.loccode + '-' + A.DetailCode " +
                //    " and A.Compcode = B.CompCode ";

                //if (type == "BR" || type == "BP")
                //{
                //    sql = sql + " and A.BookType = 'B'";
                //}
                //else
                //{
                //    sql = sql + " and A.BookType = 'C'";
                //}

                sql = sql + "where CompCode = '" + dealerCode + "'";

                SqlDataAdapter dta = new SqlDataAdapter(sql, General.GetFAMConString());
                dta.Fill(dt);

                if (dt.Rows.Count > 0)
                    {
                        lst = EnumerableExtension.ToList<StringNameValueClass>(dt);
                    }
                //json = Serializer.Serialize(lst);

                item = lst.Select(i => new SelectListItem()
                {
                    Value = i.Id.ToString(),
                    Text = i.Id.ToString() +"-"+ i.Title.ToString()
                    }).ToList();

                    item.Insert(0, new SelectListItem() { Value = "0", Text = "Select" });
                    
            }
            catch (Exception ex)
            {
            }
            return item;
        }
        public static string GetNewVoucherNo(string sTableName, string sColumn, int NoOfChar, string dealerCode)
        {

            //#GET MAX ID FROM TABLE
            string sQuery = "SELECT MAX(" + sColumn + ") MAXID FROM " + sTableName + " where CompCode = '" + dealerCode + "'";

            SqlDataReader drItemVal = SqlHelper.ExecuteReader(General.GetFAMConString(), CommandType.Text, sQuery);

            //SqlDataReader drItemVal = null;
            string sNewVersion = "0";
            int iNewVersion;
            string date = DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("yyyy-MM-dd");
            string subDate = date.Substring(2);
            try
            {
                if (drItemVal.HasRows)
                {
                    drItemVal.Read();
                    sNewVersion = sysfun.GetNullString(drItemVal["MAXID"]);
                    drItemVal.Close();

                    if (sNewVersion == "")
                    {
                        sNewVersion = "0";
                        iNewVersion = Convert.ToInt32(sNewVersion) + 1;
                        sNewVersion = Convert.ToString(iNewVersion).PadLeft(NoOfChar, '0');
                    }
                    else
                    {
                        string sub = sNewVersion.Substring(12);
                        iNewVersion = Convert.ToInt32(sub) + 1;
                        sNewVersion = Convert.ToString(iNewVersion).PadLeft(NoOfChar, '0');
                    }
                    return "JV-" + subDate + "-" + sNewVersion;

                }
                else
                {
                    iNewVersion = 0;
                    sNewVersion = Convert.ToString(iNewVersion).PadLeft(NoOfChar, '0');
                    return "JV-" + subDate + "-" + sNewVersion;
                }

            }
            catch (Exception ex)
            {
                //ShowError();
                drItemVal.Close();
            }

            return sNewVersion;
        }
        public static bool GetNewVoucherNoReceipt(string sTableName, string sColumn, int NoOfChar,string dealerCode,string VouchNo,ref string newVouch)
        {

            if (sysfun.CodeExists("ProdRecMaster", "VouchNo", VouchNo, dealerCode, ref ds))
            {
                newVouch = VouchNo;
                return false;
               

            }
            //#GET MAX ID FROM TABLE
            string sQuery = "SELECT MAX(" + sColumn + ") MAXID FROM " + sTableName + " where CompCode = '" + dealerCode+ "'";

            SqlDataReader drItemVal = SqlHelper.ExecuteReader(General.GetFAMConString(), CommandType.Text, sQuery);

            //SqlDataReader drItemVal = null;
            string sNewVersion = "0";
            int iNewVersion;
            string date = DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("yyyy-MM-dd");
            string subDate = date.Substring(2);
            try
            {
                if (drItemVal.HasRows)
                {
                    drItemVal.Read();
                    sNewVersion = sysfun.GetNullString(drItemVal["MAXID"]);
                    drItemVal.Close();

                    if (sNewVersion == "")
                    {
                        sNewVersion = "0";
                        iNewVersion = Convert.ToInt32(sNewVersion) + 1;
                        sNewVersion = Convert.ToString(iNewVersion).PadLeft(NoOfChar, '0');
                    }
                    else
                    {
                        string sub = sNewVersion.Substring(12);
                        iNewVersion = Convert.ToInt32(sub) + 1;
                        sNewVersion = Convert.ToString(iNewVersion).PadLeft(NoOfChar, '0');
                    }
                    newVouch= "JV-" + subDate + "-" + sNewVersion;

                }
                else
                {
                    iNewVersion = 0;
                    sNewVersion = Convert.ToString(iNewVersion).PadLeft(NoOfChar, '0');
                    newVouch= "JV-" + subDate + "-" + sNewVersion;
                }

            }
            catch (Exception ex)
            {
                //ShowError();
                drItemVal.Close();
            }
          //  newVouch = newVouch;
            return true;
        }
        public static List<VoucherVM> LoadCSI(string leadId, string dealerCode, string ChassisNo)
        {
            List<VoucherVM> lst = new List<VoucherVM>();


            ReceiptNo = leadId;
            //ReceiptNo = Session["leadId"];

            ds = new DataSet();

            ds.Tables.Add();

            ds.Tables[0].Columns.Add(new DataColumn("AccountCode", typeof(string)));
            ds.Tables[0].Columns.Add(new DataColumn("AccountTitle", typeof(string)));
            ds.Tables[0].Columns.Add(new DataColumn("Debit", typeof(string)));
            ds.Tables[0].Columns.Add(new DataColumn("Credit", typeof(string)));
            ds.Tables[0].Columns.Add(new DataColumn("Narration", typeof(string)));
            
            SqlParameter[] dsParamInv = {
                new SqlParameter("@DealerCode",SqlDbType.Char,5),
                new SqlParameter("@TransCode",SqlDbType.Char,8),
                new SqlParameter("@ChasisNo",SqlDbType.Char,25)
            };

            dsParamInv[0].Value = dealerCode;
            dsParamInv[1].Value = leadId;
            dsParamInv[2].Value = ChassisNo;
            ReceiptNo = leadId;
            totCredit = totDebit = 0;

            DataSet dsReceipt = new DataSet();


            dsReceipt = sysfun.FillDataSet("SP_Get_VehicleSaleAccountCode", dsParamInv);

            if (dsReceipt.Tables[0].Rows.Count > 0)
            {

                //  string Customer = dsReceipt.Tables[0].Rows[0]["CusDesc"].ToString().Trim();
                //DataRow[] dr = dsReceipt.Tables[0].Compute("SUM(ReceiptAmount)","");
                double ReceiptAmount = Convert.ToDouble(dsReceipt.Tables[0].Compute("SUM(TotalAmount)", ""));
                //double ReceiptAmount = Convert.ToDouble(dsReceipt.Tables[0].Rows[0]["ReceiptAmount"]);
                string Naration = "Type  : " + dsReceipt.Tables[0].Rows[0]["StockType"].ToString().Trim()
                + " | " + "Chassis No : " + dsReceipt.Tables[0].Rows[0]["ChassisNo"].ToString().Trim()
                + " | " + "Product : " + dsReceipt.Tables[0].Rows[0]["ProdDesc"].ToString().Trim();
                string Acc = dsReceipt.Tables[0].Rows[0]["CusAccountCode"].ToString().Trim();
                string CusDesc = dsReceipt.Tables[0].Rows[0]["CusDesc"].ToString().Trim();
                string MarketRate = dsReceipt.Tables[0].Rows[0]["MarketRate"].ToString().Trim();
                string Discount = dsReceipt.Tables[0].Rows[0]["Discount"].ToString().Trim();
                string Own = dsReceipt.Tables[0].Rows[0]["OwnMoney"].ToString().Trim();
                string DownMoney = dsReceipt.Tables[0].Rows[0]["DownMoney"].ToString().Trim();
                string AccountCode = dsReceipt.Tables[0].Rows[0]["VendorPayable"].ToString();
                string VendorDesc = dsReceipt.Tables[0].Rows[0]["VendorDesc"].ToString().Trim();
                double SalePrice = Convert.ToDouble(dsReceipt.Tables[0].Rows[0]["FactoryPrice"]);
                double market = Convert.ToDouble(MarketRate);
                double discount = Convert.ToDouble(Discount);
                if (market > 0) {
                    AddCustomerDebitAmount(ReceiptAmount, Acc, CusDesc, Naration);
                   // AddCustomerDebitAmount(discount, DownMoney, "Down Money", Naration);
                    AddCreditAmount(SalePrice, AccountCode, Naration, VendorDesc);
                    AddCreditAmount(market, Own, Naration, "Own Money");
                }
                if (discount > 0)
                {
                    AddCustomerDebitAmount(ReceiptAmount, Acc, CusDesc, Naration);
                    AddCustomerDebitAmount(discount, DownMoney, "Down Money", Naration);
                    AddCreditAmount(SalePrice, AccountCode, Naration, VendorDesc);
                   // AddCreditAmount(market, Own, Naration, "Own Money");
                }
                if (discount == 0 && market==0)
                {
                    AddCustomerDebitAmount(ReceiptAmount, Acc, CusDesc, Naration);
                    //AddCustomerDebitAmount(discount, DownMoney, "Down Money", Naration);
                    AddCreditAmount(SalePrice, AccountCode, Naration, VendorDesc);
                    // AddCreditAmount(market, Own, Naration, "Own Money");
                }
              

                return lst = EnumerableExtension.ToList<VoucherVM>(ds.Tables[0]);
            }

            return lst;

        }
        public static List<VoucherVM> LoadGVGrid(string leadId, string dealerCode,string ChassisNo)
        {
            List<VoucherVM> lst = new List<VoucherVM>();
            

            ReceiptNo = leadId;
            //ReceiptNo = Session["leadId"];

            ds = new DataSet();

            ds.Tables.Add();

            ds.Tables[0].Columns.Add(new DataColumn("AccountCode", typeof(string)));
            ds.Tables[0].Columns.Add(new DataColumn("AccountTitle", typeof(string)));
            ds.Tables[0].Columns.Add(new DataColumn("Debit", typeof(string)));
            ds.Tables[0].Columns.Add(new DataColumn("Credit", typeof(string)));
            ds.Tables[0].Columns.Add(new DataColumn("Narration", typeof(string)));

            SqlParameter[] dsParamInv = {
                new SqlParameter("@DealerCode",SqlDbType.Char,5),
                new SqlParameter("@RecNo",SqlDbType.Char,8),
                new SqlParameter("@ChasisNo",SqlDbType.Char,25)
            };

            dsParamInv[0].Value = dealerCode;
            dsParamInv[1].Value = leadId;
            dsParamInv[2].Value = ChassisNo;

            totCredit = totDebit = 0;

            DataSet dsReceipt = new DataSet();


            dsReceipt = sysfun.FillDataSet("SP_Get_VehicleAccountCodeDetail", dsParamInv);

            if (dsReceipt.Tables[0].Rows.Count > 0)
            {

              //  string Customer = dsReceipt.Tables[0].Rows[0]["CusDesc"].ToString().Trim();
                //DataRow[] dr = dsReceipt.Tables[0].Compute("SUM(ReceiptAmount)","");
                double ReceiptAmount = Convert.ToDouble(dsReceipt.Tables[0].Compute("SUM(Amount)", ""));
                //double ReceiptAmount = Convert.ToDouble(dsReceipt.Tables[0].Rows[0]["ReceiptAmount"]);
                string Naration = "Type  : " + dsReceipt.Tables[0].Rows[0]["Type"].ToString().Trim()
                + " | " +"Chassis No : " + dsReceipt.Tables[0].Rows[0]["ChasisNo"].ToString().Trim()
                + " | " + "Product : " + dsReceipt.Tables[0].Rows[0]["ProdDesc"].ToString().Trim(); 
                string Acc= dsReceipt.Tables[0].Rows[0]["VehicleStock"].ToString().Trim();
                AddDebitAmount(ReceiptAmount, Acc, Naration, dealerCode);

                for (int i = 0; i < dsReceipt.Tables[0].Rows.Count; i++)
                {
                    SqlParameter[] param = {
                                            new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                            new SqlParameter("@RecNo",SqlDbType.Char,8),
                                              new SqlParameter("@ChasisNo",SqlDbType.Char,25)
                                            //new SqlParameter("@VehExpCode",SqlDbType.Char,8)
                                           };

                    param[0].Value = dealerCode;
                    param[1].Value = leadId;
                    param[2].Value = ChassisNo;


                    DataSet dsServiceCharges = sysfun.FillDataSet("SP_Get_VehicleAccountCodeDetail", param);


                    string AccountCode = dsServiceCharges.Tables[0].Rows[0]["VendorPayable"].ToString();
                    string VehExpDesc = dsServiceCharges.Tables[0].Rows[0]["VendorDesc"].ToString().Trim();
                    double Amount = Convert.ToDouble(dsServiceCharges.Tables[0].Rows[0]["Amount"]);


                    AddCreditAmount(Amount, AccountCode, Naration, VehExpDesc);

                }
                return lst = EnumerableExtension.ToList<VoucherVM>(ds.Tables[0]);
            }

            return lst;

        }
        public static List<VoucherVM> LoadCSGrid(string leadId, string dealerCode,string ChassisNo,string VouchNo)
        {
            List<VoucherVM> lst = new List<VoucherVM>();

            ReceiptNo = VouchNo;

            ds = new DataSet();

            ds.Tables.Add();

            ds.Tables[0].Columns.Add(new DataColumn("AccountCode", typeof(string)));
            ds.Tables[0].Columns.Add(new DataColumn("AccountTitle", typeof(string)));
            ds.Tables[0].Columns.Add(new DataColumn("Debit", typeof(string)));
            ds.Tables[0].Columns.Add(new DataColumn("Credit", typeof(string)));
            ds.Tables[0].Columns.Add(new DataColumn("Narration", typeof(string)));

            SqlParameter[] dsParamInv = {
                new SqlParameter("@DealerCode",SqlDbType.Char,5),
                new SqlParameter("@RceiptNo",SqlDbType.Char,8)
            };

            dsParamInv[0].Value = dealerCode;
            dsParamInv[1].Value = leadId;

            totCredit = totDebit = 0;

            DataSet dsReceipt = new DataSet();


            dsReceipt = sysfun.FillDataSet("SP_Select_ReceiptDetail", dsParamInv);

            if (dsReceipt.Tables[0].Rows.Count > 0)
            {

                string Customer = dsReceipt.Tables[0].Rows[0]["CusDesc"].ToString().Trim();
                string ChasisNo = dsReceipt.Tables[0].Rows[0]["ChasisNo"].ToString().Trim();
                string EngineNo = dsReceipt.Tables[0].Rows[0]["EngineNo"].ToString().Trim();
                string InvoiceNo = dsReceipt.Tables[0].Rows[0]["InvoiceNo"].ToString().Trim();
                //DataRow[] dr = dsReceipt.Tables[0].Compute("SUM(ReceiptAmount)","");
                double ReceiptAmount = Convert.ToDouble(dsReceipt.Tables[0].Compute("SUM(ReceiptAmount)", ""));
                //double ReceiptAmount = Convert.ToDouble(dsReceipt.Tables[0].Rows[0]["ReceiptAmount"]);
                string Naration = "Sale Invoice : " + dsReceipt.Tables[0].Rows[0]["ReceiptNo"].ToString().Trim() + " | " +
                                      "Customer : " + dsReceipt.Tables[0].Rows[0]["CusDesc"].ToString().Trim() + " | " +
                                       "ChassisNo : " + ChasisNo;
                string Account = dsReceipt.Tables[0].Rows[0]["AccountCode"].ToString().ToString();
                AddCustomerDebitAmount(ReceiptAmount, Account,Customer,Naration);

                for (int i = 0; i < dsReceipt.Tables[0].Rows.Count; i++)
                {
                    SqlParameter[] param = {
                                            new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                            new SqlParameter("@ReceiptNo",SqlDbType.Char,8),
                                            new SqlParameter("@VehExpCode",SqlDbType.Char,8)
                                           };

                    param[0].Value = dealerCode;
                    param[1].Value = leadId;
                    param[2].Value = dsReceipt.Tables[0].Rows[i]["VehExpCode"].ToString();


                    DataSet dsServiceCharges = sysfun.FillDataSet("SP_GetServiceAmountTemp", param);


                    string AccountCode = dsServiceCharges.Tables[0].Rows[0]["ServicesAccountCode"].ToString();
                    string VehExpDesc = dsServiceCharges.Tables[0].Rows[0]["VehExpDesc"].ToString().Trim();
                    double Amount = Convert.ToDouble(dsServiceCharges.Tables[0].Rows[0]["ReceiptAmount"]);

                    
                    AddCreditAmount(Amount, AccountCode, Naration, VehExpDesc);
                   
                }
                return lst = EnumerableExtension.ToList<VoucherVM>(ds.Tables[0]);
            }

            return lst;

        }

        public static List<VoucherVM> LoadDEGrid(string leadId, string dealerCode)
        {
            List<VoucherVM> lst = new List<VoucherVM>();

            ReceiptNo = leadId;

            ds = new DataSet();

            ds.Tables.Add();

            ds.Tables[0].Columns.Add(new DataColumn("AccountCode", typeof(string)));
            ds.Tables[0].Columns.Add(new DataColumn("AccountTitle", typeof(string)));
            ds.Tables[0].Columns.Add(new DataColumn("Debit", typeof(string)));
            ds.Tables[0].Columns.Add(new DataColumn("Credit", typeof(string)));
            ds.Tables[0].Columns.Add(new DataColumn("Narration", typeof(string)));

            SqlParameter[] dsParamInv = {
                new SqlParameter("@DealerCode",SqlDbType.Char,5),
                new SqlParameter("@ExpInvNo",SqlDbType.Char,8)
            };

            dsParamInv[0].Value = dealerCode;
            dsParamInv[1].Value = leadId;

            totCredit = totDebit = 0;

            DataSet dsReceipt = new DataSet();


            dsReceipt = sysfun.FillDataSet("SP_DailyExpense_Voucher", dsParamInv);

            if (dsReceipt.Tables[0].Rows.Count > 0)
            {

                //double ReceiptAmount = Convert.ToDouble(dsReceipt.Tables[0].Compute("SUM(InsAmount)", ""));
                double ReceiptAmount = Convert.ToDouble(dsReceipt.Tables[0].Rows[0]["InsAmount"]);
                string Naration = "Daily Expenditure No : " + dsReceipt.Tables[0].Rows[0]["ExpInvNo"].ToString().Trim() + " | " +
                                      "Remarks : " + dsReceipt.Tables[0].Rows[0]["Remarks"].ToString().Trim();

                AddDebitAmount(ReceiptAmount, "", Naration, dealerCode);

                for (int i = 0; i < dsReceipt.Tables[0].Rows.Count; i++)
                {
                    SqlParameter[] param = {
                                            new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                            new SqlParameter("@ExpInvNo",SqlDbType.Char,8),
                                            new SqlParameter("@ExpFor",SqlDbType.Char,8)
                                           };

                    param[0].Value = dealerCode;
                    param[1].Value = leadId;
                    param[2].Value = dsReceipt.Tables[0].Rows[i]["ExpFor"].ToString();


                    DataSet dsServiceCharges = sysfun.FillDataSet("SP_GetExpenseHeadDetail", param);


                    string AccountCode = dsServiceCharges.Tables[0].Rows[0]["AccountCode"].ToString();
                    string VehExpDesc = dsServiceCharges.Tables[0].Rows[0]["EDesc"].ToString().Trim();
                    double Amount = Convert.ToDouble(dsServiceCharges.Tables[0].Rows[0]["Amount"]);


                    AddCreditAmount(Amount, AccountCode, Naration, VehExpDesc);

                }
                return lst = EnumerableExtension.ToList<VoucherVM>(ds.Tables[0]);
            }

            return lst;

        }

        private static string GetAccountCode(string code)
        {
            DataTable dt = new DataTable();

            string value = "";

            dt = sysfun.GetData("Select [" + code + "] from AccountCodeSetup where DealerCode = '" + DealerCode + "'", "BMS0517ConnectionString");

            if (dt == null)
            {
                return value;
            }
            if (dt.Rows.Count > 0)
            {
                value = dt.Rows[0][code].ToString();
            }
            return value;
        }
        private static string GetAccounttitle(string code,string dealerCode)
        {
            try
            {
                string value = "";

                SqlDataAdapter dta = new SqlDataAdapter("Select rtrim(A.DetailDesc) as AccountTitle from GDetail  A where A.CompCode = '" + dealerCode + "' and A.contacccode +'-'+  A.SubCode +'-'+  A.subsubcode +'-'+  A.loccode +'-'+  A.DetailCode = '" + code + "'", General.GetFAMConString());

                DataTable dt = new DataTable();
                dta.Fill(dt);

                if (dt == null)
                {
                    return value;
                }

                if (dt.Rows.Count > 0)
                {
                    value = dt.Rows[0]["AccountTitle"].ToString();
                }
                return value;
            }
            catch (Exception ex)
            {
                return "";
            }

        }
        private static void AddCustomerDebitAmount(double amount, string code, string Customer, string Naration)
        {            

            DataRow dr = ds.Tables[0].NewRow();
            dr["AccountCode"] = code;
            dr["AccountTitle"] = Customer;
            dr["Debit"] = amount;
            dr["Credit"] = "0.0";
            dr["Narration"] = Naration;

            totDebit = totDebit + amount;

            ds.Tables[0].Rows.Add(dr);
            
        }
        private void AddCustomerAmount(double amount, string code, string Customer, string Naration)
        {

            DataRow dr = ds.Tables[0].NewRow();
            dr["AccountCode"] = code;
            dr["AccountTitle"] = Customer;
            dr["Debit"] = "0.0";
            dr["Credit"] = amount;
            dr["Narration"] = Naration;

            totCredit = totCredit + amount;

            ds.Tables[0].Rows.Add(dr);
            
        }
        private static void AddCreditAmount(double amount, string code, string Naration,string title)
        {
            DataRow dr = ds.Tables[0].NewRow();
            dr["AccountCode"] = code;
            dr["AccountTitle"] = title;
            dr["Narration"] = Naration;
            dr["Debit"] = "0.0";
            dr["Credit"] = amount;
            

            totCredit = totCredit + amount;

            ds.Tables[0].Rows.Add(dr);
            
        }
        private static void AddDebitAmount(double amount, string account, string Naration,string dealerCode)
        {

            DataRow dr = ds.Tables[0].NewRow();

            dr["AccountCode"] = account;
            dr["AccountTitle"] = GetAccounttitle(account,dealerCode);
            dr["Debit"] = amount;
            dr["Credit"] = "0.0";
            dr["Narration"] = Naration;
            totDebit = totDebit + amount;

            ds.Tables[0].Rows.Add(dr);
            
        }
        private static string GetBookNo(string code,string compCode)
        {
            try
            {
                string CCon = General.GetFAMConString();
                string value = "";

                SqlDataAdapter dta = new SqlDataAdapter("Select A.BookNo from GbookSetup  A  where A.CompCode = '"+ compCode +"' and A.contacccode +'-'+  A.SubCode +'-'+  A.subsubcode +'-'+  A.loccode +'-'+  A.DetailCode = '" + code + "'", CCon);

                DataTable dt = new DataTable();
                dta.Fill(dt);
                if (dt == null)
                {
                    return value;
                }
                else if (dt.Rows.Count > 0)
                {
                    value = dt.Rows[0]["BookNo"].ToString();
                }

                return value;
            }
            catch (Exception ex)
            {
                return "";
            }

        }
        private static string GetBookType(string code , string compCode)
        {
            try
            {
                string CCon = General.GetFAMConString();
                string value = "";

                SqlDataAdapter dta = new SqlDataAdapter("Select A.Booktype from GbookSetup  A  where A.CompCode = '" + compCode + "' and A.contacccode +'-'+  A.SubCode +'-'+  A.subsubcode +'-'+  A.loccode +'-'+  A.DetailCode = '" + code + "'", CCon);

                DataTable dt = new DataTable();
                dta.Fill(dt);
                if (dt == null)
                {
                    return value;
                }
                else if (dt.Rows.Count > 0)
                {
                    value = dt.Rows[0]["Booktype"].ToString();
                }

                return value;
            }
            catch (Exception ex)
            {
                return "";
            }

        }        
        public static bool Insert_Gvoucher(List<GVouMasterVM> model2,string type,string DealerCode, ref string msg,string leadId)
        {
            int count = 1;
          
            
            try
            {

                //string sql = "Select VouchNo from VehicleSaleMaster where  DealerCode='" + DealerCode + "' and Vouchewr='" +ReceiptNo + "'";
                //dt = sysfun.GetData(sql, "BMS0517ConnectionString");

                //if (dt.Rows.Count > 0|| dt.Rows[0]["VouchNo"].ToString()!="")
                //{
                 string   sql = "Delete from GVouMaster where CompCode='" + DealerCode + "' and VouchNo='" + ReceiptNo + "' and Post='N' and DelFlag='N'";
                    dt = sysfun.GetData(sql, "FAMSConnectionString");
                
                    //string strAutoCode = GetNewVoucherNo("GVouMaster", "VouchNo", 3);

                    SqlParameter[] param = {

                                   new SqlParameter("@CompCode",SqlDbType.Char,5),         //0
                                   new SqlParameter("@Booktype",SqlDbType.VarChar,2),      //1
                                   new SqlParameter("@Journalno",SqlDbType.VarChar,4),     //2
                                   new SqlParameter("@VouchNo",SqlDbType.VarChar,50),      //3
                                   new SqlParameter("@SeqNo",SqlDbType.Int),               //4
                                   new SqlParameter("@ContAccCode",SqlDbType.Char,2),      //5
                                   new SqlParameter("@SubCode",SqlDbType.Char,2),          //6
                                   new SqlParameter("@SubSubCode",SqlDbType.Char,2),       //7
                                   new SqlParameter("@LocCode",SqlDbType.Char,2),          //8
                                   new SqlParameter("@DetailCode",SqlDbType.Char,4),       //9
                                   new SqlParameter("@VouchDate",SqlDbType.DateTime),      //10
                                   new SqlParameter("@RecPay",SqlDbType.VarChar,50),       //11
                                   new SqlParameter("@Narration01",SqlDbType.VarChar,200), //12
                                   new SqlParameter("@Narration02",SqlDbType.VarChar,200), //13
                                   new SqlParameter("@ChqBillNo",SqlDbType.VarChar,50),    //14
                                   new SqlParameter("@ChqBillDate",SqlDbType.DateTime),    //15
                                   new SqlParameter("@DebitAmt",SqlDbType.Float),          //16
                                   new SqlParameter("@CreditAmt",SqlDbType.Float),         //17
                                   new SqlParameter("@FYear",SqlDbType.DateTime),          //18
                                   new SqlParameter("@TYear",SqlDbType.DateTime),          //19
                                   new SqlParameter("@DelFlag",SqlDbType.Char,1),          //20
                                   new SqlParameter("@BookNo",SqlDbType.Char,2),           //21
                                   new SqlParameter("@AutoAcc",SqlDbType.VarChar,500),     //22
                                   new SqlParameter("@Post",SqlDbType.Char,1),             //23
                                   new SqlParameter("@Source",SqlDbType.VarChar,50),       //24
                                   new SqlParameter("@AddUser",SqlDbType.Char,50),         //25
                                   new SqlParameter("@AddDate",SqlDbType.DateTime),        //26
                                   new SqlParameter("@AddTime",SqlDbType.DateTime),        //27
                                   new SqlParameter("@AddTerm",SqlDbType.VarChar,50),      //28
                                   new SqlParameter("@CSCode",SqlDbType.Char,8),           //29
                                };

                foreach (var item in model2)
                {
                    sql = "Select Post from GVouMaster where  CompCode='" + item.CompCode + "' and VouchNo='" + item.VouchNo + "' and SeqNo='" + count + "'  ";
                    dt = sysfun.GetData(sql, "FAMSConnectionString");

                    if (dt.Rows.Count > 0)
                    {
                        Postflag = dt.Rows[0]["Post"].ToString();
                        // Delflag = dt.Rows[0]["DelFalg"].ToString();
                        if (Postflag == "Y")
                        {
                            msg = "Voucher Can't Be Edit or Delete. . .! It is Already Posted.";
                            return false;
                        }



                    }



                    string accountcode = item.AutoAcc;

                    string contAccCode = accountcode.Substring(0, 2);
                    string subCode = accountcode.Substring(3, 2);
                    string subSubCode = accountcode.Substring(6, 2);
                    string locCode = accountcode.Substring(9, 2);
                    string detailCode = accountcode.Substring(12, 4);

                    param[0].Value = item.CompCode;
                    param[1].Value = item.Booktype;
                    param[2].Value = item.Journalno;
                    param[3].Value = item.VouchNo;
                    param[4].Value = count++;
                    param[5].Value = contAccCode;
                    param[6].Value = subCode;
                    param[7].Value = subSubCode;
                    param[8].Value = locCode;
                    param[9].Value = detailCode;
                    param[10].Value = sysfun.SaveDate(item.VouchDate);
                    if (type == "ASC")
                    {
                        param[11].Value = "Account Service Charges";
                    } else if (type == "DE")
                    {
                        param[11].Value = "Daily Expense";
                    }
                    else if (type == "VReceipt")
                    {
                        param[11].Value = "Journal Voucher";
                    }
                    else if (type == "CSI")
                    {
                        param[11].Value = "Cash Sale Invoice";
                    }
                    param[12].Value = item.Narration01;
                    param[13].Value = "";
                    param[14].Value = item.ChqBillNo;
                    param[15].Value = (item.ChqBillDate == null ? DBNull.Value : sysfun.SaveDate(item.ChqBillDate));

                    if (Convert.ToDouble(item.DebitAmt) != 0)
                    {
                        param[16].Value = Convert.ToDouble(item.DebitAmt);
                    }
                    else
                    {
                        param[16].Value = 0;
                    }

                    if (Convert.ToDouble(item.CreditAmt) != 0)
                    {
                        param[17].Value = Convert.ToDouble(item.CreditAmt);
                    }
                    else
                    {
                        param[17].Value = 0;
                    }
                    sql = "Select FYear,TYear from FiscalYear where FYear<='" + sysfun.SaveDate(item.VouchDate) + "' and TYear>='" + sysfun.SaveDate(item.VouchDate) + "'";
                    dt = sysfun.GetData(sql, "FAMSConnectionString");
                    if (dt.Rows.Count > 0)
                    {
                        param[18].Value = dt.Rows[0]["FYear"].ToString();
                        param[19].Value = dt.Rows[0]["TYear"].ToString();

                    }
                    else {
                        msg = "Voucher Can't be Save! Please Check Fiscal Year Date ";
                        return false;

                    }

                    
                    param[20].Value = "N";
                    param[21].Value = GetBookNo(accountcode,item.CompCode);
                    param[22].Value = accountcode;
                    param[23].Value = "N";
                    param[24].Value = type;
                    param[25].Value = AuthBase.EmpCode;
                    param[26].Value = sysfun.SaveDate(DateTime.Now.ToString("dd-MM-yyyy"));
                    param[27].Value = DateTime.Now;
                    param[28].Value = GlobalVar.mUserIPAddress;
                    param[29].Value = "00/00000";


                    SqlHelper.ExecuteNonQuery(General.GetFAMConString(), CommandType.StoredProcedure, "sp_GVouMaster_Insert", param);
                    
                }

                string IQuery = "";

                if (type == "ASC")
                {
                    IQuery = "Update ReceiptMaster set VoucherNo ='" + model2.FirstOrDefault().VouchNo + "' , VoucherFlag = 'Y' " +
                                "Where DealerCode='" + model2.FirstOrDefault().CompCode + "' and ReceiptNo ='" + leadId + "'";
                }
                else if (type == "DE")
                {
                    IQuery = "Update DailyExpenseMaster set VoucherNo ='" + model2.FirstOrDefault().VouchNo + "' , VoucherFlag = 'Y' " +
                                "Where DealerCode='" + model2.FirstOrDefault().CompCode + "' and ExpInvNo ='" + leadId + "'";
                }
                else if(type == "VReceipt") {

                    IQuery = "Update ProdRecMaster set VouchNo ='" + model2.FirstOrDefault().VouchNo + "' , VoucherFlag = 'Y' " +
                              "Where DealerCode='" + model2.FirstOrDefault().CompCode + "' and RecNo ='" + leadId + "'";

                }
                else if (type == "CSI")
                {

                    IQuery = "Update VehicleSaleMaster set VoucherNo ='" + model2.FirstOrDefault().VouchNo + "' , VoucherFlag = 'Y' " +
                              "Where DealerCode='" + model2.FirstOrDefault().CompCode + "' and TransCode ='" + leadId + "'";

                }
                if (sysfun.ExecuteQuery_NonQuery(IQuery))
                {
                    IsSaved = true;
                }
            }
            catch (Exception ex)
            {
                IsSaved = false;

                throw ex;
            }

            return IsSaved;
        }
        public static bool Insert_GvoucherReceipt(List<GVouMasterVM> model2, string type,ref string msg)
        {
            int count = 1;
            string vouchNo;

            try
            {
               

                //string strAutoCode = GetNewVoucherNo("GVouMaster", "VouchNo", 3);

                SqlParameter[] param = {

                                   new SqlParameter("@CompCode",SqlDbType.Char,5),         //0
                                   new SqlParameter("@Booktype",SqlDbType.VarChar,2),      //1
                                   new SqlParameter("@Journalno",SqlDbType.VarChar,4),     //2
                                   new SqlParameter("@VouchNo",SqlDbType.VarChar,50),      //3
                                   new SqlParameter("@SeqNo",SqlDbType.Int),               //4
                                   new SqlParameter("@ContAccCode",SqlDbType.Char,2),      //5
                                   new SqlParameter("@SubCode",SqlDbType.Char,2),          //6
                                   new SqlParameter("@SubSubCode",SqlDbType.Char,2),       //7
                                   new SqlParameter("@LocCode",SqlDbType.Char,2),          //8
                                   new SqlParameter("@DetailCode",SqlDbType.Char,4),       //9
                                   new SqlParameter("@VouchDate",SqlDbType.DateTime),      //10
                                   new SqlParameter("@RecPay",SqlDbType.VarChar,50),       //11
                                   new SqlParameter("@Narration01",SqlDbType.VarChar,200), //12
                                   new SqlParameter("@Narration02",SqlDbType.VarChar,200), //13
                                   new SqlParameter("@ChqBillNo",SqlDbType.VarChar,50),    //14
                                   new SqlParameter("@ChqBillDate",SqlDbType.DateTime),    //15
                                   new SqlParameter("@DebitAmt",SqlDbType.Float),          //16
                                   new SqlParameter("@CreditAmt",SqlDbType.Float),         //17
                                   new SqlParameter("@FYear",SqlDbType.DateTime),          //18
                                   new SqlParameter("@TYear",SqlDbType.DateTime),          //19
                                   new SqlParameter("@DelFlag",SqlDbType.Char,1),          //20
                                   new SqlParameter("@BookNo",SqlDbType.Char,2),           //21
                                   new SqlParameter("@AutoAcc",SqlDbType.VarChar,500),     //22
                                   new SqlParameter("@Post",SqlDbType.Char,1),             //23
                                   new SqlParameter("@Source",SqlDbType.VarChar,50),       //24
                                   new SqlParameter("@AddUser",SqlDbType.Char,50),         //25
                                   new SqlParameter("@AddDate",SqlDbType.DateTime),        //26
                                   new SqlParameter("@AddTime",SqlDbType.DateTime),        //27
                                   new SqlParameter("@AddTerm",SqlDbType.VarChar,50),      //28
                                   new SqlParameter("@CSCode",SqlDbType.Char,8),           //29
                                };

                foreach (var item in model2)
                {
                    string sql = "Select Post from GVouMaster where  CompCode='" + item.CompCode + "' and VouchNo='" + item.VouchNo + "' and SeqNo='" + count + "'  ";
                    dt = sysfun.GetData(sql, "FAMSConnectionString");

                    if (dt.Rows.Count > 0)
                    {
                        Postflag = dt.Rows[0]["Post"].ToString();
                        // Delflag = dt.Rows[0]["DelFalg"].ToString();
                        if (Postflag == "Y")
                        {
                            msg = "Voucher Can't Be Edit or Delete. . .! It is Already Posted.";
                            return false;
                        }



                    }

                    string accountcode = item.AutoAcc;

                    string contAccCode = accountcode.Substring(0, 2);
                    string subCode = accountcode.Substring(3, 2);
                    string subSubCode = accountcode.Substring(6, 2);
                    string locCode = accountcode.Substring(9, 2);
                    string detailCode = accountcode.Substring(12, 4);

                    param[0].Value = item.CompCode;
                    param[1].Value = item.Booktype;
                    param[2].Value = item.Journalno;
                    param[3].Value = item.VouchNo;
                    param[4].Value = count++;
                    param[5].Value = contAccCode;
                    param[6].Value = subCode;
                    param[7].Value = subSubCode;
                    param[8].Value = locCode;
                    param[9].Value = detailCode;
                    param[10].Value = sysfun.SaveDate(item.VouchDate);
                    if (type == "ASC")
                    {
                        param[11].Value = "Account Service Charges";
                    }
                    else if (type == "DE")
                    {
                        param[11].Value = "Daily Expense";
                    }
                    else if (type == "VReceipt")
                    {
                        param[11].Value = "General Voucher";
                    }
                    param[12].Value = item.Narration01;
                    param[13].Value = "";
                    param[14].Value = item.ChqBillNo;
                    param[15].Value = (item.ChqBillDate == null ? DBNull.Value : sysfun.SaveDate(item.ChqBillDate));

                    if (Convert.ToDouble(item.DebitAmt) != 0)
                    {
                        param[16].Value = Convert.ToDouble(item.DebitAmt);
                    }
                    else
                    {
                        param[16].Value = 0;
                    }

                    if (Convert.ToDouble(item.CreditAmt) != 0)
                    {
                        param[17].Value = Convert.ToDouble(item.CreditAmt);
                    }
                    else
                    {
                        param[17].Value = 0;
                    }

                    param[18].Value = "2018-07-01";
                    param[19].Value = "2019-06-30";
                    param[20].Value = "N";
                    param[21].Value = GetBookNo(accountcode, item.CompCode);
                    param[22].Value = accountcode;
                    param[23].Value = "N";
                    param[24].Value = type;
                    param[25].Value = AuthBase.EmpCode;
                    param[26].Value = sysfun.SaveDate(DateTime.Now.ToString("dd-MM-yyyy"));
                    param[27].Value = DateTime.Now;
                    param[28].Value = GlobalVar.mUserIPAddress;
                    param[29].Value = "00/00000";


                    SqlHelper.ExecuteNonQuery(General.GetFAMConString(), CommandType.StoredProcedure, "sp_GVouMaster_Insert", param);

                }

                string IQuery = "";

                if (type == "ASC")
                {
                    IQuery = "Update ReceiptMaster set VoucherNo ='" + model2.FirstOrDefault().VouchNo + "' , VoucherFlag = 'Y' " +
                                "Where DealerCode='" + model2.FirstOrDefault().CompCode + "' and ReceiptNo ='" + ReceiptNo + "'";
                }
                else if (type == "DE")
                {
                    IQuery = "Update DailyExpenseMaster set VoucherNo ='" + model2.FirstOrDefault().VouchNo + "' , VoucherFlag = 'Y' " +
                                "Where DealerCode='" + model2.FirstOrDefault().CompCode + "' and ExpInvNo ='" + ReceiptNo + "'";
                }
                else if (type == "VReceipt")
                {

                    IQuery = "Update ProdRecMaster set VouchNo ='" + model2.FirstOrDefault().VouchNo + "' , VoucherFlag = 'Y' " +
                              "Where DealerCode='" + model2.FirstOrDefault().CompCode + "' and RecNo ='" + ReceiptNo + "'";

                }

                if (sysfun.ExecuteQuery_NonQuery(IQuery))
                {
                    IsSaved = true;
                }
            }
            catch (Exception ex)
            {
                IsSaved = false;

                throw ex;
            }

            return IsSaved;
        }
        public static List<VoucherVM> LoadVoucher(string leadId, string dealerCode)
        {
            List<VoucherVM> lst = new List<VoucherVM>();

            SqlParameter[] dsParamInv = {
                new SqlParameter("@CompCode",SqlDbType.Char,5),
                new SqlParameter("@VoucherNo",SqlDbType.VarChar,20)
            };

            dsParamInv[0].Value = dealerCode;
            dsParamInv[1].Value = leadId;
            
            DataSet dsReceipt = new DataSet();

            dsReceipt = SqlHelper.ExecuteDataset(General.GetFAMConString(), CommandType.StoredProcedure, "SP_GetVoucher", dsParamInv);

            //dsReceipt = sysfun.FillDataSet("SP_GetVoucher", dsParamInv);

            if (dsReceipt.Tables[0].Rows.Count > 0)
            {                
                return lst = EnumerableExtension.ToList<VoucherVM>(dsReceipt.Tables[0]);
            }

            return lst;
        }


    }
}
