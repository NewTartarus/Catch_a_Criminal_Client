namespace ScotlandYard.Scripts.Events
{
    using ScotlandYard.Enums;
    using ScotlandYard.Scripts.UI;
    using System;

    public class GameEvents
    {
        #region Events
        public event EventHandler OnMisterXWon;
        public event EventHandler OnDetectivesWon;
        public event EventHandler<PlayerEventArgs> OnPlayerLost;

        public event EventHandler<int> OnMakeNextMove;
        public event EventHandler<PlayerEventArgs> OnPlayerMoveFinished;
        public event EventHandler<ETicket> OnDetectiveTicketRemoved;
        public event EventHandler<TicketUpdateEventArgs> OnTicketUpdated;

        public event EventHandler<int> OnRoundHasEnded;

        //Menu Events
        public event EventHandler<EButtons> OnMainMenuButtonPressed;
        public event EventHandler OnLanguageChanged;

        //Ticket Selection
        public event EventHandler<MovementEventArgs> OnDestinationSelected;
        public event EventHandler<TicketEventArgs> OnTicketSelection_Approved;
        public event EventHandler<MovementEventArgs> OnTicketSelection_Canceled;
        public event EventHandler<TicketButton> OnTicketSelected;
        #endregion

        private static GameEvents current;

        public static GameEvents Current
        {
            get
            {
                if(current == null)
                {
                    current = new GameEvents();
                }

                return current;
            }
        }

        public static void Reset()
        {
            current = null;
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

        public void DetectivesWon(object sender, EventArgs args)
        {
            OnDetectivesWon?.Invoke(sender, args);
        }

        public void PlayerLost(object sender, PlayerEventArgs args)
        {
            OnPlayerLost?.Invoke(sender, args);
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

        public void MainMenuButtonPressed(object sender, EButtons args)
        {
            OnMainMenuButtonPressed?.Invoke(sender, args);
        }

        public void LanguageChanged(object sender, EventArgs args)
        {
            OnLanguageChanged?.Invoke(sender, args);
        }

        public void DetectiveTicketRemoved(object sender, ETicket ticket)
        {
            OnDetectiveTicketRemoved?.Invoke(sender, ticket);
        }

        public void TicketUpdated(object sender, TicketUpdateEventArgs args)
        {
            OnTicketUpdated?.Invoke(sender, args);
        }
    }
}