namespace ScotlandYard.Scripts.Database.DAOs
{
    using Mono.Data.Sqlite;
    using ScotlandYard.Interfaces;
    using ScotlandYard.Scripts.GameSettings;
    using System.Collections.Generic;

    public class SavedServerDAO : IDataAccessObject<ServerSetting>
    {
        protected static SavedServerDAO instance;

        public static SavedServerDAO getInstance()
        {
            if(instance == null)
            {
                instance = new SavedServerDAO();
            }

            return instance;
        }
        
        public int Delete(ServerSetting value)
        {
            IDbManager dbMgr = new SqliteDbManager();
            return dbMgr.Execute("DELETE FROM saved_servers WHERE id = @id;", new SqliteParameter("@id", value.Id));
        }

        public int Insert(ServerSetting value)
        {
            IDbManager dbMgr = new SqliteDbManager();
            string stmnt = "INSERT OR IGNORE INTO saved_servers (name, url, password) VALUES (@name, @url, @password);"
                           + " UPDATE saved_servers SET last_login = @last_login WHERE name = @name AND url = @url;";

            return dbMgr.Execute(stmnt, new SqliteParameter("@name", value.ServerName),
                new SqliteParameter("@url", value.ServerUrl), new SqliteParameter("@password", value.HashedPassword), new SqliteParameter("@last_login", value.LastLogin));
        }

        public List<ServerSetting> Read(params object[] values)
        {
            throw new System.NotImplementedException();
        }

        public List<ServerSetting> ReadAll()
        {
            List<ServerSetting> settings = new List<ServerSetting>();
            IDbManager dbMgr = new SqliteDbManager();

            List<object[]> values = dbMgr.Read("SELECT id, name, url, password, last_login FROM saved_servers;");
            values.ForEach(row => settings.Add(new ServerSetting(int.Parse(row[0].ToString()), row[1].ToString(), row[2].ToString(), row[3].ToString(), row[4].ToString())));
            return settings;
        }

        public int Update(ServerSetting value)
        {
            IDbManager dbMgr = new SqliteDbManager();
            return dbMgr.Execute("UPDATE saved_servers SET password =  @password, last_login = @last_login WHERE id = @id;", new SqliteParameter("@password", value.HashedPassword), new SqliteParameter("@last_login", value.LastLogin), new SqliteParameter("@id", value.Id));
        }
    }
}
