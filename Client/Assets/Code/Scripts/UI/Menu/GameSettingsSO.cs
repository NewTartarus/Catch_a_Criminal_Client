namespace ScotlandYard.ScriptableObjects
{
	using UnityEngine;
	
	[CreateAssetMenu(fileName = "New GameSettingsSO", menuName = "ScriptableObjects/GameSettingsSO")]
	public class GameSettingsSO : ScriptableObject
	{
		#region Members
		private bool isInitialized;

		// general
		[SerializeField] private string playerName;
		[SerializeField] private int languageId;

		// video
		[SerializeField] private bool isFullscreen;
		[SerializeField] private Resolution resolution;
        #endregion

        #region Properties
		public bool IsInitialized
        {
			get => isInitialized;
			set => isInitialized = value;
        }

		public string PlayerName
        {
			get => playerName;
			set => playerName = value;
        }

		public int LanguageId
        {
			get => languageId;
			set => languageId = value;
        }

		public bool IsFullscreen
        {
			get => isFullscreen;
			set => isFullscreen = value;
        }

		public Resolution Resolution
        {
			get => resolution;
			set => resolution = value;
        }
		#endregion
    }
}