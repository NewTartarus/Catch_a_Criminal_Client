using ScotlandYard.Enums;
using ScotlandYard.Scripts.Street;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ScotlandYard.Scripts.PlayerScripts
{
    [Serializable]
    public class PlayerData
    {
        protected string id;
        [SerializeField] protected string agentName;
        [SerializeField] protected EPlayerType type;
        protected Dictionary<ETicket, int> ticketList;
        [SerializeField] protected Color playerColor;
        protected StreetPoint currentPosition;
        protected bool hasLost;

        public string ID { get => id; set => id = value; }
        public string AgentName { get => agentName; set => agentName = value; }
        public EPlayerType PlayerType { get => type; set => type = value; }
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
        public StreetPoint CurrentPosition
        {
            get => currentPosition;
            set => currentPosition = value;
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

        public PlayerData Clone()
        {
            PlayerData clonedData = new PlayerData();
            clonedData.AgentName = this.AgentName;
            clonedData.ID = this.ID;
            clonedData.PlayerColor = this.PlayerColor;
            clonedData.PlayerType = this.PlayerType;
            clonedData.CurrentPosition = this.CurrentPosition;
            clonedData.HasLost = this.HasLost;

            Dictionary<ETicket, int> cloneTickets = new Dictionary<ETicket, int>();
            this.Tickets.ToList().ForEach(ticket => cloneTickets.Add(ticket.Key, ticket.Value));

            clonedData.Tickets = cloneTickets;

            return clonedData;
        }
    }
}
