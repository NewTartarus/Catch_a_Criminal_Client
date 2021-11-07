namespace ScotlandYard.Scripts.UI.Menu
{
    using ScotlandYard.Enums;
    using ScotlandYard.Scripts.Events;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class MenuButton : MonoBehaviour
    {
        [SerializeField] protected GameObject startMenu;
        [SerializeField] protected GameObject singleplayerMenu;

        public void ExitGame()
        {
            Application.Quit();
        }

        public void PlaySingleplayer()
        {
            singleplayerMenu.SetActive(true);
            startMenu.SetActive(false);
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
