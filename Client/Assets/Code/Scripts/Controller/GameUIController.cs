namespace ScotlandYard.Scripts.Controller
{
    using ScotlandYard.Scripts.Events;
    using ScotlandYard.Scripts.History;
    using ScotlandYard.Scripts.PlayerScripts;
    using ScotlandYard.Scripts.UI.InGame;
    using System;
	using System.Collections.Generic;
    using TMPro;
    using UnityEngine;
	
	public class GameUIController : MonoBehaviour
	{
		#region Members
		[SerializeField] protected TextMeshProUGUI roundText;
		[SerializeField] protected HistoryItemList historyList;
		[SerializeField] protected PlayerInfoList  playerInfoList;
		[SerializeField] protected TicketChooser ticketChooser;
		[SerializeField] protected RoundMessage roundMessage;
		[SerializeField] protected GameOverOverlay gameOver;
		#endregion

		#region Properties
		#endregion

		#region Methods
		protected void Awake()
		{
            UIEvents.Current.OnRoundAdded += Current_OnRoundAdded;
            UIEvents.Current.OnHistoryItemAdded += Current_OnHistoryItemAdded;
            UIEvents.Current.OnPlayersInitialized += Current_OnPlayersInitialized;

            UIEvents.Current.OnRoundMessageShown += Current_OnRoundMessageShown;
            UIEvents.Current.OnRoundMessageHidden += Current_OnRoundMessageHidden;

			GameEvents.Current.OnMisterXWon += Current_OnMisterXWon;
			GameEvents.Current.OnDetectivesWon += Current_OnDetectivesWon;

			ticketChooser.Init();
		}

        private void Current_OnRoundAdded(object sender, string e)
        {
			roundText.SetText(e);
		}

		private void Current_OnHistoryItemAdded(object sender, HistoryItem e)
		{
			historyList.AddVisibleItem(e);
		}

		private void Current_OnPlayersInitialized(object sender, List<Agent> e)
		{
			playerInfoList.Init(e);
		}

		private void Current_OnRoundMessageShown(object sender, string e)
		{
			roundMessage.DisplayMessage("game_turn_started", e);
		}

		private void Current_OnRoundMessageHidden(object sender, string e)
		{
			roundMessage.HideMessage();
		}

		private void Current_OnMisterXWon(object sender, EventArgs e)
		{
			gameOver.Display("game_misterX_won");
		}

		private void Current_OnDetectivesWon(object sender, EventArgs e)
		{
			gameOver.Display("game_detectives_won");
		}

		protected void OnDestroy()
		{
			UIEvents.Current.OnRoundAdded -= Current_OnRoundAdded;
			UIEvents.Current.OnHistoryItemAdded -= Current_OnHistoryItemAdded;
			UIEvents.Current.OnPlayersInitialized -= Current_OnPlayersInitialized;

			UIEvents.Current.OnRoundMessageShown -= Current_OnRoundMessageShown;
			UIEvents.Current.OnRoundMessageHidden -= Current_OnRoundMessageHidden;

			GameEvents.Current.OnMisterXWon -= Current_OnMisterXWon;
			GameEvents.Current.OnDetectivesWon -= Current_OnDetectivesWon;

			ticketChooser.Destroy();
		}
		#endregion
	}
}