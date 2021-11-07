namespace ScotlandYard.Scripts.UI.Color
{
	using System;
	using UnityEngine;
    using UnityEngine.UI;

    public class ColorItem : MonoBehaviour
	{
		#region Members
		[SerializeField] Image image;

		protected Color color;
		protected Action<Color> colorSelectedAction;
		#endregion
		
		#region Methods
		public void Init(Color color, Action<Color> colorSelectedAction)
        {
			this.color = color;
			this.image.color = color;
			this.colorSelectedAction = colorSelectedAction;
        }

		public void ColorSelected()
        {
			if(colorSelectedAction != null)
            {
				colorSelectedAction(this.color);
			}
        }
		#endregion
	}
}