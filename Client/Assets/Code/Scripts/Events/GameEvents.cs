﻿using ScotlandYard.Scripts.UI;
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

        //Ticket Selection
        public event EventHandler<MovementEventArgs> OnDestinationSelected;
        public event EventHandler<TicketEventArgs> OnTicketSelection_Approved;
        public event EventHandler<MovementEventArgs> OnTicketSelection_Canceled;
        public event EventHandler<TicketButton> OnTicketSelected;
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

        public void DestinationSelected(object sender, MovementEventArgs args)
        {
            OnDestinationSelected?.Invoke(sender, args);
        }

        public void TicketSelection_Approved(object sender, TicketEventArgs args)
        {
            OnTicketSelection_Approved?.Invoke(sender, args);
        }

        public void TicketSelection_Canceled(object sender, MovementEventArgs args)
        {
            OnTicketSelection_Canceled?.Invoke(sender, args);
        }

        public void TicketSelected(object sender, TicketButton args)
        {
            OnTicketSelected?.Invoke(sender, args);
        }
    }
}