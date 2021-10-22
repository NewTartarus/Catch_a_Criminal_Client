namespace ScotlandYard.Scripts.Helper
{
    using ScotlandYard.Interfaces;
    using ScotlandYard.Scripts.Events;
    using ScotlandYard.Scripts.PlayerScripts;
    using System.Collections.Generic;

    public class HighlightBehavior
    {
        private static List<IStreetPoint> prevHighlightedPoints = new List<IStreetPoint>();

        public static void HighlightAccesPoints(Agent agent)
        {
            UnmarkPreviouslyHighlightedPoints();

            //highlight all Points, that are accessable by the player
            var targets = MovementHelper.GetTargets(agent);
            if(targets.Count > 0)
            {
                foreach (IStreetPoint streetPoint in targets)
                {
                    prevHighlightedPoints.Add(streetPoint);
                    streetPoint.IsHighlighted = true;
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
            foreach (IStreetPoint p in prevHighlightedPoints)
            {
                p.IsHighlighted = false;
            }
            prevHighlightedPoints = new List<IStreetPoint>();
        }

        public static void HighlightOnlyOne(IStreetPoint streetPoint)
        {
            UnmarkPreviouslyHighlightedPoints();

            prevHighlightedPoints.Add(streetPoint);
            streetPoint.IsHighlighted = true;
        }

        public static void Destroy()
        {
            prevHighlightedPoints = new List<IStreetPoint>();
        }
    }
}

