namespace ScotlandYard.Scripts.Controller
{
    using ScotlandYard.Enums;
    using ScotlandYard.Interfaces;
    using ScotlandYard.Scripts.Street;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public class StreetController : MonoBehaviour
    {
        [SerializeField] protected List<Street> streetList;
        [SerializeField] protected List<StreetPoint> streetPoints;

        public void Init()
        {
            //Adds IStreet-Object to the corresponding Point-Object
            foreach (Street street in streetList)
            {
                street.StartPoint?.AddStreet(street);
                street.EndPoint?.AddStreet(street);
                street.Init();
            }
        }

        public List<StreetPoint> GetAllStreetPoints()
        {
            return streetPoints;
        }

        public HashSet<IStreetPoint> GetNeighboringStreetPoints(IStreetPoint streetPoint, int level, bool ignoreBlackTickets)
        {
            HashSet<IStreetPoint> neighbors = new HashSet<IStreetPoint>();
            IStreet[] streets = streetPoint.GetStreetArray();

            foreach(IStreet street in streets)
            {
                if(!ignoreBlackTickets || !(street.TicketCosts.Count == 1 && street.TicketCosts[0] == ETicket.BLACK_TICKET))
                {
                    neighbors.Add(street.StartPoint);
                    neighbors.Add(street.EndPoint);
                }
            }

            if(level-1 > 0)
            {
                HashSet<IStreetPoint> temp = new HashSet<IStreetPoint>();
                foreach (IStreetPoint n in neighbors)
                {
                    temp.UnionWith(GetNeighboringStreetPoints(n, --level, ignoreBlackTickets));
                }

                neighbors.UnionWith(temp);
            }

            neighbors.Remove(streetPoint);
            return neighbors;
        }

        [ContextMenu("AutoFill StreetPoints")]
        protected void AutoFillStreetPoints()
        {
            streetPoints = FindObjectsOfType<StreetPoint>().OrderBy(s => s.StreetPointName).ToList();
        }

        [ContextMenu("AutoFill Streets")]
        protected void AutoFillStreets()
        {
            streetList = FindObjectsOfType<Street>().OrderBy(s => s.name).ToList();
        }
    }
}
