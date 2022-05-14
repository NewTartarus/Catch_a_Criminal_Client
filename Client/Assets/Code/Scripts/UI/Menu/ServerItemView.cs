namespace ScotlandYard.Scripts.UI.Menu
{
    using ScotlandYard.Interfaces;
    using ScotlandYard.Scripts.Events;
	using TMPro;
	using UnityEngine;
	using UnityEngine.UI;
	
	public class ServerItemView : MonoBehaviour
	{
		#region Members
		[SerializeField] TMP_Text textServerName;
		[SerializeField] TMP_Text textUrl;
		[SerializeField] TMP_Text textLastLogin;
		[SerializeField] TMP_Text textState;
		[SerializeField] Button buttonLogin;

		protected IServerSetting setting;
		#endregion
		
		#region Properties
		public IServerSetting Setting
		{
			get => this.setting;
		}
		#endregion
		
		#region Methods
		protected void Awake() 
		{
			GameEvents.Current.OnMenuConnect           += Current_OnMenuConnect;
			GameEvents.Current.OnMenuError             += Current_OnMenuError;
			GameEvents.Current.OnMenuDisconnect        += Current_OnMenuDisconnect;
			GameEvents.Current.OnMultiplayerLoginEnded += Current_OnMultiplayerLoginEnded;
		}

		public void Init(IServerSetting setting) 
		{
			this.setting = setting;

			textServerName.text  = setting.ServerName;
			textUrl.text         = setting.ServerUrl;
			textLastLogin.text   = setting.LastLogin;
			textState.text       = setting.State;
		}

		public void Login()
		{
			GameEvents.Current.MenuConnecting(this, new string[]{setting.ServerUrl, setting.HashedPassword, "30000" });
		}

		public void Delete()
        {
			GameEvents.Current.MenuServerRemoved(this, Setting);
			Destroy(this.gameObject);
        }

		public void EnableLogin(bool enabled)
		{
			buttonLogin.interactable = enabled;
		}

		public void UpdateLastLogin(string date)
        {
			Setting.LastLogin  = date;
			textLastLogin.text = Setting.LastLogin;
		}

		protected void Current_OnMenuConnect(object sender, string[] args)
		{
			EnableLogin(false);
		}

		protected void Current_OnMenuError(object sender, string args)
		{
			EnableLogin(true);
		}

		protected void Current_OnMenuDisconnect(object sender, string args)
		{
			EnableLogin(true);
		}

		protected void Current_OnMultiplayerLoginEnded(object sender, int args)
		{
			EnableLogin(true);
		}

		protected void OnDestroy()
		{
			GameEvents.Current.OnMenuConnect           -= Current_OnMenuConnect;
			GameEvents.Current.OnMenuError             -= Current_OnMenuError;
			GameEvents.Current.OnMenuDisconnect        -= Current_OnMenuDisconnect;
			GameEvents.Current.OnMultiplayerLoginEnded -= Current_OnMultiplayerLoginEnded;
		}
		#endregion
	}
}