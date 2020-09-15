using TMPro;
using UnityEngine;

namespace ScotlandYard.Scripts.Localisation
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TextLocaliserUI : MonoBehaviour
    {
        TextMeshProUGUI textField;

        public LocalizedString localizedString;

        void Awake()
        {
            textField = GetComponent<TextMeshProUGUI>();
            textField.text = localizedString.value;
        }
    }
}
