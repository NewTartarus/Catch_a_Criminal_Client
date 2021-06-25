namespace ScotlandYard.Scripts.Helper
{
    using ScotlandYard.Scripts.Events;
    using ScotlandYard.Scripts.PlayerScripts;
    using ScotlandYard.Scripts.Street;
    using System.Collections.Generic;
    using UnityEngine;

    public class HighlightBehavior
    {
        private static List<StreetPoint> prevHighlightedPoints = new List<StreetPoint>();

        public static void HighlightAccesPoints(Agent agent)
        {
            UnmarkPreviouslyHighlightedPoints();

            //highlight all Points, that are accessable by the player
            var targets = MovementHelper.GetTargets(agent);
            if(targets.Count > 0)
            {
                foreach (GameObject go in targets)
                {
                    StreetPoint point = go.GetComponent<StreetPoint>();
                    prevHighlightedPoints.Add(point);
                    point.IsHighlighted = true;
                }
            }
            else
            {
                agent.Data.HasLost = true;
                GameEvents.Current.PlayerMoveFinished(null, new PlayerEventArgs(agent.Data));
            }
        }

        public static void UnmarkPreviouslyHighlightedPoints()
        {
            foreach (StreetPoint p in prevHighlightedPoints)
            {
                p.IsHighlighted = false;
            }
            prevHighlightedPoints = new List<StreetPoint>();
        }

        public static void HighlightOnlyOne(StreetPoint streetPoint)
        {
            UnmarkPreviouslyHighlightedPoints();

            prevHighlightedPoints.Add(streetPoint);
            streetPoint.IsHighlighted = true;
        }

        public static void Destroy()
        {
            prevHighlightedPoints = new List<StreetPoint>();
        }
    }
}

