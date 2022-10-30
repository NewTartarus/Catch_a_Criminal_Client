namespace ScotlandYard.Scripts.SaveSystem
{
    using ScotlandYard.Scripts.Database;
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class DBCreator : MonoBehaviour
	{
		#region Members
		[SerializeField] List<TextAsset> sqlScripts;
		#endregion
		
		#region Methods
		protected void Awake()
		{
			SqliteDbManager dbManager = new SqliteDbManager();
			string combinedQueries = String.Join("\n", sqlScripts);

			dbManager.CreateDatabase(combinedQueries);

			SceneManager.LoadScene("StartMenu", LoadSceneMode.Single);
		}
		#endregion
	}
}