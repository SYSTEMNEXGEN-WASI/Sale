using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Core.CRM.Helper
{
    public class DataAccess
    {
        public static object GetDataValue(object value)
        {
            if (value == null)
            {
                return DBNull.Value;
            }
            return value;
        }


        #region Async Helpers

        public async static Task<int> ExecuteNonQueryAsync(string connectionString, CommandType commandType, string commandText, SqlParameter[] commandParameters)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = commandType;
                    command.CommandText = commandText;
                    if (commandParameters != null && commandParameters.Length > 0)
                        command.Parameters.AddRange(commandParameters);
                    int retval = await command.ExecuteNonQueryAsync();
                    connection.Close();
                    command.Parameters.Clear();
                    return retval;
                }
            }
        }

        public async static Task<DataSet> ExecuteDatasetAsync(string connectionString, CommandType commandType, string commandText, SqlParameter[] commandParameters)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandTimeout = 0;
                    command.CommandType = commandType;
                    command.CommandText = commandText;

                    if (commandParameters != null && commandParameters.Length > 0)
                        command.Parameters.AddRange(commandParameters);

                    DataSet ds = await Task.Run(() => { return FillData(command, connection); });

                    return ds;
                }
            }
        }

        public static DataTable CreateTableFromSchema(DataTable schemaTable)
        {
            DataTable dt = new DataTable();
            for (int i = 0; i < schemaTable.Rows.Count; i++)
            {
                dt.Columns.Add(schemaTable.Rows[i]["ColumnName"].ToString());
            }
            return dt;
        }
        public static DataRow GetDataRow(IDataRecord dr, DataTable dt)
        {
            DataRow drow;
            drow = dt.NewRow();
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                drow[dt.Columns[i].ColumnName] = dr[dt.Columns[i].ColumnName];
            }
            return drow;
        }

        private static DataSet FillData(SqlCommand comm, SqlConnection connectionName)
        {
            SqlDataAdapter adp = new SqlDataAdapter();

            adp.SelectCommand = comm;

            DataSet ds = new DataSet();

            adp.Fill(ds);

            return ds;
        }

        #endregion


        #region Sync Helper

        public static void executeNonQuery(string spName, SqlParameter[] parameters, string connectionString)
        {
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
                sqlcon.Open();
                command.ExecuteNonQuery();
            }

        }

        public static bool executeNonQuery(string spName, SqlParameter[] parameters, string connectionString, bool returnBool)
        {
            try
            {
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
                    sqlcon.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public static int? executeNonQuery(string spName, SqlParameter[] parameters, string connectionString, int? returnUniqueId)
        {
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                int? newId = null;
                SqlCommand command = new SqlCommand(spName, sqlcon);
                command.CommandType = CommandType.StoredProcedure;
                if (parameters != null)
                {
                    foreach (SqlParameter param in parameters)
                        command.Parameters.Add(param);
                }
                sqlcon.Open();
                newId = Convert.ToInt32(command.ExecuteScalar());

                return newId;
            }
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

        public static DataTable getDataTableByQuery(string query, SqlParameter[] parameters, string connectionString)
        {

            DataTable table = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, sqlcon);
                command.CommandType = CommandType.Text;
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

        public static int? getScalerValueByQuery(string query, SqlParameter[] parameters, string connectionString)
        {
            DataTable table = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, sqlcon);
                command.CommandType = CommandType.Text;
                if (parameters != null)
                {
                    foreach (SqlParameter param in parameters)
                    {
                        command.Parameters.Add(param);
                    }
                }
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(table);
                sqlcon.Open();
                return (int?)command.ExecuteScalar();
            }
        }

        public static Int32 getScalerValueByQuery2(string spName, SqlParameter[] parameters, string connectionString)
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
                command.Parameters.Add("@RetStatusCode", SqlDbType.Int).Direction = ParameterDirection.Output;
                sqlcon.Open();
                command.ExecuteNonQuery();
                //   command.ExecuteScalar();
                return Int32.Parse(command.Parameters["@RetStatusCode"].Value.ToString());

            }
        }

        //[INGIC_GetCustomerStatusCode]



        public static DataSet getDataSet(string spName, SqlParameter[] parameters, string connectionString)
        {
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

                DataSet ds = new DataSet();

                adapter.Fill(ds);



                return ds;
            }
        }


        public static DataTable executeNonQuerySqlTransaction(string spName, SqlParameter[] parameters, SqlTransaction sqlTransaction, SqlConnection sqlcon)
        {

            DataTable table = new DataTable();
            SqlCommand command = sqlcon.CreateCommand();
            command.CommandText = spName;
            command.CommandType = CommandType.StoredProcedure;
            command.Transaction = sqlTransaction;
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


        public static DataTable getDataTableConn(string spName, SqlParameter[] parameters, SqlConnection sqlcon)
        {

            DataTable table = new DataTable();
            SqlCommand command = sqlcon.CreateCommand();
            command.CommandText = spName;
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
        public static DataSet getDataSetConn(string spName, SqlParameter[] parameters, SqlConnection sqlcon)
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
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            return ds;

        }


        #endregion




    }


    public static class Transactions
    {

        public static SqlTransaction InitiateTransactions(SqlConnection sqlCon)
        {
            SqlTransaction sqlTran;
            sqlTran = sqlCon.BeginTransaction(IsolationLevel.ReadUncommitted);
            return sqlTran;
        }

        public static void CommitTransactions(SqlTransaction sqlTrans)
        {
            sqlTrans.Commit();
        }

        public static void RollBackTransactions(SqlTransaction sqlTrans)
        {
            if (sqlTrans.Connection != null)
                sqlTrans.Rollback();
        }

    }


    public static class EnumerableExtension
    {

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumeration"></param>
        /// <param name="action"></param>
        public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
        {

            foreach (T item in enumeration)
            {
                action(item);
            }
        }

        /// <summary>
        /// Convert  list to Data Table
        /// </summary>
        /// <typeparam name="T">Target Class</typeparam>
        /// <param name="varlist">list you want to convert it to Data Table</param>
        /// <param name="fn">Delegate Function to Create Row</param>
        /// <returns>Data Table That Represent List data</returns>
        public static DataTable ToADOTable<T>(this IEnumerable<T> varlist, CreateRowDelegate<T> fn)
        {
            DataTable toReturn = new DataTable();

            // Could add a check to verify that there is an element 0
            T TopRec = varlist.ElementAtOrDefault(0);

            if (TopRec == null)
                return toReturn;

            // Use reflection to get property names, to create table
            // column names

            PropertyInfo[] oProps = ((Type)TopRec.GetType()).GetProperties();

            foreach (PropertyInfo pi in oProps)
            {
                Type pt = pi.PropertyType;
                if (pt.IsGenericType && pt.GetGenericTypeDefinition() == typeof(Nullable<>))
                    pt = Nullable.GetUnderlyingType(pt);
                toReturn.Columns.Add(pi.Name, pt);
            }

            foreach (T rec in varlist)
            {
                DataRow dr = toReturn.NewRow();
                foreach (PropertyInfo pi in oProps)
                {
                    object o = pi.GetValue(rec, null);
                    if (o == null)
                        dr[pi.Name] = DBNull.Value;
                    else
                        dr[pi.Name] = o;
                }
                toReturn.Rows.Add(dr);
            }

            return toReturn;
        }

        /// <summary>
        /// Convert  list to Data Table
        /// </summary>
        /// <typeparam name="T">Target Class</typeparam>
        /// <param name="varlist">list you want to convert it to Data Table</param>
        /// <returns>Data Table That Represent List data</returns>
        public static DataTable ToADOTable<T>(this IEnumerable<T> varlist)
        {
            DataTable toReturn = new DataTable();

            // Could add a check to verify that there is an element 0
            T TopRec = varlist.ElementAtOrDefault(0);

            if (TopRec == null)
                return toReturn;

            // Use reflection to get property names, to create table
            // column names

            PropertyInfo[] oProps = ((Type)TopRec.GetType()).GetProperties();

            foreach (PropertyInfo pi in oProps)
            {
                Type pt = pi.PropertyType;
                if (pt.IsGenericType && pt.GetGenericTypeDefinition() == typeof(Nullable<>))
                    pt = Nullable.GetUnderlyingType(pt);
                toReturn.Columns.Add(pi.Name, pt);
            }

            foreach (T rec in varlist)
            {
                DataRow dr = toReturn.NewRow();
                foreach (PropertyInfo pi in oProps)
                {
                    object o = pi.GetValue(rec, null);

                    if (o == null)
                        dr[pi.Name] = DBNull.Value;
                    else
                        dr[pi.Name] = o;
                }
                toReturn.Rows.Add(dr);
            }

            return toReturn;
        }


        /// <summary>
        /// Convert Data Table To List of Type T
        /// </summary>
        /// <typeparam name="T">Target Class to convert data table to List of T </typeparam>
        /// <param name="datatable">Data Table you want to convert it</param>
        /// <returns>List of Target Class</returns>
        public static List<T> ToList<T>(this DataTable datatable) where T : new()
        {
            List<T> Temp = new List<T>();
            try
            {
                List<string> columnsNames = new List<string>();
                foreach (DataColumn DataColumn in datatable.Columns)
                    columnsNames.Add(DataColumn.ColumnName);
                Temp = datatable.AsEnumerable().ToList().ConvertAll<T>(row => getObject<T>(row, columnsNames));
                return Temp;
            }
            catch { return Temp; }
        }

        public static T getObject<T>(DataRow row, List<string> columnsName) where T : new()
        {
            T obj = new T();
            try
            {
                string columnname = "";
                string value = "";
                PropertyInfo[] Properties; Properties = typeof(T).GetProperties();
                foreach (PropertyInfo objProperty in Properties)
                {
                    columnname = columnsName.Find(name => name.ToLower() == objProperty.Name.ToLower());
                    if (!string.IsNullOrEmpty(columnname))
                    {
                        value = row[columnname].ToString();
                        if (!string.IsNullOrEmpty(value))
                        {
                            if (Nullable.GetUnderlyingType(objProperty.PropertyType) != null)
                            {
                                value = row[columnname].ToString().Replace("$", "").Replace(",", "");
                                objProperty.SetValue(obj, Convert.ChangeType(value, Type.GetType(Nullable.GetUnderlyingType(objProperty.PropertyType).ToString())), null);
                            }
                            else
                            {
                                value = row[columnname].ToString().Replace("%", "%");
                                objProperty.SetValue(obj, Convert.ChangeType(value, Type.GetType(objProperty.PropertyType.ToString())), null);
                            }
                        }
                    }
                } return obj;
            }
            catch { return obj; }
        }

        public delegate object[] CreateRowDelegate<T>(T t);

        public static IEnumerable<T> Select<T>(
            this SqlDataReader reader, Func<SqlDataReader, T> projection)
        {
            while (reader.Read())
            {
                yield return projection(reader);
            }
        }


        public static DateTime FirstDayOfMonth(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, 1);
        }
        public static DateTime LastDayOfMonth(DateTime dateTime)
        {
            DateTime firstDayOfTheMonth = new DateTime(dateTime.Year, dateTime.Month, 1);
            return firstDayOfTheMonth.AddMonths(1).AddDays(-1);
        }

    }
}
