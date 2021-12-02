namespace ScotlandYard.Scripts.MeshGenerator
{
	using UnityEngine;
	
	[CreateAssetMenu(fileName = "New RoadCrossSection", menuName = "ScriptableObjects/RoadCrossSection")]
	public class RoadCrossSectionSO : ScriptableObject
	{
		#region Members
		[SerializeField] Vertex[] verticies;
		[SerializeField] int[] lineIndices;
		[SerializeField] Material roadMaterial;
		#endregion

		#region Properties
		public Vertex[] Vertices => verticies;
		public int[] LineIndices => lineIndices;
		public Material RoadMaterial => roadMaterial;

		public int VertexCount => verticies.Length;
		public int LineCount => lineIndices.Length;
		#endregion

		#region Methods
		public float CalculateUSpan()
        {
			float distance = 0;
            for (int i = 0; i < LineCount; i+=2)
            {
				Vector2 a = verticies[LineIndices[i]].point;
				Vector2 b = verticies[LineIndices[i + 1]].point;

				distance += Vector2.Distance(a, b);
			}

			return distance;
        }
		#endregion
	}
}