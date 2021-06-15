﻿using ScotlandYard.Enums;
using ScotlandYard.Scripts.PlayerScripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScotlandYard.Events
{
    public class TicketUpdateEventArgs : EventArgs
    {
        public Dictionary<string, Dictionary<ETicket, int>> playerTicketDictionary = new Dictionary<string, Dictionary<ETicket, int>>();

        public TicketUpdateEventArgs(List<PlayerData> dataList)
        {
            foreach(PlayerData data in dataList)
            {
                playerTicketDictionary.Add(data.ID, data.Tickets);
            }
        }

        public TicketUpdateEventArgs(PlayerData data)
        {
            playerTicketDictionary.Add(data.ID, data.Tickets);
        }
    }
}
