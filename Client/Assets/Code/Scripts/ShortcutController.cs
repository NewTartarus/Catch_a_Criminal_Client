using ScotlandYard.Scripts.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ScotlandYard.Scripts
{
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
