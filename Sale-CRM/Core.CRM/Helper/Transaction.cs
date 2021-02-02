using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CRM.Helper
{
    public class Transaction
    {

        string ConnectionString;
        public SqlConnection sql = new SqlConnection();
        //SysFunctions myFunc = new SysFunctions();
        
        public Transaction()
        {
            try
            {
                ConnectionString = General.GetBMSConString();

                //sql = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                sql = new SqlConnection(ConnectionString);
                //sql.ConnectionString.Equals(ConnectionString);
                sql.Open();
            }
            catch (Exception ex)
            {
                if (sql.State != ConnectionState.Closed)
                {
                    sql.Close();
                    sql.Open();
                }
            }
        }
        public bool BeginTransaction(ref SqlTransaction sql_transaction)
        {
            try
            {
                if (sql.State == ConnectionState.Open)
                {
                    sql.Close();
                }
                sql.Open();
                sql_transaction = sql.BeginTransaction(IsolationLevel.ReadUncommitted);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool CommittTransaction(ref SqlTransaction sql_transaction)
        {
            try
            {
                sql_transaction.Commit();
                sql.Close();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool RollBackTransaction(ref SqlTransaction sql_transaction)
        {
            try
            {
                sql_transaction.Rollback();
                sql.Close();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
