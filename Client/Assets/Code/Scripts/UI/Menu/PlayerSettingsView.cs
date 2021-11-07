namespace ScotlandYard.Scripts
{
    using ScotlandYard.Enums;
    using ScotlandYard.Scripts.GameSettings;
    using ScotlandYard.Scripts.Localisation;
    using ScotlandYard.Scripts.UI.Color;
    using System;
	using System.Collections;
	using System.Collections.Generic;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class PlayerSettingsView : MonoBehaviour
	{
		#region Members
		[SerializeField] protected Image profile;
		[SerializeField] protected TMP_InputField playerName;
		[SerializeField] protected TMP_Text playerNamePlaceholder;
		[SerializeField] protected Image colorButton;
		[SerializeField] protected TMP_Dropdown types;
		[SerializeField] protected TMP_Dropdown roles;
		[SerializeField] protected SettingsSO settings;

		protected ColorPicker colorPicker;
        #endregion

        #region Methods
        protected void OnEnable()
        {
			types.ClearOptions();
			roles.ClearOptions();

			types.AddOptions(new List<string>() { LocalisationSystem.GetLocalisedValue("player_type_player"), LocalisationSystem.GetLocalisedValue("player_type_ai") });
			roles.AddOptions(new List<string>() { LocalisationSystem.GetLocalisedValue("player_role_misterX"), LocalisationSystem.GetLocalisedValue("player_role_detective") });
        }

        public void ColorButtonPressed()
        {
			if(colorPicker != null)
            {
				List<Color> availableColors = settings.GetAvailableColors();
				availableColors.Insert(0, colorButton.color);
				colorPicker.Init("Color", availableColors, false, colorButton.color, colorButton.transform.position, c => SetColor(c));
			}
        }

		public void SetColor(Color color)
        {
			colorButton.color = settings.UseColor(color, colorButton.color);
        }

		public Color GetColor()
		{
			return colorButton.color;
		}

		public void SetColorPicker(ColorPicker picker)
		{
			colorPicker = picker;
		}

		public void SetType(int type)
		{
			types.value = type;
		}

		public PlayerSetting SaveSetting()
        {
			string tempName = string.IsNullOrEmpty(playerName.text) ? playerNamePlaceholder.text : playerName.text;
			return new PlayerSetting(colorButton.color, tempName, (EPlayerType)types.value, (EPlayerRole)roles.value);
        }
		#endregion
	}
}