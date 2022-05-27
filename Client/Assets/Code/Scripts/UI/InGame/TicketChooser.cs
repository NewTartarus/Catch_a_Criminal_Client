namespace ScotlandYard.Scripts.UI.InGame
{
    using ScotlandYard.Enums;
    using ScotlandYard.Interfaces;
    using ScotlandYard.Scripts.Events;
    using ScotlandYard.Scripts.PlayerScripts;
    using ScotlandYard.Scripts.Street;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class TicketChooser : MonoBehaviour
    {
        protected Agent player;
        protected StreetPoint streetPoint;
        protected ETicket selectedTicket;

        [SerializeField] protected List<TicketButton> ticketButtons;
        protected TicketButton selectedTicketButton;

        [SerializeField] protected Button okButton;

        public void Init()
        {
            GameEvents.Current.OnDestinationSelected += Current_OnDestinationSelected;
            GameEvents.Current.OnTicketSelected += Current_OnTicketSelected;
        }

        private void Current_OnTicketSelected(object sender, TicketButton e)
        {
            if(e != null && !e.Equals(selectedTicketButton))
            {
                UnselectAll();

                e.Select = true;
                selectedTicketButton = e;
                selectedTicket = e.GetTicket();

                okButton.interactable = true;
            }
        }

        private void Current_OnDestinationSelected(object sender, MovementEventArgs e)
        {
            this.player = e.Player;
            this.streetPoint = e.TargetPosition;

            foreach(TicketButton tb in ticketButtons)
            {
                tb.SetTicketCount(this.player.GetTicketCount(tb.GetTicket()));
            }

            IStreet street = player.Data.CurrentPosition.GetPath(streetPoint);
            var costs = street.TicketCosts;

            foreach(TicketButton tb in ticketButtons)
            {
                if(tb.gameObject.activeSelf == true && !costs.Contains(tb.GetTicket()))
                {
                    tb.gameObject.SetActive(false);
                }
            }

            okButton.interactable = false;
            this.gameObject.SetActive(true);
        }

        public void Ok_Pressed()
        {
            if(selectedTicket == ETicket.EMPTY) { return; }

            IStreet street = player.Data.CurrentPosition.GetPath(streetPoint);
            GameEvents.Current.TicketSelection_Approved(null, new TicketEventArgs(player.Data.ID, selectedTicket, street));

            this.gameObject.SetActive(false);
            UnselectAll();
        }

        public void Cancel_Pressed()
        {
            GameEvents.Current.TicketSelection_Canceled(null, new MovementEventArgs(player, streetPoint));

            this.gameObject.SetActive(false);
            UnselectAll();
        }

        protected virtual void UnselectAll()
        {
            foreach(TicketButton tb in ticketButtons)
            {
                tb.Select = false;
            }

            selectedTicketButton = null;
        }

        public void Destroy()
        {
            GameEvents.Current.OnDestinationSelected -= Current_OnDestinationSelected;
            GameEvents.Current.OnTicketSelected -= Current_OnTicketSelected;
        }
    }
}
