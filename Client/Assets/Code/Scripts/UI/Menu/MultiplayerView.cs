namespace ScotlandYard.Scripts.UI.Menu
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
    using ScotlandYard.Interfaces;
    using ScotlandYard.Scripts.Database.DAOs;
    using ScotlandYard.Scripts.Events;
	using ScotlandYard.Scripts.GameSettings;
    using ScotlandYard.Scripts.Helper;
    using ScotlandYard.Scripts.Transfer;
    using ScotlandYard.Scripts.UI.InGame;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class MultiplayerView : MonoBehaviour
	{
		#region Members
		[SerializeField] protected GameObject serverPage;
		[SerializeField] protected GameObject serverPrefab;
		[SerializeField] protected Transform serverList;
		[SerializeField] protected GameObject loginPage;
		[SerializeField] protected TMP_InputField inputAddress;
		[SerializeField] protected TMP_InputField inputPwd;
		[SerializeField] protected Button buttonLogin;
		[SerializeField] protected GameObject lobbyPage;
		[SerializeField] protected GameObject errorMessage;
		[SerializeField] protected TMP_Text errorText;
		[SerializeField] protected TMP_InputField inputMessage;

		[SerializeField] protected GameObject multiplayerMsgPrefab;
		[SerializeField] protected Transform messageList;
        [SerializeField] protected Sprite[] images = new Sprite[3];

		private List<ServerItemView> serverItemViews = new List<ServerItemView>();
        #endregion

		#region Methods
		protected void Awake() 
		{
			List<ServerSetting> savedServers = SavedServerDAO.getInstance().ReadAll();
			foreach(ServerSetting setting in savedServers)
			{
				AddSavedServer(setting);
			}

			MultiplayerEvents.Current.OnMenuConnectSucceeded += Current_OnMenuConnectSucceeded;
			MultiplayerEvents.Current.OnMultiplayerError += Current_OnMenuError;
			MultiplayerEvents.Current.OnMultiplayerMessage += Current_OnMultiplayerMessage;
			MultiplayerEvents.Current.OnMultiplayerLoginStarted += Current_OnMultiplayerLoginStarted;
			MultiplayerEvents.Current.OnMultiplayerLoginEnded += Current_OnMultiplayerLoginEnded;
		}

		protected void AddSavedServer(IServerSetting setting)
		{
			ServerItemView containedSIV = serverItemViews.FirstOrDefault(sv => sv.Setting.ServerUrl == setting.ServerUrl);

			if (containedSIV == null)
			{
				GameObject child = Instantiate(serverPrefab);
				ServerItemView siv = child.GetComponent<ServerItemView>();

				if (siv != null)
				{
					siv.Init(setting);
					serverItemViews.Add(siv);
					child.transform.SetParent(serverList, false);
				}
			}
			else
            {
				containedSIV.UpdateLastLogin(setting.LastLogin);
            }
		}

		public void ShowLoginPage()
		{
			lobbyPage.SetActive(false);
			loginPage.SetActive(true);
			serverPage.SetActive(false);
		}

		public void ExitLoginPage()
		{
			lobbyPage.SetActive(false);
			loginPage.SetActive(false);
			serverPage.SetActive(true);
		}

		public void Login()
		{
			if(!string.IsNullOrEmpty(inputAddress.text))
			{
				string address = inputAddress.text;
				string password = HashHelper.HashString(inputPwd.text);
				MultiplayerEvents.Current.MenuConnecting(this, new string[] { address, password, "30000" });
			}
		}

		public void Disconnect() 
		{
			MultiplayerEvents.Current.MenuDisconnecting(this, string.Empty);

			lobbyPage.SetActive(false);
			loginPage.SetActive(false);
			serverPage.SetActive(true);
		}

		public void SendMessage()
		{
			if(!string.IsNullOrEmpty(inputMessage.text))
			{
				string msg = inputMessage.text;
				MultiplayerEvents.Current.SendingMessage(this, msg);
			}
		}

		public void CloseError()
		{
			errorText.text = String.Empty;
			errorMessage.SetActive(false);
		}

		protected void Current_OnMenuConnectSucceeded(object sender, IServerSetting args)
		{
			loginPage.SetActive(false);
			serverPage.SetActive(false);
			lobbyPage.SetActive(true);

			AddMessage(new Message("Server", "", $" Welcome to {args.ServerName}"));
			AddSavedServer(args);
		}

		protected void Current_OnMenuError(object sender, string args)
		{
			errorText.text = args;
			errorMessage.SetActive(true);
		}

		protected void Current_OnMultiplayerMessage(object sender, Message msg)
		{
			AddMessage(msg);
			inputMessage.text = string.Empty;

			inputMessage.Select();
			inputMessage.ActivateInputField();
		}

		protected void Current_OnMultiplayerLoginStarted(object sender, bool args)
		{
			buttonLogin.interactable = false;
		}

		protected void Current_OnMultiplayerLoginEnded(object sender, int args)
		{
			buttonLogin.interactable = true;
		}

		protected void AddMessage(Message msg)
		{
			Sprite img;
			switch(msg.userName)
			{
				case "Admin":
					img = images[0];
					break;
				case "Server":
					img = images[1];
					break;
				default:
					img = images[2];
					break;
			}

			GameObject child = Instantiate(multiplayerMsgPrefab);
            child.GetComponent<MultiplayerMessage>()?.Init(msg, img);
            child.transform.SetParent(messageList, false);
		}

		protected void OnDestroy()
		{
			MultiplayerEvents.Current.OnMenuConnectSucceeded -= Current_OnMenuConnectSucceeded;
			MultiplayerEvents.Current.OnMultiplayerError -= Current_OnMenuError;
			MultiplayerEvents.Current.OnMultiplayerMessage -= Current_OnMultiplayerMessage;
			MultiplayerEvents.Current.OnMultiplayerLoginStarted -= Current_OnMultiplayerLoginStarted;
			MultiplayerEvents.Current.OnMultiplayerLoginEnded -= Current_OnMultiplayerLoginEnded;
		}
		#endregion
	}
}