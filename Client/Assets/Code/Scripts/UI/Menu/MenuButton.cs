using ScotlandYard.Enums;
using ScotlandYard.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ScotlandYard.Scripts.UI.Menu
{
    public class MenuButton : MonoBehaviour
    {
        public void ExitGame()
        {
            Application.Quit();
        }

        public void PlaySingleplayer()
        {
            SceneManager.LoadScene("TestScene", LoadSceneMode.Single);
        }

        public void PlayMultiplayer()
        {
            GameEvents.Current.MainMenuButtonPressed(this, EButtons.MULTIPLAYER);
        }

        public void DoCustomization()
        {
            GameEvents.Current.MainMenuButtonPressed(this, EButtons.CHARACTER_CUSTOMIZATION);
        }

        public void DoSettings()
        {
            GameEvents.Current.MainMenuButtonPressed(this, EButtons.SETTINGS);
        }
    }
}
