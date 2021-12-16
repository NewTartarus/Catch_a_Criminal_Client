namespace ScotlandYard.CustomEditor
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEditor;
    using ScotlandYard.Scripts.Street;

    [CustomEditor(typeof(Route), true)]
	[CanEditMultipleObjects]
	public class RouteEditor : Editor
	{
		#region Members
		Route editorObject;
		#endregion
		
		#region Methods
		protected void OnEnable()
		{
			editorObject = (Route)target;
		}

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EditorGUILayout.Separator();
			EditorGUILayout.LabelField("StreetPoints & WayPoints", EditorStyles.boldLabel);
			GUI.enabled = false;
			GUILayout.TextField(editorObject.ReturnStreetPointOrder());
			GUILayout.TextArea(editorObject.ReturnWayPoints());
			GUI.enabled = true;
		}
        #endregion
    }
}