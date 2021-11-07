namespace ScotlandYard.Scripts.UI.Color
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using TMPro;
	using UnityEngine;
	using UnityEngine.UI;

	public class ColorPicker : MonoBehaviour
	{
		#region Members
		[SerializeField] GameObject canvas;
		[SerializeField] Transform windowTransform;

		[SerializeField] GameObject titleGO;
		[SerializeField] TMP_Text titleText;
		[SerializeField] VerticalLayoutGroup layoutGroup;

		[SerializeField] GameObject colorSwatchGO;
		[SerializeField] LayoutElement colorSwatchLayout;
		[SerializeField] Transform colorItemParent;
		[SerializeField] ColorItem colorItemPrefab;

		[SerializeField] Image selectedColorImage;
		[SerializeField] TMP_Text colorCodeText;

		Action<Color> saveAction;
        #endregion

        #region Methods

        public void Init(string title, List<Color> colors, bool showColorWheel, Color defaultColor, Vector3 position, Action<Color> saveAction)
		{
			canvas.SetActive(true);

			if (!string.IsNullOrEmpty(title))
			{
				titleGO.SetActive(true);
				layoutGroup.padding = new RectOffset(0, 0, 0, 0);
				titleText.SetText(title);
			}
			else
            {
				titleGO.SetActive(false);
				layoutGroup.padding = new RectOffset(0, 0, 10, 0);
			}

			if (colors != null && colors.Count > 0)
			{
				colorSwatchGO.SetActive(true);
				foreach (Color color in colors)
				{
					ColorItem item = Instantiate(colorItemPrefab);
					item.Init(color, c => SelectColor(c));
					item.transform.SetParent(colorItemParent);
				}

				if(showColorWheel)
                {
					colorSwatchLayout.preferredHeight = 150;
				}
				else
                {
					colorSwatchLayout.preferredHeight = 200;
				}
			}
            else
            {
				colorSwatchGO.SetActive(false);
			}

			SelectColor(defaultColor);

			windowTransform.position = position;
			this.saveAction = saveAction;
		}

		protected void SelectColor(Color color)
        {
			selectedColorImage.color = color;
			colorCodeText.SetText($"#{ColorUtility.ToHtmlStringRGB(color)}");
		}

		public void Save()
        {
			if(saveAction != null)
            {
				saveAction(selectedColorImage.color);
			}

			Cancel();
		}

		public void Cancel()
        {
			foreach(Transform child in colorItemParent)
            {
				GameObject.Destroy(child.gameObject);
            }
			
			canvas.SetActive(false);
		}
		#endregion
	}
}