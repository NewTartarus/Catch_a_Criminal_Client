namespace ScotlandYard.Scripts.SaveSystem
{
    using ScotlandYard.Enums;
    using ScotlandYard.ScriptableObjects;
    using System;
    using TMPro;
	using UnityEngine;
	
	public class SaveSystemManager : MonoBehaviour
	{
        #region Members
        [Header("SaveSystem")]
		[SerializeField] private Loader loader;
		[SerializeField] private Saver saver;
		[SerializeField] private GameSettingsSO settingsSO;

		[Header("UI")]
		[SerializeField] private GameObject playernameInputWindow;
		[SerializeField] private TMP_InputField playernameInput;
		#endregion

		#region Methods
		private void Awake()
		{
			loader.Load(EUserId.USER1);

			if (!settingsSO.IsInitialized)
            {
				loader.Load(EUserId.DEFAULT);
				playernameInputWindow.SetActive(true);
			}
		}
		
		public void Save()
        {
			saver.Save(EUserId.USER1);
        }

		public void SavePlayerName()
        {
			if (String.IsNullOrEmpty(playernameInput.text)) { return; }

			settingsSO.PlayerName = playernameInput.text;
			saver.Save(EUserId.USER1);
			playernameInputWindow.SetActive(false);
		}
		#endregion
	}
}