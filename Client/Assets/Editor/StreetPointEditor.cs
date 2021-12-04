namespace ScotlandYard.CustomEditor
{
	using UnityEngine;
	using UnityEditor;
    using ScotlandYard.Scripts.Street;

	[CustomEditor(typeof(StreetPoint), true)]
	[CanEditMultipleObjects]
	public class StreetPointEditor : Editor
	{
		#region Members
		Transform[] childTransforms;
		MeshFilter[] meshFilters;
		Mesh mesh;
		Quaternion rotation;
		#endregion

		#region Properties
		#endregion

		#region Methods
		protected void OnEnable()
		{
			var targetArray = targets;
			childTransforms = new Transform[targetArray.Length];
			meshFilters = new MeshFilter[targetArray.Length];

			for(int i = 0; i < targetArray.Length; i++)
            {
				StreetPoint streetPoint = targetArray[i] as StreetPoint;
				childTransforms[i] = streetPoint.transform.GetChild(3);
				meshFilters[i] = childTransforms[i].GetComponent<MeshFilter>();
			}

			mesh = meshFilters.Length > 0 ? meshFilters[0].sharedMesh : null;
			rotation = childTransforms.Length > 0 ? childTransforms[0].localRotation : Quaternion.Euler(0,0,0);
		}

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EditorGUILayout.Separator();

			mesh = (Mesh)EditorGUILayout.ObjectField("Crossroad Mesh", mesh, typeof(Mesh), false);
			rotation.eulerAngles = EditorGUILayout.Vector3Field("Crossroad Rotation", rotation.eulerAngles);

			for(int i = 0; i < childTransforms.Length; i++)
            {
				if(rotation != childTransforms[0].localRotation )
                {
					childTransforms[i].localRotation = rotation;
				}
				
				meshFilters[i].sharedMesh = mesh;
			}
		}
        #endregion
    }
}