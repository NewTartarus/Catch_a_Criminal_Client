using ScotlandYard.Enums;
using ScotlandYard.Interface;
using ScotlandYard.Scripts.PlayerScripts;
using System;

namespace ScotlandYard.Events
{
    public class TicketEventArgs : EventArgs
    {
        public ETicket Ticket { get; set; }
        public string PlayerID { get; set; }
        public IStreet Street { get; set; }

        public TicketEventArgs(string playerID, ETicket ticket, IStreet street)
        {
            this.PlayerID = playerID;
            this.Ticket = ticket;
            this.Street = street;
        }
    }
}