namespace ScotlandYard.Interfaces
{
    using ScotlandYard.Enums;
    using System.Collections.Generic;
    using UnityEngine;

    public interface IStreet
    {
        IStreetPoint StartPoint { get; set; }
        IStreetPoint EndPoint { get; set; }
        List<ETicket> TicketCosts { get; set; }

        int GetNumberOfWaypoints();

        Transform GetWaypoint(int i);
    }
}
