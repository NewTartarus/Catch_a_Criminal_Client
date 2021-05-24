using System.Collections.Generic;
using System.Data;
using Mono.Data.Sqlite;
using ScotlandYard.Interface;
using UnityEngine;

namespace ScotlandYard.Scripts.Database
{
    public class SqliteDbManager : IDbManager
    {
        protected string connectionString = "";

        public SqliteDbManager(string connectionString = null)
        {
            if(connectionString == null)
            {
                this.connectionString = "URI=file:" + Application.dataPath + "/StreamingAssets/DataSave.db";
            }
            else
            {
                this.connectionString = connectionString;
            }
            
        }

        public List<object[]> Read(string sqlQuery, params IDbDataParameter[] parameters)
        {
            List<object[]> returnValues = new List<object[]>();
            
            using (IDbConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = sqlQuery;
                    if(parameters != null && parameters.Length > 0)
                    {
                        command.CreateParameter();
                        foreach(SqliteParameter para in parameters)
                        {
                            command.Parameters.Add(para);
                        }
                    }

                    using (IDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            object[] values = new object[reader.FieldCount];
                            int fieldCount = reader.GetValues(values);

                            returnValues.Add(values);
                        }

                        reader.Close();
                    }
                }

                connection.Close();
            }

            return returnValues;
        }

        public int Execute(string sqlQuery, params IDbDataParameter[] parameters)
        {
            int affectedRows = 0;
            
            using (IDbConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = sqlQuery;
                    if (parameters != null && parameters.Length > 0)
                    {
                        command.CreateParameter();
                        foreach (SqliteParameter para in parameters)
                        {
                            command.Parameters.Add(para);
                        }
                    }

                    affectedRows = command.ExecuteNonQuery();
                }

                connection.Close();
            }

            return affectedRows;
        }
    }
}
