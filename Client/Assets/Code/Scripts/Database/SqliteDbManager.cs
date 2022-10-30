namespace ScotlandYard.Scripts.Database
{
    using Mono.Data.Sqlite;
    using ScotlandYard.Interfaces;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using UnityEngine;

    public class SqliteDbManager : IDbManager
    {
        protected string path = Application.dataPath + "/Data/DataSave.db";
        protected string connectionString = "";

        public SqliteDbManager()
        {
            this.connectionString = "URI=file:" + path;
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

        public void CreateDatabase(string sqlQuery)
        {
            bool deleteFile = false;

            sqlQuery = sqlQuery.Replace("\n\n","\n");
            string[] queries = sqlQuery.Split(';');

            if(!File.Exists(path))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));

                SqliteConnection.CreateFile(path);

                using (IDbConnection connection = new SqliteConnection(connectionString))
                {
                    connection.Open();

                    using (IDbCommand command = connection.CreateCommand())
                    {
                        foreach (string query in queries)
                        {
                            command.CommandText = query + ";";
                            try
                            {
                                command.ExecuteNonQuery();
                            }
                            catch (SqliteException ex)
                            {
                                deleteFile = true;
                                Debug.LogWarningFormat("Query: {0}\n--------------\n{1}", query, ex.Message);
                            }
                        }
                    }

                    connection.Close();
                }

                if (deleteFile)
                {
                    File.Delete(path);
                }
            }
        }
    }
}
