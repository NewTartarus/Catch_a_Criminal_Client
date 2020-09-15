using ScotlandYard.Scripts.Localisation;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ScotlandYard.Scripts.UI
{
    public class RoundMessage : MonoBehaviour
    {
        [SerializeField] protected TextMeshProUGUI roundMessageText;

        public void DisplayMessage(string key, string replaceString = null)
        {
            string text = LocalisationSystem.GetLocalisedValue(key);

            if(text.Contains("[X]") && !string.IsNullOrEmpty(replaceString))
            {
                text = text.Replace("[X]", replaceString);
            }

            roundMessageText.text = text;
            this.gameObject.SetActive(true);
        }

        public void HideMessage()
        {
            roundMessageText.text = string.Empty;
            this.gameObject.SetActive(false);
        }
    }
}