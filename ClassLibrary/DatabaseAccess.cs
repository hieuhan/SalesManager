using System;
using System.Configuration;
using System.Data;
using System.Data.Common;

namespace ClassLibrary
{
    public class DatabaseAccess
    {
        public static string ConnectionString { get; set; }

        public static string ProviderName { get; set; }

        private DbConnection _connection;

        public DatabaseAccess()
        {
            try
            {
                ProviderName = ConfigurationManager.AppSettings["ProviderName"];
            }
            catch
            {
                ProviderName = "System.Data.SqlClient";
            }

            try
            {
                ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];
            }
            catch
            {
                throw new Exception("Không tìm thấy chuỗi kết nối.");
            }
            _connection = CreateConnection();
        }
        public DatabaseAccess(string providerName, string connectionString)
        {
            _connection = CreateConnection(providerName, connectionString);
        }

        public static DbConnection CreateConnection()
        {
            try
            {
                DbProviderFactory dbFactory = DbProviderFactories.GetFactory(ProviderName);
                DbConnection dbconn = dbFactory.CreateConnection();
                if (dbconn != null)
                {
                    dbconn.ConnectionString = ConnectionString;
                }
                return dbconn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DbConnection CreateConnection(string providerName, string connectionString)
        {
            try
            {
                DbProviderFactory dbFactory = DbProviderFactories.GetFactory(providerName);
                DbConnection dbconn = dbFactory.CreateConnection();
                if (dbconn != null)
                {
                    dbconn.ConnectionString = connectionString;
                }
                return dbconn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DbCommand GetStoredProcCommand(string storedProcedure)
        {
            try
            {
                DbCommand dbCommand = _connection.CreateCommand();
                dbCommand.CommandText = storedProcedure;
                dbCommand.CommandType = CommandType.StoredProcedure;
                return dbCommand;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DbCommand GetSqlStringCommand(string sqlQuery)
        {
            try
            {
                DbCommand dbCommand = _connection.CreateCommand();
                dbCommand.CommandText = sqlQuery;
                dbCommand.CommandType = CommandType.Text;
                return dbCommand;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Parameters
        public void AddParameterCollection(DbCommand cmd, DbParameterCollection dbParameterCollection)
        {
            try
            {
                foreach (DbParameter dbParameter in dbParameterCollection)
                {
                    cmd.Parameters.Add(dbParameter);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AddOutParameter(DbCommand cmd, string parameterName, DbType dbType, int size = 0)
        {
            try
            {
                DbParameter dbParameter = cmd.CreateParameter();
                dbParameter.DbType = dbType;
                dbParameter.ParameterName = parameterName;
                if (size > 0)
                {
                    dbParameter.Size = size;
                }
                dbParameter.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(dbParameter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AddInParameter(DbCommand cmd, string parameterName, DbType dbType, object value)
        {
            try
            {
                DbParameter dbParameter = cmd.CreateParameter();
                dbParameter.DbType = dbType;
                dbParameter.ParameterName = parameterName;
                dbParameter.Value = value;
                dbParameter.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(dbParameter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddInputOutputParameter(DbCommand cmd, string parameterName, DbType dbType, object value)
        {
            try
            {
                DbParameter dbParameter = cmd.CreateParameter();
                dbParameter.DbType = dbType;
                dbParameter.ParameterName = parameterName;
                dbParameter.Value = value;
                dbParameter.Direction = ParameterDirection.InputOutput;
                cmd.Parameters.Add(dbParameter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddReturnParameter(DbCommand cmd, string parameterName, DbType dbType)
        {
            try
            {
                DbParameter dbParameter = cmd.CreateParameter();
                dbParameter.DbType = dbType;
                dbParameter.ParameterName = parameterName;
                dbParameter.Direction = ParameterDirection.ReturnValue;
                cmd.Parameters.Add(dbParameter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DbParameter GetParameter(DbCommand cmd, string parameterName)
        {
            return cmd.Parameters[parameterName];
        }

        #endregion

        #region Execute Database Functions

        public DataSet ExecuteDataSet(DbCommand cmd)
        {
            DataSet ds = new DataSet();
            try
            {
                DbProviderFactory dbfactory = DbProviderFactories.GetFactory(ProviderName);
                DbDataAdapter dbDataAdapter = dbfactory.CreateDataAdapter();
                if (dbDataAdapter != null)
                {
                    dbDataAdapter.SelectCommand = cmd;
                    dbDataAdapter.Fill(ds);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                {
                    _connection.Close();
                    _connection.Dispose();
                    cmd.Dispose();
                }
            }
            return ds;
        }

        public DataTable ExecuteDataTable(DbCommand cmd)
        {
            DataTable dataTable = new DataTable();
            try
            {
                DbProviderFactory dbfactory = DbProviderFactories.GetFactory(ProviderName);
                DbDataAdapter dbDataAdapter = dbfactory.CreateDataAdapter();
                if (dbDataAdapter != null)
                {
                    dbDataAdapter.SelectCommand = cmd;
                    dbDataAdapter.Fill(dataTable);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                {
                    _connection.Close();
                    _connection.Dispose();
                    cmd.Dispose();
                }
            }
            return dataTable;
        }

        public DbDataReader ExecuteReader(DbCommand cmd)
        {
            DbDataReader reader = null;
            try
            {
                if (cmd.Connection.State == ConnectionState.Closed)
                {
                    cmd.Connection.Open();
                }
                reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return reader;
        }
        public int ExecuteNonQuery(DbCommand cmd)
        {
            int ret = -1;
            try
            {
                cmd.Connection.Open();
                ret = cmd.ExecuteNonQuery();
                cmd.Connection.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (cmd.Connection.State == ConnectionState.Open)
                {
                    cmd.Connection.Close();
                    cmd.Dispose();
                }
            }
            return ret;
        }

        public object ExecuteScalar(DbCommand cmd)
        {
            object ret;
            try
            {
                cmd.Connection.Open();
                ret = cmd.ExecuteScalar();
                cmd.Connection.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (cmd.Connection.State == ConnectionState.Open)
                {
                    cmd.Connection.Close();
                    cmd.Dispose();
                }
            }
            return ret;
        }

        #endregion

        #region Execute Database Functions Transactions

        public DataSet ExecuteDataSet(DbCommand cmd, DatabaseTransactions databaseTransaction)
        {
            DataSet ds = new DataSet();
            try
            {
                cmd.Connection = databaseTransaction.DbConnection;
                cmd.Transaction = databaseTransaction.DbTransactions;
                DbProviderFactory dbfactory = DbProviderFactories.GetFactory(ProviderName);
                DbDataAdapter dbDataAdapter = dbfactory.CreateDataAdapter();
                if (dbDataAdapter != null)
                {
                    dbDataAdapter.SelectCommand = cmd;
                    dbDataAdapter.Fill(ds);
                }
            }
            catch (Exception ex)
            {
                databaseTransaction.RollBack();
                throw ex;
            }
            finally
            {
                cmd.Parameters.Clear();
                if (cmd.Connection.State == ConnectionState.Open)
                {
                    cmd.Connection.Close();
                    cmd.Dispose();
                }
            }
            return ds;
        }

        public DataTable ExecuteDataTable(DbCommand cmd, DatabaseTransactions databaseTransaction)
        {
            DataTable dataTable = new DataTable();
            try
            {
                cmd.Connection = databaseTransaction.DbConnection;
                cmd.Transaction = databaseTransaction.DbTransactions;
                DbProviderFactory dbfactory = DbProviderFactories.GetFactory(ProviderName);
                DbDataAdapter dbDataAdapter = dbfactory.CreateDataAdapter();
                if (dbDataAdapter != null)
                {
                    dbDataAdapter.SelectCommand = cmd;
                    dbDataAdapter.Fill(dataTable);
                }
            }
            catch (Exception ex)
            {
                databaseTransaction.RollBack();
                throw ex;
            }
            finally
            {
                cmd.Parameters.Clear();
                if (cmd.Connection.State == ConnectionState.Open)
                {
                    cmd.Connection.Close();
                    cmd.Dispose();
                }
            }
            return dataTable;
        }

        public DbDataReader ExecuteReader(DbCommand cmd, DatabaseTransactions databaseTransaction)
        {
            DbDataReader reader;
            try
            {
                cmd.Connection.Close();
                cmd.Connection = databaseTransaction.DbConnection;
                cmd.Transaction = databaseTransaction.DbTransactions;
                reader = cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (cmd.Connection.State == ConnectionState.Open)
                {
                    cmd.Connection.Close();
                    cmd.Dispose();
                }
            }
            return reader;
        }
        public int ExecuteNonQuery(DbCommand cmd, DatabaseTransactions databaseTransaction)
        {
            int ret = -1;
            try
            {
                cmd.Connection.Close();
                cmd.Connection = databaseTransaction.DbConnection;
                cmd.Transaction = databaseTransaction.DbTransactions;
                ret = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (cmd.Connection.State == ConnectionState.Open)
                {
                    cmd.Connection.Close();
                    cmd.Dispose();
                }
            }
            return ret;
        }

        public object ExecuteScalar(DbCommand cmd, DatabaseTransactions databaseTransaction)
        {
            object obj = null;
            try
            {
                cmd.Connection.Close();
                cmd.Connection = databaseTransaction.DbConnection;
                cmd.Transaction = databaseTransaction.DbTransactions;
                obj = cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (cmd.Connection.State == ConnectionState.Open)
                {
                    cmd.Connection.Close();
                    cmd.Dispose();
                }
            }
            return obj;
        }

        #endregion

    }

    public class DatabaseTransactions : IDisposable
    {
        private DbConnection _conn;
        private DbTransaction _dbTransactions;
        public DbConnection DbConnection
        {
            get { return _conn; }
        }
        public DbTransaction DbTransactions
        {
            get { return _dbTransactions; }
        }

        public DatabaseTransactions()
        {
            _conn = DatabaseAccess.CreateConnection();
            _conn.Open();
            _dbTransactions = _conn.BeginTransaction();
        }
        public DatabaseTransactions(string providerName, string connectionString)
        {
            _conn = DatabaseAccess.CreateConnection(providerName, connectionString);
            _conn.Open();
            _dbTransactions = _conn.BeginTransaction();
        }
        public void Commit()
        {
            _dbTransactions.Commit();
            Colse();
        }

        public void RollBack()
        {
            _dbTransactions.Rollback();
            Colse();
        }

        public void Dispose()
        {
            Colse();
        }

        public void Colse()
        {
            if (_conn.State == ConnectionState.Open)
            {
                _conn.Close();
            }
        }
    }

    public static class SmartReader
    {
        public static T ReadAs<T>(this DbDataReader reader, string columnName)
        {
            try
            {
                int ordinal = reader.GetOrdinal(columnName);
                return !reader.IsDBNull(ordinal) ? (T)reader.GetValue(ordinal) : default(T);
            }
            catch (IndexOutOfRangeException exception)
            {
                throw new ApplicationException("'" + columnName + "' is invalid.", exception);
            }
        }
    }
}
