using Core.CRM.ADO.ViewModel;
using Core.CRM.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CRM.ADO
{
    public class ReversalMethods
    {
        static bool IsSaved = false;
        static bool IsDeleted = false;
        static SqlParameter[] nullSqlParam = null;
        static DataTable dt = new DataTable();
        static string strAutoCode = string.Empty;
        static SysFunction sysfun = new SysFunction();
        //static SysFunctions sysfuns = new SysFunctions();


        static Transaction ObjTrans = new Transaction();
        static SqlTransaction Trans;
        public static DataTable GetDataForModal(string dealerCode)
        {
            try
            {
                SqlParameter[] sqlParam =
                {
                    new SqlParameter("@DealerCode",dealerCode)
                };
                dt = DataAccess.getDataTable("SP_Select_AccountTransaction", sqlParam, General.GetBMSConString());

            }
            catch (Exception ex)
            {

                throw;
            }
            return dt;
        }

        public static bool Insert_Reversal(ReversalVM model,ref string msg)
        {

            try
            {
                if (model.ReversalID == null || model.ReversalID == "")
                {

                    strAutoCode = sysfun.AutoGen("Reversal", "ReversalID", DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MM/yyyy"), model.DealerCode);

                }
                else
                {
                    strAutoCode = model.ReversalID;
                }
                SqlParameter[] param = {
                                 new SqlParameter("@DealerCode",model.DealerCode),//0
								 new SqlParameter("@ReversalID",strAutoCode),//1
								 new SqlParameter("@ReversalDate",sysfun.SaveDate(model.ReversalDate)),//2								 
								 new SqlParameter("@TransactionNo",model.TransactionNo),//3
								 new SqlParameter("@TrType",model.TrType),//4
								 new SqlParameter("@TransactionAmount", model.TransactionAmount),//5
								 new SqlParameter("@Remarks",model.Remarks),//6
								 new SqlParameter("@UpdUser",AuthBase.UserId),//7
                                 new SqlParameter("@UpdDate", DateTime.Parse(DateTime.Now.ToShortDateString())),//7
                                 new SqlParameter("@UpdTime",DateTime.Now),//7
								 new SqlParameter("@UpdTerminal",General.CurrentIP)//8
								 
							};

                if (ObjTrans.BeginTransaction(ref Trans) == true)
                {
                    if (sysfun.ExecuteSP_NonQuery("SP_Insert_Reversal", param,Trans))
                    {
                        IsSaved = true;
                    }
                }
                
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            return IsSaved;
        }

        public static bool Insert_AccountTransaction(AccountTransactionVM AccountModel, ref string msg)
        {
            string strReceiptNo;
            try
            {
                DataTable dt = sysfun.GetData("SP_GET_BlnAmt_AccountTransaction '" + AccountModel.DealerCode + "','" + AccountModel.Reference + "'", "BMS0517ConnectionString");

                DataTable dt2 = sysfun.GetData("Select ShortForm from VehExpHead where VehExpCode = '" + AccountModel.TrType + "' and DealerCode in ('" + AccountModel.DealerCode + "','COMON')", "BMS0517ConnectionString");

                DataTable dt3 = sysfun.GetData("Select AccountCode from Customer where CusCode = '" + AccountModel.CusCode + "' and DealerCode = '" + AccountModel.DealerCode + "'", "BMS0517ConnectionString");
                
                    if (AccountModel.Reference != null)
                    {

                        string getNextTransCode = "declare @lastval varchar(14),@id int " +
                                           "set @id = (select count(*) from AccountTransaction) " +
                                           "set @id=@id+1 " +
                                           "if len(@id) = 1 " +
                                           "set @lastval='" + "'+cast((YEAR(getDate()) ) %100  as varchar(10)) +'00000' " +
                                           "if len(@id) = 2 " +
                                           "set @lastval='" + "'+cast((YEAR(getDate())  ) %100  as varchar(10)) +'0000' " +
                                           "if len(@id) = 3 " +
                                           "set @lastval='" + "'+cast((YEAR(getDate())  ) %100  as varchar(10)) +'000' " +
                                           "if len(@id) >= 4 " +
                                           "set @lastval='" + "'+cast((YEAR(getDate())  ) %100  as varchar(10)) +'00' " +
                                           "if len(@id) >= 5 " +
                                           "set @lastval='" + "'+cast((YEAR(getDate())  ) %100  as varchar(10)) +'0' " +
                                           "declare @i varchar(14) " +
                                           "set @i = CAST(@id as varchar(14)) " +
                                           "set @lastval = @lastval+@i " +
                                           "select @lastval as TransactionCode";

                    DataTable dt4 = DataAccess.getDataTableByQuery(getNextTransCode, nullSqlParam, General.GetBMSConString());

                        strReceiptNo = dt4.Rows[0]["TransactionCode"].ToString();

                        //strReceiptNo = sysfun.AutoGen("AccountTransaction", "TransactionCode", DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MM/yyyy"), item.DealerCode);

                        SqlParameter[] sqlParam =
                                             {
                                                 new SqlParameter("@DealerCode",AccountModel.DealerCode),//0
                                                 new SqlParameter("@TransactionCode",strReceiptNo),//3
                                                 new SqlParameter("@TransactionDate",sysfun.SaveDate(AccountModel.TransactionDate)),//4
                                                 new SqlParameter("@CusCode",AccountModel.CusCode),//5
                                                 new SqlParameter("@AccountCode",dt3.Rows[0]["AccountCode"].ToString()),//6
                                                 new SqlParameter("@InvType",AccountModel.InvType),//7
                                                 new SqlParameter("@TrType",AccountModel.TrType),//8
                                                 new SqlParameter("@Narration",AccountModel.Narration),//9
                                                 new SqlParameter("@Reference",AccountModel.Reference),//10
                                                 new SqlParameter("@Debit",AccountModel.Debit),//11
                                                 new SqlParameter("@Credit",AccountModel.Credit),//12
                                                 new SqlParameter("@Balance",dt.Rows[0]["Balance"].ToString() == "" ? "0" :dt.Rows[0]["Balance"].ToString() ),//13
                                                 new SqlParameter("@Remarks",(object)DBNull.Value),//14
                                                 new SqlParameter("@CreateDate",DateTime.Now) ,
                                                 new SqlParameter("@CreateTime",DateTime.Now) ,
                                                 new SqlParameter("@CreateUser",AuthBase.EmpCode),//17
                                                 new SqlParameter("@CreateTerm",AuthBase.UserId),//18
                                                 new SqlParameter("@UpdDate",(object)DBNull.Value),//19
                                                 new SqlParameter("@UpdTime",(object)DBNull.Value),//20
                                                 new SqlParameter("@UpdUser",(object)DBNull.Value),//21
                                                 new SqlParameter("@UpdTerm",(object)DBNull.Value),//22
                                                 new SqlParameter("@ReceiptNo",strAutoCode),//22

                                             };

                        if (sysfun.ExecuteSP_NonQuery("SP_Insert_AccountTransaction", sqlParam, Trans) == true)
                        {
                            IsSaved = true;
                        }
                        else
                        {
                            ObjTrans.RollBackTransaction(ref Trans);
                            return false;
                        }
                    }

                ObjTrans.CommittTransaction(ref Trans);

            }
            catch (Exception ex)
            {
                ObjTrans.RollBackTransaction(ref Trans);
                msg = ex.Message;
                return false;
            }

            return IsSaved;
        }
    }
}
