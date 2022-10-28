namespace ScotlandYard.Scripts.Database.Data
{
    public class GameSettingsData
    {
        #region Member
        private int    id;
        private string localizedName;
        private int    userId;
        private int    parent;
        private string value;
        private string type;
        #endregion

        #region Properties
        public int Id
        {
            get => id;
            set => id = value;
        }

        public string LocalizedName
        {
            get => this.localizedName;
            set => this.localizedName = value;
        }

        public int UserId
        {
            get => this.userId;
            set => this.userId = value;
        }

        public int Parent
        {
            get => this.parent;
            set => this.parent = value;
        }

        public string Value
        {
            get => this.value;
            set => this.value = value;
        }

        public string Type
        {
            get => this.type;
            set => this.type = value;
        }
        #endregion

        public GameSettingsData(int id, string localizedName, int userId, int parent, string value, string type)
        {
            Id            = id;
            LocalizedName = localizedName;
            UserId        = userId;
            Parent        = parent;
            Value         = value;
            Type          = type;
        }
    }
}
