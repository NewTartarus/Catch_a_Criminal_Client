namespace ScotlandYard.Interfaces
{
    using ScotlandYard.Enums;
    using UnityEngine;

    public interface IStreet
    {
        GameObject StartPoint { get; set; }
        GameObject EndPoint { get; set; }
        ETicket[] Costs { get; set; }

        int GetNumberOfWaypoints();

        Transform GetWaypoint(int i);

        Transform GetPathsTransform();

        ETicket[] ReturnTicketCost();
    }
}
