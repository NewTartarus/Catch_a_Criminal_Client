namespace ScotlandYard.Scripts.Controller
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Diagnostics;
	using ScotlandYard.Scripts.Helper;
    using Firesplash.UnityAssets.SocketIO;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;
    using ScotlandYard.Scripts.Events;
    using ScotlandYard.Scripts.Transfer;
    using ScotlandYard.Scripts.Database.DAOs;
    using ScotlandYard.Interfaces;
    using ScotlandYard.Scripts.GameSettings;

    public class MultiplayerController : MonoBehaviour
	{
		#region Members
		protected string hashedPassword;
		protected string playerName;
		
		[SerializeField] protected SocketIOCommunicator sioCom;
		[SerializeField] protected TMP_InputField testNameInput;
		#endregion

		#region Properties
		#endregion

		#region Methods
		protected void Awake() 
		{
			GameEvents.Current.OnMenuConnect    += Current_OnMenuConnect;
			GameEvents.Current.OnMenuDisconnect += Current_OnMenuDisconnect;
			GameEvents.Current.OnSendingMessage += Current_OnSendingMessage;
			GameEvents.Current.OnMenuServerRemoved += Current_OnMenuServerRemoved;
		}

		public void Current_OnMenuConnect(object sender, string[] args)
		{
			sioCom.socketIOAddress = args[0];
			hashedPassword = args[1];
			playerName = string.IsNullOrEmpty(testNameInput.text) ? "UserX" : testNameInput.text;
			SubscribeToEvents();
			sioCom.Instance.Connect();

			if(args.Length >= 3)
            {
				StartCoroutine(Login(Convert.ToDouble(args[2])));
			}
            else
            {
				StartCoroutine(Login());
			}
		}

		public void Current_OnMenuDisconnect(object sender, string args)
		{
			UnsubscribeEvents();
			sioCom.Instance.Close();
			sioCom.RemoveInstance();
		}

		public void Current_OnSendingMessage(object sender, string args)
		{
			sioCom.Instance.Emit("message", args, true);
		}

		public void Current_OnMenuServerRemoved(object sender, IServerSetting args)
        {
			SavedServerDAO.getInstance().Delete(args as ServerSetting);
		}

		protected virtual IEnumerator Login(double timeout = 180000)
		{
			GameEvents.Current.StartingMultiplayerLogin(this, true);

			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();

			yield return new WaitUntil(() => (sioCom.Instance.IsConnected() || stopwatch.Elapsed.TotalMilliseconds >= timeout));

			stopwatch.Stop();
			TimeSpan stopwatchElapsed = stopwatch.Elapsed;

			
			if (sioCom.Instance.IsConnected())
			{
				sioCom.Instance.Emit("login",
									 "{\"username\":\"" + playerName + "\",\"password\":\"" + hashedPassword + "\"}",
									 false);
			}
			else
			{
				GameEvents.Current.MenuError(this, "error_login_failed");
				GameEvents.Current.EndingMultiplayerLogin(this, -1);
				UnsubscribeEvents();
				sioCom.RemoveInstance();
			}
		}

		protected virtual void SubscribeToEvents()
		{
			sioCom.Instance.On("login-success", (serverName) => {
				GameSettings.ServerSetting serverSetting = new GameSettings.ServerSetting(serverName, sioCom.socketIOAddress, hashedPassword, DateTime.Now);

				GameEvents.Current.MenuConnectingSucceeded(this, serverSetting);
				GameEvents.Current.EndingMultiplayerLogin(this, 0);
				SavedServerDAO.getInstance().Insert(serverSetting);
			});
			
			sioCom.Instance.On("error", (payload) => {
				sioCom.Instance.Close();
				GameEvents.Current.MenuError(this, payload);
				GameEvents.Current.EndingMultiplayerLogin(this, -1);
			});

			sioCom.Instance.On("message", (payload) => {
				Message msg = Message.CreateFromJSON(payload);
				GameEvents.Current.MultiplayerMessage(this, msg);
			});
		}

		protected virtual void UnsubscribeEvents()
		{
			sioCom.Instance.Off("login-success");
			sioCom.Instance.Off("error");
			sioCom.Instance.Off("message");
		}

		protected void OnDestroy() 
		{
			GameEvents.Current.OnMenuConnect    -= Current_OnMenuConnect;
			GameEvents.Current.OnMenuDisconnect -= Current_OnMenuDisconnect;
			GameEvents.Current.OnSendingMessage -= Current_OnSendingMessage;
			GameEvents.Current.OnMenuServerRemoved -= Current_OnMenuServerRemoved;
		}
		#endregion
	}
}