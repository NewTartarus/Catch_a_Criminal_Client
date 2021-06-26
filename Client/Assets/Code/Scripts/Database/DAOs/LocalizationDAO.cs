namespace ScotlandYard.Scripts.Database.DAOs
{
    using Mono.Data.Sqlite;
    using ScotlandYard.Interfaces;
    using System.Collections.Generic;

    public class LocalizationDAO : IDataAccessObject<object[]>
    {
        protected static LocalizationDAO instance;

        public static LocalizationDAO getInstance()
        {
            if(instance == null)
            {
                instance = new LocalizationDAO();
            }

            return instance;
        }
        
        public int Delete(params object[] values)
        {
            IDbManager dbMgr = new SqliteDbManager();
            return dbMgr.Execute("DELETE FROM localization WHERE key = @key;", new SqliteParameter("@key", values[0]));
        }

        public int Insert(params object[] values)
        {
            IDbManager dbMgr = new SqliteDbManager();
            return dbMgr.Execute("INSERT INTO localization (key, text, language_id) VALUES (@key, @text, @lang_id);", new SqliteParameter("@key", values[0]),
                new SqliteParameter("@text", values[1]), new SqliteParameter("@lang_id", values[2]));
        }

        public List<object[]> Read(params object[] values)
        {
            IDbManager dbMgr = new SqliteDbManager();

            if(values.Length == 1)
            {
                return dbMgr.Read("SELECT key, text FROM localization WHERE language_id = @lang_id;", new SqliteParameter("@lang_id", values[0]));
            }
            else if (values.Length == 1)
            {
                return dbMgr.Read("SELECT text FROM localization WHERE key = @key AND language_id = @lang_id;", new SqliteParameter("@key", values[0]), new SqliteParameter("@lang_id", values[1]));
            }

            return new List<object[]>();
        }

        public List<object[]> ReadAll()
        {
            IDbManager dbMgr = new SqliteDbManager();
            return dbMgr.Read("SELECT * FROM localization;");
        }

        public int Update(params object[] values)
        {
            IDbManager dbMgr = new SqliteDbManager();
            return dbMgr.Execute("UPDATE localization SET text = @text WHERE key = @key AND language_id = @lang_id;", new SqliteParameter("@text", values[0]), new SqliteParameter("@key", values[1]), new SqliteParameter("@lang_id", values[2]));
        }
    }
}
