﻿namespace ScotlandYard.Scripts.History
{
    using ScotlandYard.Enums;
    using ScotlandYard.Scripts.PlayerScripts;

    public class HistoryItem
    {
        protected int round;
        protected ETicket ticket;
        protected PlayerData data;

        public int Round
        {
            get => round;
            set => round = value;
        }

        public ETicket Ticket
        {
            get => ticket;
            set => ticket = value;
        }

        public PlayerData Data
        {
            get => data;
            set => data = value;
        }

        public HistoryItem(int round, ETicket ticket, PlayerData data)
        {
            Round = round;
            Ticket = ticket;
            Data = data;
        }
    }
}
