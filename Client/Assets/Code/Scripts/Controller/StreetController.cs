namespace ScotlandYard.Scripts.Controller
{
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
            }
        }

        public IStreetPoint[] GetRandomPositions(int amount)
        {
            IStreetPoint[] positions = new StreetPoint[amount];
            List<int> pointsAlreadyUsed = new List<int>();

            
            for(int i = 0; i < amount; i++)
            {
                var random = new System.Random();
                pointsAlreadyUsed.Add(GetRandomUniqueInt(pointsAlreadyUsed,random));
                positions[i] = streetPoints[pointsAlreadyUsed[i]];
            }

            return positions;
        }

        protected virtual int GetRandomUniqueInt(List<int> intsInUse, System.Random random)
        {
            int randomInt = random.Next(0, streetPoints.Count);

            if (!intsInUse.Contains(randomInt))
            {
                return randomInt;
            }

            return GetRandomUniqueInt(intsInUse, random);
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
