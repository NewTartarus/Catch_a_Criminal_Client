namespace ScotlandYard.Scripts.Transfer
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
    using UnityEngine;

    [Serializable]
	public class Message
	{
		#region Members
		public string userName;
		public string time;
		public string text;
		#endregion

		public Message(string userName, string time, string text)
		{
			this.userName = userName;
			this.time     = time;
			this.text     = text;
		}

		public static Message CreateFromJSON(string jsonString)
    	{
        	return JsonUtility.FromJson<Message>(jsonString);
    	}
	}
}