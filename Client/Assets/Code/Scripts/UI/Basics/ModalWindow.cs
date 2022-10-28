namespace ScotlandYard.Scripts.UI.Basics
{
    using ScotlandYard.Scripts.Localisation;
    using System;
	using UnityEngine;
    using UnityEngine.UI;

    public class ModalWindow : MonoBehaviour
	{
		#region Members
		[SerializeField] private GameObject modalPanelObject;

		[Header("Header")]
		[SerializeField] private GameObject headerObject;
		[SerializeField] private TextLocaliserUI headerText;

        [Header("Vertical Layout")]
		[SerializeField] private GameObject verticalLayoutObject;
		[SerializeField] private Image verticalImage;
		[SerializeField] private TextLocaliserUI verticalLayoutText;

        [Header("Horizontal Layout")]
		[SerializeField] private GameObject horizontalLayoutObject;
		[SerializeField] private Image horizontalImage;
		[SerializeField] private TextLocaliserUI horizontalLayoutText;

        [Header("Footer")]
		[SerializeField] private GameObject okButton;
		[SerializeField] private TextLocaliserUI okButtonText;
		[SerializeField] private GameObject cancelButton;
		[SerializeField] private TextLocaliserUI cancelButtonText;
		[SerializeField] private GameObject alternateButton;
		[SerializeField] private TextLocaliserUI alternateButtonText;

		private Action onOkAction;
		private Action onCancelAction;
		private Action onAlternateAction;
		#endregion

		#region Properties
		#endregion

		#region Methods
		public void ShowAsHorizontal(string header, Sprite image, string content, string okText, Action onOkAction, string cancelText = null, Action onCancelAction = null, string alternateText = null, Action onAlternateAction = null)
        {
			if (String.IsNullOrEmpty(header))
            {
				headerObject.SetActive(false);
            }
            else
            {
				headerObject.SetActive(true);
				headerText.localizedString = header;
				headerText.UpdateText();
            }

			if (image == null)
            {
				horizontalImage.gameObject.SetActive(false);
            }
			else
            {
				horizontalImage.gameObject.SetActive(true);
				horizontalImage.sprite = image;
            }

			horizontalLayoutText.localizedString = content;
			horizontalLayoutText.UpdateText();

			if (onOkAction == null && String.IsNullOrEmpty(okText))
            {
				okButton.gameObject.SetActive(false);
            }
			else
            {
				okButton.gameObject.SetActive(true);
				this.okButtonText.localizedString = okText;
				this.okButtonText.UpdateText();
				this.onOkAction = onOkAction;
            }

			if (onCancelAction == null && String.IsNullOrEmpty(cancelText))
			{
				cancelButton.gameObject.SetActive(false);
			}
			else
			{
				cancelButton.gameObject.SetActive(true);
				this.cancelButtonText.localizedString = cancelText;
				this.cancelButtonText.UpdateText();
				this.onCancelAction = onCancelAction;
			}

			if (onAlternateAction == null && String.IsNullOrEmpty(alternateText))
			{
				alternateButton.gameObject.SetActive(false);
			}
			else
			{
				alternateButton.gameObject.SetActive(true);
				this.alternateButtonText.localizedString = alternateText;
				this.alternateButtonText.UpdateText();
				this.onAlternateAction = onAlternateAction;
			}

			horizontalLayoutObject.SetActive(true);
			verticalLayoutObject.SetActive(false);
			modalPanelObject.SetActive(true);
		}

		public void Ok()
        {
			onOkAction?.Invoke();
			Close();
        }

		public void Cancel()
        {
			onCancelAction?.Invoke();
			Close();
        }

		public void Alternate()
        {
			onAlternateAction?.Invoke();
			Close();
        }

		protected virtual void Close()
        {
			modalPanelObject.SetActive(false);
        }
		#endregion
	}
}