using ScotlandYard.Enums;
using ScotlandYard.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ScotlandYard.Scripts.Street
{
    class Route : MonoBehaviour, IStreet
    {
        public GameObject StartPoint { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public GameObject EndPoint { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ETicket[] Costs { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public int GetNumberOfWaypoints()
        {
            throw new NotImplementedException();
        }

        public Transform GetPathsTransform()
        {
            throw new NotImplementedException();
        }

        public Transform GetWaypoint(int i)
        {
            throw new NotImplementedException();
        }

        public ETicket[] ReturnTicketCost()
        {
            throw new NotImplementedException();
        }
    }
}
