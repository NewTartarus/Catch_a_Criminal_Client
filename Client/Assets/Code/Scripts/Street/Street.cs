namespace ScotlandYard.Scripts.Street
{
    using ScotlandYard.Enums;
    using ScotlandYard.Interfaces;
    using System;
	using System.Collections.Generic;
	using UnityEngine;
	
	public abstract class Street : MonoBehaviour, IStreet
    {
        #region Members
        [SerializeField] protected StreetPoint startPoint;
        [SerializeField] protected StreetPoint endPoint;
        [SerializeField] protected List<ETicket> ticketCosts = new List<ETicket>();
        protected List<Transform> pathWaypoints = new List<Transform>();
        #endregion

        #region Properties
        public IStreetPoint StartPoint
        { 
            get => startPoint;
            set
            {
                // Workaround because Unity does not display Interfaces out of the box
                if(value is StreetPoint sp)
                {
                    startPoint = sp;
                }
            }
        }
        public IStreetPoint EndPoint 
        { 
            get => endPoint;
            set
            {
                // Workaround because Unity does not display Interfaces out of the box
                if (value is StreetPoint sp)
                {
                    endPoint = sp;
                }
            }
        }
        public List<ETicket> TicketCosts { get => ticketCosts; set => ticketCosts = value; }
        #endregion

        #region Methods
        public virtual int GetNumberOfWaypoints()
        {
            throw new NotImplementedException();
        }

        public virtual Transform GetWaypoint(int i)
        {
            throw new NotImplementedException();
        }

        public virtual List<Transform> ReturnChildTransforms()
        {
            return pathWaypoints;
        }
        #endregion
    }
}