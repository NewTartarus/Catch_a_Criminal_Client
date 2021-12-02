namespace ScotlandYard.Scripts.MeshGenerator
{
	using System;
	using UnityEngine;

	[Serializable]
	public class Vertex
	{
		public Vector2 point;
		public Vector2 normal;
		public float u; // UVs, but in 2D space
	}
}
