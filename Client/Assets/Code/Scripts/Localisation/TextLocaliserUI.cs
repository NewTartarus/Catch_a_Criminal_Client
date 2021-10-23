namespace ScotlandYard.Scripts.Localisation
{
    using ScotlandYard.Scripts.Events;
    using TMPro;
    using UnityEngine;

    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TextLocaliserUI : MonoBehaviour
    {
        TextMeshProUGUI textField;

        public LocalizedString localizedString;

        protected void Awake()
        {
            UpdateText();

            GameEvents.Current.OnLanguageChanged += Current_OnLanguageChanged;
        }

        public void UpdateText()
        {
            textField = GetComponent<TextMeshProUGUI>();
            textField.text = localizedString.Value;
        }

        protected void Current_OnLanguageChanged(object sender, System.EventArgs e)
        {
            UpdateText();
        }

        protected void OnDestroy()
        {
            GameEvents.Current.OnLanguageChanged -= Current_OnLanguageChanged;
        }
    }
}
