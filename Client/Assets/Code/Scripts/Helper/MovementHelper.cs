﻿namespace ScotlandYard.Scripts.Helper
{
    using ScotlandYard.Enums;
    using ScotlandYard.Interfaces;
    using ScotlandYard.Scripts.PlayerScripts;
    using ScotlandYard.Scripts.Street;
    using System.Collections.Generic;
    using UnityEngine;

    public class MovementHelper
    {
        public static List<GameObject> GetTargets(Agent agent)
        {
            return GetTargets(agent, agent.Position);
        }

        public static List<GameObject> GetTargets(Agent agent, GameObject position)
        {
            List<GameObject> targets = new List<GameObject>();
            IStreet[] streetList = position.GetComponent<StreetPoint>().GetStreetArray();

            foreach (IStreet street in streetList)
            {
                var target = !street.StartPoint.Equals(position) ? street.StartPoint : street.EndPoint;

                if(!target.GetComponent<StreetPoint>().IsOccupied)
                {
                    bool playerHasTicket = false;
                    foreach (ETicket ticket in street.ReturnTicketCost())
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
