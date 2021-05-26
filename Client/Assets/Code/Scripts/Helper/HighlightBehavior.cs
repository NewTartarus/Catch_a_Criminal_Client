using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScotlandYard.Scripts.Street;
using ScotlandYard.Scripts.PlayerScripts;

namespace ScotlandYard.Scripts.Helper
{
    public class HighlightBehavior
    {
        private static List<StreetPoint> prevHighlightedPoints = new List<StreetPoint>();

        public static void HighlightAccesPoints(Agent agent)
        {
            UnmarkPreviouslyHighlightedPoints();

            //highlight all Points, that are accessable by the player
            var targets = MovementHelper.GetTargets(agent);
            foreach (GameObject go in targets)
            {
                StreetPoint point = go.GetComponent<StreetPoint>();
                prevHighlightedPoints.Add(point);
                point.IsHighlighted = true;
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
    }
}

