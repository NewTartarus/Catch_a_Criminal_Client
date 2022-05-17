namespace ScotlandYard.Scripts
{
    using ScotlandYard.Scripts.History;
    using ScotlandYard.Scripts.PlayerScripts;
    using System;
	using System.Collections.Generic;
	
	public class UIEvents
	{
		#region Events
		public event EventHandler<HistoryItem> OnHistoryItemAdded;
		public event EventHandler<string> OnRoundAdded;
		public event EventHandler<List<Agent>> OnPlayersInitialized;
		public event EventHandler<string> OnRoundMessageShown;
		public event EventHandler<string> OnRoundMessageHidden;
		#endregion

		#region Singleton
		private static UIEvents current;

		public static UIEvents Current
		{
			get
			{
				if (current == null)
				{
					current = new UIEvents();
				}

				return current;
			}
		}

		public static void Reset()
		{
			current = null;
		}
		#endregion

		#region Methods
		public void HistoryItemAdded(object sender, HistoryItem args)
		{
			OnHistoryItemAdded?.Invoke(sender, args);
		}

		public void RoundAdded(object sender, string args)
		{
			OnRoundAdded?.Invoke(sender, args);
		}

		public void PlayersInitialized(object sender, List<Agent> args)
		{
			OnPlayersInitialized?.Invoke(sender, args);
		}

		public void ShowRoundMessage(object sender, string args)
        {
			OnRoundMessageShown?.Invoke(sender, args);
		}

		public void HideRoundMessage(object sender, string args)
		{
			OnRoundMessageHidden?.Invoke(sender, args);
		}
		#endregion
	}
}