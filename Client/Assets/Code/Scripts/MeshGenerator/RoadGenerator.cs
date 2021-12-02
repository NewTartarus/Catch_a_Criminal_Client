namespace ScotlandYard.Scripts.MeshGenerator
{
	using System.Linq;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEditor;
	
	public class RoadGenerator
	{
		#region Members
		protected int sectionsCount = 8;
		protected int resolution = 8;
		protected Transform[] controlPoints = new Transform[4];
		#endregion

		#region Properties
		protected int SegmentsCount => (controlPoints.Length - 1) / 3;
		#endregion

		public RoadGenerator(Transform[] controlPoints, int sectionsCount = 8, int resolution = 8)
        {
			this.controlPoints = controlPoints;
			this.sectionsCount = sectionsCount;
			this.resolution = resolution;
        }

		#region Methods
		protected Vector3[] GetLocalSegmentPositions(int segmentIndex)
        {
			return new Vector3[] { controlPoints[segmentIndex*3].localPosition, controlPoints[segmentIndex * 3 + 1].localPosition ,
				                   controlPoints[segmentIndex * 3 + 2].localPosition , controlPoints[segmentIndex * 3 + 3].localPosition };
        }

		protected Vector3[] GetGlobalSegmentPositions(int segmentIndex)
		{
			return new Vector3[] { controlPoints[segmentIndex*3].position, controlPoints[segmentIndex * 3 + 1].position ,
								   controlPoints[segmentIndex * 3 + 2].position , controlPoints[segmentIndex * 3 + 3].position };
		}

		public Mesh GenerateMesh(RoadCrossSectionSO shape2D)
        {
			Mesh mesh = new Mesh();
			mesh.name = "Road";

			float uSpan = shape2D.CalculateUSpan();

			List<Vector3> verts = new List<Vector3>();
			List<Vector3> normals = new List<Vector3>();
			List<Vector2> uvs = new List<Vector2>();

			for(int segment = 0; segment < SegmentsCount; segment++)
            {
				for (int section = 0; section < sectionsCount; section++)
				{
					if (segment > 0 && section == 0) { continue; }

					float t = section / (sectionsCount - 1f);
					OrientedPoint op = GetBezierPoint(segment, t);

					for (int vertex = 0; vertex < shape2D.VertexCount; vertex++)
					{
						verts.Add(op.LocalToWorldPosition(shape2D.Vertices[vertex].point));
						normals.Add(op.LocalToWorldVector(shape2D.Vertices[vertex].normal));
						uvs.Add(new Vector2(shape2D.Vertices[vertex].u, t * GetApproxLength(segment, resolution) / uSpan));
					}
				}
			}
			
			List<int> triangles = new List<int>();
			for (int i = 0; i < (SegmentsCount * sectionsCount) - SegmentsCount; i++)
			{
				int rootIndex = i * shape2D.VertexCount;
				int rootIndexNext = (i + 1) * shape2D.VertexCount;

                for (int line = 0; line < shape2D.LineCount; line+=2)
                {
					int lineIndexA = shape2D.LineIndices[line];
					int lineIndexB = shape2D.LineIndices[line+1];

					int currentA = rootIndex + lineIndexA;
					int currentB = rootIndex + lineIndexB;
					int nextA = rootIndexNext + lineIndexA;
					int nextB = rootIndexNext + lineIndexB;

					triangles.Add(currentA);
					triangles.Add(nextA);
					triangles.Add(nextB);
					triangles.Add(currentA);
					triangles.Add(nextB);
					triangles.Add(currentB);
				}
			}

			mesh.SetVertices(verts);
			mesh.SetNormals(normals);
			mesh.SetUVs(0, uvs);
			mesh.SetTriangles(triangles, 0);

			return mesh;
		}

		protected OrientedPoint GetBezierPoint(int segment, float t)
		{
			Vector3[] segmentPoints = GetLocalSegmentPositions(segment);
			return GetBezierPoint(segmentPoints, t);
		}

		protected OrientedPoint GetBezierPoint(Vector3[] segmentPoints, float t)
		{
			Vector3 p0 = segmentPoints[0];
			Vector3 p1 = segmentPoints[1];
			Vector3 p2 = segmentPoints[2];
			Vector3 p3 = segmentPoints[3];

			Vector3 a = Vector3.Lerp(p0, p1, t);
			Vector3 b = Vector3.Lerp(p1, p2, t);
			Vector3 c = Vector3.Lerp(p2, p3, t);

			Vector3 d = Vector3.Lerp(a, b, t);
			Vector3 e = Vector3.Lerp(b, c, t);

			Vector3 pos = Vector3.Lerp(d, e, t);
			Vector3 tangent = (e - d).normalized;

			return new OrientedPoint(pos, tangent);
		}

		protected float GetApproxLength(int segment, int precision)
        {
			Vector3[] points = new Vector3[precision];

            for (int i = 0; i < precision; i++)
            {
				float t = i / (precision - 1f);
				points[i] = GetBezierPoint(segment, t).Position;
            }

			float distance = 0;
			for (int i = 0; i < precision-1; i++)
			{
				Vector3 a = points[i];
				Vector3 b = points[i + 1];
				distance += Vector3.Distance(a, b);
			}

			return distance;
		}


		public List<Vector3> CalculateWaypoints()
        {
			List<Vector3> wp = new List<Vector3>();

			for (int segment = 0; segment < SegmentsCount; segment++)
			{
				Vector3[] segmentPoints = GetGlobalSegmentPositions(segment);
				for (int s = 0; s < sectionsCount; s++)
				{
					if (segment > 0 && s == 0) { continue; }

					float t = s / (sectionsCount - 1f);
					OrientedPoint op = GetBezierPoint(segmentPoints, t);

					wp.Add(op.Position);
				}
			}

			return wp;
        }
        #endregion
    }
}