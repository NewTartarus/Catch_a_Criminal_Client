﻿namespace ScotlandYard.Scripts.UI.InGame
{
    using ScotlandYard.Enums;
    using ScotlandYard.Scripts.Events;
    using ScotlandYard.Scripts.Localisation;
    using ScotlandYard.Scripts.PlayerScripts;
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class PlayerInfoItem : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] protected TextMeshProUGUI playerIndex;
        [SerializeField] protected TextLocaliserUI playerRole;
        [SerializeField] protected TextMeshProUGUI playerName;
        [SerializeField] protected TextMeshProUGUI taxiCount;
        [SerializeField] protected TextMeshProUGUI busCount;
        [SerializeField] protected TextMeshProUGUI undergroundCount;
        [SerializeField] protected TextMeshProUGUI blackTicketCount;
        [SerializeField] protected TextMeshProUGUI doubleTicketCount;
        [SerializeField] protected Image profileImage;
        [SerializeField] protected Image indexImage;

        protected string agentID;
        protected Transform ownTransform;

        public void Init(PlayerData data, int index)
        {
            agentID = data.ID;

            playerName.text = data.AgentName;
            playerIndex.text = index.ToString();
            taxiCount.text = GetTicketValue(data.Tickets, ETicket.TAXI);
            busCount.text = GetTicketValue(data.Tickets, ETicket.BUS);
            undergroundCount.text = GetTicketValue(data.Tickets, ETicket.UNDERGROUND);
            blackTicketCount.text = GetTicketValue(data.Tickets, ETicket.BLACK_TICKET);
            doubleTicketCount.text = GetTicketValue(data.Tickets, ETicket.DOUBLE_TICKET);

            profileImage.color = data.PlayerColor;
            indexImage.color = data.PlayerColor;

            string localizationKey;
            switch(data.PlayerRole)
            {
                case EPlayerRole.DETECTIVE:
                    localizationKey = "player_role_detective";
                    break;
                case EPlayerRole.MISTERX:
                    localizationKey = "player_role_misterX";
                    break;
                default:
                    localizationKey = "player_role_detective";
                    break;
            }

            playerRole.localizedString.key = localizationKey;
            playerRole.UpdateText();

            ownTransform = this.transform;

            GameEvents.Current.OnTicketUpdated += Current_OnTicketUpdated;
            GameEvents.Current.OnPlayerActivated += Current_OnPlayerActivated;
        }

        private void Current_OnPlayerActivated(object sender, PlayerEventArgs e)
        {
            if(e.PlayerId == agentID)
            {
                if(e.IsActive)
                {
                    ownTransform.localScale = new Vector3(1.05f, 1.05f, 1.05f);
                }
                else
                {
                    ownTransform.localScale = Vector3.one;
                }
            }
        }

        private void Current_OnTicketUpdated(object sender, TicketUpdateEventArgs e)
        {
            if (e.playerTicketDictionary.ContainsKey(agentID))
            {
                foreach(KeyValuePair<ETicket, int> entry in e.playerTicketDictionary[agentID])
                {
                    SetTicketText(entry.Key, entry.Value.ToString());
                }
            }
        }

        protected void SetTicketText(ETicket ticket, string value)
        {
            switch(ticket)
            {
                case ETicket.TAXI:
                    taxiCount.text = value;
                    break;
                case ETicket.BUS:
                    busCount.text = value;
                    break;
                case ETicket.UNDERGROUND:
                    undergroundCount.text = value;
                    break;
                case ETicket.BLACK_TICKET:
                    blackTicketCount.text = value;
                    break;
                case ETicket.DOUBLE_TICKET:
                    doubleTicketCount.text = value;
                    break;
            }
        }

        protected string GetTicketValue(Dictionary<ETicket, int> ticketDict, ETicket ticketKey)
        {
            if(ticketDict.ContainsKey(ticketKey))
            {
                return ticketDict[ticketKey].ToString();
            }

            return "0";
        }

        protected void OnDestroy()
        {
            GameEvents.Current.OnTicketUpdated -= Current_OnTicketUpdated;
            GameEvents.Current.OnPlayerActivated -= Current_OnPlayerActivated;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            GameEvents.Current.PlayerItemClicked(this, agentID);
        }
    }
}
