namespace ScotlandYard.Scripts.Database.DAOs
{
    using Mono.Data.Sqlite;
    using ScotlandYard.Interfaces;
    using System;
    using System.Collections.Generic;

    public class AiTemplateDAO : IDataAccessObject<object[]>
    {
        protected static AiTemplateDAO instance = null;
        public static AiTemplateDAO getInstance()
        {
            if (instance == null)
            {
                instance = new AiTemplateDAO();
            }

            return instance;
        }

        public int Delete(object[] value)
        {
            IDbManager dbMgr = new SqliteDbManager();
            if(value[0] is int id)
            {
                return dbMgr.Execute("DELETE FROM ai_templates WHERE id = @id;", new SqliteParameter("@id", id));
            }
            else if(value[0] is string name)
            {
                return dbMgr.Execute("DELETE FROM ai_templates WHERE name = @name;", new SqliteParameter("@name", name));
            }

            return -1;
        }

        public int Insert(object[] value)
        {
            IDbManager dbMgr = new SqliteDbManager();

            return dbMgr.Execute("INSERT INTO ai_templates (name) VALUES (@name);", new SqliteParameter("@name", value[0]));
        }

        public List<object[]> Read(params object[] values)
        {
            IDbManager dbMgr = new SqliteDbManager();

            if (values[0] is int id)
            {
                return dbMgr.Read("SELECT * FROM ai_templates WHERE id = @id;", new SqliteParameter("@id", id));
            }
            else if (values[0] is string name)
            {
                return dbMgr.Read("SELECT * FROM ai_templates WHERE name = @name;", new SqliteParameter("@name", name));
            }

            return new List<object[]>();
        }

        public List<object[]> ReadAll()
        {
            IDbManager dbMgr = new SqliteDbManager();
            return dbMgr.Read("SELECT * FROM ai_templates;");
        }

        public int Update(object[] value)
        {
            IDbManager dbMgr = new SqliteDbManager();
            return dbMgr.Execute("UPDATE ai_templates SET name = @name WHERE id = @id;", new SqliteParameter("@id", value[0]), new SqliteParameter("@name", value[1]));
        }

        public int Count()
        {
            IDbManager dbMgr = new SqliteDbManager();
            List<object[]> values = dbMgr.Read("SELECT count(*) FROM ai_templates;");

            if(values.Count > 0)
            {
                return Convert.ToInt32(values[0][0]);
            }

            return -1;
        }
    }
}
