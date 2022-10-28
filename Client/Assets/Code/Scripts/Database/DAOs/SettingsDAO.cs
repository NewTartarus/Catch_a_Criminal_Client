namespace ScotlandYard.Scripts.Database.DAOs
{
    using Mono.Data.Sqlite;
    using ScotlandYard.Interfaces;
    using ScotlandYard.Scripts.Database.Data;
    using System;
    using System.Collections.Generic;

    public class SettingsDAO : IDataAccessObject<GameSettingsData>
    {
        protected static SettingsDAO instance;

        public static SettingsDAO getInstance()
        {
            if (instance == null)
            {
                instance = new SettingsDAO();
            }

            return instance;
        }

        public int Delete(GameSettingsData value)
        {
            IDbManager dbMgr = new SqliteDbManager();
            return dbMgr.Execute("DELETE FROM settings WHERE id = @id;", new SqliteParameter("@id", value.Id));
        }

        public int Insert(GameSettingsData value)
        {
            IDbManager dbMgr = new SqliteDbManager();
            string stmnt = "INSERT INTO settings (localized_name, user_id, parent, value, type) VALUES (@localizedName, @userId, @parent, @value, @type);";

            return dbMgr.Execute(stmnt, new SqliteParameter("@localizedName", value.LocalizedName),
                new SqliteParameter("@userId", value.UserId), new SqliteParameter("@parent", value.Parent), new SqliteParameter("@value", value.Value), new SqliteParameter("@type", value.Type));
        }

        public List<GameSettingsData> Read(params object[] values)
        {
            List<GameSettingsData> settings = new List<GameSettingsData>();
            IDbManager dbMgr = new SqliteDbManager();

            List<object[]> data = dbMgr.Read("SELECT id, localized_name, user_id, parent, value, type FROM settings WHERE user_id = @userId;", new SqliteParameter("@userId", values[0]));
            data.ForEach(row => settings.Add(new GameSettingsData(ParseToInt(row[0]), row[1]?.ToString(), ParseToInt(row[2]), ParseToInt(row[3]), row[4]?.ToString(), row[5]?.ToString())));
            return settings;
        }

        public List<GameSettingsData> ReadAll()
        {
            List<GameSettingsData> settings = new List<GameSettingsData>();
            IDbManager dbMgr = new SqliteDbManager();

            List<object[]> data = dbMgr.Read("SELECT id, localized_name, user_id, parent, value, type FROM settings;");
            data.ForEach(row => settings.Add(new GameSettingsData(ParseToInt(row[0]), row[1]?.ToString(), ParseToInt(row[2]), ParseToInt(row[3]), row[4]?.ToString(), row[5]?.ToString())));
            return settings;
        }

        public GameSettingsData ReadSingle(string name, int userId)
        {
            IDbManager dbMgr = new SqliteDbManager();
            List<SqliteParameter> parameters = new List<SqliteParameter>() { new SqliteParameter("@localName", name), 
                                                                             new SqliteParameter("@userId", userId) };
            List<object[]> data = dbMgr.Read("SELECT id, localized_name, user_id, parent, value, type FROM settings WHERE localized_name = @localName AND user_id = @userId;", parameters.ToArray() );

            if (data != null && data.Count > 0)
            {
                object[] row = data[0];
                return new GameSettingsData(ParseToInt(row[0]), row[1]?.ToString(), ParseToInt(row[2]), ParseToInt(row[3]), row[4]?.ToString(), row[5]?.ToString());
            }

            return null;
        }

        public int Update(GameSettingsData value)
        {
            IDbManager dbMgr = new SqliteDbManager();
            return dbMgr.Execute("UPDATE settings SET parent = @parent, value = @value, type = @type WHERE user_id = @userId AND localized_name = @localizedName;", new SqliteParameter("@localizedName", value.LocalizedName),
                new SqliteParameter("@userId", value.UserId), new SqliteParameter("@parent", value.Parent), new SqliteParameter("@value", value.Value), new SqliteParameter("@type", value.Type), new SqliteParameter("@id", value.Id));
        }

        public int UpdateOrInsert(GameSettingsData value)
        {
            GameSettingsData temp = ReadSingle(value.LocalizedName, value.UserId);

            if (temp == null)
            {
                return Insert(value);
            }
            else
            {
                return Update(value);
            }
        }

        private int ParseToInt(object value)
        {
            string valueString = value?.ToString();

            if (!String.IsNullOrEmpty(valueString))
            {
                return Int32.Parse(valueString);
            }

            return 0;
        }
    }
}
