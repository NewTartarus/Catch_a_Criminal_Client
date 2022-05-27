namespace ScotlandYard.Scripts.UI.InGame
{
    using ScotlandYard.Scripts.Events;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;

    public class PauseMenu : MonoBehaviour
    {
        protected bool isGamePaused;
        [SerializeField] protected Selectable firstSelected;

        public void PauseGame()
        {
            if(isGamePaused)
            {
                Resume();
                isGamePaused = false;
            }
            else
            {
                Display();
                isGamePaused = true;
            }
        }

        protected void Display()
        {
            Time.timeScale = 0f;
            this.gameObject.SetActive(true);
            firstSelected?.Select();
        }

        public void Resume()
        {
            Time.timeScale = 1f;
            this.gameObject.SetActive(false);
        }

        public void Resign()
        {
            GameEvents.Current.PlayerResigned(this, true);
            Resume();
        }

        public void GoToMenu()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("StartMenu", LoadSceneMode.Single);
        }

        public void ExitGame()
        {
            Application.Quit();
        }
    }
}
