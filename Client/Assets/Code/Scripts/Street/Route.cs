namespace ScotlandYard.Scripts.Street
{
    using ScotlandYard.Enums;
    using ScotlandYard.Interfaces;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public class Route : Street, IStreet
    {
        #region Members
        [SerializeField] protected Street[] streets;
        #endregion

        #region Methods
        public override void Init()
        {
            if(!isInitialized)
            {
                streets = OrderStreets(streets);
                pathWaypoints = DetermineWayPoints(streets);
                DrawLines(TicketCosts.ToArray());

                isInitialized = true;
            }
        }

        public override void DrawLines(params ETicket[] tickets)
        {
            foreach (Street s in streets)
            {
                s.DrawLines(TicketCosts.ToArray());
            }
        }

        public override int GetNumberOfWaypoints()
        {
            int count = 0;

            foreach(Street s in streets)
            {
                count += s.GetNumberOfWaypoints();
            }

            count += streets.Length - 1;

            return count;
        }

        public override Vector3 GetWaypoint(int i)
        {
            if (StartPoint == null && EndPoint == null)
            {
                return Vector3.zero;
            }

            if (this.pathWaypoints.Count == 0)
            {
                this.pathWaypoints = DetermineWayPoints(streets);
            }

            if (i == -1)
            {
                return StartPoint.GetTransform().position;
            }
            else if (i == GetNumberOfWaypoints())
            {
                return EndPoint.GetTransform().position;
            }

            return this.pathWaypoints[i];
        }

        protected Street[] OrderStreets(Street[] streets)
        {
            if(streets.Length > 1)
            {
                List<Street> strList = new List<Street>(streets);

                Street[] orderedStreets = new Street[streets.Length];
                orderedStreets[0] = strList.First(s => s.StartPoint.Equals(this.StartPoint) || s.EndPoint.Equals(this.StartPoint));
                strList.Remove(orderedStreets[0]);

                IStreetPoint lastSP = GetStreetPoint(orderedStreets[0], this.StartPoint);
                for (int i = 1; i < orderedStreets.Length; i++)
                {
                    orderedStreets[i] = strList.First(s => s.StartPoint.Equals(lastSP) || s.EndPoint.Equals(lastSP));
                    lastSP = GetStreetPoint(orderedStreets[i], lastSP);
                    strList.Remove(orderedStreets[i]);
                }

                return orderedStreets;
            }
            else
            {
                return streets;
            }
        }

        protected List<Vector3> DetermineWayPoints(Street[] streets)
        {
            List<Vector3> wpList = new List<Vector3>();
            IStreetPoint lastSP = this.StartPoint;

            foreach (Street s in streets)
            {
                bool isStarting = s.StartPoint.Equals(lastSP);

                if(isStarting)
                {
                    for (int i = 0; i <= s.GetNumberOfWaypoints(); i++)
                    {
                        wpList.Add(s.GetWaypoint(i));
                    }
                }
                else
                {
                    for (int i = s.GetNumberOfWaypoints()-1; i >= -1 ; i--)
                    {
                        wpList.Add(s.GetWaypoint(i));
                    }
                }

                lastSP = GetStreetPoint(s, lastSP);
            }

            wpList.Remove(this.EndPoint.GetTransform().position);

            return wpList;
        }

        protected IStreetPoint GetStreetPoint(Street street, IStreetPoint sp)
        {
            if(street.StartPoint.Equals(sp))
            {
                return street.EndPoint;
            }
            else
            {
                return street.StartPoint;
            }
        }

        protected override float CalculateStreetDistance()
        {
            float dst = 0f;
            foreach(Street s in streets)
            {
                dst += s.Distance;
            }

            return dst;
        }
        #endregion

#if UNITY_EDITOR
        public string ReturnStreetPointOrder()
        {
            string sps = StartPoint.StreetPointName;
            Street[] strList = OrderStreets(streets);

            IStreetPoint lastPoint = StartPoint;
            foreach(Street s in strList)
            {
                lastPoint = GetStreetPoint(s, lastPoint);
                sps += $" -> {lastPoint.StreetPointName}";
            }

            return sps;
        }

        public string ReturnWayPoints()
        {
            return string.Join("\n",DetermineWayPoints(streets));
        }
#endif
    }
}
