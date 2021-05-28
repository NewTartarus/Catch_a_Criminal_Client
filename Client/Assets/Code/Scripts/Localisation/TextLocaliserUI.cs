using ScotlandYard.Events;
using TMPro;
using UnityEngine;

namespace ScotlandYard.Scripts.Localisation
{
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

        protected void UpdateText()
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
