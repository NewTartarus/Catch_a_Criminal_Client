namespace ScotlandYard.Scripts.PlayerScripts
{
    using ScotlandYard.Enums;
    using ScotlandYard.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    [Serializable]
    public class PlayerData
    {
        protected string id;
        [SerializeField] protected string agentName;
        [SerializeField] protected EPlayerRole role;
        protected Dictionary<ETicket, int> ticketList;
        [SerializeField] protected Color playerColor;
        protected IStreetPoint currentPosition;
        protected bool hasLost;

        public string ID { get => id; set => id = value; }
        public string AgentName { get => agentName; set => agentName = value; }
        public EPlayerRole PlayerRole { get => role; set => role = value; }
        public Dictionary<ETicket, int> Tickets
        {
            get
            {
                if(ticketList == null)
                {
                    ticketList = new Dictionary<ETicket, int>();
                }

                return ticketList;
            }
            set => ticketList = value;
        }
        public Color PlayerColor
        {
            get => playerColor;
            set => playerColor = value;
        }
        public IStreetPoint CurrentPosition
        {
            get => currentPosition;
            set
            {
                if(currentPosition != value)
                {
                    if (PlayerRole != EPlayerRole.MISTERX)
                    {
                        if (currentPosition != null)
                        {
                            currentPosition.IsOccupied = false;
                        }
                        value.IsOccupied = true;
                    }

                    currentPosition = value;
                }
            }
        }
        public bool HasLost
        {
            get => hasLost;
            set
            {
                if (value)
                {
                    hasLost = value;
                    Debug.Log($"{AgentName} lost this Game.");
                }
            }
        }

        public PlayerData() { }

        public PlayerData(string agentName, EPlayerRole role, Color playerColor)
        {
            this.agentName = agentName;
            this.role = role;
            this.playerColor = playerColor;
        }

        public PlayerData Clone()
        {
            PlayerData clonedData = new PlayerData();
            clonedData.AgentName = this.AgentName;
            clonedData.ID = this.ID;
            clonedData.PlayerColor = this.PlayerColor;
            clonedData.PlayerRole = this.PlayerRole;
            clonedData.CurrentPosition = this.CurrentPosition;
            clonedData.HasLost = this.HasLost;

            Dictionary<ETicket, int> cloneTickets = new Dictionary<ETicket, int>();
            this.Tickets.ToList().ForEach(ticket => cloneTickets.Add(ticket.Key, ticket.Value));

            clonedData.Tickets = cloneTickets;

            return clonedData;
        }
    }
}
