using ScotlandYard.Events;
using ScotlandYard.Scripts.PlayerScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ScotlandYard.Scripts.UI
{
    public class PauseMenu : MonoBehaviour
    {
        protected bool isGamePaused;
        [SerializeField] protected Player player;

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
        }

        public void Resume()
        {
            Time.timeScale = 1f;
            this.gameObject.SetActive(false);
        }

        public void Resign()
        {
            player.Data.HasLost = true;
            GameEvents.Current.PlayerMoveFinished(this, new PlayerEventArgs(player.Data));
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
