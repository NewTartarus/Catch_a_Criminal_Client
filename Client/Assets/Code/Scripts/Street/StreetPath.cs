namespace ScotlandYard.Scripts.Street
{
    using ScotlandYard.Enums;
    using ScotlandYard.Interfaces;
    using ScotlandYard.Scripts.MeshGenerator;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEditor;
    using UnityEngine;

    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class StreetPath : Street, IStreet
    {
        [SerializeField] public bool drawMeshInEditor = true;

        [SerializeField] protected RoadCrossSectionSO crossSection;
        [Range(2, 32)] [SerializeField] protected int sectionsCount = 8;
        [SerializeField] protected int resolution = 8;
        [SerializeField] protected List<Transform> controlPoints = new List<Transform>();

        protected RoadGenerator generator;

        public RoadCrossSectionSO CrossSection => crossSection;

        protected void Awake()
        {
            generator = new RoadGenerator(controlPoints.ToArray(), sectionsCount, resolution);

            GetComponent<MeshFilter>().sharedMesh = generator.GenerateMesh(crossSection);

            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
            meshRenderer.material = crossSection.RoadMaterial;
            meshRenderer.material.SetInt("_ShowFirstLine", ticketCosts.Contains(ETicket.UNDERGROUND) ? 1 : 0);
            meshRenderer.material.SetInt("_ShowSecondLine", ticketCosts.Contains(ETicket.TAXI) ? 1 : 0);
            meshRenderer.material.SetInt("_ShowThirdLine", ticketCosts.Contains(ETicket.BUS) ? 1 : 0);

            pathWaypoints = generator.CalculateWaypoints();
        }

        public override int GetNumberOfWaypoints()
        {
            return this.pathWaypoints.Count;
        }

        public override Vector3 GetWaypoint(int i)
        {
            if (this.transform.childCount == 0 && StartPoint == null && EndPoint == null)
            {
                return Vector3.zero;
            }

            if (i == -1)
            {
                return StartPoint.GetTransform().position;
            }
            else if (i == GetNumberOfWaypoints())
            {
                return EndPoint.GetTransform().position;
            }

            return this.pathWaypoints[i];
        }

#if UNITY_EDITOR
        public void AddControlPoint(Transform transform)
        {
            controlPoints.Add(transform);
        }

        public void RemoveControlPoint(int index)
        {
            controlPoints.RemoveAt(index);
        }

        public void GenerateMesh()
        {
             RoadGenerator roadGenerator = new RoadGenerator(controlPoints.ToArray(), sectionsCount, resolution);

            GetComponent<MeshFilter>().sharedMesh = roadGenerator.GenerateMesh(crossSection);
            GetComponent<MeshRenderer>().material = crossSection.RoadMaterial;
        }

        protected virtual void OnDrawGizmos()
        {
            if(controlPoints.Count == 0) { return; }

            RoadGenerator roadGenerator = new RoadGenerator(controlPoints.ToArray(), sectionsCount, resolution);
            int segmentsCount = (controlPoints.Count - 1) / 3;

            Gizmos.color = Color.red;
            for (int i = 0; i < controlPoints.Count; i++)
            {
                Gizmos.DrawSphere(controlPoints[i].position, 0.2f);
            }

            Gizmos.color = Color.black;
            List<Vector3> wpList = roadGenerator.CalculateWaypoints();
            foreach (Vector3 wp in wpList)
            {
                if (!controlPoints.Select(c => c.position).Contains(wp))
                {
                    Gizmos.DrawSphere(wp, 0.2f);
                }
            }

            Gizmos.color = Color.white;
            for (int segment = 0; segment < segmentsCount; segment++)
            {
                Vector3[] segmentPoints = new Vector3[] { controlPoints[segment*3].position, controlPoints[segment * 3 + 1].position ,
                                                          controlPoints[segment * 3 + 2].position , controlPoints[segment * 3 + 3].position };

                Handles.DrawBezier(segmentPoints[0], segmentPoints[3], segmentPoints[1], segmentPoints[2], Color.white, EditorGUIUtility.whiteTexture, 1f);
                Gizmos.DrawLine(segmentPoints[0], segmentPoints[1]);
                Gizmos.DrawLine(segmentPoints[3], segmentPoints[2]);
            }

            Gizmos.DrawLine(controlPoints[0].position, startPoint.GetTransform().position);
            Gizmos.DrawLine(controlPoints[controlPoints.Count - 1].position, endPoint.GetTransform().position);
        }
#endif
    }
}

