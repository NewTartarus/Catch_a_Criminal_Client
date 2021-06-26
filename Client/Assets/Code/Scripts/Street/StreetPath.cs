namespace ScotlandYard.Scripts.Street
{
    using ScotlandYard.Enums;
    using ScotlandYard.Interfaces;
    using UnityEngine;

    public class StreetPath : MonoBehaviour, IStreet
    {
        [SerializeField] protected GameObject pointA;
        [SerializeField] protected GameObject pointB;

        public GameObject StartPoint { get => pointA; set => pointA = value; }
        public GameObject EndPoint { get => pointB; set => pointB = value; }

        public Color waypointColor;
        [Range(0, 5)]
        [SerializeField] protected float width = 2;

        public float Width { get => width; set => width = value; }

        [SerializeField] public ETicket[] Costs { get; set; }

        protected virtual void Start()
        {
            LineRenderer renderer = this.gameObject.AddComponent<LineRenderer>();

            renderer.positionCount = GetNumberOfWaypoints() + 2;
            renderer.numCornerVertices = 20;
            for (int i = -1; i <= GetNumberOfWaypoints(); i++)
            {
                renderer.SetPosition(i + 1, GetWaypoint(i).position);
            }

            renderer.startWidth = 0.1f;
            renderer.endWidth = 0.1f;
            renderer.material = new Material(Shader.Find("Universal Render Pipeline/Unlit"));
            renderer.material.SetColor("_BaseColor", waypointColor);
        }

        public virtual int GetNumberOfWaypoints()
        {
            return this.transform.childCount;
        }

        public virtual Transform GetWaypoint(int i)
        {
            if (this.transform.childCount == 0 && StartPoint == null && EndPoint == null)
            {
                return null;
            }

            if (i == -1)
            {
                return StartPoint.transform;
            }
            else if (i == this.transform.childCount)
            {
                return EndPoint.transform;
            }

            Transform trans = this.transform.GetChild(i);
            return trans.CompareTag("WayPoint") ? trans : null;
        }

        public virtual Transform GetPathsTransform()
        {
            return this.transform; // for getting childs: foreach(Transform trans in this.transform)
        }

        public virtual ETicket[] ReturnTicketCost()
        {
            return Costs;
        }
    }
}

