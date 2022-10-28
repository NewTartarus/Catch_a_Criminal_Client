namespace ScotlandYard.Scripts.SaveSystem
{
    using ScotlandYard.Enums;
    using ScotlandYard.ScriptableObjects;
    using ScotlandYard.Scripts.Database.DAOs;
    using ScotlandYard.Scripts.Database.Data;
	using System.Collections.Generic;
    using UnityEngine;

    public class SettingsSaver : Saver
	{
		#region Members
		[SerializeField] private GameSettingsSO settingsSO;
		#endregion

		#region Methods
		public override void Save(EUserId userId)
        {
			List<GameSettingsData> dataList = new List<GameSettingsData>();

            dataList.AddRange(SaveGeneralSettings((int)userId));
            dataList.AddRange(SaveControlsSettings((int)userId));
            dataList.AddRange(SaveAudioSettings((int)userId));
            dataList.AddRange(SaveVideoSettings((int)userId));

            foreach(GameSettingsData data in dataList)
            {
                SettingsDAO.getInstance().UpdateOrInsert(data);
            }
        }

        protected virtual List<GameSettingsData> SaveGeneralSettings(int userId)
        {
            List<GameSettingsData> dataList = new List<GameSettingsData>();

            dataList.Add(new GameSettingsData(0, ESettingsNames.OPTIONS_PLAYERNAME, userId, (int)ESettingsParent.GENERAL, SerializeValue(settingsSO.PlayerName), settingsSO.PlayerName.GetType().Name));
            dataList.Add(new GameSettingsData(0, ESettingsNames.OPTIONS_LANGUAGE, userId, (int)ESettingsParent.GENERAL, SerializeValue(settingsSO.LanguageId), settingsSO.LanguageId.GetType().Name));

            return dataList;
        }

        protected virtual List<GameSettingsData> SaveControlsSettings(int userId)
        {
            List<GameSettingsData> dataList = new List<GameSettingsData>();

            return dataList;
        }

        protected virtual List<GameSettingsData> SaveAudioSettings(int userId)
        {
            List<GameSettingsData> dataList = new List<GameSettingsData>();

            return dataList;
        }

        protected virtual List<GameSettingsData> SaveVideoSettings(int userId)
        {
            List<GameSettingsData> dataList = new List<GameSettingsData>();

            dataList.Add(new GameSettingsData(0, ESettingsNames.OPTIONS_FULLSCREEN, userId, (int)ESettingsParent.VIDEO, SerializeValue(settingsSO.IsFullscreen), settingsSO.IsFullscreen.GetType().Name));
            dataList.Add(new GameSettingsData(0, ESettingsNames.OPTIONS_RESOLUTION, userId, (int)ESettingsParent.VIDEO, SerializeValue(settingsSO.Resolution), settingsSO.Resolution.GetType().Name));

            return dataList;
        }
        #endregion
    }
}