namespace ScotlandYard.Scripts.Street
{
    using ScotlandYard.Enums;
    using ScotlandYard.Interfaces;
    using UnityEngine;

    public class StreetContainer : MonoBehaviour
    {
        //IStreet
        [SerializeField] protected GameObject startPoint;
        [SerializeField] protected GameObject endPoint;
        [SerializeField] protected ETicket[] cost;


        //StreetPath
        [Range(0, 5)]
        [SerializeField] protected float width = 2;
        [SerializeField] protected Color waypointColor;


        //Container
        protected IStreet instance;
        public IStreet Instance
        {
            get
            {
                if(this.instance == null)
                {
                    InitInstance();
                }

                return this.instance;
            }
        }
        [SerializeField] protected EStreetType type;

        protected virtual void InitInstance()
        {
            
            switch (type)
            {
                case EStreetType.ROUTE:
                    instance = gameObject.AddComponent<Route>();
                    break;
                case EStreetType.STREET:
                    instance = gameObject.AddComponent<StreetPath>();
                    break;
                default:
                    instance = gameObject.AddComponent<StreetPath>();
                    break;
            }

            this.Instance.EndPoint = this.endPoint;
            this.Instance.StartPoint = this.startPoint;
            this.Instance.Costs = this.cost;

            if (this.Instance is StreetPath street)
            {
                street.Width = width;
                street.waypointColor = this.waypointColor;
            }
        }


#if UNITY_EDITOR
        protected virtual Transform GetWaypoint(int i)
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
            if(type == EStreetType.STREET)
            {
                int childCount = this.transform.childCount;

                for (int i = 0; i < childCount; i++)
                {
                    if (i == 0)
                    {
                        Gizmos.color = Color.white;
                        Gizmos.DrawLine(startPoint.transform.position, GetWaypoint(i).position);
                        Gizmos.color = new Color32(200, 0, 0, 170);
                        Gizmos.DrawCube(startPoint.transform.position, Vector3.one * 0.5f);
                    }
                    else if (i == childCount - 1)
                    {
                        Gizmos.color = Color.white;
                        Gizmos.DrawLine(GetWaypoint(i).position, endPoint.transform.position);
                        Gizmos.color = new Color32(200, 0, 0, 170);
                        Gizmos.DrawCube(endPoint.transform.position, Vector3.one * 0.5f);
                    }

                    Transform wp = GetWaypoint(i);
                    if (wp != null)
                    {
                        if (i < childCount - 1 && GetWaypoint(i + 1) is Transform posB)
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
        }
#endif
    }
}
