namespace ScotlandYard.Scripts.UI.InGame
{
    using ScotlandYard.Scripts.Localisation;
    using TMPro;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class GameOverOverlay : MonoBehaviour
	{
		#region Members
		[SerializeField] protected GameObject overlay;
		[SerializeField] protected TextMeshProUGUI gameOverMessage;
		#endregion

		#region Properties
		#endregion

		#region Methods

        public void BackToStart()
        {
			SceneManager.LoadScene("StartMenu", LoadSceneMode.Single);
		}

        public void Display(string key)
        {
            overlay.SetActive(false);

			string text = LocalisationSystem.GetLocalisedValue(key);
			gameOverMessage.SetText(text);

			this.gameObject.SetActive(true);
        }		
		#endregion
	}
}