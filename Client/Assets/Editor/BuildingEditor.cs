namespace ScotlandYard.CustomEditor
{
	using System;
	using UnityEngine;
	using UnityEditor;
    using ScotlandYard.Scripts.Buildings;

    [CustomEditor(typeof(Building), true)]
	public class BuildingEditor : Editor
	{
		#region Members
		Building editorObject;
		int tab = 0;
		Vector2 scrollPosVoxels;
		Vector2 scrollPosParts;
		float scrollViewHeight = 300f;
		#endregion
		
		#region Methods
		protected void OnEnable()
		{
			editorObject = (Building)target;
		}

        public override void OnInspectorGUI()
        {
			editorObject.PlaceParts();

			serializedObject.Update();
			EditorGUILayout.PropertyField(serializedObject.FindProperty("buildingType"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("image"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("size"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("debugShowVoxels"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("debugShowLevel"));

			GUILayout.Space(10f);

			tab = GUILayout.Toolbar(tab, new string[] { "Voxels", "Building Parts" });
			
			switch(tab)
            {
				case 0:
					ShowVoxels();
					break;
				case 1:
				default:
					ShowBuildingParts();
					break;
            }

			GUILayout.Space(10f);

			if (GUILayout.Button("Update Building"))
            {
				editorObject.UpdateBuildingParts();
			}

            serializedObject.ApplyModifiedProperties();
		}

		private void ShowVoxels()
        {
			SerializedProperty voxels = serializedObject.FindProperty("voxels");

			int index = 0;
			Vector3 size = editorObject.Size;

			EditorGUI.indentLevel++;

			scrollPosVoxels = EditorGUILayout.BeginScrollView(scrollPosVoxels, GUILayout.Height(scrollViewHeight));
			for (int y = 0; y < size.y; y++)
			{
				EditorGUILayout.LabelField("Floor " + y);
				for (float z = ((size.z + 1) / 2 * -1); z < ((size.z + 1) / 2); z++)
				{
					EditorGUI.indentLevel++;
					EditorGUILayout.BeginHorizontal();
					for (float x = ((size.x + 1) / 2 * -1); x < ((size.x + 1) / 2); x++)
					{
						EditorGUILayout.PropertyField(voxels.GetArrayElementAtIndex(index), new GUIContent(""), GUILayout.Width(20));
						index++;
					}
					EditorGUILayout.EndHorizontal();
					EditorGUI.indentLevel--;
				}
			}
			EditorGUILayout.EndScrollView();

			EditorGUI.indentLevel--;
		}

		private void ShowBuildingParts()
        {
			SerializedProperty buildParts = serializedObject.FindProperty("buildParts");

			EditorGUI.indentLevel++;
			EditorGUILayout.BeginHorizontal();

			EditorGUILayout.LabelField("", GUILayout.Width(40));
			EditorGUILayout.LabelField("Model", EditorStyles.boldLabel, GUILayout.Width(90));
			EditorGUILayout.LabelField("Active", EditorStyles.boldLabel, GUILayout.Width(60));
			EditorGUILayout.LabelField("Top/Bot", EditorStyles.boldLabel, GUILayout.Width(65));
			EditorGUILayout.LabelField("Variant", EditorStyles.boldLabel, GUILayout.MaxWidth(200));

			EditorGUILayout.EndHorizontal();
			scrollPosParts = EditorGUILayout.BeginScrollView(scrollPosParts, GUILayout.Height(scrollViewHeight));

			int floorSize = (int)editorObject.Size.x * (int)editorObject.Size.z;
			int floor = 0;

			for (int i = 0; i < buildParts.arraySize; i++)
			{
				if (i % floorSize == 0)
				{
					EditorGUILayout.LabelField("Floor " + floor);
					floor++;
				}

				SerializedProperty bp = buildParts.GetArrayElementAtIndex(i);

				EditorGUILayout.BeginHorizontal();

				EditorGUILayout.LabelField("[" + i.ToString() + "]", GUILayout.Width(40));
				EditorGUILayout.LabelField(String.Format("{0,3} [{1}]", Convert.ToInt32(editorObject.GetByteString(i), 2), editorObject.GetByteString(i)), GUILayout.Width(90));
				EditorGUILayout.PropertyField(bp.FindPropertyRelative("isActive"), new GUIContent(""), GUILayout.Width(60));
				EditorGUILayout.PropertyField(bp.FindPropertyRelative("hasTop"), new GUIContent(""), GUILayout.Width(30));
				EditorGUILayout.PropertyField(bp.FindPropertyRelative("hasBottom"), new GUIContent(""), GUILayout.Width(30));
				EditorGUILayout.PropertyField(bp.FindPropertyRelative("variant"), new GUIContent(""), GUILayout.MaxWidth(200));

				EditorGUILayout.EndHorizontal();
			}

			EditorGUILayout.EndScrollView();
			EditorGUI.indentLevel--;
		}
        #endregion
    }
}