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
        protected List<Vector3> pathWaypoints = new List<Vector3>();
        protected float distance;
        protected bool isInitialized;
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

        public float Distance
        {
            get
            {
                if (distance == 0f)
                {
                    distance = CalculateStreetDistance();
                }

                return distance;
            }

            set => distance = value;
        }
        #endregion

        #region Methods
        public virtual void Init()
        {
            throw new NotImplementedException();
        }

        public virtual void DrawLines(params ETicket[] tickets)
        {
            throw new NotImplementedException();
        }

        public virtual int GetNumberOfWaypoints()
        {
            throw new NotImplementedException();
        }

        public virtual Vector3 GetWaypoint(int i)
        {
            throw new NotImplementedException();
        }

        public virtual List<Vector3> ReturnChildTransforms()
        {
            return pathWaypoints;
        }

        protected virtual float CalculateStreetDistance()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}