using ScotlandYard.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ScotlandYard.Interface
{
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
