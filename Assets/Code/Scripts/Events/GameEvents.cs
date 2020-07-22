using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ScotlandYard.Events
{
    public class GameEvents : MonoBehaviour
    {
        #region Events
        public event EventHandler OnMisterXWon;
        public event EventHandler OnDetectivesWon;
        public event EventHandler<PlayerEventArgs> OnDetectiveLost;

        public event EventHandler<int> OnMakeNextMove;
        public event EventHandler<PlayerEventArgs> OnPlayerMoveFinished;

        public event EventHandler<int> OnRoundHasEnded;
        #endregion

        public static GameEvents current;

        private void Awake()
        {
            current = this;
        }

        public void MakeNextMove(object sender, int args)
        {
            OnMakeNextMove?.Invoke(sender, args);
        }

        public void PlayerMoveFinished(object sender, PlayerEventArgs args)
        {
            OnPlayerMoveFinished?.Invoke(sender, args);
        }

        public void RoundHasEnded(object sender, int args)
        {
            OnRoundHasEnded?.Invoke(sender, args);
        }

        public void MisterXWon(object sender, EventArgs args)
        {
            OnMisterXWon?.Invoke(sender, args);
        }
    }
}
