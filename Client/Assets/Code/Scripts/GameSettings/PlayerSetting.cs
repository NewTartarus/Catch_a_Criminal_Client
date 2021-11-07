namespace ScotlandYard.Scripts.GameSettings
{
    using ScotlandYard.Enums;
    using System;
	using System.Collections;
	using System.Collections.Generic;
    using UnityEngine;

    public class PlayerSetting
	{
		#region Members
		protected string playerName;
		protected Color color;
		protected EPlayerType type;
		protected EPlayerRole role;
		#endregion
		
		#region Properties
		public string PlayerName { get => playerName; set => playerName = value; }

		public Color PlayerColor { get => color; set => color = value; }

		public EPlayerType Type { get => type; set => type = value; }

		public EPlayerRole Role { get => role; set => role = value; }
		#endregion

		public PlayerSetting(Color color, string name = "COM", EPlayerType type = EPlayerType.AI, EPlayerRole role = EPlayerRole.DETECTIVE)
		{
			PlayerName = name;
			PlayerColor = color;
			Type = type;
			Role = role;
		}
	}
}