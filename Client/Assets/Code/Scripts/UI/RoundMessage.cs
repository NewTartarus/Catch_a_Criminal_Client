namespace ScotlandYard.Scripts.UI
{
    using ScotlandYard.Scripts.Localisation;
    using System;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class RoundMessage : MonoBehaviour
    {
        [SerializeField] protected TextMeshProUGUI roundMessageText;

        [SerializeField] protected Button button;
        [SerializeField] protected TextMeshProUGUI buttonText;

        protected Action buttonAction;

        public void DisplayMessage(string key, string replaceString = null)
        {
            string text = LocalisationSystem.GetLocalisedValue(key);

            if(text.Contains("[X]") && !string.IsNullOrEmpty(replaceString))
            {
                text = text.Replace("[X]", replaceString);
            }

            roundMessageText.text = text;
            HideButton();
            this.gameObject.SetActive(true);
        }

        public void DisplayMessage(string key, string buttonTextKey, Action buttonAction)
        {
            string text = LocalisationSystem.GetLocalisedValue(key);
            string buttonText = LocalisationSystem.GetLocalisedValue(buttonTextKey);

            roundMessageText.text = text;
            this.buttonText.text = buttonText;
            this.buttonAction = buttonAction;

            this.gameObject.SetActive(true);
            this.button.gameObject.SetActive(true);
        }

        public void HideMessage()
        {
            roundMessageText.text = string.Empty;
            this.gameObject.SetActive(false);
        }

        protected void HideButton()
        {
            buttonText.text = string.Empty;
            button.gameObject.SetActive(false);
            buttonAction = null;
        }

        public void DoAction()
        {
            if(buttonAction != null)
            {
                buttonAction();
            }
        }
    }
}