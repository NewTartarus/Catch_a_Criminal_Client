namespace ScotlandYard.Scripts
{
    using ScotlandYard.Scripts.Helper;
	using System.Collections.Generic;
    using UnityEngine;
	
	public class QuadRingGenerator
	{
        #region Methods
        public static Mesh GenerateMesh(float innerRadius, float thickness, int detail, float textureTiling)
        {
			float outerRadius = innerRadius + thickness;

			Mesh mesh = new Mesh();
			List<Vector3> vertices = new List<Vector3>();
			List<Vector3> normals = new List<Vector3>();
			List<Vector2> uvs = new List<Vector2>();

			for (int i = 0; i < detail+1; i++)
            {
				float t = i / (float)detail;
				float angleRad = t * MathHelper.TAU;
				Vector2 direction = MathHelper.GetUnitVectorByAngle(angleRad);

				vertices.Add(direction * outerRadius);
				vertices.Add(direction * innerRadius);
				normals.Add(Vector3.forward);
				normals.Add(Vector3.forward);
				uvs.Add(new Vector2(1, t * textureTiling));
				uvs.Add(new Vector2(0, t * textureTiling));
			}

			List<int> triangleIndicies = new List<int>();
            for (int i = 0; i < detail; i++)
            {
				int rootIndex = i * 2;
				int indexInnerRoot = rootIndex + 1;
				int indexOuterNext = rootIndex + 2;
				int indexInnerNext = rootIndex + 3;

				triangleIndicies.Add(rootIndex);
				triangleIndicies.Add(indexOuterNext);
				triangleIndicies.Add(indexInnerNext);
				triangleIndicies.Add(rootIndex);
				triangleIndicies.Add(indexInnerNext);
				triangleIndicies.Add(indexInnerRoot);
			}

			mesh.SetVertices(vertices);
			mesh.SetTriangles(triangleIndicies, 0);
			mesh.SetNormals(normals);
			mesh.SetUVs(0, uvs);

			return mesh;
		}
		#endregion
	}
}