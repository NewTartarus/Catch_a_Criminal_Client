namespace ScotlandYard.Scripts.Localisation
{
    [System.Serializable]
    public class LocalizedString
    {
        public string key;
        public LocalizedString(string key)
        {
            this.key = key;
        }

        public string value
        {
            get
            {
                return LocalisationSystem.GetLocalisedValue(key);
            }
        }

        public static implicit operator LocalizedString(string key)
        {
            return new LocalizedString(key);
        }
    }
}