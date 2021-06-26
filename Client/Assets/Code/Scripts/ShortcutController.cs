namespace ScotlandYard.Scripts
{
    using ScotlandYard.Scripts.UI.InGame;
    using UnityEngine;

    public class ShortcutController : MonoBehaviour
    {
        [SerializeField] protected PauseMenu pauseMenu;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                pauseMenu.PauseGame();
            }
        }
    }
}
