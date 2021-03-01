using System.Collections.Generic;
using Mono.Data.Sqlite;
using ScotlandYard.Interface;
using ScotlandYard.Scripts.Localisation;

namespace ScotlandYard.Scripts.Database.DAOs
{
    public class LanguageDAO : IDataAccessObject<Language>
    {
        protected static LanguageDAO instance = null;
        public static LanguageDAO getInstance()
        {
            if(instance == null)
            {
                instance = new LanguageDAO();
            }

            return instance;
        }

        public int Delete(Language value)
        {
            IDbManager dbMgr = new SqliteDbManager();
            return dbMgr.Execute("DELETE FROM Languages WHERE language_id = @id AND name = @name;", new SqliteParameter("@id", value.ID), new SqliteParameter("@name", value.Name));
        }

        public int Insert(Language value)
        {
            IDbManager dbMgr = new SqliteDbManager();
            return dbMgr.Execute("INSERT INTO Languages (language_id, name) VALUES (@id, @name);", new SqliteParameter("@id", value.ID), new SqliteParameter("@name", value.Name));
        }

        public List<Language> Read(params object[] values)
        {
            throw new System.NotImplementedException();
        }

        public List<Language> ReadAll()
        {
            List<Language> languages = new List<Language>();
            
            IDbManager dbMgr = new SqliteDbManager();
            List<object[]> values = dbMgr.Read("SELECT * FROM Languages;");

            foreach(object[] row in values)
            {
                languages.Add(new Language(int.Parse(row[0].ToString()), row[1].ToString()));
            }

            return languages;
        }

        public int Update(Language value)
        {
            IDbManager dbMgr = new SqliteDbManager();
            return dbMgr.Execute("UPDATE Languages SET name = @name WHERE language_id = @id;", new SqliteParameter("@id", value.ID), new SqliteParameter("@name", value.Name));
        }
    }
}
