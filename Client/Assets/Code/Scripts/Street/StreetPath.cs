namespace ScotlandYard.Scripts.Street
{
    using ScotlandYard.Interfaces;
    using UnityEngine;

    public class StreetPath : Street, IStreet
    {
        [SerializeField] public Color waypointColor;
        [Range(0, 5)] [SerializeField] protected float width = 2;

        public float Width { get => width; set => width = value; }

        protected void Awake()
        {
            foreach (Transform child in this.transform)
            {
                if (child.CompareTag("WayPoint"))
                {
                    this.pathWaypoints.Add(child);
                }
            }
        }

        protected void Start()
        {
            //Todo: Replace LineRenderer with a component, that procedurally generates a street-mesh
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

        public override int GetNumberOfWaypoints()
        {
            return this.pathWaypoints.Count;
        }

        public override Transform GetWaypoint(int i)
        {
            if (this.transform.childCount == 0 && StartPoint == null && EndPoint == null)
            {
                return null;
            }

            if (i == -1)
            {
                return StartPoint.GetTransform();
            }
            else if (i == this.transform.childCount)
            {
                return EndPoint.GetTransform();
            }

            return this.pathWaypoints[i];
        }

#if UNITY_EDITOR
        protected virtual Transform GetWaypointForEditor(int i)
        {
            if (this.transform.childCount == 0 && startPoint == null && endPoint == null)
            {
                return null;
            }

            if (i == -1)
            {
                return startPoint.transform;
            }
            else if (i == this.transform.childCount)
            {
                return endPoint.transform;
            }

            Transform trans = this.transform.GetChild(i);
            return trans.CompareTag("WayPoint") ? trans : null;
        }
        protected virtual void OnDrawGizmos()
        {
            int childCount = this.transform.childCount;
    
            for (int i = 0; i < childCount; i++)
            {
                if (i == 0)
                {
                    Gizmos.color = Color.white;
                    Gizmos.DrawLine(startPoint.GetTransform().position, GetWaypointForEditor(i).position);
                    Gizmos.color = new Color32(200, 0, 0, 170);
                    Gizmos.DrawCube(startPoint.GetTransform().position, Vector3.one * 0.5f);
                }
                else if (i == childCount - 1)
                {
                    Gizmos.color = Color.white;
                    Gizmos.DrawLine(GetWaypointForEditor(i).position, endPoint.GetTransform().position);
                    Gizmos.color = new Color32(200, 0, 0, 170);
                    Gizmos.DrawCube(endPoint.GetTransform().position, Vector3.one * 0.5f);
                }

                Transform wp = GetWaypointForEditor(i);
                if (wp != null)
                {
                    if (i < childCount - 1 && GetWaypointForEditor(i + 1) is Transform posB)
                    {
                        Gizmos.color = Color.white;
                        Gizmos.DrawLine(wp.position, posB.position);
                    }
                    Gizmos.color = Color.black;
                    Gizmos.DrawLine(wp.position + (wp.right * width * 0.5f), wp.position - (wp.right * width * 0.5f));

                    Gizmos.color = waypointColor;
                    Gizmos.DrawSphere(wp.position, 0.2f);
                }

            }
        }
#endif
    }
}

