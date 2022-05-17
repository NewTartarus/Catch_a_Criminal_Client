namespace ScotlandYard.Scripts.Events
{
    using ScotlandYard.Interfaces;
    using ScotlandYard.Scripts.Transfer;
    using System;
	using System.Collections;
	using System.Collections.Generic;
	
	public class MultiplayerEvents
	{
		#region Events
		public event EventHandler<Message> OnMultiplayerMessage;
		public event EventHandler<string> OnSendingMessage;
		public event EventHandler<string> OnMultiplayerError;

		//Multiplayer Login
		public event EventHandler<bool> OnMultiplayerLoginStarted;
		public event EventHandler<int> OnMultiplayerLoginEnded;

		// Multiplayer Menu
		public event EventHandler<string[]> OnMenuConnect;
		public event EventHandler<string> OnMenuDisconnect;
		public event EventHandler<IServerSetting> OnMenuConnectSucceeded;
		public event EventHandler<IServerSetting> OnMenuServerRemoved;
		#endregion

		#region Singleton
		private static MultiplayerEvents current;

		public static MultiplayerEvents Current
		{
			get
			{
				if (current == null)
				{
					current = new MultiplayerEvents();
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
		public void MultiplayerMessage(object sender, Message msg)
		{
			OnMultiplayerMessage?.Invoke(sender, msg);
		}

		public void SendingMessage(object sender, string msg)
		{
			OnSendingMessage?.Invoke(sender, msg);
		}

		public void MultiplayerError(object sender, string args)
		{
			OnMultiplayerError?.Invoke(sender, args);
		}

		public void StartingMultiplayerLogin(object sender, bool args)
		{
			OnMultiplayerLoginStarted?.Invoke(sender, args);
		}

		public void EndingMultiplayerLogin(object sender, int args)
		{
			OnMultiplayerLoginEnded?.Invoke(sender, args);
		}

		public void MenuConnecting(object sender, string[] args)
		{
			OnMenuConnect?.Invoke(sender, args);
		}

		public void MenuConnectingSucceeded(object sender, IServerSetting args)
		{
			OnMenuConnectSucceeded?.Invoke(sender, args);
		}

		public void MenuServerRemoved(object sender, IServerSetting args)
		{
			OnMenuServerRemoved?.Invoke(sender, args);
		}

		public void MenuDisconnecting(object sender, string args)
		{
			OnMenuDisconnect?.Invoke(sender, args);
		}
		#endregion
	}
}