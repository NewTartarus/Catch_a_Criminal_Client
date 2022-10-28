namespace ScotlandYard.Scripts.SaveSystem
{
    using ScotlandYard.Enums;
    using ScotlandYard.ScriptableObjects;
    using ScotlandYard.Scripts.Database.DAOs;
    using ScotlandYard.Scripts.Database.Data;
    using ScotlandYard.Scripts.Events;
    using ScotlandYard.Scripts.Localisation;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using UnityEngine;
	
	public class SettingsLoader : Loader
	{
		#region Constants
		private const string OPTIONS_PLAYERNAME = "options_playername";
		private const string OPTIONS_LANGUAGE   = "options_language";
		private const string OPTIONS_RESOLUTION = "options_resolution";
		private const string OPTIONS_FULLSCREEN = "options_fullscreen";
		#endregion

		#region Members
		[SerializeField] GameSettingsSO settingsSO;
		#endregion
				
		#region Methods
		public override void Load(EUserId userId)
        {
			bool isDefault = userId == EUserId.DEFAULT;
			List<GameSettingsData> settingsDataList = SettingsDAO.getInstance().Read((int)userId);

			if (settingsDataList == null || settingsDataList.Count == 0)
            {
				settingsSO.IsInitialized = false;
				return;
			}
			else
            {
				settingsSO.IsInitialized = true;
            }

			LoadGeneralSettings(settingsDataList, isDefault);
			LoadControlsSettings(settingsDataList, isDefault);
			LoadAudioSettings(settingsDataList, isDefault);
			LoadVideoSettings(settingsDataList, isDefault);
		}

		protected virtual void LoadGeneralSettings(List<GameSettingsData> settingsDataList, bool isDefault)
        {
			GameSettingsData data = settingsDataList.FirstOrDefault(s => OPTIONS_PLAYERNAME.Equals(s.LocalizedName));
			settingsSO.PlayerName = DeserializeValue(data?.Value, data?.Type) as string;

			if (isDefault)
            {
				settingsSO.LanguageId = LoadDefaultLanguage();
			}
            else
            {
				data = settingsDataList.FirstOrDefault(s => OPTIONS_LANGUAGE.Equals(s.LocalizedName));
				settingsSO.LanguageId = (int)DeserializeValue(data?.Value, data?.Type);
				LocalisationSystem.Lang = new Language(settingsSO.LanguageId, LocalisationSystem.GetLanguages()[settingsSO.LanguageId]);
				GameEvents.Current.LanguageChanged(this, null);
			}
		}

		protected virtual void LoadControlsSettings(List<GameSettingsData> settingsDataList, bool isDefault)
        {

        }

		protected virtual void LoadAudioSettings(List<GameSettingsData> settingsDataList, bool isDefault)
		{

		}

		protected virtual void LoadVideoSettings(List<GameSettingsData> settingsDataList, bool isDefault)
        {
			if (isDefault)
            {
				settingsSO.Resolution = LoadDefaultResolution();
				settingsSO.IsFullscreen = true;
			}
            else
            {
				GameSettingsData data = settingsDataList.FirstOrDefault(s => OPTIONS_RESOLUTION.Equals(s.LocalizedName));
				settingsSO.Resolution = (Resolution)DeserializeValue(data?.Value, data?.Type);

				data = settingsDataList.FirstOrDefault(s => OPTIONS_FULLSCREEN.Equals(s.LocalizedName));
				settingsSO.IsFullscreen = (bool)DeserializeValue(data?.Value, data?.Type);
			}

			Screen.SetResolution(settingsSO.Resolution.width, settingsSO.Resolution.height, settingsSO.IsFullscreen);
		}

		private int LoadDefaultLanguage()
        {
			string cultureName = CultureInfo.CurrentCulture.NativeName.Split(' ')[0];

			List<Language> languages = LanguageDAO.getInstance().ReadAll();
			Language defaultLang;

			try
			{
				defaultLang = languages.Find(l => l.Name.Equals(cultureName));
			}
			catch (Exception ex)
			{
				Debug.Log($"The language {cultureName} could not be found.\n{ex.Message}");
				defaultLang = languages.Find(l => l.Name.Equals("English"));
			}

			LocalisationSystem.Lang = defaultLang;
			GameEvents.Current.LanguageChanged(this, null);

			return defaultLang.ID;
		}

		private Resolution LoadDefaultResolution()
		{
			return Screen.currentResolution;
		}
		#endregion
	}
}