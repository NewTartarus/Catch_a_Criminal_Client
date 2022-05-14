namespace ScotlandYard.Scripts.GameSettings
{
    using ScotlandYard.Interfaces;
    using System;
	using System.Collections;
	using System.Collections.Generic;
	
	public class ServerSetting : IServerSetting
	{
		#region Members
		protected int id;
		protected string serverName;
		protected string serverUrl;
		protected string hashedPassword;
		protected string state;
		protected DateTime lastLogin;
		#endregion
		
		#region Properties
		public int Id
		{ 
			get => id;
		}

		public string ServerName
		{
			get => serverName;
		}

		public string ServerUrl
		{
			get => serverUrl;
		}

		public string HashedPassword
		{
			get => hashedPassword;
		}

		public string State
		{
			get => state;
			set => state = value;
		}

		public string LastLogin
		{
			get => lastLogin.ToString("g");
			set => lastLogin = DateTime.Parse(value);
		}
		#endregion
		
		#region Methods
		public ServerSetting(int id, string name, string url, string hashedPassword, string lastLogin)
		{
			this.id             = id;
			this.serverName     = name;
			this.serverUrl      = url;
			this.hashedPassword = hashedPassword;
			this.lastLogin      = DateTime.Parse(lastLogin);
		}

		public ServerSetting(string name, string url, string hashedPassword, DateTime lastLogin)
		{
			this.serverName     = name;
			this.serverUrl      = url;
			this.hashedPassword = hashedPassword;
			this.lastLogin      = lastLogin;
		}
		#endregion
	}
}