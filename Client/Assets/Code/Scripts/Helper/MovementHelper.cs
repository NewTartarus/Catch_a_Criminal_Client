namespace ScotlandYard.Scripts.Helper
{
    using ScotlandYard.Enums;
    using ScotlandYard.Interfaces;
    using ScotlandYard.Scripts.PlayerScripts;
    using System.Collections.Generic;

    public class MovementHelper
    {
        public static List<IStreetPoint> GetTargets(Agent agent)
        {
            return GetTargets(agent, agent.Data.CurrentPosition);
        }

        public static List<IStreetPoint> GetTargets(Agent agent, IStreetPoint position)
        {
            List<IStreetPoint> targets = new List<IStreetPoint>();
            IStreet[] streetList = position.GetStreetArray();

            foreach (IStreet street in streetList)
            {
                var target = !street.StartPoint.Equals(position) ? street.StartPoint : street.EndPoint;

                if(!target.IsOccupied)
                {
                    bool playerHasTicket = false;
                    foreach (ETicket ticket in street.TicketCosts)
                    {
                        if (agent.HasTicket(ticket))
                        {
                            playerHasTicket = true;
                            break;
                        }
                    }

                    if (playerHasTicket)
                    {
                        targets.Add(target);
                    }
                }
            }

            return targets;
        }
    }
}
