namespace ScotlandYard.Scripts.GameSettings
{
    using ScotlandYard.Enums;
    using System.Linq;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	
	[CreateAssetMenu(fileName = "New SettingsSO", menuName = "ScriptableObjects/SettingsSO")]
	public class SettingsSO : ScriptableObject
	{
		#region Members
		[SerializeField] protected int maxPlayer;
		[SerializeField] protected List<Color> colorList = new List<Color>();
		[SerializeField] protected List<Color> usedColors = new List<Color>();
		[SerializeField] protected float agentSpeed;
		[SerializeField] protected bool isMultiplayer;
		protected List<PlayerSetting> playerSettings = new List<PlayerSetting>();
		[SerializeField] protected EDifficulty difficulty;
		#endregion

		#region Properties
		public float AgentSpeed
		{
			get => agentSpeed;
			set => agentSpeed = value;
		}

		public bool IsMultiplayer
        {
			get => isMultiplayer;
			set => isMultiplayer = value;
        }

		public List<PlayerSetting> PlayerSettings
        {
			get => playerSettings;
			set => playerSettings = value;
        }

		public EDifficulty Difficulty
        {
			get => difficulty;
			set => difficulty = value;
        }
		#endregion
		
		#region Methods
		public Color UseColor(Color newColor, Color prevColor)
        {
			RemoveColor(prevColor);
			return AddColor(newColor);
		}

		public Color AddColor(Color color)
        {
			usedColors.Add(color);
			return color;
		}

		public void RemoveColor(Color color)
        {
			usedColors.Remove(color);
		}
		
		public List<Color> GetAvailableColors()
        {
			return colorList.Where(c => !usedColors.Contains(c)).ToList();
        }

		public void SetPlayerCount(int count)
        {
			if (count == PlayerSettings.Count) { return; }
			
			int difference;

			if (PlayerSettings.Count < count)
			{
				difference = PlayerSettings.Count - count;
				for (int i = 0; i < difference; i++)
				{
					AddPlayer();

				}
			}
			else if (PlayerSettings.Count > count)
			{
				difference = count - PlayerSettings.Count;
				PlayerSettings.RemoveRange(count - 1, difference);
			}
		}

		protected void AddPlayer()
        {
			Color playerColor = GetAvailableColors()[0];
			PlayerSettings.Add(new PlayerSetting(playerColor));
		}

		public void Reset()
        {
			SetPlayerCount(0);
			usedColors = new List<Color>();
        }
		#endregion
	}
}