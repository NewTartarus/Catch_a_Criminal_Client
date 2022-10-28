namespace ScotlandYard.Scripts.UI.Menu
{
    using ScotlandYard.Enums;
    using ScotlandYard.ScriptableObjects;
    using ScotlandYard.Scripts.Events;
    using ScotlandYard.Scripts.Localisation;
    using ScotlandYard.Scripts.SaveSystem;
    using ScotlandYard.Scripts.UI.Basics;
	using System.Collections.Generic;
    using System.Linq;
    using TMPro;
	using UnityEngine;
    using UnityEngine.UI;

    public class GameSettingsView : MonoBehaviour
	{
		#region Members


		[Header("General Settings Input")]
		[SerializeField] private TMP_InputField playernameInput;
		[SerializeField] private TMP_Dropdown languageDropdown;

		[Header("Video Settings Input")]
		[SerializeField] private Toggle fullscreenToggle;
		[SerializeField] private TMP_Dropdown resolutionDropdown;

        [Header("Save-System")]
		[SerializeField] private GameSettingsSO settingsSO;
		[SerializeField] private SaveSystemManager saveSystem;

		[Header("Cancel")]
		[SerializeField] private ModalWindow modalWindow;

		protected List<string> languages;
		protected Resolution[] resolutions;
		#endregion

		#region Properties
		#endregion

		#region Methods
        private void OnEnable()
        {
			LoadSettings();
		}

        private void OnDisable()
        {
			modalWindow.ShowAsHorizontal("dialog_confirm", null, "settings_quit_dialog", "save", () => SaveAction(), "back", () => CancelAction());
		}

        private void LoadSettings()
        {
			playernameInput.text = settingsSO.PlayerName;
			InitLanguagesDropDown();

			fullscreenToggle.isOn = settingsSO.IsFullscreen;
			InitResolutionDropdown();
		}

		private void InitResolutionDropdown()
		{
			resolutions = Screen.resolutions.Select(resolution => new Resolution { width = resolution.width, height = resolution.height }).Distinct().ToArray();

			resolutionDropdown.ClearOptions();
			resolutionDropdown.AddOptions(this.resolutions.Select(res => $"{res.width} x {res.height}").ToList());

			Resolution currentReso = settingsSO.Resolution;
			resolutionDropdown.value = this.resolutions
										   .Select((r, i) => new { reso = r, index = i })
										   .First(a => a.reso.height == currentReso.height && a.reso.width == currentReso.width).index;
			resolutionDropdown.RefreshShownValue();
		}

		private void InitLanguagesDropDown()
		{
			languages = LocalisationSystem.GetLanguages();

			languageDropdown.ClearOptions();
			languageDropdown.AddOptions(languages);

			languageDropdown.value = settingsSO.LanguageId;
			languageDropdown.RefreshShownValue();
		}

		public void SetFullscreen(bool isFullscreen)
		{
			Screen.fullScreen = isFullscreen;
		}

		public void SetResolution(Resolution res)
		{
			Screen.SetResolution(res.width, res.height, Screen.fullScreen);
		}

		public void SetLanguage(int languageIndex)
		{
			LocalisationSystem.Lang = new Language(languageIndex, languages[languageIndex]);
			GameEvents.Current.LanguageChanged(this, null);
		}

		public void SaveSettings()
        {
			settingsSO.PlayerName = playernameInput.text;
			settingsSO.LanguageId = languageDropdown.value;

			settingsSO.Resolution = resolutions[resolutionDropdown.value];
			settingsSO.IsFullscreen = fullscreenToggle.isOn;

			saveSystem.Save();
        }

		public void ShowSaveDialog()
		{
			modalWindow.ShowAsHorizontal("dialog_confirm", null, "settings_save_dialog", "save", () => SaveAction(), "cancel", () => CancelAction());
		}

		public void ShowBackDialog()
		{
			modalWindow.ShowAsHorizontal("dialog_confirm", null, "settings_quit_dialog", "save", () => SaveAction(), "back", () => BackAction());
		}

		private void SaveAction()
		{
			SaveSettings();

			SetLanguage(settingsSO.LanguageId);

			SetFullscreen(settingsSO.IsFullscreen);
			SetResolution(settingsSO.Resolution);
		}

		private void CancelAction()
		{
			return;
		}

		private void BackAction()
		{
			GameEvents.Current.MainMenuButtonPressed(this, EButtons.MAIN_MENU);
		}
		#endregion
	}
}