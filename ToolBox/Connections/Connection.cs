using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace ToolBox.Connections
{
    public class Connection : IConnection
    {
        private string _connectionString;
        private DbProviderFactory _providerFactory;

        public Connection(IConnectionInfo connectionInfo, DbProviderFactory providerFactory)
        {
            if (providerFactory is null)
                throw new ArgumentNullException(nameof(providerFactory));

            _connectionString = connectionInfo.ConnectionString;
            _providerFactory = providerFactory;

            using (DbConnection dbConnection = CreateConnection())
            {
                dbConnection.Open();
            }
        }

        private DbConnection CreateConnection()
        {
            DbConnection dbConnection = _providerFactory.CreateConnection();
            dbConnection.ConnectionString = _connectionString;
            return dbConnection;
        }

        private DbCommand CreateCommand(Command command, DbConnection dbConnection)
        {
            DbCommand dbCommand = dbConnection.CreateCommand();
            dbCommand.CommandText = command.Query;

            if (command.IsStoredProcedure)
                dbCommand.CommandType = CommandType.StoredProcedure;

            foreach (KeyValuePair<string, object> keyValuePair in command.Parameters)
            {
                DbParameter dbParameter = _providerFactory.CreateParameter();
                dbParameter.ParameterName = keyValuePair.Key;
                dbParameter.Value = keyValuePair.Value;

                dbCommand.Parameters.Add(dbParameter);
            }

            return dbCommand;
        }

        public DataTable GetDataTable(Command command)
        {
            using (DbConnection dbConnection = CreateConnection())
            {
                using (DbCommand dbCommand = CreateCommand(command, dbConnection))
                {
                    using (DbDataAdapter dbDataAdapter = _providerFactory.CreateDataAdapter())
                    {
                        dbDataAdapter.SelectCommand = dbCommand;
                        DataTable dataTable = new DataTable();

                        dbDataAdapter.Fill(dataTable);
                        return dataTable;
                    }
                }
            }
        }



        public int ExecuteNonQuery(Command command)
        {
            using (DbConnection dbConnection = CreateConnection())
            {
                using (DbCommand dbCommand = CreateCommand(command, dbConnection))
                {
                    dbConnection.Open();
                    return dbCommand.ExecuteNonQuery();
                }
            }
        }

        public object ExecuteScalar(Command command)
        {
            using (DbConnection dbConnection = CreateConnection())
            {
                using (DbCommand dbCommand = CreateCommand(command, dbConnection))
                {
                    dbConnection.Open();
                    object o = dbCommand.ExecuteScalar();
                    return o is DBNull ? null : o;
                }
            }
        }

        public IEnumerable<TResult> ExecuteReader<TResult>(Command command, Func<IDataRecord, TResult> selector)
        {
            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            using (DbConnection dbConnection = CreateConnection())
            {
                using (DbCommand dbCommand = CreateCommand(command, dbConnection))
                {
                    dbConnection.Open();
                    using (DbDataReader dbDataReader = dbCommand.ExecuteReader())
                    {
                        while (dbDataReader.Read())
                            yield return selector(dbDataReader);
                    }
                }
            }
        }
    }
}
