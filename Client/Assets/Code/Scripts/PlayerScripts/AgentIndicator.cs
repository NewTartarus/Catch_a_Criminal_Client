namespace ScotlandYard.Scripts
{
    using ScotlandYard.Scripts.Helper;
	using UnityEngine;

	[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
	public class AgentIndicator : MonoBehaviour
	{
		#region Members
		[SerializeField] protected float innerRadius;
		[SerializeField] protected float thickness;
		[Range(3, 50)] [SerializeField] protected int detail = 3;
		[Range(0, 10)] [SerializeField] protected float textureTiling = 1;

		protected Material material;
		#endregion

		#region Methods
		protected void Awake()
		{
			Mesh mesh = QuadRingGenerator.GenerateMesh(innerRadius, thickness, detail, textureTiling);
			mesh.name = "Indicator";

			MeshFilter mf = GetComponent<MeshFilter>();
			mf.sharedMesh = mesh;

			MeshRenderer mr = GetComponent<MeshRenderer>();
			material = mr.material;
		}

		public void SetColor(Color color)
		{
			material.SetColor("_MainColor", color);
		}

		public void SetEmissive(bool isEmissive)
        {
			material.SetInt("_IsEmissive", isEmissive ? 1 : 0);
        }
		#endregion

#if UNITY_EDITOR
		protected void OnDrawGizmosSelected()
		{
			GizmosHelper.DrawWiredCircle(transform.position, transform.rotation, innerRadius, detail);
			GizmosHelper.DrawWiredCircle(transform.position, transform.rotation, innerRadius + thickness, detail);
		}
#endif
	}
}